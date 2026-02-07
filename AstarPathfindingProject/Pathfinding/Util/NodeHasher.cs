using System;
using Pathfinding.Drawing;

namespace Pathfinding.Util
{
	// Token: 0x02000293 RID: 659
	public struct NodeHasher
	{
		// Token: 0x06000FC8 RID: 4040 RVA: 0x00060A4C File Offset: 0x0005EC4C
		public NodeHasher(AstarPath active)
		{
			this.hasher = default(DrawingData.Hasher);
			this.debugData = active.debugPathData;
			this.includePathSearchInfo = (this.debugData != null && (active.debugMode == GraphDebugMode.F || active.debugMode == GraphDebugMode.G || active.debugMode == GraphDebugMode.H || active.showSearchTree));
			this.includeAreaInfo = (active.debugMode == GraphDebugMode.Areas);
			this.includeHierarchicalNodeInfo = (active.debugMode == GraphDebugMode.HierarchicalNode);
			this.hasher.Add<GraphDebugMode>(active.debugMode);
			this.hasher.Add<float>(active.debugFloor);
			this.hasher.Add<float>(active.debugRoof);
			this.hasher.Add<bool>(active.showSearchTree);
			this.hasher.Add<int>(AstarColor.ColorHash());
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00060B18 File Offset: 0x0005ED18
		public unsafe void HashNode(GraphNode node)
		{
			this.hasher.Add<int>(node.GetGizmoHashCode());
			if (this.includeAreaInfo)
			{
				this.hasher.Add<int>((int)node.Area);
			}
			if (this.includeHierarchicalNodeInfo)
			{
				this.hasher.Add<int>(node.HierarchicalNodeIndex);
			}
			if (this.includePathSearchInfo)
			{
				PathNode pathNode = *this.debugData.pathNodes[node.NodeIndex];
				this.hasher.Add<ushort>(pathNode.pathID);
				this.hasher.Add<bool>(pathNode.pathID == this.debugData.PathID);
			}
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00060BBB File Offset: 0x0005EDBB
		public void Add<T>(T v)
		{
			this.hasher.Add<T>(v);
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00060BC9 File Offset: 0x0005EDC9
		public static implicit operator DrawingData.Hasher(NodeHasher hasher)
		{
			return hasher.hasher;
		}

		// Token: 0x04000B93 RID: 2963
		private readonly bool includePathSearchInfo;

		// Token: 0x04000B94 RID: 2964
		private readonly bool includeAreaInfo;

		// Token: 0x04000B95 RID: 2965
		private readonly bool includeHierarchicalNodeInfo;

		// Token: 0x04000B96 RID: 2966
		private readonly PathHandler debugData;

		// Token: 0x04000B97 RID: 2967
		public DrawingData.Hasher hasher;
	}
}
