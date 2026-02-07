using System;

namespace Pathfinding.Util
{
	// Token: 0x02000283 RID: 643
	public struct PathPartWithLinkInfo
	{
		// Token: 0x06000F52 RID: 3922 RVA: 0x0005EDCD File Offset: 0x0005CFCD
		public PathPartWithLinkInfo(int startIndex, int endIndex, OffMeshLinks.OffMeshLinkTracer linkInfo = default(OffMeshLinks.OffMeshLinkTracer))
		{
			this.startIndex = startIndex;
			this.endIndex = endIndex;
			this.linkInfo = linkInfo;
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000F53 RID: 3923 RVA: 0x0005EDE4 File Offset: 0x0005CFE4
		public Funnel.PartType type
		{
			get
			{
				if (this.linkInfo.link == null)
				{
					return Funnel.PartType.NodeSequence;
				}
				return Funnel.PartType.OffMeshLink;
			}
		}

		// Token: 0x04000B5C RID: 2908
		public int startIndex;

		// Token: 0x04000B5D RID: 2909
		public int endIndex;

		// Token: 0x04000B5E RID: 2910
		public OffMeshLinks.OffMeshLinkTracer linkInfo;
	}
}
