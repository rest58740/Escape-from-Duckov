using System;

namespace Pathfinding
{
	// Token: 0x020000CC RID: 204
	public class LinkNode : PointNode
	{
		// Token: 0x06000673 RID: 1651 RVA: 0x00022594 File Offset: 0x00020794
		public LinkNode()
		{
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0002259C File Offset: 0x0002079C
		public LinkNode(AstarPath active) : base(active)
		{
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x000225A5 File Offset: 0x000207A5
		public override void RemovePartialConnection(GraphNode node)
		{
			this.linkConcrete.staleConnections = true;
			AstarPath.active.offMeshLinks.DirtyNoSchedule(this.linkSource);
			base.RemovePartialConnection(node);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x000225D0 File Offset: 0x000207D0
		public unsafe override void Open(Path path, uint pathNodeIndex, uint gScore)
		{
			if (this.connections == null)
			{
				return;
			}
			PathHandler pathHandler = ((IPathInternals)path).PathHandler;
			PathNode pathNode = *pathHandler.pathNodes[pathNodeIndex];
			bool flag = !pathHandler.IsTemporaryNode(pathNode.parentIndex) && pathHandler.GetNode(pathNode.parentIndex).GraphIndex == base.GraphIndex;
			for (int i = 0; i < this.connections.Length; i++)
			{
				GraphNode node = this.connections[i].node;
				if (flag == (node.GraphIndex != base.GraphIndex) && path.CanTraverse(this, node))
				{
					if (node is PointNode)
					{
						path.OpenCandidateConnection(pathNodeIndex, node.NodeIndex, gScore, this.connections[i].cost, 0U, node.position);
					}
					else
					{
						node.OpenAtPoint(path, pathNodeIndex, this.position, gScore);
					}
				}
			}
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x000226B8 File Offset: 0x000208B8
		public override void OpenAtPoint(Path path, uint pathNodeIndex, Int3 pos, uint gScore)
		{
			if (path.CanTraverse(this))
			{
				uint costMagnitude = (uint)(pos - this.position).costMagnitude;
				path.OpenCandidateConnection(pathNodeIndex, base.NodeIndex, gScore, costMagnitude, 0U, this.position);
			}
		}

		// Token: 0x04000468 RID: 1128
		public OffMeshLinks.OffMeshLinkSource linkSource;

		// Token: 0x04000469 RID: 1129
		public OffMeshLinks.OffMeshLinkConcrete linkConcrete;

		// Token: 0x0400046A RID: 1130
		public int nodeInGraphIndex;
	}
}
