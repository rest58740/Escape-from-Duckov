using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	// Token: 0x0200000D RID: 13
	public class PendingBuffer
	{
		// Token: 0x0600008D RID: 141 RVA: 0x000057B4 File Offset: 0x000039B4
		public PendingBuffer() : this(4096)
		{
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000057C4 File Offset: 0x000039C4
		public PendingBuffer(int bufferSize)
		{
			this.buffer_ = new byte[bufferSize];
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000057D8 File Offset: 0x000039D8
		public void Reset()
		{
			this.start = (this.end = (this.bitCount = 0));
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005800 File Offset: 0x00003A00
		public void WriteByte(int value)
		{
			this.buffer_[this.end++] = (byte)value;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005828 File Offset: 0x00003A28
		public void WriteShort(int value)
		{
			this.buffer_[this.end++] = (byte)value;
			this.buffer_[this.end++] = (byte)(value >> 8);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000586C File Offset: 0x00003A6C
		public void WriteInt(int value)
		{
			this.buffer_[this.end++] = (byte)value;
			this.buffer_[this.end++] = (byte)(value >> 8);
			this.buffer_[this.end++] = (byte)(value >> 16);
			this.buffer_[this.end++] = (byte)(value >> 24);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000058EC File Offset: 0x00003AEC
		public void WriteBlock(byte[] block, int offset, int length)
		{
			Array.Copy(block, offset, this.buffer_, this.end, length);
			this.end += length;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000591C File Offset: 0x00003B1C
		public int BitCount
		{
			get
			{
				return this.bitCount;
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005924 File Offset: 0x00003B24
		public void AlignToByte()
		{
			if (this.bitCount > 0)
			{
				this.buffer_[this.end++] = (byte)this.bits;
				if (this.bitCount > 8)
				{
					this.buffer_[this.end++] = (byte)(this.bits >> 8);
				}
			}
			this.bits = 0U;
			this.bitCount = 0;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005998 File Offset: 0x00003B98
		public void WriteBits(int b, int count)
		{
			this.bits |= (uint)((uint)b << this.bitCount);
			this.bitCount += count;
			if (this.bitCount >= 16)
			{
				this.buffer_[this.end++] = (byte)this.bits;
				this.buffer_[this.end++] = (byte)(this.bits >> 8);
				this.bits >>= 16;
				this.bitCount -= 16;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005A38 File Offset: 0x00003C38
		public void WriteShortMSB(int s)
		{
			this.buffer_[this.end++] = (byte)(s >> 8);
			this.buffer_[this.end++] = (byte)s;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00005A7C File Offset: 0x00003C7C
		public bool IsFlushed
		{
			get
			{
				return this.end == 0;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005A88 File Offset: 0x00003C88
		public int Flush(byte[] output, int offset, int length)
		{
			if (this.bitCount >= 8)
			{
				this.buffer_[this.end++] = (byte)this.bits;
				this.bits >>= 8;
				this.bitCount -= 8;
			}
			if (length > this.end - this.start)
			{
				length = this.end - this.start;
				Array.Copy(this.buffer_, this.start, output, offset, length);
				this.start = 0;
				this.end = 0;
			}
			else
			{
				Array.Copy(this.buffer_, this.start, output, offset, length);
				this.start += length;
			}
			return length;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005B48 File Offset: 0x00003D48
		public byte[] ToByteArray()
		{
			byte[] array = new byte[this.end - this.start];
			Array.Copy(this.buffer_, this.start, array, 0, array.Length);
			this.start = 0;
			this.end = 0;
			return array;
		}

		// Token: 0x0400005F RID: 95
		private byte[] buffer_;

		// Token: 0x04000060 RID: 96
		private int start;

		// Token: 0x04000061 RID: 97
		private int end;

		// Token: 0x04000062 RID: 98
		private uint bits;

		// Token: 0x04000063 RID: 99
		private int bitCount;
	}
}
