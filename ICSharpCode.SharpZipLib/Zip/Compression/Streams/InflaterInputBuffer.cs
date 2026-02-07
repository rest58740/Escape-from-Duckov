using System;
using System.IO;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
	// Token: 0x02000002 RID: 2
	public class InflaterInputBuffer
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020EC File Offset: 0x000002EC
		public InflaterInputBuffer(Stream stream) : this(stream, 4096)
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020FC File Offset: 0x000002FC
		public InflaterInputBuffer(Stream stream, int bufferSize)
		{
			this.inputStream = stream;
			if (bufferSize < 1024)
			{
				bufferSize = 1024;
			}
			this.rawData = new byte[bufferSize];
			this.clearText = this.rawData;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002138 File Offset: 0x00000338
		public int RawLength
		{
			get
			{
				return this.rawLength;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002140 File Offset: 0x00000340
		public byte[] RawData
		{
			get
			{
				return this.rawData;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002148 File Offset: 0x00000348
		public int ClearTextLength
		{
			get
			{
				return this.clearTextLength;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002150 File Offset: 0x00000350
		public byte[] ClearText
		{
			get
			{
				return this.clearText;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002158 File Offset: 0x00000358
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002160 File Offset: 0x00000360
		public int Available
		{
			get
			{
				return this.available;
			}
			set
			{
				this.available = value;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000216C File Offset: 0x0000036C
		public void SetInflaterInput(Inflater inflater)
		{
			if (this.available > 0)
			{
				inflater.SetInput(this.clearText, this.clearTextLength - this.available, this.available);
				this.available = 0;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021AC File Offset: 0x000003AC
		public void Fill()
		{
			this.rawLength = 0;
			int num;
			for (int i = this.rawData.Length; i > 0; i -= num)
			{
				num = this.inputStream.Read(this.rawData, this.rawLength, i);
				if (num <= 0)
				{
					break;
				}
				this.rawLength += num;
			}
			if (this.cryptoTransform != null)
			{
				this.clearTextLength = this.cryptoTransform.TransformBlock(this.rawData, 0, this.rawLength, this.clearText, 0);
			}
			else
			{
				this.clearTextLength = this.rawLength;
			}
			this.available = this.clearTextLength;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000225C File Offset: 0x0000045C
		public int ReadRawBuffer(byte[] buffer)
		{
			return this.ReadRawBuffer(buffer, 0, buffer.Length);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000226C File Offset: 0x0000046C
		public int ReadRawBuffer(byte[] outBuffer, int offset, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			int num = offset;
			int i = length;
			while (i > 0)
			{
				if (this.available <= 0)
				{
					this.Fill();
					if (this.available <= 0)
					{
						return 0;
					}
				}
				int num2 = Math.Min(i, this.available);
				Array.Copy(this.rawData, this.rawLength - this.available, outBuffer, num, num2);
				num += num2;
				i -= num2;
				this.available -= num2;
			}
			return length;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022FC File Offset: 0x000004FC
		public int ReadClearTextBuffer(byte[] outBuffer, int offset, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			int num = offset;
			int i = length;
			while (i > 0)
			{
				if (this.available <= 0)
				{
					this.Fill();
					if (this.available <= 0)
					{
						return 0;
					}
				}
				int num2 = Math.Min(i, this.available);
				Array.Copy(this.clearText, this.clearTextLength - this.available, outBuffer, num, num2);
				num += num2;
				i -= num2;
				this.available -= num2;
			}
			return length;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000238C File Offset: 0x0000058C
		public int ReadLeByte()
		{
			if (this.available <= 0)
			{
				this.Fill();
				if (this.available <= 0)
				{
					throw new ZipException("EOF in header");
				}
			}
			byte result = this.rawData[this.rawLength - this.available];
			this.available--;
			return (int)result;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000023E8 File Offset: 0x000005E8
		public int ReadLeShort()
		{
			return this.ReadLeByte() | this.ReadLeByte() << 8;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000023FC File Offset: 0x000005FC
		public int ReadLeInt()
		{
			return this.ReadLeShort() | this.ReadLeShort() << 16;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002410 File Offset: 0x00000610
		public long ReadLeLong()
		{
			return (long)((ulong)this.ReadLeInt() | (ulong)((ulong)((long)this.ReadLeInt()) << 32));
		}

		// Token: 0x17000006 RID: 6
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002424 File Offset: 0x00000624
		public ICryptoTransform CryptoTransform
		{
			set
			{
				this.cryptoTransform = value;
				if (this.cryptoTransform != null)
				{
					if (this.rawData == this.clearText)
					{
						if (this.internalClearText == null)
						{
							this.internalClearText = new byte[this.rawData.Length];
						}
						this.clearText = this.internalClearText;
					}
					this.clearTextLength = this.rawLength;
					if (this.available > 0)
					{
						this.cryptoTransform.TransformBlock(this.rawData, this.rawLength - this.available, this.available, this.clearText, this.rawLength - this.available);
					}
				}
				else
				{
					this.clearText = this.rawData;
					this.clearTextLength = this.rawLength;
				}
			}
		}

		// Token: 0x04000001 RID: 1
		private int rawLength;

		// Token: 0x04000002 RID: 2
		private byte[] rawData;

		// Token: 0x04000003 RID: 3
		private int clearTextLength;

		// Token: 0x04000004 RID: 4
		private byte[] clearText;

		// Token: 0x04000005 RID: 5
		private byte[] internalClearText;

		// Token: 0x04000006 RID: 6
		private int available;

		// Token: 0x04000007 RID: 7
		private ICryptoTransform cryptoTransform;

		// Token: 0x04000008 RID: 8
		private Stream inputStream;
	}
}
