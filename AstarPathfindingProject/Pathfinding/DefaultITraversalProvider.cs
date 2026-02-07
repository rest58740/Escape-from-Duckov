using System;

namespace Pathfinding
{
	// Token: 0x020000A8 RID: 168
	public static class DefaultITraversalProvider
	{
		// Token: 0x06000528 RID: 1320 RVA: 0x00019DE8 File Offset: 0x00017FE8
		public static bool CanTraverse(Path path, GraphNode node)
		{
			return node.Walkable && (path == null || (path.enabledTags >> (int)node.Tag & 1) != 0);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00019E0E File Offset: 0x0001800E
		public static uint GetTraversalCost(Path path, GraphNode node)
		{
			return node.Penalty + ((path != null) ? path.GetTagPenalty((int)node.Tag) : 0U);
		}
	}
}
