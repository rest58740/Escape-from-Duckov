using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
	// Token: 0x02000005 RID: 5
	public class StreamManipulator
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00002C1C File Offset: 0x00000E1C
		public int PeekBits(int bitCount)
		{
			if (this.bitsInBuffer_ < bitCount)
			{
				if (this.windowStart_ == this.windowEnd_)
				{
					return -1;
				}
				this.buffer_ |= (uint)((uint)((int)(this.window_[this.windowStart_++] & byte.MaxValue) | (int)(this.window_[this.windowStart_++] & byte.MaxValue) << 8) << this.bitsInBuffer_);
				this.bitsInBuffer_ += 16;
			}
			return (int)((ulong)this.buffer_ & (ulong)((long)((1 << bitCount) - 1)));
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002CC0 File Offset: 0x00000EC0
		public void DropBits(int bitCount)
		{
			this.buffer_ >>= bitCount;
			this.bitsInBuffer_ -= bitCount;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002CE4 File Offset: 0x00000EE4
		public int GetBits(int bitCount)
		{
			int num = this.PeekBits(bitCount);
			if (num >= 0)
			{
				this.DropBits(bitCount);
			}
			return num;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002D08 File Offset: 0x00000F08
		public int AvailableBits
		{
			get
			{
				return this.bitsInBuffer_;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002D10 File Offset: 0x00000F10
		public int AvailableBytes
		{
			get
			{
				return this.windowEnd_ - this.windowStart_ + (this.bitsInBuffer_ >> 3);
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002D28 File Offset: 0x00000F28
		public void SkipToByteBoundary()
		{
			this.buffer_ >>= (this.bitsInBuffer_ & 7);
			this.bitsInBuffer_ &= -8;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002D54 File Offset: 0x00000F54
		public bool IsNeedingInput
		{
			get
			{
				return this.windowStart_ == this.windowEnd_;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002D64 File Offset: 0x00000F64
		public int CopyBytes(byte[] output, int offset, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			if ((this.bitsInBuffer_ & 7) != 0)
			{
				throw new InvalidOperationException("Bit buffer is not byte aligned!");
			}
			int num = 0;
			while (this.bitsInBuffer_ > 0 && length > 0)
			{
				output[offset++] = (byte)this.buffer_;
				this.buffer_ >>= 8;
				this.bitsInBuffer_ -= 8;
				length--;
				num++;
			}
			if (length == 0)
			{
				return num;
			}
			int num2 = this.windowEnd_ - this.windowStart_;
			if (length > num2)
			{
				length = num2;
			}
			Array.Copy(this.window_, this.windowStart_, output, offset, length);
			this.windowStart_ += length;
			if ((this.windowStart_ - this.windowEnd_ & 1) != 0)
			{
				this.buffer_ = (uint)(this.window_[this.windowStart_++] & byte.MaxValue);
				this.bitsInBuffer_ = 8;
			}
			return num + length;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002E70 File Offset: 0x00001070
		public void Reset()
		{
			this.buffer_ = 0U;
			this.windowStart_ = (this.windowEnd_ = (this.bitsInBuffer_ = 0));
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002EA0 File Offset: 0x000010A0
		public void SetInput(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Cannot be negative");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Cannot be negative");
			}
			if (this.windowStart_ < this.windowEnd_)
			{
				throw new InvalidOperationException("Old input was not completely processed");
			}
			int num = offset + count;
			if (offset > num || num > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if ((count & 1) != 0)
			{
				this.buffer_ |= (uint)((uint)(buffer[offset++] & byte.MaxValue) << this.bitsInBuffer_);
				this.bitsInBuffer_ += 8;
			}
			this.window_ = buffer;
			this.windowStart_ = offset;
			this.windowEnd_ = num;
		}

		// Token: 0x04000017 RID: 23
		private byte[] window_;

		// Token: 0x04000018 RID: 24
		private int windowStart_;

		// Token: 0x04000019 RID: 25
		private int windowEnd_;

		// Token: 0x0400001A RID: 26
		private uint buffer_;

		// Token: 0x0400001B RID: 27
		private int bitsInBuffer_;
	}
}
