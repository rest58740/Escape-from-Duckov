using System;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000E7 RID: 231
	[BurstCompile]
	public sealed class TriangleMeshNode : MeshNode
	{
		// Token: 0x06000797 RID: 1943 RVA: 0x0002834D File Offset: 0x0002654D
		public TriangleMeshNode()
		{
			base.HierarchicalNodeIndex = 0;
			base.NodeIndex = 268435454U;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00028367 File Offset: 0x00026567
		public TriangleMeshNode(AstarPath astar)
		{
			astar.InitializeNode(this);
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x00028376 File Offset: 0x00026576
		internal override int PathNodeVariants
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00028379 File Offset: 0x00026579
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static INavmeshHolder GetNavmeshHolder(uint graphIndex)
		{
			return TriangleMeshNode._navmeshHolders[(int)graphIndex];
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x00028382 File Offset: 0x00026582
		public int TileIndex
		{
			get
			{
				return this.v0 >> 12 & 524287;
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00028394 File Offset: 0x00026594
		public static void SetNavmeshHolder(int graphIndex, INavmeshHolder graph)
		{
			object obj = TriangleMeshNode.lockObject;
			lock (obj)
			{
				if (graphIndex >= TriangleMeshNode._navmeshHolders.Length)
				{
					INavmeshHolder[] array = new INavmeshHolder[graphIndex + 1];
					TriangleMeshNode._navmeshHolders.CopyTo(array, 0);
					TriangleMeshNode._navmeshHolders = array;
				}
				TriangleMeshNode._navmeshHolders[graphIndex] = graph;
			}
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x000283FC File Offset: 0x000265FC
		public static void ClearNavmeshHolder(int graphIndex, INavmeshHolder graph)
		{
			object obj = TriangleMeshNode.lockObject;
			lock (obj)
			{
				if (graphIndex < TriangleMeshNode._navmeshHolders.Length && TriangleMeshNode._navmeshHolders[graphIndex] == graph)
				{
					TriangleMeshNode._navmeshHolders[graphIndex] = null;
				}
			}
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00028454 File Offset: 0x00026654
		public void UpdatePositionFromVertices()
		{
			Int3 lhs;
			Int3 rhs;
			Int3 rhs2;
			this.GetVertices(out lhs, out rhs, out rhs2);
			this.position = (lhs + rhs + rhs2) * 0.333333f;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0002848A File Offset: 0x0002668A
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetVertexIndex(int i)
		{
			if (i == 0)
			{
				return this.v0;
			}
			if (i != 1)
			{
				return this.v2;
			}
			return this.v1;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x000284A7 File Offset: 0x000266A7
		public int GetVertexArrayIndex(int i)
		{
			return TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).GetVertexArrayIndex((i == 0) ? this.v0 : ((i == 1) ? this.v1 : this.v2));
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x000284D8 File Offset: 0x000266D8
		public void GetVertices(out Int3 v0, out Int3 v1, out Int3 v2)
		{
			INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			v0 = navmeshHolder.GetVertex(this.v0);
			v1 = navmeshHolder.GetVertex(this.v1);
			v2 = navmeshHolder.GetVertex(this.v2);
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00028528 File Offset: 0x00026728
		public void GetVerticesInGraphSpace(out Int3 v0, out Int3 v1, out Int3 v2)
		{
			INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			v0 = navmeshHolder.GetVertexInGraphSpace(this.v0);
			v1 = navmeshHolder.GetVertexInGraphSpace(this.v1);
			v2 = navmeshHolder.GetVertexInGraphSpace(this.v2);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00028577 File Offset: 0x00026777
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override Int3 GetVertex(int i)
		{
			return TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).GetVertex(this.GetVertexIndex(i));
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00028590 File Offset: 0x00026790
		public Int3 GetVertexInGraphSpace(int i)
		{
			return TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).GetVertexInGraphSpace(this.GetVertexIndex(i));
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00028376 File Offset: 0x00026576
		public override int GetVertexCount()
		{
			return 3;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x000285AC File Offset: 0x000267AC
		public Vector3 ProjectOnSurface(Vector3 point)
		{
			Int3 ob;
			Int3 ob2;
			Int3 ob3;
			this.GetVertices(out ob, out ob2, out ob3);
			Vector3 b = (Vector3)ob;
			Vector3 a = (Vector3)ob2;
			Vector3 a2 = (Vector3)ob3;
			Vector3 normalized = Vector3.Cross(a - b, a2 - b).normalized;
			return point - normalized * Vector3.Dot(normalized, point - b);
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00028614 File Offset: 0x00026814
		public override Vector3 ClosestPointOnNode(Vector3 p)
		{
			Int3 ob;
			Int3 ob2;
			Int3 ob3;
			this.GetVertices(out ob, out ob2, out ob3);
			return Polygon.ClosestPointOnTriangle((Vector3)ob, (Vector3)ob2, (Vector3)ob3, p);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00028660 File Offset: 0x00026860
		internal Int3 ClosestPointOnNodeXZInGraphSpace(Vector3 p)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			this.GetVerticesInGraphSpace(out @int, out int2, out int3);
			p = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).transform.InverseTransform(p);
			Int3 int4 = (Int3)Polygon.ClosestPointOnTriangleXZ((Vector3)@int, (Vector3)int2, (Vector3)int3, p);
			if (this.ContainsPointInGraphSpace(int4))
			{
				return int4;
			}
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					if (i != 0 || j != 0)
					{
						Int3 int5 = new Int3(int4.x + i, int4.y, int4.z + j);
						if (this.ContainsPointInGraphSpace(int5))
						{
							return int5;
						}
					}
				}
			}
			long sqrMagnitudeLong = (@int - int4).sqrMagnitudeLong;
			long sqrMagnitudeLong2 = (int2 - int4).sqrMagnitudeLong;
			long sqrMagnitudeLong3 = (int3 - int4).sqrMagnitudeLong;
			if (sqrMagnitudeLong >= sqrMagnitudeLong2)
			{
				if (sqrMagnitudeLong2 >= sqrMagnitudeLong3)
				{
					return int3;
				}
				return int2;
			}
			else
			{
				if (sqrMagnitudeLong >= sqrMagnitudeLong3)
				{
					return int3;
				}
				return @int;
			}
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00028760 File Offset: 0x00026960
		public override Vector3 ClosestPointOnNodeXZ(Vector3 p)
		{
			Int3 ob;
			Int3 ob2;
			Int3 ob3;
			this.GetVertices(out ob, out ob2, out ob3);
			return Polygon.ClosestPointOnTriangleXZ((Vector3)ob, (Vector3)ob2, (Vector3)ob3, p);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00028791 File Offset: 0x00026991
		public override bool ContainsPoint(Vector3 p)
		{
			return this.ContainsPointInGraphSpace((Int3)TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).transform.InverseTransform(p));
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x000287B4 File Offset: 0x000269B4
		public bool ContainsPoint(Vector3 p, NativeMovementPlane movementPlane)
		{
			Int3 ob;
			Int3 ob2;
			Int3 ob3;
			this.GetVertices(out ob, out ob2, out ob3);
			int3 @int = (int3)ob;
			int3 int2 = (int3)ob2;
			int3 int3 = (int3)ob3;
			int3 int4 = (int3)((Int3)p);
			return Polygon.ContainsPoint(ref @int, ref int2, ref int3, ref int4, ref movementPlane);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00028800 File Offset: 0x00026A00
		public override bool ContainsPointInGraphSpace(Int3 p)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			this.GetVerticesInGraphSpace(out @int, out int2, out int3);
			return (long)(int2.x - @int.x) * (long)(p.z - @int.z) - (long)(p.x - @int.x) * (long)(int2.z - @int.z) <= 0L && (long)(int3.x - int2.x) * (long)(p.z - int2.z) - (long)(p.x - int2.x) * (long)(int3.z - int2.z) <= 0L && (long)(@int.x - int3.x) * (long)(p.z - int3.z) - (long)(p.x - int3.x) * (long)(@int.z - int3.z) <= 0L;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x000288E0 File Offset: 0x00026AE0
		public override Int3 DecodeVariantPosition(uint pathNodeIndex, uint fractionAlongEdge)
		{
			int num = (int)(pathNodeIndex - base.NodeIndex);
			Int3 vertex = this.GetVertex(num);
			Int3 vertex2 = this.GetVertex((num + 1) % 3);
			Int3 result;
			TriangleMeshNode.InterpolateEdge(ref vertex, ref vertex2, fractionAlongEdge, out result);
			return result;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00028917 File Offset: 0x00026B17
		[BurstCompile(FloatMode = FloatMode.Fast)]
		private static void InterpolateEdge(ref Int3 p1, ref Int3 p2, uint fractionAlongEdge, out Int3 pos)
		{
			TriangleMeshNode.InterpolateEdge_00000761$BurstDirectCall.Invoke(ref p1, ref p2, fractionAlongEdge, out pos);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00028922 File Offset: 0x00026B22
		public override void OpenAtPoint(Path path, uint pathNodeIndex, Int3 point, uint gScore)
		{
			this.OpenAtPoint(path, pathNodeIndex, point, -1, gScore);
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00028930 File Offset: 0x00026B30
		public override void Open(Path path, uint pathNodeIndex, uint gScore)
		{
			PathHandler pathHandler = ((IPathInternals)path).PathHandler;
			int edge = (int)(pathNodeIndex - base.NodeIndex);
			this.OpenAtPoint(path, pathNodeIndex, this.DecodeVariantPosition(pathNodeIndex, pathHandler.pathNodes[pathNodeIndex].fractionAlongEdge), edge, gScore);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00028970 File Offset: 0x00026B70
		private unsafe void OpenAtPoint(Path path, uint pathNodeIndex, Int3 pos, int edge, uint gScore)
		{
			PathHandler pathHandler = ((IPathInternals)path).PathHandler;
			PathNode pathNode = *pathHandler.pathNodes[pathNodeIndex];
			if (pathNode.flag1)
			{
				path.OpenCandidateConnectionsToEndNode(pos, pathNodeIndex, base.NodeIndex, gScore);
			}
			int num = 0;
			bool flag = pathNode.parentIndex >= base.NodeIndex && pathNode.parentIndex < base.NodeIndex + 3U;
			if (this.connections != null)
			{
				for (int i = this.connections.Length - 1; i >= 0; i--)
				{
					Connection connection = this.connections[i];
					if (connection.isOutgoing)
					{
						GraphNode node = connection.node;
						if (connection.isEdgeShared)
						{
							int adjacentShapeEdge = connection.adjacentShapeEdge;
							uint num2 = node.NodeIndex + (uint)adjacentShapeEdge;
							if (num2 != pathNode.parentIndex)
							{
								if (connection.shapeEdge == edge)
								{
									if (path.CanTraverse(this, node))
									{
										TriangleMeshNode triangleMeshNode = node as TriangleMeshNode;
										if (path.ShouldConsiderPathNode(num2))
										{
											if (connection.edgesAreIdentical)
											{
												uint gScore2 = gScore + path.GetTraversalCost(node);
												path.SkipOverNode(num2, pathNodeIndex, PathNode.ReverseFractionAlongEdge(pathNode.fractionAlongEdge), uint.MaxValue, gScore2);
												triangleMeshNode.OpenAtPoint(path, num2, pos, adjacentShapeEdge, gScore2);
											}
											else
											{
												this.OpenSingleEdge(path, pathNodeIndex, triangleMeshNode, adjacentShapeEdge, pos, gScore);
											}
										}
									}
								}
								else if (!flag && (num & 1 << connection.shapeEdge) == 0)
								{
									num |= 1 << connection.shapeEdge;
									this.OpenSingleEdge(path, pathNodeIndex, this, connection.shapeEdge, pos, gScore);
								}
							}
						}
						else if (!flag && path.CanTraverse(this, node) && path.ShouldConsiderPathNode(node.NodeIndex))
						{
							uint costMagnitude = (uint)(node.position - pos).costMagnitude;
							if (edge != -1)
							{
								path.OpenCandidateConnection(pathNodeIndex, node.NodeIndex, gScore, costMagnitude, 0U, node.position);
							}
							else
							{
								uint num3 = pathHandler.AddTemporaryNode(new TemporaryNode
								{
									associatedNode = base.NodeIndex,
									position = pos,
									targetIndex = 0,
									type = TemporaryNodeType.Ignore
								});
								ref PathNode ptr = ref pathHandler.pathNodes[num3];
								ptr.pathID = path.pathID;
								ptr.parentIndex = pathNodeIndex;
								path.OpenCandidateConnection(num3, node.NodeIndex, gScore, costMagnitude, 0U, node.position);
							}
						}
					}
				}
			}
			if (edge == -1)
			{
				if (pathHandler.pathNodes[base.NodeIndex].flag1)
				{
					path.OpenCandidateConnectionsToEndNode(pos, pathNodeIndex, base.NodeIndex, gScore);
				}
				if (num == 0)
				{
					this.OpenSingleEdge(path, pathNodeIndex, this, 0, pos, gScore);
				}
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00028C24 File Offset: 0x00026E24
		private void OpenSingleEdge(Path path, uint pathNodeIndex, TriangleMeshNode other, int sharedEdgeOnOtherNode, Int3 pos, uint gScore)
		{
			uint num = other.NodeIndex + (uint)sharedEdgeOnOtherNode;
			if (!path.ShouldConsiderPathNode(num))
			{
				return;
			}
			Int3 vertex = other.GetVertex(sharedEdgeOnOtherNode);
			Int3 vertex2 = other.GetVertex((sharedEdgeOnOtherNode + 1) % 3);
			PathHandler pathHandler = ((IPathInternals)path).PathHandler;
			uint traversalCost = path.GetTraversalCost(other);
			uint candidateG = gScore + traversalCost;
			TriangleMeshNode.OpenSingleEdgeBurst(ref vertex, ref vertex2, ref pos, path.pathID, pathNodeIndex, num, other.NodeIndex, candidateG, ref pathHandler.pathNodes, ref pathHandler.heap, path.heuristicObjectiveInternal);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00028CA0 File Offset: 0x00026EA0
		[BurstCompile]
		private static void OpenSingleEdgeBurst(ref Int3 s1, ref Int3 s2, ref Int3 pos, ushort pathID, uint pathNodeIndex, uint candidatePathNodeIndex, uint candidateNodeIndex, uint candidateG, ref UnsafeSpan<PathNode> pathNodes, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective)
		{
			TriangleMeshNode.OpenSingleEdgeBurst_00000766$BurstDirectCall.Invoke(ref s1, ref s2, ref pos, pathID, pathNodeIndex, candidatePathNodeIndex, candidateNodeIndex, candidateG, ref pathNodes, ref heap, ref heuristicObjective);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00028CC4 File Offset: 0x00026EC4
		[BurstCompile]
		private static void CalculateBestEdgePosition(ref Int3 s1, ref Int3 s2, ref Int3 pos, out int3 closestPointAlongEdge, out uint quantizedFractionAlongEdge, out uint cost)
		{
			TriangleMeshNode.CalculateBestEdgePosition_00000767$BurstDirectCall.Invoke(ref s1, ref s2, ref pos, out closestPointAlongEdge, out quantizedFractionAlongEdge, out cost);
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00028CD4 File Offset: 0x00026ED4
		public int SharedEdge(GraphNode other)
		{
			int result = -1;
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i].node == other && this.connections[i].isEdgeShared)
					{
						result = this.connections[i].shapeEdge;
					}
				}
			}
			return result;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00028D38 File Offset: 0x00026F38
		public override bool GetPortal(GraphNode toNode, out Vector3 left, out Vector3 right)
		{
			int num;
			int num2;
			return this.GetPortal(toNode, out left, out right, out num, out num2);
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00028D54 File Offset: 0x00026F54
		public bool GetPortalInGraphSpace(TriangleMeshNode toNode, out Int3 a, out Int3 b, out int aIndex, out int bIndex)
		{
			aIndex = -1;
			bIndex = -1;
			a = Int3.zero;
			b = Int3.zero;
			if (toNode.GraphIndex != base.GraphIndex)
			{
				return false;
			}
			int num = -1;
			int num2 = -1;
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i].node == toNode && this.connections[i].isEdgeShared)
					{
						num = this.connections[i].shapeEdge;
						num2 = this.connections[i].adjacentShapeEdge;
					}
				}
			}
			if (num == -1)
			{
				return false;
			}
			aIndex = num;
			bIndex = (num + 1) % 3;
			INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			a = navmeshHolder.GetVertexInGraphSpace(this.GetVertexIndex(aIndex));
			b = navmeshHolder.GetVertexInGraphSpace(this.GetVertexIndex(bIndex));
			int tileIndex = this.TileIndex;
			int tileIndex2 = toNode.TileIndex;
			if (tileIndex != tileIndex2)
			{
				Int3 vertexInGraphSpace = toNode.GetVertexInGraphSpace(num2);
				Int3 vertexInGraphSpace2 = toNode.GetVertexInGraphSpace((num2 + 1) % 3);
				int num3;
				int num4;
				navmeshHolder.GetTileCoordinates(tileIndex, out num3, out num4);
				int num5;
				int num6;
				navmeshHolder.GetTileCoordinates(tileIndex2, out num5, out num6);
				int i2 = (num3 == num5) ? 0 : 2;
				int min = Mathf.Min(vertexInGraphSpace[i2], vertexInGraphSpace2[i2]);
				int max = Mathf.Max(vertexInGraphSpace[i2], vertexInGraphSpace2[i2]);
				a[i2] = Mathf.Clamp(a[i2], min, max);
				b[i2] = Mathf.Clamp(b[i2], min, max);
			}
			return true;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00028F00 File Offset: 0x00027100
		public bool GetPortal(GraphNode toNode, out Vector3 left, out Vector3 right, out int aIndex, out int bIndex)
		{
			TriangleMeshNode triangleMeshNode = toNode as TriangleMeshNode;
			Int3 ob;
			Int3 ob2;
			if (triangleMeshNode != null && this.GetPortalInGraphSpace(triangleMeshNode, out ob, out ob2, out aIndex, out bIndex))
			{
				INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
				left = navmeshHolder.transform.Transform((Vector3)ob);
				right = navmeshHolder.transform.Transform((Vector3)ob2);
				return true;
			}
			aIndex = -1;
			bIndex = -1;
			left = Vector3.zero;
			right = Vector3.zero;
			return false;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00028F84 File Offset: 0x00027184
		public override float SurfaceArea()
		{
			INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			return (float)Math.Abs(VectorMath.SignedTriangleAreaTimes2XZ(navmeshHolder.GetVertex(this.v0), navmeshHolder.GetVertex(this.v1), navmeshHolder.GetVertex(this.v2))) * 0.5f;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00028FD4 File Offset: 0x000271D4
		public override Vector3 RandomPointOnSurface()
		{
			float2 @float;
			do
			{
				@float = AstarMath.ThreadSafeRandomFloat2();
			}
			while (@float.x + @float.y > 1f);
			Int3 @int;
			Int3 lhs;
			Int3 lhs2;
			this.GetVertices(out @int, out lhs, out lhs2);
			return (Vector3)(lhs - @int) * @float.x + (Vector3)(lhs2 - @int) * @float.y + (Vector3)@int;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00029045 File Offset: 0x00027245
		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.writer.Write(this.v0);
			ctx.writer.Write(this.v1);
			ctx.writer.Write(this.v2);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00029081 File Offset: 0x00027281
		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.v0 = ctx.reader.ReadInt32();
			this.v1 = ctx.reader.ReadInt32();
			this.v2 = ctx.reader.ReadInt32();
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00029110 File Offset: 0x00027310
		[BurstCompile(FloatMode = FloatMode.Fast)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void InterpolateEdge$BurstManaged(ref Int3 p1, ref Int3 p2, uint fractionAlongEdge, out Int3 pos)
		{
			int3 @int = (int3)math.lerp((int3)p1, (int3)p2, PathNode.UnQuantizeFractionAlongEdge(fractionAlongEdge));
			pos = new Int3(@int.x, @int.y, @int.z);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0002916C File Offset: 0x0002736C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void OpenSingleEdgeBurst$BurstManaged(ref Int3 s1, ref Int3 s2, ref Int3 pos, ushort pathID, uint pathNodeIndex, uint candidatePathNodeIndex, uint candidateNodeIndex, uint candidateG, ref UnsafeSpan<PathNode> pathNodes, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective)
		{
			int3 targetNodePosition;
			uint fractionAlongEdge;
			uint num;
			TriangleMeshNode.CalculateBestEdgePosition(ref s1, ref s2, ref pos, out targetNodePosition, out fractionAlongEdge, out num);
			candidateG += num;
			Path.OpenCandidateParams openCandidateParams = new Path.OpenCandidateParams
			{
				pathID = pathID,
				parentPathNode = pathNodeIndex,
				targetPathNode = candidatePathNodeIndex,
				targetNodeIndex = candidateNodeIndex,
				candidateG = candidateG,
				fractionAlongEdge = fractionAlongEdge,
				targetNodePosition = targetNodePosition,
				pathNodes = pathNodes
			};
			Path.OpenCandidateConnectionBurst(ref openCandidateParams, ref heap, ref heuristicObjective);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x000291F0 File Offset: 0x000273F0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void CalculateBestEdgePosition$BurstManaged(ref Int3 s1, ref Int3 s2, ref Int3 pos, out int3 closestPointAlongEdge, out uint quantizedFractionAlongEdge, out uint cost)
		{
			float3 @float = (int3)s1;
			float3 float2 = (int3)s2;
			int3 @int = (int3)pos;
			float num = math.clamp(VectorMath.ClosestPointOnLineFactor(@float, float2, @int), 0f, 1f);
			quantizedFractionAlongEdge = PathNode.QuantizeFractionAlongEdge(num);
			num = PathNode.UnQuantizeFractionAlongEdge(quantizedFractionAlongEdge);
			float3 v = math.lerp(@float, float2, num);
			closestPointAlongEdge = (int3)v;
			int3 int2 = @int - closestPointAlongEdge;
			cost = (uint)new Int3(int2.x, int2.y, int2.z).costMagnitude;
		}

		// Token: 0x040004E4 RID: 1252
		public const bool InaccuratePathSearch = false;

		// Token: 0x040004E5 RID: 1253
		public int v0;

		// Token: 0x040004E6 RID: 1254
		public int v1;

		// Token: 0x040004E7 RID: 1255
		public int v2;

		// Token: 0x040004E8 RID: 1256
		private static INavmeshHolder[] _navmeshHolders = new INavmeshHolder[0];

		// Token: 0x040004E9 RID: 1257
		private static readonly object lockObject = new object();

		// Token: 0x040004EA RID: 1258
		public static readonly ProfilerMarker MarkerDecode = new ProfilerMarker("Decode");

		// Token: 0x040004EB RID: 1259
		public static readonly ProfilerMarker MarkerGetVertices = new ProfilerMarker("GetVertex");

		// Token: 0x040004EC RID: 1260
		public static readonly ProfilerMarker MarkerClosest = new ProfilerMarker("MarkerClosest");

		// Token: 0x020000E8 RID: 232
		// (Invoke) Token: 0x060007C2 RID: 1986
		internal delegate void InterpolateEdge_00000761$PostfixBurstDelegate(ref Int3 p1, ref Int3 p2, uint fractionAlongEdge, out Int3 pos);

		// Token: 0x020000E9 RID: 233
		internal static class InterpolateEdge_00000761$BurstDirectCall
		{
			// Token: 0x060007C5 RID: 1989 RVA: 0x000292A1 File Offset: 0x000274A1
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (TriangleMeshNode.InterpolateEdge_00000761$BurstDirectCall.Pointer == 0)
				{
					TriangleMeshNode.InterpolateEdge_00000761$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(TriangleMeshNode.InterpolateEdge_00000761$BurstDirectCall.DeferredCompilation, methodof(TriangleMeshNode.InterpolateEdge$BurstManaged(Int3*, Int3*, uint, Int3*)).MethodHandle, typeof(TriangleMeshNode.InterpolateEdge_00000761$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = TriangleMeshNode.InterpolateEdge_00000761$BurstDirectCall.Pointer;
			}

			// Token: 0x060007C6 RID: 1990 RVA: 0x000292D0 File Offset: 0x000274D0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				TriangleMeshNode.InterpolateEdge_00000761$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x060007C7 RID: 1991 RVA: 0x000292E8 File Offset: 0x000274E8
			public unsafe static void Constructor()
			{
				TriangleMeshNode.InterpolateEdge_00000761$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(TriangleMeshNode.InterpolateEdge(Int3*, Int3*, uint, Int3*)).MethodHandle);
			}

			// Token: 0x060007C8 RID: 1992 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x060007C9 RID: 1993 RVA: 0x000292F9 File Offset: 0x000274F9
			// Note: this type is marked as 'beforefieldinit'.
			static InterpolateEdge_00000761$BurstDirectCall()
			{
				TriangleMeshNode.InterpolateEdge_00000761$BurstDirectCall.Constructor();
			}

			// Token: 0x060007CA RID: 1994 RVA: 0x00029300 File Offset: 0x00027500
			public static void Invoke(ref Int3 p1, ref Int3 p2, uint fractionAlongEdge, out Int3 pos)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = TriangleMeshNode.InterpolateEdge_00000761$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Int3&,Pathfinding.Int3&,System.UInt32,Pathfinding.Int3&), ref p1, ref p2, fractionAlongEdge, ref pos, functionPointer);
						return;
					}
				}
				TriangleMeshNode.InterpolateEdge$BurstManaged(ref p1, ref p2, fractionAlongEdge, out pos);
			}

			// Token: 0x040004ED RID: 1261
			private static IntPtr Pointer;

			// Token: 0x040004EE RID: 1262
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020000EA RID: 234
		// (Invoke) Token: 0x060007CC RID: 1996
		internal delegate void OpenSingleEdgeBurst_00000766$PostfixBurstDelegate(ref Int3 s1, ref Int3 s2, ref Int3 pos, ushort pathID, uint pathNodeIndex, uint candidatePathNodeIndex, uint candidateNodeIndex, uint candidateG, ref UnsafeSpan<PathNode> pathNodes, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective);

		// Token: 0x020000EB RID: 235
		internal static class OpenSingleEdgeBurst_00000766$BurstDirectCall
		{
			// Token: 0x060007CF RID: 1999 RVA: 0x00029337 File Offset: 0x00027537
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (TriangleMeshNode.OpenSingleEdgeBurst_00000766$BurstDirectCall.Pointer == 0)
				{
					TriangleMeshNode.OpenSingleEdgeBurst_00000766$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(TriangleMeshNode.OpenSingleEdgeBurst_00000766$BurstDirectCall.DeferredCompilation, methodof(TriangleMeshNode.OpenSingleEdgeBurst$BurstManaged(Int3*, Int3*, Int3*, ushort, uint, uint, uint, uint, UnsafeSpan<PathNode>*, BinaryHeap*, HeuristicObjective*)).MethodHandle, typeof(TriangleMeshNode.OpenSingleEdgeBurst_00000766$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = TriangleMeshNode.OpenSingleEdgeBurst_00000766$BurstDirectCall.Pointer;
			}

			// Token: 0x060007D0 RID: 2000 RVA: 0x00029364 File Offset: 0x00027564
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				TriangleMeshNode.OpenSingleEdgeBurst_00000766$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x060007D1 RID: 2001 RVA: 0x0002937C File Offset: 0x0002757C
			public unsafe static void Constructor()
			{
				TriangleMeshNode.OpenSingleEdgeBurst_00000766$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(TriangleMeshNode.OpenSingleEdgeBurst(Int3*, Int3*, Int3*, ushort, uint, uint, uint, uint, UnsafeSpan<PathNode>*, BinaryHeap*, HeuristicObjective*)).MethodHandle);
			}

			// Token: 0x060007D2 RID: 2002 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x060007D3 RID: 2003 RVA: 0x0002938D File Offset: 0x0002758D
			// Note: this type is marked as 'beforefieldinit'.
			static OpenSingleEdgeBurst_00000766$BurstDirectCall()
			{
				TriangleMeshNode.OpenSingleEdgeBurst_00000766$BurstDirectCall.Constructor();
			}

			// Token: 0x060007D4 RID: 2004 RVA: 0x00029394 File Offset: 0x00027594
			public static void Invoke(ref Int3 s1, ref Int3 s2, ref Int3 pos, ushort pathID, uint pathNodeIndex, uint candidatePathNodeIndex, uint candidateNodeIndex, uint candidateG, ref UnsafeSpan<PathNode> pathNodes, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = TriangleMeshNode.OpenSingleEdgeBurst_00000766$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Int3&,Pathfinding.Int3&,Pathfinding.Int3&,System.UInt16,System.UInt32,System.UInt32,System.UInt32,System.UInt32,Pathfinding.Collections.UnsafeSpan`1<Pathfinding.PathNode>&,Pathfinding.BinaryHeap&,Pathfinding.HeuristicObjective&), ref s1, ref s2, ref pos, pathID, pathNodeIndex, candidatePathNodeIndex, candidateNodeIndex, candidateG, ref pathNodes, ref heap, ref heuristicObjective, functionPointer);
						return;
					}
				}
				TriangleMeshNode.OpenSingleEdgeBurst$BurstManaged(ref s1, ref s2, ref pos, pathID, pathNodeIndex, candidatePathNodeIndex, candidateNodeIndex, candidateG, ref pathNodes, ref heap, ref heuristicObjective);
			}

			// Token: 0x040004EF RID: 1263
			private static IntPtr Pointer;

			// Token: 0x040004F0 RID: 1264
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020000EC RID: 236
		// (Invoke) Token: 0x060007D6 RID: 2006
		internal delegate void CalculateBestEdgePosition_00000767$PostfixBurstDelegate(ref Int3 s1, ref Int3 s2, ref Int3 pos, out int3 closestPointAlongEdge, out uint quantizedFractionAlongEdge, out uint cost);

		// Token: 0x020000ED RID: 237
		internal static class CalculateBestEdgePosition_00000767$BurstDirectCall
		{
			// Token: 0x060007D9 RID: 2009 RVA: 0x000293E7 File Offset: 0x000275E7
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (TriangleMeshNode.CalculateBestEdgePosition_00000767$BurstDirectCall.Pointer == 0)
				{
					TriangleMeshNode.CalculateBestEdgePosition_00000767$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(TriangleMeshNode.CalculateBestEdgePosition_00000767$BurstDirectCall.DeferredCompilation, methodof(TriangleMeshNode.CalculateBestEdgePosition$BurstManaged(Int3*, Int3*, Int3*, int3*, uint*, uint*)).MethodHandle, typeof(TriangleMeshNode.CalculateBestEdgePosition_00000767$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = TriangleMeshNode.CalculateBestEdgePosition_00000767$BurstDirectCall.Pointer;
			}

			// Token: 0x060007DA RID: 2010 RVA: 0x00029414 File Offset: 0x00027614
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				TriangleMeshNode.CalculateBestEdgePosition_00000767$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x060007DB RID: 2011 RVA: 0x0002942C File Offset: 0x0002762C
			public unsafe static void Constructor()
			{
				TriangleMeshNode.CalculateBestEdgePosition_00000767$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(TriangleMeshNode.CalculateBestEdgePosition(Int3*, Int3*, Int3*, int3*, uint*, uint*)).MethodHandle);
			}

			// Token: 0x060007DC RID: 2012 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x060007DD RID: 2013 RVA: 0x0002943D File Offset: 0x0002763D
			// Note: this type is marked as 'beforefieldinit'.
			static CalculateBestEdgePosition_00000767$BurstDirectCall()
			{
				TriangleMeshNode.CalculateBestEdgePosition_00000767$BurstDirectCall.Constructor();
			}

			// Token: 0x060007DE RID: 2014 RVA: 0x00029444 File Offset: 0x00027644
			public static void Invoke(ref Int3 s1, ref Int3 s2, ref Int3 pos, out int3 closestPointAlongEdge, out uint quantizedFractionAlongEdge, out uint cost)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = TriangleMeshNode.CalculateBestEdgePosition_00000767$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Int3&,Pathfinding.Int3&,Pathfinding.Int3&,Unity.Mathematics.int3&,System.UInt32&,System.UInt32&), ref s1, ref s2, ref pos, ref closestPointAlongEdge, ref quantizedFractionAlongEdge, ref cost, functionPointer);
						return;
					}
				}
				TriangleMeshNode.CalculateBestEdgePosition$BurstManaged(ref s1, ref s2, ref pos, out closestPointAlongEdge, out quantizedFractionAlongEdge, out cost);
			}

			// Token: 0x040004F1 RID: 1265
			private static IntPtr Pointer;

			// Token: 0x040004F2 RID: 1266
			private static IntPtr DeferredCompilation;
		}
	}
}
