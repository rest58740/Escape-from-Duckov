using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Pathfinding.Ionic.BZip2
{
	// Token: 0x02000042 RID: 66
	public class BZip2OutputStream : Stream
	{
		// Token: 0x06000332 RID: 818 RVA: 0x0001480C File Offset: 0x00012A0C
		public BZip2OutputStream(Stream output) : this(output, BZip2.MaxBlockSize, false)
		{
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0001481C File Offset: 0x00012A1C
		public BZip2OutputStream(Stream output, int blockSize) : this(output, blockSize, false)
		{
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00014828 File Offset: 0x00012A28
		public BZip2OutputStream(Stream output, bool leaveOpen) : this(output, BZip2.MaxBlockSize, leaveOpen)
		{
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00014838 File Offset: 0x00012A38
		public BZip2OutputStream(Stream output, int blockSize, bool leaveOpen)
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
			this.compressor = new BZip2Compressor(this.bw, blockSize);
			this.leaveOpen = leaveOpen;
			this.combinedCRC = 0U;
			this.EmitHeader();
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00014900 File Offset: 0x00012B00
		public override void Close()
		{
			if (this.output != null)
			{
				Stream stream = this.output;
				this.Finish();
				if (!this.leaveOpen)
				{
					stream.Close();
				}
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00014938 File Offset: 0x00012B38
		public override void Flush()
		{
			if (this.output != null)
			{
				this.bw.Flush();
				this.output.Flush();
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0001495C File Offset: 0x00012B5C
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

		// Token: 0x06000339 RID: 825 RVA: 0x00014998 File Offset: 0x00012B98
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

		// Token: 0x0600033A RID: 826 RVA: 0x00014A14 File Offset: 0x00012C14
		private void Finish()
		{
			try
			{
				int totalBytesWrittenOut = this.bw.TotalBytesWrittenOut;
				this.compressor.CompressAndWrite();
				this.combinedCRC = (this.combinedCRC << 1 | this.combinedCRC >> 31);
				this.combinedCRC ^= this.compressor.Crc32;
				this.EmitTrailer();
			}
			finally
			{
				this.output = null;
				this.compressor = null;
				this.bw = null;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00014AA8 File Offset: 0x00012CA8
		public int BlockSize
		{
			get
			{
				return this.blockSize100k;
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00014AB0 File Offset: 0x00012CB0
		public override void Write(byte[] buffer, int offset, int count)
		{
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
			if (this.output == null)
			{
				throw new IOException("the stream is not open");
			}
			if (count == 0)
			{
				return;
			}
			int num = 0;
			int num2 = count;
			do
			{
				int num3 = this.compressor.Fill(buffer, offset, num2);
				if (num3 != num2)
				{
					int totalBytesWrittenOut = this.bw.TotalBytesWrittenOut;
					this.compressor.CompressAndWrite();
					this.combinedCRC = (this.combinedCRC << 1 | this.combinedCRC >> 31);
					this.combinedCRC ^= this.compressor.Crc32;
					offset += num3;
				}
				num2 -= num3;
				num += num3;
			}
			while (num2 > 0);
			this.totalBytesWrittenIn += num;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00014BC8 File Offset: 0x00012DC8
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00014BCC File Offset: 0x00012DCC
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00014BD0 File Offset: 0x00012DD0
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

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00014BF4 File Offset: 0x00012DF4
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000341 RID: 833 RVA: 0x00014BFC File Offset: 0x00012DFC
		// (set) Token: 0x06000342 RID: 834 RVA: 0x00014C08 File Offset: 0x00012E08
		public override long Position
		{
			get
			{
				return (long)this.totalBytesWrittenIn;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00014C10 File Offset: 0x00012E10
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00014C18 File Offset: 0x00012E18
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00014C20 File Offset: 0x00012E20
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00014C28 File Offset: 0x00012E28
		[Conditional("Trace")]
		private void TraceOutput(BZip2OutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this.desiredTrace) != BZip2OutputStream.TraceBits.None)
			{
				int hashCode = Thread.CurrentThread.GetHashCode();
				Console.Write("{0:000} PBOS ", hashCode);
				Console.WriteLine(format, varParams);
			}
		}

		// Token: 0x040001E4 RID: 484
		private int totalBytesWrittenIn;

		// Token: 0x040001E5 RID: 485
		private bool leaveOpen;

		// Token: 0x040001E6 RID: 486
		private BZip2Compressor compressor;

		// Token: 0x040001E7 RID: 487
		private uint combinedCRC;

		// Token: 0x040001E8 RID: 488
		private Stream output;

		// Token: 0x040001E9 RID: 489
		private BitWriter bw;

		// Token: 0x040001EA RID: 490
		private int blockSize100k;

		// Token: 0x040001EB RID: 491
		private BZip2OutputStream.TraceBits desiredTrace = BZip2OutputStream.TraceBits.Crc | BZip2OutputStream.TraceBits.Write;

		// Token: 0x02000043 RID: 67
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x040001ED RID: 493
			None = 0U,
			// Token: 0x040001EE RID: 494
			Crc = 1U,
			// Token: 0x040001EF RID: 495
			Write = 2U,
			// Token: 0x040001F0 RID: 496
			All = 4294967295U
		}
	}
}
