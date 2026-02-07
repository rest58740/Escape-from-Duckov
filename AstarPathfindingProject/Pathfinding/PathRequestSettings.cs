using System;

namespace Pathfinding
{
	// Token: 0x0200008D RID: 141
	[Serializable]
	public struct PathRequestSettings
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x00017290 File Offset: 0x00015490
		public static PathRequestSettings Default
		{
			get
			{
				return new PathRequestSettings
				{
					graphMask = GraphMask.everything,
					tagPenalties = new int[32],
					traversableTags = -1,
					traversalProvider = null
				};
			}
		}

		// Token: 0x040002FF RID: 767
		public GraphMask graphMask;

		// Token: 0x04000300 RID: 768
		public int[] tagPenalties;

		// Token: 0x04000301 RID: 769
		public int traversableTags;

		// Token: 0x04000302 RID: 770
		public ITraversalProvider traversalProvider;
	}
}
