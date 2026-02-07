using System;

namespace Pathfinding
{
	// Token: 0x020000B2 RID: 178
	public struct TemporaryNode
	{
		// Token: 0x040003B7 RID: 951
		public uint associatedNode;

		// Token: 0x040003B8 RID: 952
		public Int3 position;

		// Token: 0x040003B9 RID: 953
		public int targetIndex;

		// Token: 0x040003BA RID: 954
		public TemporaryNodeType type;
	}
}
