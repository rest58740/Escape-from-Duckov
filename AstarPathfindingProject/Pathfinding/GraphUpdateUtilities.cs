using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Pathfinding.Util;

namespace Pathfinding
{
	// Token: 0x0200014D RID: 333
	public static class GraphUpdateUtilities
	{
		// Token: 0x06000A06 RID: 2566 RVA: 0x00036E70 File Offset: 0x00035070
		public static bool UpdateGraphsNoBlock(GraphUpdateObject guo, GraphNode node1, GraphNode node2, bool alwaysRevert = false)
		{
			List<GraphNode> list = ListPool<GraphNode>.Claim();
			list.Add(node1);
			list.Add(node2);
			bool result = GraphUpdateUtilities.UpdateGraphsNoBlock(guo, list, alwaysRevert);
			ListPool<GraphNode>.Release(ref list);
			return result;
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00036EA0 File Offset: 0x000350A0
		public static bool UpdateGraphsNoBlock(GraphUpdateObject guo, List<GraphNode> nodes, bool alwaysRevert = false)
		{
			PathProcessor.GraphUpdateLock graphUpdateLock = AstarPath.active.PausePathfinding();
			bool flag;
			try
			{
				AstarPath.active.FlushGraphUpdates();
				for (int i = 0; i < nodes.Count; i++)
				{
					if (!nodes[i].Walkable)
					{
						return false;
					}
				}
				GraphSnapshot graphSnapshot = AstarPath.active.Snapshot(guo.bounds, guo.nnConstraint.graphMask);
				AstarPath.active.UpdateGraphs(guo);
				AstarPath.active.FlushGraphUpdates();
				flag = PathUtilities.IsPathPossible(nodes);
				if (!flag || alwaysRevert)
				{
					AstarPath.active.AddWorkItem(new Action<IWorkItemContext>(graphSnapshot.Restore));
					AstarPath.active.FlushWorkItems();
				}
				graphSnapshot.Dispose();
			}
			finally
			{
				graphUpdateLock.Release();
			}
			return flag;
		}
	}
}
