using System;

namespace ICSharpCode.SharpZipLib.Checksums
{
	// Token: 0x02000039 RID: 57
	public sealed class Adler32 : IChecksum
	{
		// Token: 0x06000247 RID: 583 RVA: 0x0000E958 File Offset: 0x0000CB58
		public Adler32()
		{
			this.Reset();
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000E968 File Offset: 0x0000CB68
		public long Value
		{
			get
			{
				return (long)((ulong)this.checksum);
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000E974 File Offset: 0x0000CB74
		public void Reset()
		{
			this.checksum = 1U;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000E980 File Offset: 0x0000CB80
		public void Update(int value)
		{
			uint num = this.checksum & 65535U;
			uint num2 = this.checksum >> 16;
			num = (num + (uint)(value & 255)) % 65521U;
			num2 = (num + num2) % 65521U;
			this.checksum = (num2 << 16) + num;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000E9CC File Offset: 0x0000CBCC
		public void Update(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.Update(buffer, 0, buffer.Length);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000E9EC File Offset: 0x0000CBEC
		public void Update(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "cannot be negative");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "cannot be negative");
			}
			if (offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset", "not a valid index into buffer");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count", "exceeds buffer size");
			}
			uint num = this.checksum & 65535U;
			uint num2 = this.checksum >> 16;
			while (count > 0)
			{
				int num3 = 3800;
				if (num3 > count)
				{
					num3 = count;
				}
				count -= num3;
				while (--num3 >= 0)
				{
					num += (uint)(buffer[offset++] & byte.MaxValue);
					num2 += num;
				}
				num %= 65521U;
				num2 %= 65521U;
			}
			this.checksum = (num2 << 16 | num);
		}

		// Token: 0x040001C7 RID: 455
		private const uint BASE = 65521U;

		// Token: 0x040001C8 RID: 456
		private uint checksum;
	}
}
