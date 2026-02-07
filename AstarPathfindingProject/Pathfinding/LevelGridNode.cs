using System;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000E4 RID: 228
	public class LevelGridNode : GridNodeBase
	{
		// Token: 0x06000764 RID: 1892 RVA: 0x00026B42 File Offset: 0x00024D42
		public LevelGridNode()
		{
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00026B4A File Offset: 0x00024D4A
		public LevelGridNode(AstarPath astar)
		{
			astar.InitializeNode(this);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x000276B0 File Offset: 0x000258B0
		public static LayerGridGraph GetGridGraph(uint graphIndex)
		{
			return LevelGridNode._gridGraphs[(int)graphIndex];
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x000276BC File Offset: 0x000258BC
		public static void SetGridGraph(int graphIndex, LayerGridGraph graph)
		{
			GridNode.SetGridGraph(graphIndex, graph);
			if (LevelGridNode._gridGraphs.Length <= graphIndex)
			{
				LayerGridGraph[] array = new LayerGridGraph[graphIndex + 1];
				for (int i = 0; i < LevelGridNode._gridGraphs.Length; i++)
				{
					array[i] = LevelGridNode._gridGraphs[i];
				}
				LevelGridNode._gridGraphs = array;
			}
			LevelGridNode._gridGraphs[graphIndex] = graph;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0002770D File Offset: 0x0002590D
		public static void ClearGridGraph(int graphIndex, LayerGridGraph graph)
		{
			if (graphIndex < LevelGridNode._gridGraphs.Length && LevelGridNode._gridGraphs[graphIndex] == graph)
			{
				LevelGridNode._gridGraphs[graphIndex] = null;
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0002772B File Offset: 0x0002592B
		public override void ResetConnectionsInternal()
		{
			this.gridConnections = uint.MaxValue;
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x00027744 File Offset: 0x00025944
		public override bool HasAnyGridConnections
		{
			get
			{
				return this.gridConnections != uint.MaxValue;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x00027754 File Offset: 0x00025954
		public override bool HasConnectionsToAllEightNeighbours
		{
			get
			{
				for (int i = 0; i < 8; i++)
				{
					if (!this.HasConnectionInDirection(i))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x00027779 File Offset: 0x00025979
		public override bool HasConnectionsToAllAxisAlignedNeighbours
		{
			get
			{
				return (this.gridConnections & 65535U) == 65535U;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x0002778E File Offset: 0x0002598E
		// (set) Token: 0x0600076E RID: 1902 RVA: 0x00027799 File Offset: 0x00025999
		public int LayerCoordinateInGrid
		{
			get
			{
				return this.nodeInGridIndex >> 24;
			}
			set
			{
				this.nodeInGridIndex = ((this.nodeInGridIndex & 16777215) | value << 24);
			}
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x000277B2 File Offset: 0x000259B2
		public void SetPosition(Int3 position)
		{
			this.position = position;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x000277BB File Offset: 0x000259BB
		public override int GetGizmoHashCode()
		{
			return base.GetGizmoHashCode() ^ (int)(805306457UL * (ulong)this.gridConnections ^ 402653189UL * (ulong)this.gridConnections);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000277E4 File Offset: 0x000259E4
		public override GridNodeBase GetNeighbourAlongDirection(int direction)
		{
			int connectionValue = this.GetConnectionValue(direction);
			if (connectionValue != 15)
			{
				LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
				return gridGraph.nodes[base.NodeInGridIndex + gridGraph.neighbourOffsets[direction] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
			}
			return null;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00027834 File Offset: 0x00025A34
		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse)
			{
				LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
				int[] neighbourOffsets = gridGraph.neighbourOffsets;
				GridNodeBase[] nodes = gridGraph.nodes;
				for (int i = 0; i < 8; i++)
				{
					int connectionValue = this.GetConnectionValue(i);
					if (connectionValue != 15)
					{
						LevelGridNode levelGridNode = nodes[base.NodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue] as LevelGridNode;
						if (levelGridNode != null)
						{
							levelGridNode.SetConnectionValue((i + 2) % 4, 15);
						}
					}
				}
			}
			this.ResetConnectionsInternal();
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x000278B8 File Offset: 0x00025AB8
		public override void GetConnections<T>(GraphNode.GetConnectionsWithData<T> action, ref T data, int connectionFilter)
		{
			if ((connectionFilter & 48) == 0)
			{
				return;
			}
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNodeBase[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			for (int i = 0; i < 8; i++)
			{
				int connectionValue = this.GetConnectionValue(i);
				if (connectionValue != 15)
				{
					GridNodeBase gridNodeBase = nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					if (gridNodeBase != null)
					{
						action(gridNodeBase, ref data);
					}
				}
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00027935 File Offset: 0x00025B35
		public override bool HasConnectionInDirection(int direction)
		{
			return (this.gridConnections >> direction * 4 & 15U) != 15U;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0002794E File Offset: 0x00025B4E
		public void SetConnectionValue(int dir, int value)
		{
			this.gridConnections = ((this.gridConnections & ~(15U << dir * 4)) | (uint)((uint)value << dir * 4));
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00027980 File Offset: 0x00025B80
		public void SetAllConnectionInternal(ulong value)
		{
			this.gridConnections = (uint)value;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0002798A File Offset: 0x00025B8A
		public int GetConnectionValue(int dir)
		{
			return (int)(this.gridConnections >> dir * 4 & 15U);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0002799C File Offset: 0x00025B9C
		public override void AddPartialConnection(GraphNode node, uint cost, bool isOutgoing, bool isIncoming)
		{
			LevelGridNode levelGridNode = node as LevelGridNode;
			if (levelGridNode != null && levelGridNode.GraphIndex == base.GraphIndex)
			{
				this.RemoveGridConnection(levelGridNode);
			}
			base.AddPartialConnection(node, cost, isOutgoing, isIncoming);
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x000279D4 File Offset: 0x00025BD4
		public override void RemovePartialConnection(GraphNode node)
		{
			base.RemovePartialConnection(node);
			LevelGridNode levelGridNode = node as LevelGridNode;
			if (levelGridNode != null && levelGridNode.GraphIndex == base.GraphIndex)
			{
				this.RemoveGridConnection(levelGridNode);
			}
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00027A08 File Offset: 0x00025C08
		protected void RemoveGridConnection(LevelGridNode node)
		{
			int nodeInGridIndex = base.NodeInGridIndex;
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			for (int i = 0; i < 8; i++)
			{
				if (nodeInGridIndex + gridGraph.neighbourOffsets[i] == node.NodeInGridIndex && this.GetNeighbourAlongDirection(i) == node)
				{
					this.SetConnectionValue(i, 15);
					return;
				}
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00027A5C File Offset: 0x00025C5C
		public override bool GetPortal(GraphNode other, out Vector3 left, out Vector3 right)
		{
			if (other.GraphIndex != base.GraphIndex)
			{
				left = (right = Vector3.zero);
				return false;
			}
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			Vector2Int vector2Int = (other as GridNodeBase).CoordinatesInGrid - base.CoordinatesInGrid;
			int num = GridNodeBase.OffsetToConnectionDirection(vector2Int.x, vector2Int.y);
			if (num == -1 || this.GetNeighbourAlongDirection(num) != other)
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
				GridNodeBase neighbourAlongDirection = this.GetNeighbourAlongDirection(num - 4);
				if (neighbourAlongDirection != null && neighbourAlongDirection.Walkable && neighbourAlongDirection.GetNeighbourAlongDirection((num - 4 + 1) % 4) == other)
				{
					flag = true;
				}
				neighbourAlongDirection = this.GetNeighbourAlongDirection((num - 4 + 1) % 4);
				if (neighbourAlongDirection != null && neighbourAlongDirection.Walkable && neighbourAlongDirection.GetNeighbourAlongDirection(num - 4) == other)
				{
					flag2 = true;
				}
				Vector3 a2 = (Vector3)(this.position + other.position) * 0.5f;
				Vector3 vector = gridGraph.transform.TransformVector(new Vector3((float)vector2Int.y, 0f, (float)(-(float)vector2Int.x)));
				left = a2 - (flag2 ? vector : Vector3.zero);
				right = a2 + (flag ? vector : Vector3.zero);
			}
			return true;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00027C40 File Offset: 0x00025E40
		public override void Open(Path path, uint pathNodeIndex, uint gScore)
		{
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			uint[] neighbourCosts = gridGraph.neighbourCosts;
			GridNodeBase[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			int num = 255;
			for (int i = 0; i < 8; i++)
			{
				if (i == 4 && (path.traversalProvider == null || path.traversalProvider.filterDiagonalGridConnections))
				{
					num = GridNode.FilterDiagonalConnections(num, gridGraph.neighbours, gridGraph.cutCorners);
				}
				int connectionValue = this.GetConnectionValue(i);
				if (connectionValue != 15 && (num >> i & 1) != 0)
				{
					GraphNode graphNode = nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					if (!path.CanTraverse(this, graphNode))
					{
						num &= ~(1 << i);
					}
					else
					{
						path.OpenCandidateConnection(pathNodeIndex, graphNode.NodeIndex, gScore, neighbourCosts[i], 0U, graphNode.position);
					}
				}
				else
				{
					num &= ~(1 << i);
				}
			}
			base.Open(path, pathNodeIndex, gScore);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00027D48 File Offset: 0x00025F48
		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(this.position);
			ctx.writer.Write(this.gridFlags);
			ulong num = 0UL;
			for (int i = 0; i < 8; i++)
			{
				num |= (ulong)((ulong)((long)this.GetConnectionValue(i)) << i * 8);
			}
			ctx.writer.Write(num);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00027DA8 File Offset: 0x00025FA8
		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.position = ctx.DeserializeInt3();
			this.gridFlags = ctx.reader.ReadUInt16();
			if (ctx.meta.version < AstarSerializer.V4_3_12)
			{
				ulong num;
				if (ctx.meta.version < AstarSerializer.V3_9_0)
				{
					num = ((ulong)ctx.reader.ReadUInt32() | 1085102592318504960UL);
				}
				else
				{
					num = ctx.reader.ReadUInt64();
				}
				this.gridConnections = 0U;
				for (int i = 0; i < 8; i++)
				{
					ulong num2 = num >> i * 8 & 255UL;
					if ((num2 & 15UL) != num2)
					{
						num2 = 15UL;
					}
					this.SetConnectionValue(i, (int)num2);
				}
				return;
			}
			ulong num3 = ctx.reader.ReadUInt64();
			uint num4 = 0U;
			if (ctx.meta.version < AstarSerializer.V4_3_83)
			{
				num4 = (uint)num3;
			}
			else
			{
				for (int j = 0; j < 8; j++)
				{
					num4 |= ((uint)(num3 >> j * 8) & 15U) << 4 * j;
				}
			}
			this.gridConnections = num4;
		}

		// Token: 0x040004D7 RID: 1239
		private static LayerGridGraph[] _gridGraphs = new LayerGridGraph[0];

		// Token: 0x040004D8 RID: 1240
		public uint gridConnections;

		// Token: 0x040004D9 RID: 1241
		protected static LayerGridGraph[] gridGraphs;

		// Token: 0x040004DA RID: 1242
		private const int MaxNeighbours = 8;

		// Token: 0x040004DB RID: 1243
		public const int ConnectionMask = 15;

		// Token: 0x040004DC RID: 1244
		public const int ConnectionStride = 4;

		// Token: 0x040004DD RID: 1245
		public const int AxisAlignedConnectionsMask = 65535;

		// Token: 0x040004DE RID: 1246
		public const uint AllConnectionsMask = 4294967295U;

		// Token: 0x040004DF RID: 1247
		public const int NoConnection = 15;

		// Token: 0x040004E0 RID: 1248
		internal const ulong DiagonalConnectionsMask = 4294901760UL;

		// Token: 0x040004E1 RID: 1249
		public const int MaxLayerCount = 15;
	}
}
