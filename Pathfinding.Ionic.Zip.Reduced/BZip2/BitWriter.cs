using System;
using System.IO;

namespace Pathfinding.Ionic.BZip2
{
	// Token: 0x02000044 RID: 68
	internal class BitWriter
	{
		// Token: 0x06000347 RID: 839 RVA: 0x00014C64 File Offset: 0x00012E64
		public BitWriter(Stream s)
		{
			this.output = s;
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00014C74 File Offset: 0x00012E74
		public byte RemainingBits
		{
			get
			{
				return (byte)(this.accumulator >> 32 - this.nAccumulatedBits & 255U);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000349 RID: 841 RVA: 0x00014C90 File Offset: 0x00012E90
		public int NumRemainingBits
		{
			get
			{
				return this.nAccumulatedBits;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00014C98 File Offset: 0x00012E98
		public int TotalBytesWrittenOut
		{
			get
			{
				return this.totalBytesWrittenOut;
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00014CA0 File Offset: 0x00012EA0
		public void Reset()
		{
			this.accumulator = 0U;
			this.nAccumulatedBits = 0;
			this.totalBytesWrittenOut = 0;
			this.output.Seek(0L, 0);
			this.output.SetLength(0L);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00014CD4 File Offset: 0x00012ED4
		public void WriteBits(int nbits, uint value)
		{
			int i = this.nAccumulatedBits;
			uint num = this.accumulator;
			while (i >= 8)
			{
				this.output.WriteByte((byte)(num >> 24 & 255U));
				this.totalBytesWrittenOut++;
				num <<= 8;
				i -= 8;
			}
			this.accumulator = (num | value << 32 - i - nbits);
			this.nAccumulatedBits = i + nbits;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00014D44 File Offset: 0x00012F44
		public void WriteByte(byte b)
		{
			this.WriteBits(8, (uint)b);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00014D50 File Offset: 0x00012F50
		public void WriteInt(uint u)
		{
			this.WriteBits(8, u >> 24 & 255U);
			this.WriteBits(8, u >> 16 & 255U);
			this.WriteBits(8, u >> 8 & 255U);
			this.WriteBits(8, u & 255U);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00014DA0 File Offset: 0x00012FA0
		public void Flush()
		{
			this.WriteBits(0, 0U);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00014DAC File Offset: 0x00012FAC
		public void FinishAndPad()
		{
			this.Flush();
			if (this.NumRemainingBits > 0)
			{
				byte b = (byte)(this.accumulator >> 24 & 255U);
				this.output.WriteByte(b);
				this.totalBytesWrittenOut++;
			}
		}

		// Token: 0x040001F1 RID: 497
		private uint accumulator;

		// Token: 0x040001F2 RID: 498
		private int nAccumulatedBits;

		// Token: 0x040001F3 RID: 499
		private Stream output;

		// Token: 0x040001F4 RID: 500
		private int totalBytesWrittenOut;
	}
}
