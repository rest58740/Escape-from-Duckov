using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000E3 RID: 227
	public abstract class GridNodeBase : GraphNode
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0002725A File Offset: 0x0002545A
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x00027268 File Offset: 0x00025468
		public int NodeInGridIndex
		{
			get
			{
				return this.nodeInGridIndex & 16777215;
			}
			set
			{
				this.nodeInGridIndex = ((this.nodeInGridIndex & -16777216) | value);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0002727E File Offset: 0x0002547E
		public int XCoordinateInGrid
		{
			get
			{
				return this.NodeInGridIndex % GridNode.GetGridGraph(base.GraphIndex).width;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x00027297 File Offset: 0x00025497
		public int ZCoordinateInGrid
		{
			get
			{
				return this.NodeInGridIndex / GridNode.GetGridGraph(base.GraphIndex).width;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x000272B0 File Offset: 0x000254B0
		public Vector2Int CoordinatesInGrid
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				int width = GridNode.GetGridGraph(base.GraphIndex).width;
				int num = this.NodeInGridIndex;
				int num2 = num / width;
				return new Vector2Int(num - num2 * width, num2);
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x000272E2 File Offset: 0x000254E2
		// (set) Token: 0x06000748 RID: 1864 RVA: 0x000272F3 File Offset: 0x000254F3
		public bool WalkableErosion
		{
			get
			{
				return (this.gridFlags & 256) > 0;
			}
			set
			{
				this.gridFlags = (ushort)(((int)this.gridFlags & -257) | (value ? 256 : 0));
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x00027314 File Offset: 0x00025514
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x00027325 File Offset: 0x00025525
		public bool TmpWalkable
		{
			get
			{
				return (this.gridFlags & 512) > 0;
			}
			set
			{
				this.gridFlags = (ushort)(((int)this.gridFlags & -513) | (value ? 512 : 0));
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600074B RID: 1867
		public abstract bool HasConnectionsToAllEightNeighbours { get; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600074C RID: 1868
		public abstract bool HasConnectionsToAllAxisAlignedNeighbours { get; }

		// Token: 0x0600074D RID: 1869 RVA: 0x00027346 File Offset: 0x00025546
		public static int OppositeConnectionDirection(int dir)
		{
			if (dir >= 4)
			{
				return (dir - 2) % 4 + 4;
			}
			return (dir + 2) % 4;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00027359 File Offset: 0x00025559
		public static int OffsetToConnectionDirection(int dx, int dz)
		{
			dx++;
			dz++;
			if (dx > 2 || dz > 2)
			{
				return -1;
			}
			return GridNodeBase.offsetToDirection[3 * dz + dx];
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0002737C File Offset: 0x0002557C
		public Vector3 ProjectOnSurface(Vector3 point)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			Vector3 vector = (Vector3)this.position;
			Vector3 vector2 = gridGraph.transform.WorldUpAtGraphPosition(vector);
			return point - vector2 * Vector3.Dot(vector2, point - vector);
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x000273C8 File Offset: 0x000255C8
		public override Vector3 ClosestPointOnNode(Vector3 p)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			Vector3 vector = (Vector3)this.position;
			Vector3 vector2 = gridGraph.transform.InverseTransformVector(p - vector);
			vector2.y = 0f;
			vector2.x = Mathf.Clamp(vector2.x, -0.5f, 0.5f);
			vector2.z = Mathf.Clamp(vector2.z, -0.5f, 0.5f);
			return vector + gridGraph.transform.TransformVector(vector2);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00027458 File Offset: 0x00025658
		public override bool ContainsPoint(Vector3 point)
		{
			GridGraph gridGraph = base.Graph as GridGraph;
			return this.ContainsPointInGraphSpace((Int3)gridGraph.transform.InverseTransform(point));
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00027488 File Offset: 0x00025688
		public override bool ContainsPointInGraphSpace(Int3 point)
		{
			int num = this.XCoordinateInGrid * 1000;
			int num2 = this.ZCoordinateInGrid * 1000;
			return point.x >= num && point.x <= num + 1000 && point.z >= num2 && point.z <= num2 + 1000;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x000274E4 File Offset: 0x000256E4
		public override float SurfaceArea()
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			return gridGraph.nodeSize * gridGraph.nodeSize;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0002750C File Offset: 0x0002570C
		public override Vector3 RandomPointOnSurface()
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			Vector3 a = gridGraph.transform.InverseTransform((Vector3)this.position);
			float2 @float = AstarMath.ThreadSafeRandomFloat2();
			return gridGraph.transform.Transform(a + new Vector3(@float.x - 0.5f, 0f, @float.y - 0.5f));
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00027574 File Offset: 0x00025774
		public Vector2 NormalizePoint(Vector3 worldPoint)
		{
			Vector3 vector = GridNode.GetGridGraph(base.GraphIndex).transform.InverseTransform(worldPoint);
			return new Vector2(vector.x - (float)this.XCoordinateInGrid, vector.z - (float)this.ZCoordinateInGrid);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000275BC File Offset: 0x000257BC
		public Vector3 UnNormalizePoint(Vector2 normalizedPointOnSurface)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			return (Vector3)this.position + gridGraph.transform.TransformVector(new Vector3(normalizedPointOnSurface.x - 0.5f, 0f, normalizedPointOnSurface.y - 0.5f));
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00027612 File Offset: 0x00025812
		public override int GetGizmoHashCode()
		{
			return base.GetGizmoHashCode() ^ (int)(109 * this.gridFlags);
		}

		// Token: 0x06000758 RID: 1880
		public abstract GridNodeBase GetNeighbourAlongDirection(int direction);

		// Token: 0x06000759 RID: 1881 RVA: 0x00027624 File Offset: 0x00025824
		public virtual bool HasConnectionInDirection(int direction)
		{
			return this.GetNeighbourAlongDirection(direction) != null;
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600075A RID: 1882
		public abstract bool HasAnyGridConnections { get; }

		// Token: 0x0600075B RID: 1883 RVA: 0x00027630 File Offset: 0x00025830
		public override bool ContainsOutgoingConnection(GraphNode node)
		{
			for (int i = 0; i < 8; i++)
			{
				if (node == this.GetNeighbourAlongDirection(i))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600075C RID: 1884
		public abstract void ResetConnectionsInternal();

		// Token: 0x0600075D RID: 1885 RVA: 0x00027656 File Offset: 0x00025856
		public override void OpenAtPoint(Path path, uint pathNodeIndex, Int3 pos, uint gScore)
		{
			path.OpenCandidateConnectionsToEndNode(pos, pathNodeIndex, pathNodeIndex, gScore);
			path.OpenCandidateConnection(pathNodeIndex, base.NodeIndex, gScore, 0U, 0U, this.position);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0002767A File Offset: 0x0002587A
		public override void Open(Path path, uint pathNodeIndex, uint gScore)
		{
			path.OpenCandidateConnectionsToEndNode(this.position, pathNodeIndex, pathNodeIndex, gScore);
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0002768B File Offset: 0x0002588B
		public override void AddPartialConnection(GraphNode node, uint cost, bool isOutgoing, bool isIncoming)
		{
			throw new NotImplementedException("GridNodes do not have support for adding manual connections with your current settings.\nPlease disable ASTAR_GRID_NO_CUSTOM_CONNECTIONS in the Optimizations tab in the A* Inspector");
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x000035CE File Offset: 0x000017CE
		public override void RemovePartialConnection(GraphNode node)
		{
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x000035CE File Offset: 0x000017CE
		public void ClearCustomConnections(bool alsoReverse)
		{
		}

		// Token: 0x040004CE RID: 1230
		private const int GridFlagsWalkableErosionOffset = 8;

		// Token: 0x040004CF RID: 1231
		private const int GridFlagsWalkableErosionMask = 256;

		// Token: 0x040004D0 RID: 1232
		private const int GridFlagsWalkableTmpOffset = 9;

		// Token: 0x040004D1 RID: 1233
		private const int GridFlagsWalkableTmpMask = 512;

		// Token: 0x040004D2 RID: 1234
		public const int NodeInGridIndexLayerOffset = 24;

		// Token: 0x040004D3 RID: 1235
		protected const int NodeInGridIndexMask = 16777215;

		// Token: 0x040004D4 RID: 1236
		protected int nodeInGridIndex;

		// Token: 0x040004D5 RID: 1237
		protected ushort gridFlags;

		// Token: 0x040004D6 RID: 1238
		internal static readonly int[] offsetToDirection = new int[]
		{
			7,
			0,
			4,
			3,
			-1,
			1,
			6,
			2,
			5
		};
	}
}
