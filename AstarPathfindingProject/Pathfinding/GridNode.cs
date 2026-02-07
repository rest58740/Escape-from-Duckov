using System;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000E2 RID: 226
	public class GridNode : GridNodeBase
	{
		// Token: 0x06000724 RID: 1828 RVA: 0x00026B42 File Offset: 0x00024D42
		public GridNode()
		{
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00026B4A File Offset: 0x00024D4A
		public GridNode(AstarPath astar)
		{
			astar.InitializeNode(this);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00026B59 File Offset: 0x00024D59
		public static GridGraph GetGridGraph(uint graphIndex)
		{
			return GridNode._gridGraphs[(int)graphIndex];
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00026B64 File Offset: 0x00024D64
		public static void SetGridGraph(int graphIndex, GridGraph graph)
		{
			if (GridNode._gridGraphs.Length <= graphIndex)
			{
				GridGraph[] array = new GridGraph[graphIndex + 1];
				for (int i = 0; i < GridNode._gridGraphs.Length; i++)
				{
					array[i] = GridNode._gridGraphs[i];
				}
				GridNode._gridGraphs = array;
			}
			GridNode._gridGraphs[graphIndex] = graph;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00026BAE File Offset: 0x00024DAE
		public static void ClearGridGraph(int graphIndex, GridGraph graph)
		{
			if (graphIndex < GridNode._gridGraphs.Length && GridNode._gridGraphs[graphIndex] == graph)
			{
				GridNode._gridGraphs[graphIndex] = null;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x00026BCC File Offset: 0x00024DCC
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x00026BD4 File Offset: 0x00024DD4
		internal ushort InternalGridFlags
		{
			get
			{
				return this.gridFlags;
			}
			set
			{
				this.gridFlags = value;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x00026BDD File Offset: 0x00024DDD
		public override bool HasConnectionsToAllEightNeighbours
		{
			get
			{
				return (this.InternalGridFlags & 255) == 255;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x00026BF2 File Offset: 0x00024DF2
		public override bool HasConnectionsToAllAxisAlignedNeighbours
		{
			get
			{
				return (this.InternalGridFlags & 15) == 15;
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00026C01 File Offset: 0x00024E01
		public override bool HasConnectionInDirection(int dir)
		{
			return (this.gridFlags >> dir & 1) != 0;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00026C13 File Offset: 0x00024E13
		public void SetConnection(int dir, bool value)
		{
			this.SetConnectionInternal(dir, value);
			GridNode.GetGridGraph(base.GraphIndex).nodeDataRef.connections[base.NodeInGridIndex] = (ulong)((long)this.GetAllConnectionInternal());
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00026C44 File Offset: 0x00024E44
		public void SetConnectionInternal(int dir, bool value)
		{
			this.gridFlags = (ushort)(((int)this.gridFlags & ~(1 << dir)) | (value ? 1 : 0) << (dir & 31));
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00026C78 File Offset: 0x00024E78
		public void SetAllConnectionInternal(int connections)
		{
			this.gridFlags = (ushort)(((int)this.gridFlags & -256) | connections);
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00026C9F File Offset: 0x00024E9F
		public int GetAllConnectionInternal()
		{
			return (int)(this.gridFlags & 255);
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x00026CAD File Offset: 0x00024EAD
		public override bool HasAnyGridConnections
		{
			get
			{
				return this.GetAllConnectionInternal() != 0;
			}
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00026CB8 File Offset: 0x00024EB8
		public override void ResetConnectionsInternal()
		{
			this.gridFlags = (ushort)((int)this.gridFlags & -256);
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x00026CDD File Offset: 0x00024EDD
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x00026CEE File Offset: 0x00024EEE
		public bool EdgeNode
		{
			get
			{
				return (this.gridFlags & 1024) > 0;
			}
			set
			{
				this.gridFlags = (ushort)(((int)this.gridFlags & -1025) | (value ? 1024 : 0));
			}
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00026D10 File Offset: 0x00024F10
		public override GridNodeBase GetNeighbourAlongDirection(int direction)
		{
			if (this.HasConnectionInDirection(direction))
			{
				GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
				return gridGraph.nodes[base.NodeInGridIndex + gridGraph.neighbourOffsets[direction]];
			}
			return null;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00026D4C File Offset: 0x00024F4C
		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse)
			{
				for (int i = 0; i < 8; i++)
				{
					GridNode gridNode = this.GetNeighbourAlongDirection(i) as GridNode;
					if (gridNode != null)
					{
						gridNode.SetConnectionInternal(GridNodeBase.OppositeConnectionDirection(i), false);
					}
				}
			}
			this.ResetConnectionsInternal();
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00026D8C File Offset: 0x00024F8C
		public override void GetConnections<T>(GraphNode.GetConnectionsWithData<T> action, ref T data, int connectionFilter)
		{
			if ((connectionFilter & 48) == 0)
			{
				return;
			}
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNodeBase[] nodes = gridGraph.nodes;
			for (int i = 0; i < 8; i++)
			{
				if ((this.gridFlags >> i & 1) != 0)
				{
					GridNodeBase gridNodeBase = nodes[base.NodeInGridIndex + neighbourOffsets[i]];
					if (gridNodeBase != null)
					{
						action(gridNodeBase, ref data);
					}
				}
			}
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00026DEC File Offset: 0x00024FEC
		public override bool GetPortal(GraphNode other, out Vector3 left, out Vector3 right)
		{
			if (other.GraphIndex != base.GraphIndex)
			{
				left = (right = Vector3.zero);
				return false;
			}
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			Vector2Int vector2Int = (other as GridNodeBase).CoordinatesInGrid - base.CoordinatesInGrid;
			int num = GridNodeBase.OffsetToConnectionDirection(vector2Int.x, vector2Int.y);
			if (num == -1 || !this.HasConnectionInDirection(num))
			{
				left = (right = Vector3.zero);
				return false;
			}
			if (num < 4)
			{
				Vector3 a = (Vector3)(this.position + other.position) * 0.5f;
				Vector3 b = gridGraph.transform.TransformVector(new Vector3((float)vector2Int.y, 0f, (float)(-(float)vector2Int.x)) * 0.5f);
				left = a - b;
				right = a + b;
			}
			else
			{
				bool flag = false;
				bool flag2 = false;
				if (this.HasConnectionInDirection(num - 4))
				{
					GridNodeBase gridNodeBase = gridGraph.nodes[base.NodeInGridIndex + gridGraph.neighbourOffsets[num - 4]];
					if (gridNodeBase.Walkable && gridNodeBase.HasConnectionInDirection((num - 4 + 1) % 4))
					{
						flag = true;
					}
				}
				if (this.HasConnectionInDirection((num - 4 + 1) % 4))
				{
					GridNodeBase gridNodeBase2 = gridGraph.nodes[base.NodeInGridIndex + gridGraph.neighbourOffsets[(num - 4 + 1) % 4]];
					if (gridNodeBase2.Walkable && gridNodeBase2.HasConnectionInDirection(num - 4))
					{
						flag2 = true;
					}
				}
				Vector3 a2 = (Vector3)(this.position + other.position) * 0.5f;
				Vector3 vector = gridGraph.transform.TransformVector(new Vector3((float)vector2Int.y, 0f, (float)(-(float)vector2Int.x)));
				left = a2 - (flag2 ? vector : Vector3.zero);
				right = a2 + (flag ? vector : Vector3.zero);
			}
			return true;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00027000 File Offset: 0x00025200
		public static int FilterDiagonalConnections(int conns, NumNeighbours neighbours, bool cutCorners)
		{
			switch (neighbours)
			{
			case NumNeighbours.Four:
				return conns & 15;
			case NumNeighbours.Six:
				return conns & 175;
			}
			if (cutCorners)
			{
				int num = conns & 15;
				int num2 = (num | (num >> 1 | num << 3)) << 4;
				num2 &= conns;
				return num | num2;
			}
			int num3 = conns & 15;
			int num4 = (num3 & (num3 >> 1 | num3 << 3)) << 4;
			num4 &= conns;
			return num3 | num4;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00027064 File Offset: 0x00025264
		public override void Open(Path path, uint pathNodeIndex, uint gScore)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			uint[] neighbourCosts = gridGraph.neighbourCosts;
			GridNodeBase[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			int num = (int)(this.gridFlags & 255);
			for (int i = 0; i < 8; i++)
			{
				if (i == 4 && (path.traversalProvider == null || path.traversalProvider.filterDiagonalGridConnections))
				{
					num = GridNode.FilterDiagonalConnections(num, gridGraph.neighbours, gridGraph.cutCorners);
				}
				if ((num >> i & 1) != 0)
				{
					GridNodeBase gridNodeBase = nodes[nodeInGridIndex + neighbourOffsets[i]];
					if (path.CanTraverse(this, gridNodeBase))
					{
						path.OpenCandidateConnection(pathNodeIndex, gridNodeBase.NodeIndex, gScore, neighbourCosts[i], 0U, gridNodeBase.position);
					}
					else
					{
						num &= ~(1 << i);
					}
				}
			}
			base.Open(path, pathNodeIndex, gScore);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00027142 File Offset: 0x00025342
		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(this.position);
			ctx.writer.Write(this.gridFlags);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00027168 File Offset: 0x00025368
		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.position = ctx.DeserializeInt3();
			this.gridFlags = ctx.reader.ReadUInt16();
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00027190 File Offset: 0x00025390
		public override void AddPartialConnection(GraphNode node, uint cost, bool isOutgoing, bool isIncoming)
		{
			GridNode gridNode = node as GridNode;
			if (gridNode != null && gridNode.GraphIndex == base.GraphIndex)
			{
				this.RemoveGridConnection(gridNode);
			}
			base.AddPartialConnection(node, cost, isOutgoing, isIncoming);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000271C8 File Offset: 0x000253C8
		public override void RemovePartialConnection(GraphNode node)
		{
			base.RemovePartialConnection(node);
			GridNode gridNode = node as GridNode;
			if (gridNode != null && gridNode.GraphIndex == base.GraphIndex)
			{
				this.RemoveGridConnection(gridNode);
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x000271FC File Offset: 0x000253FC
		protected void RemoveGridConnection(GridNode node)
		{
			int nodeInGridIndex = base.NodeInGridIndex;
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			for (int i = 0; i < 8; i++)
			{
				if (nodeInGridIndex + gridGraph.neighbourOffsets[i] == node.NodeInGridIndex && this.GetNeighbourAlongDirection(i) == node)
				{
					this.SetConnectionInternal(i, false);
					return;
				}
			}
		}

		// Token: 0x040004C7 RID: 1223
		private static GridGraph[] _gridGraphs = new GridGraph[0];

		// Token: 0x040004C8 RID: 1224
		private const int GridFlagsConnectionOffset = 0;

		// Token: 0x040004C9 RID: 1225
		private const int GridFlagsConnectionBit0 = 1;

		// Token: 0x040004CA RID: 1226
		private const int GridFlagsConnectionMask = 255;

		// Token: 0x040004CB RID: 1227
		private const int GridFlagsAxisAlignedConnectionMask = 15;

		// Token: 0x040004CC RID: 1228
		private const int GridFlagsEdgeNodeOffset = 10;

		// Token: 0x040004CD RID: 1229
		private const int GridFlagsEdgeNodeMask = 1024;
	}
}
