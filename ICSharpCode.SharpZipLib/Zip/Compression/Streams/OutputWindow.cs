using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
	// Token: 0x02000006 RID: 6
	public class OutputWindow
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00002F90 File Offset: 0x00001190
		public void Write(int value)
		{
			if (this.windowFilled++ == 32768)
			{
				throw new InvalidOperationException("Window full");
			}
			this.window[this.windowEnd++] = (byte)value;
			this.windowEnd &= 32767;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002FF0 File Offset: 0x000011F0
		private void SlowRepeat(int repStart, int length, int distance)
		{
			while (length-- > 0)
			{
				this.window[this.windowEnd++] = this.window[repStart++];
				this.windowEnd &= 32767;
				repStart &= 32767;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003050 File Offset: 0x00001250
		public void Repeat(int length, int distance)
		{
			if ((this.windowFilled += length) > 32768)
			{
				throw new InvalidOperationException("Window full");
			}
			int num = this.windowEnd - distance & 32767;
			int num2 = 32768 - length;
			if (num <= num2 && this.windowEnd < num2)
			{
				if (length <= distance)
				{
					Array.Copy(this.window, num, this.window, this.windowEnd, length);
					this.windowEnd += length;
				}
				else
				{
					while (length-- > 0)
					{
						this.window[this.windowEnd++] = this.window[num++];
					}
				}
			}
			else
			{
				this.SlowRepeat(num, length, distance);
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003124 File Offset: 0x00001324
		public int CopyStored(StreamManipulator input, int length)
		{
			length = Math.Min(Math.Min(length, 32768 - this.windowFilled), input.AvailableBytes);
			int num = 32768 - this.windowEnd;
			int num2;
			if (length > num)
			{
				num2 = input.CopyBytes(this.window, this.windowEnd, num);
				if (num2 == num)
				{
					num2 += input.CopyBytes(this.window, 0, length - num);
				}
			}
			else
			{
				num2 = input.CopyBytes(this.window, this.windowEnd, length);
			}
			this.windowEnd = (this.windowEnd + num2 & 32767);
			this.windowFilled += num2;
			return num2;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000031D0 File Offset: 0x000013D0
		public void CopyDict(byte[] dictionary, int offset, int length)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			if (this.windowFilled > 0)
			{
				throw new InvalidOperationException();
			}
			if (length > 32768)
			{
				offset += length - 32768;
				length = 32768;
			}
			Array.Copy(dictionary, offset, this.window, 0, length);
			this.windowEnd = (length & 32767);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000323C File Offset: 0x0000143C
		public int GetFreeSpace()
		{
			return 32768 - this.windowFilled;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000324C File Offset: 0x0000144C
		public int GetAvailable()
		{
			return this.windowFilled;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003254 File Offset: 0x00001454
		public int CopyOutput(byte[] output, int offset, int len)
		{
			int num = this.windowEnd;
			if (len > this.windowFilled)
			{
				len = this.windowFilled;
			}
			else
			{
				num = (this.windowEnd - this.windowFilled + len & 32767);
			}
			int num2 = len;
			int num3 = len - num;
			if (num3 > 0)
			{
				Array.Copy(this.window, 32768 - num3, output, offset, num3);
				offset += num3;
				len = num;
			}
			Array.Copy(this.window, num - len, output, offset, len);
			this.windowFilled -= num2;
			if (this.windowFilled < 0)
			{
				throw new InvalidOperationException();
			}
			return num2;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000032F4 File Offset: 0x000014F4
		public void Reset()
		{
			this.windowFilled = (this.windowEnd = 0);
		}

		// Token: 0x0400001C RID: 28
		private const int WindowSize = 32768;

		// Token: 0x0400001D RID: 29
		private const int WindowMask = 32767;

		// Token: 0x0400001E RID: 30
		private byte[] window = new byte[32768];

		// Token: 0x0400001F RID: 31
		private int windowEnd;

		// Token: 0x04000020 RID: 32
		private int windowFilled;
	}
}
