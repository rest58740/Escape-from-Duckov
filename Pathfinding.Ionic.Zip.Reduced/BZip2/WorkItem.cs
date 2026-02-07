using System;
using System.IO;

namespace Pathfinding.Ionic.BZip2
{
	// Token: 0x02000045 RID: 69
	internal class WorkItem
	{
		// Token: 0x06000351 RID: 849 RVA: 0x00014DF8 File Offset: 0x00012FF8
		public WorkItem(int ix, int blockSize)
		{
			this.ms = new MemoryStream();
			this.bw = new BitWriter(this.ms);
			this.Compressor = new BZip2Compressor(this.bw, blockSize);
			this.index = ix;
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00014E38 File Offset: 0x00013038
		// (set) Token: 0x06000353 RID: 851 RVA: 0x00014E40 File Offset: 0x00013040
		public BZip2Compressor Compressor { get; private set; }

		// Token: 0x040001F5 RID: 501
		public int index;

		// Token: 0x040001F6 RID: 502
		public MemoryStream ms;

		// Token: 0x040001F7 RID: 503
		public int ordinal;

		// Token: 0x040001F8 RID: 504
		public BitWriter bw;
	}
}
