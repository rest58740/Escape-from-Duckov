using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
	// Token: 0x02000003 RID: 3
	public class InflaterInputStream : Stream
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000024EC File Offset: 0x000006EC
		public InflaterInputStream(Stream baseInputStream) : this(baseInputStream, new Inflater(), 4096)
		{
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002500 File Offset: 0x00000700
		public InflaterInputStream(Stream baseInputStream, Inflater inf) : this(baseInputStream, inf, 4096)
		{
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002510 File Offset: 0x00000710
		public InflaterInputStream(Stream baseInputStream, Inflater inflater, int bufferSize)
		{
			if (baseInputStream == null)
			{
				throw new ArgumentNullException("baseInputStream");
			}
			if (inflater == null)
			{
				throw new ArgumentNullException("inflater");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			this.baseInputStream = baseInputStream;
			this.inf = inflater;
			this.inputBuffer = new InflaterInputBuffer(baseInputStream, bufferSize);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000257C File Offset: 0x0000077C
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002584 File Offset: 0x00000784
		public bool IsStreamOwner
		{
			get
			{
				return this.isStreamOwner;
			}
			set
			{
				this.isStreamOwner = value;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002590 File Offset: 0x00000790
		public long Skip(long count)
		{
			if (count <= 0L)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.baseInputStream.CanSeek)
			{
				this.baseInputStream.Seek(count, SeekOrigin.Current);
				return count;
			}
			int num = 2048;
			if (count < (long)num)
			{
				num = (int)count;
			}
			byte[] buffer = new byte[num];
			int num2 = 1;
			long num3 = count;
			while (num3 > 0L && num2 > 0)
			{
				if (num3 < (long)num)
				{
					num = (int)num3;
				}
				num2 = this.baseInputStream.Read(buffer, 0, num);
				num3 -= (long)num2;
			}
			return count - num3;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002624 File Offset: 0x00000824
		protected void StopDecrypting()
		{
			this.inputBuffer.CryptoTransform = null;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002634 File Offset: 0x00000834
		public virtual int Available
		{
			get
			{
				return (!this.inf.IsFinished) ? 1 : 0;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002650 File Offset: 0x00000850
		protected void Fill()
		{
			if (this.inputBuffer.Available <= 0)
			{
				this.inputBuffer.Fill();
				if (this.inputBuffer.Available <= 0)
				{
					throw new SharpZipBaseException("Unexpected EOF");
				}
			}
			this.inputBuffer.SetInflaterInput(this.inf);
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000026A8 File Offset: 0x000008A8
		public override bool CanRead
		{
			get
			{
				return this.baseInputStream.CanRead;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000026B8 File Offset: 0x000008B8
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000026BC File Offset: 0x000008BC
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000026C0 File Offset: 0x000008C0
		public override long Length
		{
			get
			{
				return (long)this.inputBuffer.RawLength;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000026D0 File Offset: 0x000008D0
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000026E0 File Offset: 0x000008E0
		public override long Position
		{
			get
			{
				return this.baseInputStream.Position;
			}
			set
			{
				throw new NotSupportedException("InflaterInputStream Position not supported");
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000026EC File Offset: 0x000008EC
		public override void Flush()
		{
			this.baseInputStream.Flush();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000026FC File Offset: 0x000008FC
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("Seek not supported");
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002708 File Offset: 0x00000908
		public override void SetLength(long value)
		{
			throw new NotSupportedException("InflaterInputStream SetLength not supported");
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002714 File Offset: 0x00000914
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("InflaterInputStream Write not supported");
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002720 File Offset: 0x00000920
		public override void WriteByte(byte value)
		{
			throw new NotSupportedException("InflaterInputStream WriteByte not supported");
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000272C File Offset: 0x0000092C
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException("InflaterInputStream BeginWrite not supported");
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002738 File Offset: 0x00000938
		public override void Close()
		{
			if (!this.isClosed)
			{
				this.isClosed = true;
				if (this.isStreamOwner)
				{
					this.baseInputStream.Close();
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002770 File Offset: 0x00000970
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.inf.IsNeedingDictionary)
			{
				throw new SharpZipBaseException("Need a dictionary");
			}
			int num = count;
			for (;;)
			{
				int num2 = this.inf.Inflate(buffer, offset, num);
				offset += num2;
				num -= num2;
				if (num == 0 || this.inf.IsFinished)
				{
					break;
				}
				if (this.inf.IsNeedingInput)
				{
					this.Fill();
				}
				else if (num2 == 0)
				{
					goto Block_4;
				}
			}
			return count - num;
			Block_4:
			throw new ZipException("Dont know what to do");
		}

		// Token: 0x04000009 RID: 9
		protected Inflater inf;

		// Token: 0x0400000A RID: 10
		protected InflaterInputBuffer inputBuffer;

		// Token: 0x0400000B RID: 11
		private Stream baseInputStream;

		// Token: 0x0400000C RID: 12
		protected long csize;

		// Token: 0x0400000D RID: 13
		private bool isClosed;

		// Token: 0x0400000E RID: 14
		private bool isStreamOwner = true;
	}
}
