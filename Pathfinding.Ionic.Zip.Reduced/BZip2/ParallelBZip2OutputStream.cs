using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Pathfinding.Ionic.BZip2
{
	// Token: 0x02000046 RID: 70
	public class ParallelBZip2OutputStream : Stream
	{
		// Token: 0x06000354 RID: 852 RVA: 0x00014E4C File Offset: 0x0001304C
		public ParallelBZip2OutputStream(Stream output) : this(output, BZip2.MaxBlockSize, false)
		{
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00014E5C File Offset: 0x0001305C
		public ParallelBZip2OutputStream(Stream output, int blockSize) : this(output, blockSize, false)
		{
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00014E68 File Offset: 0x00013068
		public ParallelBZip2OutputStream(Stream output, bool leaveOpen) : this(output, BZip2.MaxBlockSize, leaveOpen)
		{
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00014E78 File Offset: 0x00013078
		public ParallelBZip2OutputStream(Stream output, int blockSize, bool leaveOpen)
		{
			if (blockSize < BZip2.MinBlockSize || blockSize > BZip2.MaxBlockSize)
			{
				string text = string.Format("blockSize={0} is out of range; must be between {1} and {2}", blockSize, BZip2.MinBlockSize, BZip2.MaxBlockSize);
				throw new ArgumentException(text, "blockSize");
			}
			this.output = output;
			if (!this.output.CanWrite)
			{
				throw new ArgumentException("The stream is not writable.", "output");
			}
			this.bw = new BitWriter(this.output);
			this.blockSize100k = blockSize;
			this.leaveOpen = leaveOpen;
			this.combinedCRC = 0U;
			this.MaxWorkers = 16;
			this.EmitHeader();
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00014F60 File Offset: 0x00013160
		private void InitializePoolOfWorkItems()
		{
			this.toWrite = new Queue<int>();
			this.toFill = new Queue<int>();
			this.pool = new List<WorkItem>();
			int num = ParallelBZip2OutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			num = Math.Min(num, this.MaxWorkers);
			for (int i = 0; i < num; i++)
			{
				this.pool.Add(new WorkItem(i, this.blockSize100k));
				this.toFill.Enqueue(i);
			}
			this.newlyCompressedBlob = new AutoResetEvent(false);
			this.currentlyFilling = -1;
			this.lastFilled = -1;
			this.lastWritten = -1;
			this.latestCompressed = -1;
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600035A RID: 858 RVA: 0x00015004 File Offset: 0x00013204
		// (set) Token: 0x0600035B RID: 859 RVA: 0x0001500C File Offset: 0x0001320C
		public int MaxWorkers
		{
			get
			{
				return this._maxWorkers;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxWorkers", "Value must be 4 or greater.");
				}
				this._maxWorkers = value;
			}
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0001502C File Offset: 0x0001322C
		public override void Close()
		{
			if (this.pendingException != null)
			{
				this.handlingException = true;
				Exception ex = this.pendingException;
				this.pendingException = null;
				throw ex;
			}
			if (this.handlingException)
			{
				return;
			}
			if (this.output == null)
			{
				return;
			}
			Stream stream = this.output;
			try
			{
				this.FlushOutput(true);
			}
			finally
			{
				this.output = null;
				this.bw = null;
			}
			if (!this.leaveOpen)
			{
				stream.Close();
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x000150C8 File Offset: 0x000132C8
		private void FlushOutput(bool lastInput)
		{
			if (this.emitting)
			{
				return;
			}
			if (this.currentlyFilling >= 0)
			{
				WorkItem wi = this.pool[this.currentlyFilling];
				this.CompressOne(wi);
				this.currentlyFilling = -1;
			}
			if (lastInput)
			{
				this.EmitPendingBuffers(true, false);
				this.EmitTrailer();
			}
			else
			{
				this.EmitPendingBuffers(false, false);
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00015130 File Offset: 0x00013330
		public override void Flush()
		{
			if (this.output != null)
			{
				this.FlushOutput(false);
				this.bw.Flush();
				this.output.Flush();
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00015168 File Offset: 0x00013368
		private void EmitHeader()
		{
			byte[] array = new byte[]
			{
				66,
				90,
				104,
				0
			};
			this.output.Write(array, 0, array.Length);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000151A4 File Offset: 0x000133A4
		private void EmitTrailer()
		{
			this.bw.WriteByte(23);
			this.bw.WriteByte(114);
			this.bw.WriteByte(69);
			this.bw.WriteByte(56);
			this.bw.WriteByte(80);
			this.bw.WriteByte(144);
			this.bw.WriteInt(this.combinedCRC);
			this.bw.FinishAndPad();
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00015220 File Offset: 0x00013420
		public int BlockSize
		{
			get
			{
				return this.blockSize100k;
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00015228 File Offset: 0x00013428
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool mustWait = false;
			if (this.output == null)
			{
				throw new IOException("the stream is not open");
			}
			if (this.pendingException != null)
			{
				this.handlingException = true;
				Exception ex = this.pendingException;
				this.pendingException = null;
				throw ex;
			}
			if (offset < 0)
			{
				throw new IndexOutOfRangeException(string.Format("offset ({0}) must be > 0", offset));
			}
			if (count < 0)
			{
				throw new IndexOutOfRangeException(string.Format("count ({0}) must be > 0", count));
			}
			if (offset + count > buffer.Length)
			{
				throw new IndexOutOfRangeException(string.Format("offset({0}) count({1}) bLength({2})", offset, count, buffer.Length));
			}
			if (count == 0)
			{
				return;
			}
			if (!this.firstWriteDone)
			{
				this.InitializePoolOfWorkItems();
				this.firstWriteDone = true;
			}
			int num = 0;
			int num2 = count;
			for (;;)
			{
				this.EmitPendingBuffers(false, mustWait);
				mustWait = false;
				int num3;
				if (this.currentlyFilling >= 0)
				{
					num3 = this.currentlyFilling;
					goto IL_124;
				}
				if (this.toFill.Count != 0)
				{
					num3 = this.toFill.Dequeue();
					this.lastFilled++;
					goto IL_124;
				}
				mustWait = true;
				IL_1A0:
				if (num2 <= 0)
				{
					goto Block_12;
				}
				continue;
				IL_124:
				WorkItem workItem = this.pool[num3];
				workItem.ordinal = this.lastFilled;
				int num4 = workItem.Compressor.Fill(buffer, offset, num2);
				if (num4 != num2)
				{
					if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this.CompressOne), workItem))
					{
						break;
					}
					this.currentlyFilling = -1;
					offset += num4;
				}
				else
				{
					this.currentlyFilling = num3;
				}
				num2 -= num4;
				num += num4;
				goto IL_1A0;
			}
			throw new Exception("Cannot enqueue workitem");
			Block_12:
			this.totalBytesWrittenIn += (long)num;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x000153EC File Offset: 0x000135EC
		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (this.emitting)
			{
				return;
			}
			this.emitting = true;
			if (doAll || mustWait)
			{
				this.newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = (!doAll) ? ((!mustWait) ? 0 : -1) : 200;
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(this.toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (this.toWrite.Count > 0)
							{
								num3 = this.toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(this.toWrite);
						}
						if (num3 >= 0)
						{
							WorkItem workItem = this.pool[num3];
							if (workItem.ordinal != this.lastWritten + 1)
							{
								object obj = this.toWrite;
								lock (obj)
								{
									this.toWrite.Enqueue(num3);
								}
								if (num == num3)
								{
									this.newlyCompressedBlob.WaitOne();
									num = -1;
								}
								else if (num == -1)
								{
									num = num3;
								}
							}
							else
							{
								num = -1;
								BitWriter bitWriter = workItem.bw;
								bitWriter.Flush();
								MemoryStream ms = workItem.ms;
								ms.Seek(0L, 0);
								long num4 = 0L;
								byte[] array = new byte[1024];
								int num5;
								while ((num5 = ms.Read(array, 0, array.Length)) > 0)
								{
									for (int i = 0; i < num5; i++)
									{
										this.bw.WriteByte(array[i]);
									}
									num4 += (long)num5;
								}
								if (bitWriter.NumRemainingBits > 0)
								{
									this.bw.WriteBits(bitWriter.NumRemainingBits, (uint)bitWriter.RemainingBits);
								}
								this.combinedCRC = (this.combinedCRC << 1 | this.combinedCRC >> 31);
								this.combinedCRC ^= workItem.Compressor.Crc32;
								this.totalBytesWrittenOut += num4;
								bitWriter.Reset();
								this.lastWritten = workItem.ordinal;
								workItem.ordinal = -1;
								this.toFill.Enqueue(workItem.index);
								if (num2 == -1)
								{
									num2 = 0;
								}
							}
						}
					}
					else
					{
						num3 = -1;
					}
				}
				while (num3 >= 0);
			}
			while (doAll && this.lastWritten != this.latestCompressed);
			if (doAll)
			{
			}
			this.emitting = false;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00015684 File Offset: 0x00013884
		private void CompressOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				workItem.Compressor.CompressAndWrite();
				object obj = this.latestLock;
				lock (obj)
				{
					if (workItem.ordinal > this.latestCompressed)
					{
						this.latestCompressed = workItem.ordinal;
					}
				}
				object obj2 = this.toWrite;
				lock (obj2)
				{
					this.toWrite.Enqueue(workItem.index);
				}
				this.newlyCompressedBlob.Set();
			}
			catch (Exception ex)
			{
				object obj3 = this.eLock;
				lock (obj3)
				{
					if (this.pendingException != null)
					{
						this.pendingException = ex;
					}
				}
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000365 RID: 869 RVA: 0x000157B0 File Offset: 0x000139B0
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000366 RID: 870 RVA: 0x000157B4 File Offset: 0x000139B4
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000367 RID: 871 RVA: 0x000157B8 File Offset: 0x000139B8
		public override bool CanWrite
		{
			get
			{
				if (this.output == null)
				{
					throw new ObjectDisposedException("BZip2Stream");
				}
				return this.output.CanWrite;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000368 RID: 872 RVA: 0x000157DC File Offset: 0x000139DC
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000369 RID: 873 RVA: 0x000157E4 File Offset: 0x000139E4
		// (set) Token: 0x0600036A RID: 874 RVA: 0x000157EC File Offset: 0x000139EC
		public override long Position
		{
			get
			{
				return this.totalBytesWrittenIn;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600036B RID: 875 RVA: 0x000157F4 File Offset: 0x000139F4
		public long BytesWrittenOut
		{
			get
			{
				return this.totalBytesWrittenOut;
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x000157FC File Offset: 0x000139FC
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00015804 File Offset: 0x00013A04
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0001580C File Offset: 0x00013A0C
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00015814 File Offset: 0x00013A14
		[Conditional("Trace")]
		private void TraceOutput(ParallelBZip2OutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this.desiredTrace) != ParallelBZip2OutputStream.TraceBits.None)
			{
				object obj = this.outputLock;
				lock (obj)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.Write("{0:000} PBOS ", hashCode);
					Console.WriteLine(format, varParams);
				}
			}
		}

		// Token: 0x040001FA RID: 506
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x040001FB RID: 507
		private int _maxWorkers;

		// Token: 0x040001FC RID: 508
		private bool firstWriteDone;

		// Token: 0x040001FD RID: 509
		private int lastFilled;

		// Token: 0x040001FE RID: 510
		private int lastWritten;

		// Token: 0x040001FF RID: 511
		private int latestCompressed;

		// Token: 0x04000200 RID: 512
		private int currentlyFilling;

		// Token: 0x04000201 RID: 513
		private volatile Exception pendingException;

		// Token: 0x04000202 RID: 514
		private bool handlingException;

		// Token: 0x04000203 RID: 515
		private bool emitting;

		// Token: 0x04000204 RID: 516
		private Queue<int> toWrite;

		// Token: 0x04000205 RID: 517
		private Queue<int> toFill;

		// Token: 0x04000206 RID: 518
		private List<WorkItem> pool;

		// Token: 0x04000207 RID: 519
		private object latestLock = new object();

		// Token: 0x04000208 RID: 520
		private object eLock = new object();

		// Token: 0x04000209 RID: 521
		private object outputLock = new object();

		// Token: 0x0400020A RID: 522
		private AutoResetEvent newlyCompressedBlob;

		// Token: 0x0400020B RID: 523
		private long totalBytesWrittenIn;

		// Token: 0x0400020C RID: 524
		private long totalBytesWrittenOut;

		// Token: 0x0400020D RID: 525
		private bool leaveOpen;

		// Token: 0x0400020E RID: 526
		private uint combinedCRC;

		// Token: 0x0400020F RID: 527
		private Stream output;

		// Token: 0x04000210 RID: 528
		private BitWriter bw;

		// Token: 0x04000211 RID: 529
		private int blockSize100k;

		// Token: 0x04000212 RID: 530
		private ParallelBZip2OutputStream.TraceBits desiredTrace = ParallelBZip2OutputStream.TraceBits.Crc | ParallelBZip2OutputStream.TraceBits.Write;

		// Token: 0x02000047 RID: 71
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x04000214 RID: 532
			None = 0U,
			// Token: 0x04000215 RID: 533
			Crc = 1U,
			// Token: 0x04000216 RID: 534
			Write = 2U,
			// Token: 0x04000217 RID: 535
			All = 4294967295U
		}
	}
}
