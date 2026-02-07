using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Pathfinding.Ionic.Crc;

namespace Pathfinding.Ionic.Zlib
{
	// Token: 0x02000058 RID: 88
	public class ParallelDeflateOutputStream : Stream
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x0001C6B0 File Offset: 0x0001A8B0
		public ParallelDeflateOutputStream(Stream stream) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001C6BC File Offset: 0x0001A8BC
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level) : this(stream, level, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0001C6C8 File Offset: 0x0001A8C8
		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001C6D4 File Offset: 0x0001A8D4
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001C6E0 File Offset: 0x0001A8E0
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			this._outStream = stream;
			this._compressLevel = level;
			this.Strategy = strategy;
			this._leaveOpen = leaveOpen;
			this.MaxBufferPairs = 16;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0001C764 File Offset: 0x0001A964
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x0001C76C File Offset: 0x0001A96C
		public CompressionStrategy Strategy { get; private set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0001C778 File Offset: 0x0001A978
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x0001C780 File Offset: 0x0001A980
		public int MaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x0001C7A0 File Offset: 0x0001A9A0
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x0001C7A8 File Offset: 0x0001A9A8
		public int BufferSize
		{
			get
			{
				return this._bufferSize;
			}
			set
			{
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("BufferSize", "BufferSize must be greater than 1024 bytes");
				}
				this._bufferSize = value;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0001C7D8 File Offset: 0x0001A9D8
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0001C7E0 File Offset: 0x0001A9E0
		public long BytesProcessed
		{
			get
			{
				return this._totalBytesProcessed;
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001C7E8 File Offset: 0x0001A9E8
		private void _InitializePoolOfWorkItems()
		{
			this._toWrite = new Queue<int>();
			this._toFill = new Queue<int>();
			this._pool = new List<WorkItem>();
			int num = ParallelDeflateOutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			num = Math.Min(num, this._maxBufferPairs);
			for (int i = 0; i < num; i++)
			{
				this._pool.Add(new WorkItem(this._bufferSize, this._compressLevel, this.Strategy, i));
				this._toFill.Enqueue(i);
			}
			this._newlyCompressedBlob = new AutoResetEvent(false);
			this._runningCrc = new CRC32();
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001C8A4 File Offset: 0x0001AAA4
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool mustWait = false;
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (count == 0)
			{
				return;
			}
			if (!this._firstWriteDone)
			{
				this._InitializePoolOfWorkItems();
				this._firstWriteDone = true;
			}
			for (;;)
			{
				this.EmitPendingBuffers(false, mustWait);
				mustWait = false;
				int num;
				if (this._currentlyFilling >= 0)
				{
					num = this._currentlyFilling;
					goto IL_AF;
				}
				if (this._toFill.Count != 0)
				{
					num = this._toFill.Dequeue();
					this._lastFilled++;
					goto IL_AF;
				}
				mustWait = true;
				IL_173:
				if (count <= 0)
				{
					return;
				}
				continue;
				IL_AF:
				WorkItem workItem = this._pool[num];
				int num2 = (workItem.buffer.Length - workItem.inputBytesAvailable <= count) ? (workItem.buffer.Length - workItem.inputBytesAvailable) : count;
				workItem.ordinal = this._lastFilled;
				Buffer.BlockCopy(buffer, offset, workItem.buffer, workItem.inputBytesAvailable, num2);
				count -= num2;
				offset += num2;
				workItem.inputBytesAvailable += num2;
				if (workItem.inputBytesAvailable == workItem.buffer.Length)
				{
					if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this._DeflateOne), workItem))
					{
						break;
					}
					this._currentlyFilling = -1;
				}
				else
				{
					this._currentlyFilling = num;
				}
				if (count > 0)
				{
					goto IL_173;
				}
				goto IL_173;
			}
			throw new Exception("Cannot enqueue workitem");
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001CA2C File Offset: 0x0001AC2C
		private void _FlushFinish()
		{
			byte[] array = new byte[128];
			ZlibCodec zlibCodec = new ZlibCodec();
			int num = zlibCodec.InitializeDeflate(this._compressLevel, false);
			zlibCodec.InputBuffer = null;
			zlibCodec.NextIn = 0;
			zlibCodec.AvailableBytesIn = 0;
			zlibCodec.OutputBuffer = array;
			zlibCodec.NextOut = 0;
			zlibCodec.AvailableBytesOut = array.Length;
			num = zlibCodec.Deflate(FlushType.Finish);
			if (num != 1 && num != 0)
			{
				throw new Exception("deflating: " + zlibCodec.Message);
			}
			if (array.Length - zlibCodec.AvailableBytesOut > 0)
			{
				this._outStream.Write(array, 0, array.Length - zlibCodec.AvailableBytesOut);
			}
			zlibCodec.EndDeflate();
			this._Crc32 = this._runningCrc.Crc32Result;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001CAF0 File Offset: 0x0001ACF0
		private void _Flush(bool lastInput)
		{
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this.emitting)
			{
				return;
			}
			if (this._currentlyFilling >= 0)
			{
				WorkItem wi = this._pool[this._currentlyFilling];
				this._DeflateOne(wi);
				this._currentlyFilling = -1;
			}
			if (lastInput)
			{
				this.EmitPendingBuffers(true, false);
				this._FlushFinish();
			}
			else
			{
				this.EmitPendingBuffers(false, false);
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001CB68 File Offset: 0x0001AD68
		public override void Flush()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			this._Flush(false);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001CBB0 File Offset: 0x0001ADB0
		public override void Close()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			if (this._isClosed)
			{
				return;
			}
			this._Flush(true);
			if (!this._leaveOpen)
			{
				this._outStream.Close();
			}
			this._isClosed = true;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001CC24 File Offset: 0x0001AE24
		public void Dispose()
		{
			this.Close();
			this._pool = null;
			this.Dispose(true);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001CC3C File Offset: 0x0001AE3C
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0001CC48 File Offset: 0x0001AE48
		public void Reset(Stream stream)
		{
			if (!this._firstWriteDone)
			{
				return;
			}
			this._toWrite.Clear();
			this._toFill.Clear();
			foreach (WorkItem workItem in this._pool)
			{
				this._toFill.Enqueue(workItem.index);
				workItem.ordinal = -1;
			}
			this._firstWriteDone = false;
			this._totalBytesProcessed = 0L;
			this._runningCrc = new CRC32();
			this._isClosed = false;
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
			this._outStream = stream;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001CD28 File Offset: 0x0001AF28
		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (this.emitting)
			{
				return;
			}
			this.emitting = true;
			if (doAll || mustWait)
			{
				this._newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = (!doAll) ? ((!mustWait) ? 0 : -1) : 200;
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(this._toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (this._toWrite.Count > 0)
							{
								num3 = this._toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(this._toWrite);
						}
						if (num3 >= 0)
						{
							WorkItem workItem = this._pool[num3];
							if (workItem.ordinal != this._lastWritten + 1)
							{
								object toWrite = this._toWrite;
								lock (toWrite)
								{
									this._toWrite.Enqueue(num3);
								}
								if (num == num3)
								{
									this._newlyCompressedBlob.WaitOne();
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
								this._outStream.Write(workItem.compressed, 0, workItem.compressedBytesAvailable);
								this._runningCrc.Combine(workItem.crc, workItem.inputBytesAvailable);
								this._totalBytesProcessed += (long)workItem.inputBytesAvailable;
								workItem.inputBytesAvailable = 0;
								this._lastWritten = workItem.ordinal;
								this._toFill.Enqueue(workItem.index);
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
			while (doAll && this._lastWritten != this._latestCompressed);
			this.emitting = false;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001CF0C File Offset: 0x0001B10C
		private void _DeflateOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				int index = workItem.index;
				CRC32 crc = new CRC32();
				crc.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
				this.DeflateOneSegment(workItem);
				workItem.crc = crc.Crc32Result;
				object latestLock = this._latestLock;
				lock (latestLock)
				{
					if (workItem.ordinal > this._latestCompressed)
					{
						this._latestCompressed = workItem.ordinal;
					}
				}
				object toWrite = this._toWrite;
				lock (toWrite)
				{
					this._toWrite.Enqueue(workItem.index);
				}
				this._newlyCompressedBlob.Set();
			}
			catch (Exception pendingException)
			{
				object eLock = this._eLock;
				lock (eLock)
				{
					if (this._pendingException != null)
					{
						this._pendingException = pendingException;
					}
				}
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001D068 File Offset: 0x0001B268
		private bool DeflateOneSegment(WorkItem workitem)
		{
			ZlibCodec compressor = workitem.compressor;
			compressor.ResetDeflate();
			compressor.NextIn = 0;
			compressor.AvailableBytesIn = workitem.inputBytesAvailable;
			compressor.NextOut = 0;
			compressor.AvailableBytesOut = workitem.compressed.Length;
			do
			{
				compressor.Deflate(FlushType.None);
			}
			while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);
			int num = compressor.Deflate(FlushType.Sync);
			workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
			return true;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001D0E4 File Offset: 0x0001B2E4
		[Conditional("Trace")]
		private void TraceOutput(ParallelDeflateOutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this._DesiredTrace) != ParallelDeflateOutputStream.TraceBits.None)
			{
				object outputLock = this._outputLock;
				lock (outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.Write("{0:000} PDOS ", hashCode);
					Console.WriteLine(format, varParams);
				}
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x0001D158 File Offset: 0x0001B358
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0001D15C File Offset: 0x0001B35C
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x0001D160 File Offset: 0x0001B360
		public override bool CanWrite
		{
			get
			{
				return this._outStream.CanWrite;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0001D170 File Offset: 0x0001B370
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0001D178 File Offset: 0x0001B378
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x0001D188 File Offset: 0x0001B388
		public override long Position
		{
			get
			{
				return this._outStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001D190 File Offset: 0x0001B390
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001D198 File Offset: 0x0001B398
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001D1A0 File Offset: 0x0001B3A0
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040002F5 RID: 757
		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		// Token: 0x040002F6 RID: 758
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x040002F7 RID: 759
		private List<WorkItem> _pool;

		// Token: 0x040002F8 RID: 760
		private bool _leaveOpen;

		// Token: 0x040002F9 RID: 761
		private bool emitting;

		// Token: 0x040002FA RID: 762
		private Stream _outStream;

		// Token: 0x040002FB RID: 763
		private int _maxBufferPairs;

		// Token: 0x040002FC RID: 764
		private int _bufferSize = ParallelDeflateOutputStream.IO_BUFFER_SIZE_DEFAULT;

		// Token: 0x040002FD RID: 765
		private AutoResetEvent _newlyCompressedBlob;

		// Token: 0x040002FE RID: 766
		private object _outputLock = new object();

		// Token: 0x040002FF RID: 767
		private bool _isClosed;

		// Token: 0x04000300 RID: 768
		private bool _firstWriteDone;

		// Token: 0x04000301 RID: 769
		private int _currentlyFilling;

		// Token: 0x04000302 RID: 770
		private int _lastFilled;

		// Token: 0x04000303 RID: 771
		private int _lastWritten;

		// Token: 0x04000304 RID: 772
		private int _latestCompressed;

		// Token: 0x04000305 RID: 773
		private int _Crc32;

		// Token: 0x04000306 RID: 774
		private CRC32 _runningCrc;

		// Token: 0x04000307 RID: 775
		private object _latestLock = new object();

		// Token: 0x04000308 RID: 776
		private Queue<int> _toWrite;

		// Token: 0x04000309 RID: 777
		private Queue<int> _toFill;

		// Token: 0x0400030A RID: 778
		private long _totalBytesProcessed;

		// Token: 0x0400030B RID: 779
		private CompressionLevel _compressLevel;

		// Token: 0x0400030C RID: 780
		private volatile Exception _pendingException;

		// Token: 0x0400030D RID: 781
		private bool _handlingException;

		// Token: 0x0400030E RID: 782
		private object _eLock = new object();

		// Token: 0x0400030F RID: 783
		private ParallelDeflateOutputStream.TraceBits _DesiredTrace = ParallelDeflateOutputStream.TraceBits.EmitLock | ParallelDeflateOutputStream.TraceBits.EmitEnter | ParallelDeflateOutputStream.TraceBits.EmitBegin | ParallelDeflateOutputStream.TraceBits.EmitDone | ParallelDeflateOutputStream.TraceBits.EmitSkip | ParallelDeflateOutputStream.TraceBits.Session | ParallelDeflateOutputStream.TraceBits.Compress | ParallelDeflateOutputStream.TraceBits.WriteEnter | ParallelDeflateOutputStream.TraceBits.WriteTake;

		// Token: 0x02000059 RID: 89
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x04000312 RID: 786
			None = 0U,
			// Token: 0x04000313 RID: 787
			NotUsed1 = 1U,
			// Token: 0x04000314 RID: 788
			EmitLock = 2U,
			// Token: 0x04000315 RID: 789
			EmitEnter = 4U,
			// Token: 0x04000316 RID: 790
			EmitBegin = 8U,
			// Token: 0x04000317 RID: 791
			EmitDone = 16U,
			// Token: 0x04000318 RID: 792
			EmitSkip = 32U,
			// Token: 0x04000319 RID: 793
			EmitAll = 58U,
			// Token: 0x0400031A RID: 794
			Flush = 64U,
			// Token: 0x0400031B RID: 795
			Lifecycle = 128U,
			// Token: 0x0400031C RID: 796
			Session = 256U,
			// Token: 0x0400031D RID: 797
			Synch = 512U,
			// Token: 0x0400031E RID: 798
			Instance = 1024U,
			// Token: 0x0400031F RID: 799
			Compress = 2048U,
			// Token: 0x04000320 RID: 800
			Write = 4096U,
			// Token: 0x04000321 RID: 801
			WriteEnter = 8192U,
			// Token: 0x04000322 RID: 802
			WriteTake = 16384U,
			// Token: 0x04000323 RID: 803
			All = 4294967295U
		}
	}
}
