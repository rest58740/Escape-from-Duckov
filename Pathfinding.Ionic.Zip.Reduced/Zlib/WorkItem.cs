using System;

namespace Pathfinding.Ionic.Zlib
{
	// Token: 0x02000057 RID: 87
	internal class WorkItem
	{
		// Token: 0x060003FF RID: 1023 RVA: 0x0001C630 File Offset: 0x0001A830
		public WorkItem(int size, CompressionLevel compressLevel, CompressionStrategy strategy, int ix)
		{
			this.buffer = new byte[size];
			int num = size + (size / 32768 + 1) * 5 * 2;
			this.compressed = new byte[num];
			this.compressor = new ZlibCodec();
			this.compressor.InitializeDeflate(compressLevel, false);
			this.compressor.OutputBuffer = this.compressed;
			this.compressor.InputBuffer = this.buffer;
			this.index = ix;
		}

		// Token: 0x040002ED RID: 749
		public byte[] buffer;

		// Token: 0x040002EE RID: 750
		public byte[] compressed;

		// Token: 0x040002EF RID: 751
		public int crc;

		// Token: 0x040002F0 RID: 752
		public int index;

		// Token: 0x040002F1 RID: 753
		public int ordinal;

		// Token: 0x040002F2 RID: 754
		public int inputBytesAvailable;

		// Token: 0x040002F3 RID: 755
		public int compressedBytesAvailable;

		// Token: 0x040002F4 RID: 756
		public ZlibCodec compressor;
	}
}
