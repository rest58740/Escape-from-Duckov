using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Pooling;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000151 RID: 337
	[BurstCompile]
	public struct PathTracer
	{
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x00037DC6 File Offset: 0x00035FC6
		// (set) Token: 0x06000A17 RID: 2583 RVA: 0x00037DCE File Offset: 0x00035FCE
		public ushort version { [IgnoredByDeepProfiler] readonly get; [IgnoredByDeepProfiler] private set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x00037DD7 File Offset: 0x00035FD7
		public readonly bool isCreated
		{
			get
			{
				return this.funnelState.unwrappedPortals.IsCreated;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00037DE9 File Offset: 0x00035FE9
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x00037E08 File Offset: 0x00036008
		public GraphNode startNode
		{
			[IgnoredByDeepProfiler]
			readonly get
			{
				if (this.startNodeInternal == null || this.startNodeInternal.Destroyed)
				{
					return null;
				}
				return this.startNodeInternal;
			}
			[IgnoredByDeepProfiler]
			private set
			{
				this.startNodeInternal = value;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x00037E11 File Offset: 0x00036011
		public readonly bool isStale
		{
			[IgnoredByDeepProfiler]
			get
			{
				return !this.endIsUpToDate || !this.startIsUpToDate || this.firstPartContainsDestroyedNodes;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x00037E2B File Offset: 0x0003602B
		public readonly int partCount
		{
			get
			{
				if (this.parts == null)
				{
					return 0;
				}
				return this.parts.Length - this.firstPartIndex;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x00037E46 File Offset: 0x00036046
		public readonly bool hasPath
		{
			get
			{
				return this.partCount > 0;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x00037E51 File Offset: 0x00036051
		public readonly Vector3 startPoint
		{
			get
			{
				return this.parts[this.firstPartIndex].startPoint;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x00037E69 File Offset: 0x00036069
		public readonly Vector3 endPoint
		{
			get
			{
				return this.parts[this.parts.Length - 1].endPoint;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x00037E85 File Offset: 0x00036085
		public readonly Vector3 endPointOfFirstPart
		{
			get
			{
				return this.parts[this.firstPartIndex].endPoint;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x00037E9D File Offset: 0x0003609D
		public int desiredCornersForGoodSimplification
		{
			get
			{
				if (this.partGraphType != PathTracer.PartGraphType.Grid)
				{
					return 2;
				}
				return 3;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x00037EAB File Offset: 0x000360AB
		public readonly bool isNextPartValidLink
		{
			get
			{
				return this.partCount > 1 && this.GetPartType(1) == Funnel.PartType.OffMeshLink && !this.PartContainsDestroyedNodes(1);
			}
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00037ECC File Offset: 0x000360CC
		public PathTracer(Allocator allocator)
		{
			this.funnelState = new Funnel.FunnelState(16, allocator);
			this.parts = null;
			this.nodes = new CircularBuffer<GraphNode>(16);
			this.portalIsNotInnerCorner = new CircularBuffer<byte>(16);
			this.nodeHashes = new CircularBuffer<int>(16);
			this.unclampedEndPoint = (this.unclampedStartPoint = Vector3.zero);
			this.firstPartIndex = 0;
			this.startIsUpToDate = false;
			this.endIsUpToDate = false;
			this.firstPartContainsDestroyedNodes = false;
			this.startNodeInternal = null;
			this.version = 1;
			this.nnConstraint = NNConstraint.Walkable;
			this.partGraphType = PathTracer.PartGraphType.Navmesh;
			this.Clear();
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00037F6B File Offset: 0x0003616B
		public void Dispose()
		{
			this.Clear();
			this.funnelState.Dispose();
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x00037F7E File Offset: 0x0003617E
		public Vector3 UpdateStart(Vector3 position, PathTracer.RepairQuality quality, NativeMovementPlane movementPlane, ITraversalProvider traversalProvider, Path path)
		{
			this.Repair(position, true, quality, movementPlane, traversalProvider, path, true);
			return this.parts[this.firstPartIndex].startPoint;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00037FA5 File Offset: 0x000361A5
		public Vector3 UpdateEnd(Vector3 position, PathTracer.RepairQuality quality, NativeMovementPlane movementPlane, ITraversalProvider traversalProvider, Path path)
		{
			this.Repair(position, false, quality, movementPlane, traversalProvider, path, true);
			return this.parts[this.parts.Length - 1].endPoint;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00037FD0 File Offset: 0x000361D0
		private void AppendNode(bool toStart, GraphNode node)
		{
			int num = toStart ? this.firstPartIndex : (this.parts.Length - 1);
			ref Funnel.PathPart ptr = ref this.parts[num];
			GraphNode graphNode = (ptr.endIndex >= ptr.startIndex) ? this.nodes.GetBoundaryValue(toStart) : null;
			if (node == graphNode)
			{
				return;
			}
			if (node == null)
			{
				throw new ArgumentNullException();
			}
			if (ptr.endIndex >= ptr.startIndex + 1 && this.nodes.GetAbsolute(toStart ? (ptr.startIndex + 1) : (ptr.endIndex - 1)) == node)
			{
				if (toStart)
				{
					ptr.startIndex++;
				}
				else
				{
					ptr.endIndex--;
				}
				this.nodes.Pop(toStart);
				this.nodeHashes.Pop(toStart);
				if (num == this.firstPartIndex && this.funnelState.leftFunnel.Length > 0)
				{
					this.funnelState.Pop(toStart);
					this.portalIsNotInnerCorner.Pop(toStart);
				}
				return;
			}
			if (num == this.firstPartIndex && graphNode != null)
			{
				Vector3 newLeftPortal;
				Vector3 newRightPortal;
				if (toStart)
				{
					if (!node.GetPortal(graphNode, out newLeftPortal, out newRightPortal))
					{
						throw new NotImplementedException();
					}
				}
				else if (!graphNode.GetPortal(node, out newLeftPortal, out newRightPortal))
				{
					throw new NotImplementedException();
				}
				this.funnelState.Push(toStart, newLeftPortal, newRightPortal);
				this.portalIsNotInnerCorner.Push(toStart, 0);
			}
			this.nodes.Push(toStart, node);
			this.nodeHashes.Push(toStart, PathTracer.HashNode(node));
			if (toStart)
			{
				ptr.startIndex--;
				return;
			}
			ptr.endIndex++;
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00038158 File Offset: 0x00036358
		private unsafe void AppendPath(bool toStart, CircularBuffer<GraphNode> path)
		{
			if (path.Length == 0)
			{
				return;
			}
			while (path.Length > 0)
			{
				this.AppendNode(toStart, path.PopStart());
			}
			if (toStart)
			{
				this.startNode = *this.nodes.First;
				int num = Mathf.Min(this.parts[this.firstPartIndex].startIndex + 5, this.parts[this.firstPartIndex].endIndex);
				bool flag = false;
				for (int i = this.parts[this.firstPartIndex].startIndex; i <= num; i++)
				{
					flag |= !this.ValidInPath(i);
				}
				this.firstPartContainsDestroyedNodes = flag;
			}
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x000035CE File Offset: 0x000017CE
		[Conditional("UNITY_EDITOR")]
		private void CheckInvariants()
		{
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00038208 File Offset: 0x00036408
		private bool SplicePath(int startIndex, int toRemove, List<GraphNode> toInsert)
		{
			ref Funnel.PathPart ptr = ref this.parts[this.firstPartIndex];
			if (startIndex < ptr.startIndex || startIndex + toRemove - 1 > ptr.endIndex)
			{
				throw new ArgumentException("This method can only handle splicing the first part of the path");
			}
			if (toInsert != null)
			{
				int num = 0;
				int num2 = 0;
				while (num < toInsert.Count && num < toRemove && toInsert[num] == this.nodes.GetAbsolute(startIndex + num))
				{
					num++;
				}
				if (num == toInsert.Count && num == toRemove)
				{
					return true;
				}
				while (num2 < toInsert.Count - num && num2 < toRemove - num && toInsert[toInsert.Count - num2 - 1] == this.nodes.GetAbsolute(startIndex + toRemove - num2 - 1))
				{
					num2++;
				}
				toInsert.RemoveRange(toInsert.Count - num2, num2);
				toInsert.RemoveRange(0, num);
				startIndex += num;
				toRemove -= num + num2;
			}
			int num3 = (toInsert != null) ? toInsert.Count : 0;
			if (startIndex - 1 >= ptr.startIndex && !this.ValidInPath(startIndex - 1))
			{
				return false;
			}
			if (startIndex + toRemove <= ptr.endIndex && !this.ValidInPath(startIndex + toRemove))
			{
				return false;
			}
			this.nodes.SpliceAbsolute(startIndex, toRemove, toInsert);
			this.nodeHashes.SpliceUninitializedAbsolute(startIndex, toRemove, num3);
			if (toInsert != null)
			{
				for (int i = 0; i < toInsert.Count; i++)
				{
					this.nodeHashes.SetAbsolute(startIndex + i, PathTracer.HashNode(toInsert[i]));
				}
			}
			int num4 = num3 - toRemove;
			int num5 = math.max(startIndex - 1, ptr.startIndex);
			int toRemove2 = math.min(startIndex + toRemove, ptr.endIndex) - num5;
			ptr.endIndex += num4;
			for (int j = this.firstPartIndex + 1; j < this.parts.Length; j++)
			{
				Funnel.PathPart[] array = this.parts;
				int num6 = j;
				array[num6].startIndex = array[num6].startIndex + num4;
				Funnel.PathPart[] array2 = this.parts;
				int num7 = j;
				array2[num7].endIndex = array2[num7].endIndex + num4;
			}
			List<float3> list = ListPool<float3>.Claim();
			List<float3> list2 = ListPool<float3>.Claim();
			int num8 = startIndex + num3 - 1;
			int num9 = math.max(startIndex - 1, ptr.startIndex);
			int endNodeIndex = math.min(num8 + 1, ptr.endIndex);
			this.CalculateFunnelPortals(num9, endNodeIndex, list, list2);
			this.funnelState.Splice(num9 - ptr.startIndex, toRemove2, list, list2);
			this.portalIsNotInnerCorner.SpliceUninitialized(num9 - ptr.startIndex, toRemove2, list.Count);
			for (int k = 0; k < list.Count; k++)
			{
				this.portalIsNotInnerCorner[num9 - ptr.startIndex + k] = 0;
			}
			ListPool<float3>.Release(ref list);
			ListPool<float3>.Release(ref list2);
			return true;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x000384BC File Offset: 0x000366BC
		private static bool ContainsPoint(GraphNode node, Vector3 point, NativeMovementPlane plane)
		{
			TriangleMeshNode triangleMeshNode = node as TriangleMeshNode;
			if (triangleMeshNode != null)
			{
				return triangleMeshNode.ContainsPoint(point, plane);
			}
			return node.ContainsPoint(point);
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x000384E3 File Offset: 0x000366E3
		[BurstCompile]
		private static bool ContainsAndProject(ref Int3 a, ref Int3 b, ref Int3 c, ref Vector3 p, float height, ref NativeMovementPlane movementPlane, out Vector3 projected)
		{
			return PathTracer.ContainsAndProject_00000992$BurstDirectCall.Invoke(ref a, ref b, ref c, ref p, height, ref movementPlane, out projected);
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x000384F4 File Offset: 0x000366F4
		private static float3 ProjectOnSurface(float3 a, float3 b, float3 c, float3 p, float3 up)
		{
			float3 x = math.cross(c - a, b - a);
			float num = math.dot(x, up);
			if (math.abs(num) > 1.1754944E-38f)
			{
				float3 y = p - a;
				float lhs = -math.dot(x, y) / num;
				return p + lhs * up;
			}
			return p;
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00038550 File Offset: 0x00036750
		private void Repair(Vector3 point, bool isStart, PathTracer.RepairQuality quality, NativeMovementPlane movementPlane, ITraversalProvider traversalProvider, Path path, bool allowCache = true)
		{
			int num;
			int num2;
			GraphNode absolute;
			bool flag;
			if (isStart)
			{
				num = this.firstPartIndex;
				num2 = this.parts[num].startIndex;
				absolute = this.nodes.GetAbsolute(num2);
				flag = (this.unclampedStartPoint == point);
			}
			else
			{
				num = this.parts.Length - 1;
				num2 = this.parts[num].endIndex;
				absolute = this.nodes.GetAbsolute(num2);
				flag = (this.unclampedEndPoint == point);
			}
			bool flag2 = this.ValidInPath(num2);
			if (allowCache && flag && flag2)
			{
				return;
			}
			if (this.partGraphType == PathTracer.PartGraphType.OffMeshLink)
			{
				throw new InvalidOperationException("Cannot repair path while on an off-mesh link");
			}
			ref Funnel.PathPart ptr = ref this.parts[num];
			ushort version;
			if (float.IsFinite(point.x))
			{
				if (flag2)
				{
					bool flag3 = false;
					Vector3 vector = Vector3.zero;
					TriangleMeshNode triangleMeshNode = absolute as TriangleMeshNode;
					if (triangleMeshNode != null)
					{
						Int3 @int;
						Int3 int2;
						Int3 int3;
						triangleMeshNode.GetVertices(out @int, out int2, out int3);
						flag3 = PathTracer.ContainsAndProject(ref @int, ref int2, ref int3, ref point, 1f, ref movementPlane, out vector);
					}
					else
					{
						GridNode gridNode = absolute as GridNode;
						if (gridNode != null && gridNode.ContainsPoint(point))
						{
							flag3 = true;
							vector = gridNode.ClosestPointOnNode(point);
						}
					}
					if (flag3)
					{
						if (isStart)
						{
							ptr.startPoint = vector;
							this.unclampedStartPoint = point;
							this.startIsUpToDate = true;
							this.startNode = absolute;
						}
						else
						{
							ptr.endPoint = vector;
							this.unclampedEndPoint = point;
							this.endIsUpToDate = true;
						}
						version = this.version;
						this.version = version + 1;
						return;
					}
				}
				this.RepairFull(point, isStart, quality, movementPlane, traversalProvider, path);
				version = this.version;
				this.version = version + 1;
				return;
			}
			if (isStart)
			{
				throw new ArgumentException("Position must be a finite vector");
			}
			this.unclampedEndPoint = point;
			this.endIsUpToDate = false;
			this.RemoveAllPartsExceptFirst();
			ref Funnel.PathPart ptr2 = ref this.parts[this.firstPartIndex];
			if (ptr2.endIndex > ptr2.startIndex)
			{
				this.SplicePath(ptr2.startIndex + 1, ptr2.endIndex - ptr2.startIndex, null);
			}
			ptr2.endPoint = ptr2.startPoint;
			version = this.version;
			this.version = version + 1;
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x00038774 File Offset: 0x00036974
		private unsafe void HeuristicallyPopPortals(bool isStartOfPart, Vector3 point)
		{
			ref Funnel.PathPart ptr = ref this.parts[this.firstPartIndex];
			if (isStartOfPart)
			{
				while (this.funnelState.IsReasonableToPopStart(point, ptr.endPoint))
				{
					ptr.startIndex++;
					this.nodes.PopStart();
					this.nodeHashes.PopStart();
					this.funnelState.PopStart();
					this.portalIsNotInnerCorner.PopStart();
				}
				if (this.ValidInPath(this.nodes.AbsoluteStartIndex))
				{
					this.startNode = *this.nodes.First;
				}
			}
			else
			{
				int num = 0;
				while (this.funnelState.IsReasonableToPopEnd(ptr.startPoint, point))
				{
					ptr.endIndex--;
					num++;
					this.funnelState.PopEnd();
					this.portalIsNotInnerCorner.PopEnd();
				}
				if (num > 0)
				{
					this.nodes.SpliceAbsolute(ptr.endIndex + 1, num, null);
					this.nodeHashes.SpliceAbsolute(ptr.endIndex + 1, num, null);
					for (int i = this.firstPartIndex + 1; i < this.parts.Length; i++)
					{
						Funnel.PathPart[] array = this.parts;
						int num2 = i;
						array[num2].startIndex = array[num2].startIndex - num;
						Funnel.PathPart[] array2 = this.parts;
						int num3 = i;
						array2[num3].endIndex = array2[num3].endIndex - num;
					}
				}
			}
			int num4 = Mathf.Min(ptr.startIndex + 5, ptr.endIndex);
			bool flag = false;
			for (int j = ptr.startIndex; j <= num4; j++)
			{
				flag |= !this.ValidInPath(j);
			}
			this.firstPartContainsDestroyedNodes = flag;
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x000035CE File Offset: 0x000017CE
		[Conditional("UNITY_ASSERTIONS")]
		private void AssertValidInPath(int absoluteNodeIndex)
		{
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x00038922 File Offset: 0x00036B22
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly bool ValidInPath(int absoluteNodeIndex)
		{
			return PathTracer.HashNode(this.nodes.GetAbsolute(absoluteNodeIndex)) == this.nodeHashes.GetAbsolute(absoluteNodeIndex);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x00038943 File Offset: 0x00036B43
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool Valid(GraphNode node)
		{
			return !node.Destroyed && node.Walkable;
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x00038958 File Offset: 0x00036B58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int HashNode(GraphNode node)
		{
			int num = (int)node.NodeIndex;
			num ^= (node.Walkable ? 100663319 : 0);
			GridNodeBase gridNodeBase = node as GridNodeBase;
			if (gridNodeBase != null)
			{
				num ^= gridNodeBase.NodeInGridIndex * 25165843;
			}
			return num;
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0003899C File Offset: 0x00036B9C
		private unsafe void RepairFull(Vector3 point, bool isStart, PathTracer.RepairQuality quality, NativeMovementPlane movementPlane, ITraversalProvider traversalProvider, Path path)
		{
			int maxNodesToSearch = (quality == PathTracer.RepairQuality.High) ? 16 : 9;
			int num = isStart ? this.firstPartIndex : (this.parts.Length - 1);
			ref Funnel.PathPart ptr = ref this.parts[num];
			int num2 = isStart ? ptr.startIndex : ptr.endIndex;
			if ((!this.ValidInPath(num2) || (ptr.endIndex != ptr.startIndex && !this.ValidInPath(isStart ? (ptr.startIndex + 1) : (ptr.endIndex - 1)))) && num == this.firstPartIndex)
			{
				this.HeuristicallyPopPortals(isStart, point);
				num2 = (isStart ? ptr.startIndex : ptr.endIndex);
			}
			if (!this.ValidInPath(num2))
			{
				if (!isStart)
				{
					this.unclampedEndPoint = point;
					ptr.endPoint = point;
					this.endIsUpToDate = false;
					return;
				}
				this.firstPartContainsDestroyedNodes = true;
				this.unclampedStartPoint = point;
				this.startIsUpToDate = false;
				NNConstraint nnconstraint = this.nnConstraint;
				nnconstraint.distanceMetric = DistanceMetric.ClosestAsSeenFromAboveSoft(movementPlane.ToWorld(float2.zero, 1f));
				GraphNode graphNode = (AstarPath.active != null) ? AstarPath.active.GetNearest(point, nnconstraint).node : null;
				if (traversalProvider != null && graphNode != null && !traversalProvider.CanTraverse(path, graphNode))
				{
					graphNode = null;
				}
				this.startNode = graphNode;
				if (this.startNode == null)
				{
					ptr.startPoint = point;
					return;
				}
				ptr.startPoint = this.startNode.ClosestPointOnNode(point);
				if (ptr.endIndex - ptr.startIndex < 10 && this.partCount <= 1)
				{
					Vector3 startPoint = ptr.startPoint;
					this.Clear();
					this.startNode = graphNode;
					this.partGraphType = PathTracer.PartGraphTypeFromNode(this.startNode);
					this.unclampedStartPoint = point;
					this.unclampedEndPoint = startPoint;
					this.nodes.PushEnd(graphNode);
					this.nodeHashes.PushEnd(PathTracer.HashNode(graphNode));
					this.parts = new Funnel.PathPart[1];
					this.parts[0] = new Funnel.PathPart
					{
						startIndex = this.nodes.AbsoluteStartIndex,
						endIndex = this.nodes.AbsoluteEndIndex,
						startPoint = startPoint,
						endPoint = startPoint
					};
					return;
				}
			}
			else
			{
				CircularBuffer<GraphNode> path2 = this.LocalSearch(this.nodes.GetAbsolute(num2), point, maxNodesToSearch, movementPlane, isStart, traversalProvider, path);
				GraphNode graphNode2 = *path2.Last;
				NNConstraint nnconstraint2 = this.nnConstraint;
				nnconstraint2.constrainArea = true;
				nnconstraint2.area = (int)graphNode2.Area;
				NNInfo nearest = AstarPath.active.GetNearest(point, nnconstraint2);
				nnconstraint2.constrainArea = false;
				Vector3 vector = isStart ? ptr.startPoint : ptr.endPoint;
				bool flag;
				Vector3 vector2;
				if (nearest.node == graphNode2)
				{
					flag = true;
					vector2 = nearest.position;
				}
				else
				{
					float sqrMagnitude = ((isStart ? this.unclampedStartPoint : this.unclampedEndPoint) - vector).sqrMagnitude;
					bool flag2 = isStart ? this.startIsUpToDate : this.endIsUpToDate;
					vector2 = graphNode2.ClosestPointOnNode(point);
					float sqrMagnitude2 = (point - vector2).sqrMagnitude;
					flag = (flag2 && sqrMagnitude2 <= sqrMagnitude + 0.010000001f);
				}
				if (!flag && !isStart)
				{
					path2.Clear();
					vector2 = vector;
				}
				this.AppendPath(isStart, path2);
				path2.Pool();
				if (isStart)
				{
					this.startNode = *this.nodes.First;
				}
				if (isStart)
				{
					this.unclampedStartPoint = point;
					ptr.startPoint = vector2;
					this.startIsUpToDate = true;
					return;
				}
				this.unclampedEndPoint = point;
				ptr.endPoint = vector2;
				this.endIsUpToDate = flag;
			}
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x00038D40 File Offset: 0x00036F40
		private static float SquaredDistanceToNode(GraphNode node, Vector3 point, ref BBTree.ProjectionParams projectionParams)
		{
			TriangleMeshNode triangleMeshNode = node as TriangleMeshNode;
			if (triangleMeshNode != null)
			{
				Int3 @int;
				Int3 int2;
				Int3 int3;
				triangleMeshNode.GetVerticesInGraphSpace(out @int, out int2, out int3);
				float3 @float;
				float result;
				float num;
				Polygon.ClosestPointOnTriangleProjected(ref @int, ref int2, ref int3, ref projectionParams, UnsafeUtility.As<Vector3, float3>(ref point), out @float, out result, out num);
				return result;
			}
			GridNodeBase gridNodeBase = node as GridNodeBase;
			if (gridNodeBase != null)
			{
				Vector2Int coordinatesInGrid = gridNodeBase.CoordinatesInGrid;
				float x = math.clamp(point.x, (float)coordinatesInGrid.x, (float)(coordinatesInGrid.x + 1));
				float z = math.clamp(point.z, (float)coordinatesInGrid.y, (float)(coordinatesInGrid.y + 1));
				return math.lengthsq(new float3(x, 0f, z) - point);
			}
			Vector3 b = node.ClosestPointOnNode(point);
			return (point - b).sqrMagnitude;
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x00038E08 File Offset: 0x00037008
		private static bool QueueHasNode(PathTracer.QueueItem[] queue, int count, GraphNode node)
		{
			for (int i = 0; i < count; i++)
			{
				if (queue[i].node == node)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x00038E34 File Offset: 0x00037034
		private void GetTempQueue(out PathTracer.QueueItem[] queue, out List<GraphNode> connections)
		{
			int threadIndex = JobsUtility.ThreadIndex;
			queue = PathTracer.TempQueues[threadIndex];
			connections = PathTracer.TempConnectionLists[threadIndex];
			if (queue == null)
			{
				queue = (PathTracer.TempQueues[threadIndex] = new PathTracer.QueueItem[16]);
				connections = (PathTracer.TempConnectionLists[threadIndex] = new List<GraphNode>());
			}
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00038E84 File Offset: 0x00037084
		private CircularBuffer<GraphNode> LocalSearch(GraphNode currentNode, Vector3 point, int maxNodesToSearch, NativeMovementPlane movementPlane, bool reverse, ITraversalProvider traversalProvider, Path path)
		{
			NNConstraint nnconstraint = this.nnConstraint;
			nnconstraint.distanceMetric = DistanceMetric.ClosestAsSeenFromAboveSoft(movementPlane.up);
			PathTracer.QueueItem[] array;
			List<GraphNode> list;
			this.GetTempQueue(out array, out list);
			int i = 0;
			int num = 0;
			NavGraph graph = currentNode.Graph;
			BBTree.ProjectionParams projectionParams;
			Vector3 point2;
			if (this.partGraphType == PathTracer.PartGraphType.Navmesh)
			{
				NavmeshBase navmeshBase = graph as NavmeshBase;
				projectionParams = new BBTree.ProjectionParams(nnconstraint, navmeshBase.transform);
				point2 = navmeshBase.transform.InverseTransform(point);
			}
			else if (this.partGraphType == PathTracer.PartGraphType.Grid)
			{
				projectionParams = default(BBTree.ProjectionParams);
				point2 = (graph as GridGraph).transform.InverseTransform(point);
			}
			else
			{
				projectionParams = default(BBTree.ProjectionParams);
				point2 = point;
			}
			float num2 = PathTracer.SquaredDistanceToNode(currentNode, point2, ref projectionParams);
			array[0] = new PathTracer.QueueItem
			{
				node = currentNode,
				parent = -1,
				distance = num2
			};
			num++;
			int num3 = 0;
			while (i < num)
			{
				int num4 = i;
				GraphNode node2 = array[num4].node;
				i++;
				if (PathTracer.ContainsPoint(node2, point, movementPlane))
				{
					num3 = num4;
					break;
				}
				float distance = array[num4].distance;
				if (distance < num2)
				{
					num2 = distance;
					num3 = num4;
				}
				float num5 = distance * 1.1024998f + 0.05f;
				node2.GetConnections<List<GraphNode>>(delegate(GraphNode node, ref List<GraphNode> ls)
				{
					ls.Add(node);
				}, ref list, 32);
				for (int j = 0; j < list.Count; j++)
				{
					GraphNode graphNode = list[j];
					if (num < maxNodesToSearch && graphNode.GraphIndex == node2.GraphIndex && nnconstraint.Suitable(graphNode) && (traversalProvider == null || (reverse ? (traversalProvider.CanTraverse(path, graphNode) && traversalProvider.CanTraverse(path, graphNode, node2)) : traversalProvider.CanTraverse(path, node2, graphNode))))
					{
						float num6 = PathTracer.SquaredDistanceToNode(graphNode, point2, ref projectionParams);
						if (num6 < num5 && !PathTracer.QueueHasNode(array, num, graphNode))
						{
							array[num] = new PathTracer.QueueItem
							{
								node = graphNode,
								parent = num4,
								distance = num6
							};
							num++;
						}
					}
				}
				list.Clear();
			}
			CircularBuffer<GraphNode> result = new CircularBuffer<GraphNode>(8);
			while (num3 != -1)
			{
				result.PushStart(array[num3].node);
				num3 = array[num3].parent;
			}
			list.Clear();
			for (int k = 0; k < num; k++)
			{
				array[k].node = null;
			}
			if (this.partGraphType == PathTracer.PartGraphType.Grid)
			{
				CircularBuffer<int> circularBuffer = default(CircularBuffer<int>);
				PathTracer.RemoveGridPathDiagonals(null, 0, ref result, ref circularBuffer, this.nnConstraint, traversalProvider, path);
			}
			return result;
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00039158 File Offset: 0x00037358
		public void DrawFunnel(CommandBuilder draw, NativeMovementPlane movementPlane)
		{
			if (this.parts == null)
			{
				return;
			}
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex];
			this.funnelState.PushStart(pathPart.startPoint, pathPart.startPoint);
			this.funnelState.PushEnd(pathPart.endPoint, pathPart.endPoint);
			using (draw.WithLineWidth(2f, true))
			{
				draw.Polyline<NativeCircularBuffer<float3>>(this.funnelState.leftFunnel, false);
				draw.Polyline<NativeCircularBuffer<float3>>(this.funnelState.rightFunnel, false);
			}
			if (this.funnelState.unwrappedPortals.Length > 1)
			{
				using (draw.WithLineWidth(1f, true))
				{
					float3 up = movementPlane.up;
					float4x3 float4x = this.funnelState.UnwrappedPortalsToWorldMatrix(up);
					float4x4 m = new float4x4(float4x.c0, float4x.c1, new float4(0f, 0f, 1f, 0f), float4x.c2);
					using (draw.WithMatrix(m))
					{
						float2 a = this.funnelState.unwrappedPortals[0].xy;
						float2 a2 = this.funnelState.unwrappedPortals[1].xy;
						for (int i = 0; i < this.funnelState.unwrappedPortals.Length; i++)
						{
							float2 xy = this.funnelState.unwrappedPortals[i].xy;
							float2 zw = this.funnelState.unwrappedPortals[i].zw;
							draw.xy.Line(xy, zw, Palette.Colorbrewer.Set1.Brown);
							draw.xy.Line(a, xy, Palette.Colorbrewer.Set1.Brown);
							draw.xy.Line(a2, zw, Palette.Colorbrewer.Set1.Brown);
							a = xy;
							a2 = zw;
						}
					}
				}
			}
			using (draw.WithColor(new Color(0f, 0f, 0f, 0.2f)))
			{
				for (int j = 0; j < this.funnelState.leftFunnel.Length - 1; j++)
				{
					draw.SolidTriangle(this.funnelState.leftFunnel[j], this.funnelState.rightFunnel[j], this.funnelState.rightFunnel[j + 1]);
					draw.SolidTriangle(this.funnelState.leftFunnel[j], this.funnelState.leftFunnel[j + 1], this.funnelState.rightFunnel[j + 1]);
				}
			}
			using (draw.WithColor(new Color(0f, 0f, 1f, 0.5f)))
			{
				for (int k = 0; k < this.funnelState.leftFunnel.Length; k++)
				{
					draw.Line(this.funnelState.leftFunnel[k], this.funnelState.rightFunnel[k]);
				}
			}
			this.funnelState.PopStart();
			this.funnelState.PopEnd();
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00039568 File Offset: 0x00037768
		private static Int3 MaybeSetYZero(Int3 p, bool setYToZero)
		{
			if (setYToZero)
			{
				p.y = 0;
			}
			return p;
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00039578 File Offset: 0x00037778
		private static bool IsInnerVertex(CircularBuffer<GraphNode> nodes, Funnel.PathPart part, int portalIndex, bool rightSide, List<GraphNode> alternativeNodes, NNConstraint nnConstraint, out int startIndex, out int endIndex, ITraversalProvider traversalProvider, Path path)
		{
			GraphNode absolute = nodes.GetAbsolute(portalIndex);
			if (absolute is TriangleMeshNode)
			{
				return PathTracer.IsInnerVertexTriangleMesh(nodes, part, portalIndex, rightSide, alternativeNodes, nnConstraint, out startIndex, out endIndex, traversalProvider, path);
			}
			if (absolute is GridNodeBase)
			{
				throw new InvalidOperationException("Grid nodes are not supported. Should have been handled by the SimplifyGridInnerVertex method");
			}
			startIndex = portalIndex;
			endIndex = portalIndex + 1;
			return false;
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x000395CC File Offset: 0x000377CC
		private static bool IsInnerVertexTriangleMesh(CircularBuffer<GraphNode> nodes, Funnel.PathPart part, int portalIndex, bool rightSide, List<GraphNode> alternativeNodes, NNConstraint nnConstraint, out int startIndex, out int endIndex, ITraversalProvider traversalProvider, Path path)
		{
			startIndex = portalIndex;
			endIndex = portalIndex + 1;
			TriangleMeshNode triangleMeshNode = nodes.GetAbsolute(startIndex) as TriangleMeshNode;
			TriangleMeshNode triangleMeshNode2 = nodes.GetAbsolute(endIndex) as TriangleMeshNode;
			if (triangleMeshNode == null || triangleMeshNode2 == null || !PathTracer.Valid(triangleMeshNode) || !PathTracer.Valid(triangleMeshNode2))
			{
				return false;
			}
			Int3 @int;
			Int3 int2;
			int num;
			int num2;
			if (!triangleMeshNode.GetPortalInGraphSpace(triangleMeshNode2, out @int, out int2, out num, out num2))
			{
				return false;
			}
			bool recalculateNormals = (triangleMeshNode.Graph as NavmeshBase).RecalculateNormals;
			Int3 rhs = PathTracer.MaybeSetYZero(rightSide ? int2 : @int, recalculateNormals);
			while (startIndex > part.startIndex)
			{
				TriangleMeshNode triangleMeshNode3 = nodes.GetAbsolute(startIndex - 1) as TriangleMeshNode;
				Int3 int3;
				Int3 int4;
				if (triangleMeshNode3 == null || !PathTracer.Valid(triangleMeshNode3) || !triangleMeshNode3.GetPortalInGraphSpace(triangleMeshNode, out int3, out int4, out num2, out num) || !(PathTracer.MaybeSetYZero(rightSide ? int4 : int3, recalculateNormals) == rhs))
				{
					break;
				}
				triangleMeshNode = triangleMeshNode3;
				startIndex--;
			}
			while (endIndex < part.endIndex)
			{
				TriangleMeshNode triangleMeshNode4 = nodes.GetAbsolute(endIndex + 1) as TriangleMeshNode;
				Int3 int5;
				Int3 int6;
				if (triangleMeshNode4 == null || !PathTracer.Valid(triangleMeshNode4) || !triangleMeshNode2.GetPortalInGraphSpace(triangleMeshNode4, out int5, out int6, out num, out num2) || !(PathTracer.MaybeSetYZero(rightSide ? int6 : int5, recalculateNormals) == rhs))
				{
					break;
				}
				triangleMeshNode2 = triangleMeshNode4;
				endIndex++;
				if (triangleMeshNode2 == triangleMeshNode)
				{
					break;
				}
			}
			TriangleMeshNode triangleMeshNode5 = triangleMeshNode;
			int num3 = 0;
			alternativeNodes.Add(triangleMeshNode);
			if (triangleMeshNode == triangleMeshNode2)
			{
				return true;
			}
			bool flag = false;
			while (!flag)
			{
				bool flag2 = false;
				int i = 0;
				while (i < triangleMeshNode5.connections.Length)
				{
					TriangleMeshNode triangleMeshNode6 = triangleMeshNode5.connections[i].node as TriangleMeshNode;
					Int3 int7;
					Int3 int8;
					if (triangleMeshNode6 != null && ((traversalProvider != null) ? traversalProvider.CanTraverse(path, triangleMeshNode5, triangleMeshNode6) : nnConstraint.Suitable(triangleMeshNode6)) && triangleMeshNode5.connections[i].isOutgoing && triangleMeshNode5.GetPortalInGraphSpace(triangleMeshNode6, out int7, out int8, out num2, out num) && PathTracer.MaybeSetYZero(rightSide ? int7 : int8, recalculateNormals) == rhs)
					{
						triangleMeshNode5 = triangleMeshNode6;
						alternativeNodes.Add(triangleMeshNode5);
						flag2 = true;
						if (num3++ > 100)
						{
							throw new Exception("Caught in a potentially infinite loop. The navmesh probably contains degenerate geometry.");
						}
						if (triangleMeshNode5 == triangleMeshNode2)
						{
							flag = true;
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
				if (!flag2)
				{
					return false;
				}
			}
			int num4 = 0;
			for (int j = 0; j < alternativeNodes.Count; j++)
			{
				num4 += (int)((traversalProvider != null) ? traversalProvider.GetTraversalCost(path, alternativeNodes[j]) : DefaultITraversalProvider.GetTraversalCost(path, alternativeNodes[j]));
			}
			for (int k = startIndex; k < endIndex; k++)
			{
				num4 -= (int)((traversalProvider != null) ? traversalProvider.GetTraversalCost(path, nodes.GetAbsolute(k)) : DefaultITraversalProvider.GetTraversalCost(path, nodes.GetAbsolute(k)));
			}
			return num4 <= 0;
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x000398A4 File Offset: 0x00037AA4
		private bool FirstInnerVertex(NativeArray<int> indices, int numCorners, List<GraphNode> alternativePath, out int alternativeStartIndex, out int alternativeEndIndex, ITraversalProvider traversalProvider, Path path)
		{
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex];
			for (int i = 0; i < numCorners; i++)
			{
				int num = indices[i];
				bool flag = (num & 1073741824) != 0;
				int num2 = num & 1073741823;
				if ((this.portalIsNotInnerCorner[num2] & ((flag || 2 != 0) ? 1 : 0)) == 0)
				{
					alternativePath.Clear();
					if (PathTracer.IsInnerVertex(this.nodes, pathPart, pathPart.startIndex + num2, flag, alternativePath, this.nnConstraint, out alternativeStartIndex, out alternativeEndIndex, traversalProvider, path))
					{
						return true;
					}
					this.portalIsNotInnerCorner[num2] = (this.portalIsNotInnerCorner[num2] | (flag ? 1 : 2));
				}
			}
			alternativeStartIndex = -1;
			alternativeEndIndex = -1;
			return false;
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0003995D File Offset: 0x00037B5D
		public float EstimateRemainingPath(int maxCorners, ref NativeMovementPlane movementPlane)
		{
			return PathTracer.EstimateRemainingPath(ref this.funnelState, ref this.parts[this.firstPartIndex], maxCorners, ref movementPlane);
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0003997D File Offset: 0x00037B7D
		[BurstCompile]
		private static float EstimateRemainingPath(ref Funnel.FunnelState funnelState, ref Funnel.PathPart part, int maxCorners, ref NativeMovementPlane movementPlane)
		{
			return PathTracer.EstimateRemainingPath_000009A5$BurstDirectCall.Invoke(ref funnelState, ref part, maxCorners, ref movementPlane);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00039988 File Offset: 0x00037B88
		public void GetNextCorners(NativeList<float3> buffer, int maxCorners, ref NativeArray<int> scratchArray, Allocator allocator, ITraversalProvider traversalProvider, Path path)
		{
			bool lastCorner;
			int nextCornerIndices = this.GetNextCornerIndices(ref scratchArray, maxCorners, allocator, out lastCorner, traversalProvider, path);
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex];
			this.funnelState.ConvertCornerIndicesToPath(scratchArray, nextCornerIndices, false, pathPart.startPoint, pathPart.endPoint, lastCorner, buffer);
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x000399E4 File Offset: 0x00037BE4
		public int GetNextCornerIndices(ref NativeArray<int> buffer, int maxCorners, Allocator allocator, out bool lastCorner, ITraversalProvider traversalProvider, Path path)
		{
			int num = 3;
			maxCorners--;
			if (PathTracer.scratchList == null)
			{
				PathTracer.scratchList = new List<GraphNode>(8);
			}
			List<GraphNode> list = PathTracer.scratchList;
			int num3;
			for (;;)
			{
				int num2 = math.max(0, math.min(maxCorners, this.funnelState.leftFunnel.Length));
				if (!buffer.IsCreated || buffer.Length < num2)
				{
					if (buffer.IsCreated)
					{
						buffer.Dispose();
					}
					buffer = new NativeArray<int>(math.ceilpow2(num2), allocator, NativeArrayOptions.UninitializedMemory);
				}
				NativeArray<int> nativeArray = buffer;
				Funnel.PathPart pathPart = this.parts[this.firstPartIndex];
				num3 = this.funnelState.CalculateNextCornerIndices(num2, nativeArray, pathPart.startPoint, pathPart.endPoint, out lastCorner);
				if (num <= 0)
				{
					return num3;
				}
				if (this.partGraphType == PathTracer.PartGraphType.Grid)
				{
					int num4;
					int num5;
					if (!PathTracer.SimplifyGridInnerVertex(ref this.nodes, nativeArray.AsUnsafeSpan<int>().Slice(0, num3), pathPart, ref this.portalIsNotInnerCorner, list, out num4, out num5, this.nnConstraint, traversalProvider, path, lastCorner))
					{
						return num3;
					}
					if (!this.SplicePath(num4, num5 - num4 + 1, list))
					{
						break;
					}
					num--;
					ushort version = this.version;
					this.version = version + 1;
				}
				else
				{
					int num6;
					int num7;
					if (!this.FirstInnerVertex(nativeArray, num3, list, out num6, out num7, traversalProvider, path))
					{
						return num3;
					}
					if (!this.SplicePath(num6, num7 - num6 + 1, list))
					{
						goto IL_176;
					}
					num--;
					ushort version = this.version;
					this.version = version + 1;
				}
			}
			this.firstPartContainsDestroyedNodes = true;
			return num3;
			IL_176:
			this.firstPartContainsDestroyedNodes = true;
			return num3;
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00039B70 File Offset: 0x00037D70
		public void ConvertCornerIndicesToPathProjected(NativeArray<int> cornerIndices, int numCorners, bool lastCorner, NativeList<float3> buffer, float3 up)
		{
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex];
			this.funnelState.ConvertCornerIndicesToPathProjected(cornerIndices.AsUnsafeReadOnlySpan<int>().Slice(0, numCorners), false, pathPart.startPoint, pathPart.endPoint, lastCorner, buffer, up);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00039BC6 File Offset: 0x00037DC6
		[BurstCompile]
		public static float RemainingDistanceLowerBound(in UnsafeSpan<float3> nextCorners, in float3 endOfPart, in NativeMovementPlane movementPlane)
		{
			return PathTracer.RemainingDistanceLowerBound_000009A9$BurstDirectCall.Invoke(nextCorners, endOfPart, movementPlane);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00039BD0 File Offset: 0x00037DD0
		public unsafe void PopParts(int count, ITraversalProvider traversalProvider, Path path)
		{
			if (this.firstPartIndex + count >= this.parts.Length)
			{
				throw new InvalidOperationException("Cannot pop the last part of a path");
			}
			this.firstPartIndex += count;
			ushort version = this.version;
			this.version = version + 1;
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex];
			while (this.nodes.AbsoluteStartIndex < pathPart.startIndex)
			{
				this.nodes.PopStart();
				this.nodeHashes.PopStart();
			}
			this.startNode = ((this.nodes.Length > 0) ? (*this.nodes.First) : null);
			this.firstPartContainsDestroyedNodes = false;
			if (this.GetPartType(0) == Funnel.PartType.OffMeshLink)
			{
				this.partGraphType = PathTracer.PartGraphType.OffMeshLink;
				this.SetFunnelState(pathPart);
				return;
			}
			this.partGraphType = PathTracer.PartGraphTypeFromNode(this.startNode);
			int i = pathPart.startIndex;
			while (i <= pathPart.endIndex)
			{
				if (!this.ValidInPath(i))
				{
					this.RemoveAllPartsExceptFirst();
					while (this.nodes.AbsoluteEndIndex > i)
					{
						this.nodes.PopEnd();
						this.nodeHashes.PopEnd();
					}
					pathPart.endIndex = i;
					this.parts[this.firstPartIndex] = pathPart;
					if (i == pathPart.startIndex)
					{
						this.firstPartContainsDestroyedNodes = true;
						this.funnelState.Clear();
						this.portalIsNotInnerCorner.Clear();
						this.startNode = null;
						return;
					}
					this.endIsUpToDate = false;
					this.nodes.PopEnd();
					this.nodeHashes.PopEnd();
					pathPart.endIndex = i - 1;
					this.parts[this.firstPartIndex] = pathPart;
					break;
				}
				else
				{
					i++;
				}
			}
			if (this.partGraphType == PathTracer.PartGraphType.Grid)
			{
				PathTracer.RemoveGridPathDiagonals(this.parts, this.firstPartIndex, ref this.nodes, ref this.nodeHashes, this.nnConstraint, traversalProvider, path);
				pathPart = this.parts[this.firstPartIndex];
			}
			this.SetFunnelState(pathPart);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00039DD0 File Offset: 0x00037FD0
		public void RemoveAllButFirstNode(NativeMovementPlane movementPlane, ITraversalProvider traversalProvider)
		{
			PathRequestSettings pathfindingSettings = new PathRequestSettings
			{
				graphMask = this.nnConstraint.graphMask,
				traversableTags = this.nnConstraint.tags,
				tagPenalties = null,
				traversalProvider = traversalProvider
			};
			this.SetFromSingleNode(this.startNode, this.startPoint, movementPlane, pathfindingSettings);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00039E30 File Offset: 0x00038030
		private void RemoveAllPartsExceptFirst()
		{
			if (this.partCount <= 1)
			{
				return;
			}
			this.parts = new Funnel.PathPart[]
			{
				this.parts[this.firstPartIndex]
			};
			this.firstPartIndex = 0;
			while (this.nodes.AbsoluteEndIndex > this.parts[0].endIndex)
			{
				this.nodes.PopEnd();
				this.nodeHashes.PopEnd();
			}
			ushort version = this.version;
			this.version = version + 1;
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00039EBD File Offset: 0x000380BD
		public readonly Funnel.PartType GetPartType(int partIndex = 0)
		{
			return this.parts[this.firstPartIndex + partIndex].type;
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00039ED8 File Offset: 0x000380D8
		public readonly bool PartContainsDestroyedNodes(int partIndex = 0)
		{
			if (partIndex < 0 || partIndex >= this.partCount)
			{
				throw new ArgumentOutOfRangeException("partIndex");
			}
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex + partIndex];
			for (int i = pathPart.startIndex; i <= pathPart.endIndex; i++)
			{
				if (!this.ValidInPath(i))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00039F34 File Offset: 0x00038134
		public OffMeshLinks.OffMeshLinkTracer GetLinkInfo(int partIndex = 0)
		{
			if (partIndex < 0 || partIndex >= this.partCount)
			{
				throw new ArgumentOutOfRangeException("partIndex");
			}
			if (this.GetPartType(partIndex) != Funnel.PartType.OffMeshLink)
			{
				throw new ArgumentException("Part is not an off-mesh link");
			}
			Funnel.PathPart pathPart = this.parts[this.firstPartIndex + partIndex];
			LinkNode linkNode = this.nodes.GetAbsolute(pathPart.startIndex) as LinkNode;
			LinkNode linkNode2 = this.nodes.GetAbsolute(pathPart.endIndex) as LinkNode;
			if (linkNode == null)
			{
				throw new Exception("Expected a link node");
			}
			if (linkNode2 == null)
			{
				throw new Exception("Expected a link node");
			}
			if (linkNode.Destroyed)
			{
				throw new Exception("Start node is destroyed");
			}
			if (linkNode2.Destroyed)
			{
				throw new Exception("End node is destroyed");
			}
			bool reversed;
			if (linkNode.linkConcrete.startLinkNode == linkNode)
			{
				reversed = false;
			}
			else
			{
				if (linkNode.linkConcrete.startLinkNode != linkNode2)
				{
					throw new Exception("Link node is not part of the link");
				}
				reversed = true;
			}
			return new OffMeshLinks.OffMeshLinkTracer(linkNode.linkConcrete, reversed);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0003A02C File Offset: 0x0003822C
		private void SetFunnelState(Funnel.PathPart part)
		{
			this.funnelState.Clear();
			this.portalIsNotInnerCorner.Clear();
			if (part.type == Funnel.PartType.NodeSequence)
			{
				GridGraph gridGraph = this.nodes.GetAbsolute(part.startIndex).Graph as GridGraph;
				if (gridGraph != null)
				{
					this.funnelState.projectionAxis = gridGraph.transform.WorldUpAtGraphPosition(Vector3.zero);
				}
				List<float3> list = ListPool<float3>.Claim(part.endIndex - part.startIndex);
				List<float3> list2 = ListPool<float3>.Claim(part.endIndex - part.startIndex);
				this.CalculateFunnelPortals(part.startIndex, part.endIndex, list, list2);
				this.funnelState.Splice(0, 0, list, list2);
				for (int i = 0; i < list.Count; i++)
				{
					this.portalIsNotInnerCorner.PushEnd(0);
				}
				ListPool<float3>.Release(ref list);
				ListPool<float3>.Release(ref list2);
			}
			ushort version = this.version;
			this.version = version + 1;
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0003A124 File Offset: 0x00038324
		private void CalculateFunnelPortals(int startNodeIndex, int endNodeIndex, List<float3> outLeftPortals, List<float3> outRightPortals)
		{
			GraphNode graphNode = this.nodes.GetAbsolute(startNodeIndex);
			for (int i = startNodeIndex + 1; i <= endNodeIndex; i++)
			{
				GraphNode absolute = this.nodes.GetAbsolute(i);
				Vector3 v;
				Vector3 v2;
				if (!graphNode.GetPortal(absolute, out v, out v2))
				{
					string[] array = new string[6];
					array[0] = "Couldn't find a portal from ";
					int num = 1;
					GraphNode graphNode2 = graphNode;
					array[num] = ((graphNode2 != null) ? graphNode2.ToString() : null);
					array[2] = " ";
					int num2 = 3;
					GraphNode graphNode3 = absolute;
					array[num2] = ((graphNode3 != null) ? graphNode3.ToString() : null);
					array[4] = " ";
					array[5] = graphNode.ContainsOutgoingConnection(absolute).ToString();
					throw new InvalidOperationException(string.Concat(array));
				}
				outLeftPortals.Add(v);
				outRightPortals.Add(v2);
				graphNode = absolute;
			}
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0003A1E8 File Offset: 0x000383E8
		public void SetFromSingleNode(GraphNode node, Vector3 position, NativeMovementPlane movementPlane, PathRequestSettings pathfindingSettings)
		{
			this.SetPath(new List<Funnel.PathPart>
			{
				new Funnel.PathPart
				{
					startIndex = 0,
					endIndex = 0,
					startPoint = position,
					endPoint = position
				}
			}, new List<GraphNode>
			{
				node
			}, position, position, movementPlane, pathfindingSettings, null);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0003A244 File Offset: 0x00038444
		public void Clear()
		{
			this.funnelState.Clear();
			this.parts = null;
			this.nodes.Clear();
			this.nodeHashes.Clear();
			this.portalIsNotInnerCorner.Clear();
			this.unclampedEndPoint = (this.unclampedStartPoint = Vector3.zero);
			this.firstPartIndex = 0;
			this.startIsUpToDate = false;
			this.endIsUpToDate = false;
			this.firstPartContainsDestroyedNodes = false;
			this.startNodeInternal = null;
			this.partGraphType = PathTracer.PartGraphType.Navmesh;
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0003A2C4 File Offset: 0x000384C4
		private unsafe static int2 ResolveNormalizedGridPoint(GridGraph grid, ref CircularBuffer<GraphNode> nodes, UnsafeSpan<int> cornerIndices, Funnel.PathPart part, int index, out int nodeIndex)
		{
			if (index < 0 || index >= cornerIndices.Length)
			{
				Vector3 point = (index < 0) ? part.startPoint : part.endPoint;
				nodeIndex = ((index < 0) ? part.startIndex : part.endIndex);
				Vector3 vector = grid.transform.InverseTransform(point);
				Vector2Int coordinatesInGrid = (nodes.GetAbsolute(nodeIndex) as GridNodeBase).CoordinatesInGrid;
				return new int2(math.clamp((int)(1024f * (vector.x - (float)coordinatesInGrid.x)), 0, 1024), math.clamp((int)(1024f * (vector.z - (float)coordinatesInGrid.y)), 0, 1024));
			}
			bool flag = (*cornerIndices[index] & 1073741824) != 0;
			nodeIndex = part.startIndex + (*cornerIndices[index] & 1073741823);
			GridNodeBase gridNodeBase = nodes.GetAbsolute(nodeIndex) as GridNodeBase;
			GridNodeBase gridNodeBase2 = nodes.GetAbsolute(nodeIndex + 1) as GridNodeBase;
			Vector2Int coordinatesInGrid2 = gridNodeBase.CoordinatesInGrid;
			Vector2Int coordinatesInGrid3 = gridNodeBase2.CoordinatesInGrid;
			int dx = coordinatesInGrid3.x - coordinatesInGrid2.x;
			int dz = coordinatesInGrid3.y - coordinatesInGrid2.y;
			int num = GridNodeBase.OffsetToConnectionDirection(dx, dz);
			if (num > 4)
			{
				throw new Exception("Diagonal connections are not supported");
			}
			int num2 = GridGraph.neighbourXOffsets[num] + GridGraph.neighbourXOffsets[(num + (flag ? -1 : 1) + 4) % 4];
			int num3 = GridGraph.neighbourZOffsets[num] + GridGraph.neighbourZOffsets[(num + (flag ? -1 : 1) + 4) % 4];
			return new int2(512 + 512 * num2, 512 + 512 * num3);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0003A470 File Offset: 0x00038670
		private unsafe static bool SimplifyGridInnerVertex(ref CircularBuffer<GraphNode> nodes, UnsafeSpan<int> cornerIndices, Funnel.PathPart part, ref CircularBuffer<byte> portalIsNotInnerCorner, List<GraphNode> alternativePath, out int alternativeStartIndex, out int alternativeEndIndex, NNConstraint nnConstraint, ITraversalProvider traversalProvider, Path path, bool lastCorner)
		{
			bool flag = (lastCorner ? cornerIndices.Length : (cornerIndices.Length - 1)) != 0;
			alternativeStartIndex = -1;
			alternativeEndIndex = -1;
			if (!flag)
			{
				return false;
			}
			int num = 0;
			int index = *cornerIndices[num] & 1073741823;
			int num2 = (int)(portalIsNotInnerCorner[index] % 32);
			portalIsNotInnerCorner[index] = (byte)(num2 + 1);
			if ((num2 & 3) != 0)
			{
				return false;
			}
			num2 /= 4;
			int num3 = (cornerIndices.length < 2U) ? part.endIndex : math.min(part.endIndex, part.startIndex + (*cornerIndices[1] & 1073741823) + 1);
			for (int i = part.startIndex; i < num3; i++)
			{
				GraphNode absolute = nodes.GetAbsolute(i);
				GraphNode absolute2 = nodes.GetAbsolute(i + 1);
				if (!PathTracer.Valid(absolute2) || !absolute.ContainsOutgoingConnection(absolute2))
				{
					return false;
				}
			}
			GridGraph gridGraph = GridNode.GetGridGraph(nodes.GetAbsolute(part.startIndex).GraphIndex);
			int num4;
			int2 fixedNormalizedFromPoint = PathTracer.ResolveNormalizedGridPoint(gridGraph, ref nodes, cornerIndices, part, num - 1, out num4);
			int num5;
			int2 @int = PathTracer.ResolveNormalizedGridPoint(gridGraph, ref nodes, cornerIndices, part, num + 1, out num5);
			int num6;
			int2 rhs = PathTracer.ResolveNormalizedGridPoint(gridGraph, ref nodes, cornerIndices, part, num, out num6);
			GridNodeBase fromNode = nodes.GetAbsolute(num4) as GridNodeBase;
			GridNodeBase gridNodeBase = nodes.GetAbsolute(num6) as GridNodeBase;
			GridNodeBase gridNodeBase2 = nodes.GetAbsolute(num5) as GridNodeBase;
			if (num2 > 0)
			{
				int num7 = PathTracer.SplittingCoefficients[num2 * 2];
				int num8 = PathTracer.SplittingCoefficients[num2 * 2 + 1];
				num5 += (num6 - num5) * num7 / num8;
				if (num5 == num6)
				{
					return false;
				}
				Vector2Int coordinatesInGrid = gridNodeBase2.CoordinatesInGrid;
				Vector2Int coordinatesInGrid2 = gridNodeBase.CoordinatesInGrid;
				int2 int2 = new int2(coordinatesInGrid2.x * 1024, coordinatesInGrid2.y * 1024) + rhs;
				int2 int3 = new int2(coordinatesInGrid.x * 1024, coordinatesInGrid.y * 1024) + @int;
				gridNodeBase2 = (nodes.GetAbsolute(num5) as GridNodeBase);
				coordinatesInGrid = gridNodeBase2.CoordinatesInGrid;
				float t = VectorMath.ClosestPointOnLineFactor(new Vector2Int(int2.x, int2.y), new Vector2Int(int3.x, int3.y), new Vector2Int(coordinatesInGrid.x * 1024 + 512, coordinatesInGrid.y * 1024 + 512));
				int2 int4 = new int2((int)math.lerp((float)int2.x, (float)int3.x, t), (int)math.lerp((float)int2.y, (float)int3.y, t)) - new int2(coordinatesInGrid.x * 1024, coordinatesInGrid.y * 1024);
				@int = new int2(math.clamp(int4.x, 0, 1024), math.clamp(int4.y, 0, 1024));
			}
			alternativePath.Clear();
			GridHitInfo gridHitInfo;
			if (!gridGraph.Linecast(fromNode, fixedNormalizedFromPoint, gridNodeBase2, @int, out gridHitInfo, alternativePath, null, false))
			{
				for (int j = 1; j < alternativePath.Count; j++)
				{
					if ((traversalProvider != null) ? (!traversalProvider.CanTraverse(path, alternativePath[j - 1], alternativePath[j])) : (!nnConstraint.Suitable(alternativePath[j])))
					{
						return false;
					}
				}
				uint num9 = 0U;
				for (int k = 0; k < alternativePath.Count; k++)
				{
					num9 += ((traversalProvider != null) ? traversalProvider.GetTraversalCost(path, alternativePath[k]) : DefaultITraversalProvider.GetTraversalCost(path, alternativePath[k]));
				}
				if (num9 > 0U)
				{
					uint num10 = 0U;
					for (int l = num4; l <= num5; l++)
					{
						num10 += ((traversalProvider != null) ? traversalProvider.GetTraversalCost(path, nodes.GetAbsolute(l)) : DefaultITraversalProvider.GetTraversalCost(path, nodes.GetAbsolute(l)));
					}
					if (num9 > num10)
					{
						return false;
					}
				}
				alternativeStartIndex = num4;
				alternativeEndIndex = num5;
				return true;
			}
			return false;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0003A864 File Offset: 0x00038A64
		private static void RemoveGridPathDiagonals(Funnel.PathPart[] parts, int partIndex, ref CircularBuffer<GraphNode> path, ref CircularBuffer<int> pathNodeHashes, NNConstraint nnConstraint, ITraversalProvider traversalProvider, Path pathObject)
		{
			int num = 0;
			Funnel.PathPart pathPart = (parts != null) ? parts[partIndex] : new Funnel.PathPart
			{
				startIndex = path.AbsoluteStartIndex,
				endIndex = path.AbsoluteEndIndex
			};
			for (int i = pathPart.endIndex - 1; i >= pathPart.startIndex; i--)
			{
				GridNodeBase gridNodeBase = path.GetAbsolute(i) as GridNodeBase;
				GridNodeBase gridNodeBase2 = path.GetAbsolute(i + 1) as GridNodeBase;
				int dx = gridNodeBase2.XCoordinateInGrid - gridNodeBase.XCoordinateInGrid;
				int dz = gridNodeBase2.ZCoordinateInGrid - gridNodeBase.ZCoordinateInGrid;
				int num2 = GridNodeBase.OffsetToConnectionDirection(dx, dz);
				if (num2 >= 4)
				{
					int direction = num2 - 4;
					int direction2 = (num2 - 4 + 1) % 4;
					GridNodeBase gridNodeBase3 = gridNodeBase.GetNeighbourAlongDirection(direction);
					if (gridNodeBase3 != null && ((traversalProvider != null) ? (!traversalProvider.CanTraverse(pathObject, gridNodeBase, gridNodeBase3)) : (!nnConstraint.Suitable(gridNodeBase3))))
					{
						gridNodeBase3 = null;
					}
					if (gridNodeBase3 != null && gridNodeBase3.GetNeighbourAlongDirection(direction2) == gridNodeBase2 && (traversalProvider == null || traversalProvider.CanTraverse(pathObject, gridNodeBase3, gridNodeBase2)))
					{
						path.InsertAbsolute(i + 1, gridNodeBase3);
						if (pathNodeHashes.Length > 0)
						{
							pathNodeHashes.InsertAbsolute(i + 1, PathTracer.HashNode(gridNodeBase3));
						}
						num++;
					}
					else
					{
						GridNodeBase gridNodeBase4 = gridNodeBase.GetNeighbourAlongDirection(direction2);
						if (gridNodeBase4 != null && ((traversalProvider != null) ? (!traversalProvider.CanTraverse(pathObject, gridNodeBase, gridNodeBase4)) : (!nnConstraint.Suitable(gridNodeBase4))))
						{
							gridNodeBase4 = null;
						}
						if (gridNodeBase4 == null || gridNodeBase4.GetNeighbourAlongDirection(direction) != gridNodeBase2 || (traversalProvider != null && !traversalProvider.CanTraverse(pathObject, gridNodeBase4, gridNodeBase2)))
						{
							throw new Exception("Axis-aligned connection not found");
						}
						path.InsertAbsolute(i + 1, gridNodeBase4);
						if (pathNodeHashes.Length > 0)
						{
							pathNodeHashes.InsertAbsolute(i + 1, PathTracer.HashNode(gridNodeBase4));
						}
						num++;
					}
				}
			}
			if (parts != null)
			{
				parts[partIndex].endIndex = parts[partIndex].endIndex + num;
				for (int j = partIndex + 1; j < parts.Length; j++)
				{
					int num3 = j;
					parts[num3].startIndex = parts[num3].startIndex + num;
					int num4 = j;
					parts[num4].endIndex = parts[num4].endIndex + num;
				}
			}
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0003AA82 File Offset: 0x00038C82
		private static PathTracer.PartGraphType PartGraphTypeFromNode(GraphNode node)
		{
			if (node == null)
			{
				return PathTracer.PartGraphType.Navmesh;
			}
			if (node is GridNodeBase)
			{
				return PathTracer.PartGraphType.Grid;
			}
			if (node is TriangleMeshNode)
			{
				return PathTracer.PartGraphType.Navmesh;
			}
			throw new Exception("The PathTracer (and by extension FollowerEntity component) cannot be used on graphs of type " + node.Graph.GetType().Name);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0003AABC File Offset: 0x00038CBC
		public void SetPath(ABPath path, NativeMovementPlane movementPlane)
		{
			List<Funnel.PathPart> list = Funnel.SplitIntoParts(path);
			PathRequestSettings pathfindingSettings = new PathRequestSettings
			{
				graphMask = path.nnConstraint.graphMask,
				traversableTags = path.nnConstraint.tags,
				tagPenalties = null,
				traversalProvider = path.traversalProvider
			};
			this.SetPath(list, path.path, path.originalStartPoint, path.originalEndPoint, movementPlane, pathfindingSettings, path);
			ListPool<Funnel.PathPart>.Release(ref list);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0003AB38 File Offset: 0x00038D38
		public void SetPath(List<Funnel.PathPart> parts, List<GraphNode> nodes, Vector3 unclampedStartPoint, Vector3 unclampedEndPoint, NativeMovementPlane movementPlane, PathRequestSettings pathfindingSettings, Path path)
		{
			this.nnConstraint.UseSettings(pathfindingSettings);
			this.startNode = ((nodes.Count > 0) ? nodes[0] : null);
			this.partGraphType = PathTracer.PartGraphTypeFromNode(this.startNode);
			this.unclampedEndPoint = unclampedEndPoint;
			this.unclampedStartPoint = unclampedStartPoint;
			this.firstPartContainsDestroyedNodes = false;
			this.startIsUpToDate = true;
			this.endIsUpToDate = true;
			this.parts = parts.ToArray();
			this.nodes.Clear();
			this.nodes.AddRange(nodes);
			this.nodeHashes.Clear();
			for (int i = 0; i < nodes.Count; i++)
			{
				this.nodeHashes.PushEnd(PathTracer.HashNode(nodes[i]));
			}
			this.firstPartIndex = 0;
			if (this.partGraphType == PathTracer.PartGraphType.Grid)
			{
				PathTracer.RemoveGridPathDiagonals(this.parts, 0, ref this.nodes, ref this.nodeHashes, this.nnConstraint, pathfindingSettings.traversalProvider, path);
			}
			this.SetFunnelState(this.parts[this.firstPartIndex]);
			ushort version = this.version;
			this.version = version + 1;
			this.Repair(unclampedStartPoint, true, PathTracer.RepairQuality.Low, movementPlane, pathfindingSettings.traversalProvider, path, false);
			this.Repair(unclampedEndPoint, false, PathTracer.RepairQuality.Low, movementPlane, pathfindingSettings.traversalProvider, path, false);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0003AC80 File Offset: 0x00038E80
		public PathTracer Clone()
		{
			return new PathTracer
			{
				parts = ((this.parts != null) ? (this.parts.Clone() as Funnel.PathPart[]) : null),
				nodes = this.nodes.Clone(),
				nodeHashes = this.nodeHashes.Clone(),
				portalIsNotInnerCorner = this.portalIsNotInnerCorner.Clone(),
				funnelState = this.funnelState.Clone(),
				unclampedEndPoint = this.unclampedEndPoint,
				unclampedStartPoint = this.unclampedStartPoint,
				startNodeInternal = this.startNodeInternal,
				firstPartIndex = this.firstPartIndex,
				startIsUpToDate = this.startIsUpToDate,
				endIsUpToDate = this.endIsUpToDate,
				firstPartContainsDestroyedNodes = this.firstPartContainsDestroyedNodes,
				version = this.version,
				nnConstraint = NNConstraint.Walkable,
				partGraphType = this.partGraphType
			};
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0003AE04 File Offset: 0x00039004
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool ContainsAndProject$BurstManaged(ref Int3 a, ref Int3 b, ref Int3 c, ref Vector3 p, float height, ref NativeMovementPlane movementPlane, out Vector3 projected)
		{
			int3 @int = (int3)a;
			int3 int2 = (int3)b;
			int3 int3 = (int3)c;
			int3 int4 = (int3)((Int3)p);
			if (!Polygon.ContainsPoint(ref @int, ref int2, ref int3, ref int4, ref movementPlane))
			{
				projected = Vector3.zero;
				return false;
			}
			float3 a2 = (Vector3)a;
			float3 b2 = (Vector3)b;
			float3 c2 = (Vector3)c;
			float3 @float = p;
			float num = height * 0.5f;
			float3 float2 = PathTracer.ProjectOnSurface(a2, b2, c2, @float, movementPlane.up);
			if (math.lengthsq(float2 - @float) > num * num)
			{
				projected = Vector3.zero;
				return false;
			}
			projected = float2;
			return true;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0003AEF8 File Offset: 0x000390F8
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static float EstimateRemainingPath$BurstManaged(ref Funnel.FunnelState funnelState, ref Funnel.PathPart part, int maxCorners, ref NativeMovementPlane movementPlane)
		{
			NativeList<float3> nativeList = new NativeList<float3>(maxCorners, Allocator.Temp);
			NativeArray<int> nativeArray = new NativeArray<int>(maxCorners, Allocator.Temp, NativeArrayOptions.ClearMemory);
			maxCorners--;
			maxCorners = math.max(0, math.min(maxCorners, funnelState.leftFunnel.Length));
			bool lastCorner;
			int numCorners = funnelState.CalculateNextCornerIndices(maxCorners, nativeArray, part.startPoint, part.endPoint, out lastCorner);
			funnelState.ConvertCornerIndicesToPath(nativeArray, numCorners, false, part.startPoint, part.endPoint, lastCorner, nativeList);
			UnsafeSpan<float3> unsafeSpan = nativeList.AsUnsafeSpan<float3>();
			float3 @float = part.endPoint;
			return PathTracer.RemainingDistanceLowerBound(unsafeSpan, @float, movementPlane);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0003AF9C File Offset: 0x0003919C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static float RemainingDistanceLowerBound$BurstManaged(in UnsafeSpan<float3> nextCorners, in float3 endOfPart, in NativeMovementPlane movementPlane)
		{
			if (nextCorners.Length == 0)
			{
				return 0f;
			}
			float3 @float = *nextCorners[0];
			float num = 0f;
			for (int i = 1; i < nextCorners.Length; i++)
			{
				float3 float2 = *nextCorners[i];
				num += math.length(movementPlane.ToPlane(float2 - @float));
				@float = float2;
			}
			return num + math.length(movementPlane.ToPlane(@float - endOfPart));
		}

		// Token: 0x040006D0 RID: 1744
		private Funnel.PathPart[] parts;

		// Token: 0x040006D1 RID: 1745
		private CircularBuffer<GraphNode> nodes;

		// Token: 0x040006D2 RID: 1746
		private CircularBuffer<int> nodeHashes;

		// Token: 0x040006D3 RID: 1747
		private CircularBuffer<byte> portalIsNotInnerCorner;

		// Token: 0x040006D4 RID: 1748
		private Funnel.FunnelState funnelState;

		// Token: 0x040006D5 RID: 1749
		private Vector3 unclampedEndPoint;

		// Token: 0x040006D6 RID: 1750
		private Vector3 unclampedStartPoint;

		// Token: 0x040006D7 RID: 1751
		private GraphNode startNodeInternal;

		// Token: 0x040006D8 RID: 1752
		private NNConstraint nnConstraint;

		// Token: 0x040006D9 RID: 1753
		private int firstPartIndex;

		// Token: 0x040006DA RID: 1754
		private bool startIsUpToDate;

		// Token: 0x040006DB RID: 1755
		private bool endIsUpToDate;

		// Token: 0x040006DC RID: 1756
		private bool firstPartContainsDestroyedNodes;

		// Token: 0x040006DD RID: 1757
		public PathTracer.PartGraphType partGraphType;

		// Token: 0x040006DF RID: 1759
		private static readonly ProfilerMarker MarkerContains = new ProfilerMarker("ContainsNode");

		// Token: 0x040006E0 RID: 1760
		private static readonly ProfilerMarker MarkerClosest = new ProfilerMarker("ClosestPointOnNode");

		// Token: 0x040006E1 RID: 1761
		private static readonly ProfilerMarker MarkerGetNearest = new ProfilerMarker("GetNearest");

		// Token: 0x040006E2 RID: 1762
		private const int NODES_TO_CHECK_FOR_DESTRUCTION = 5;

		// Token: 0x040006E3 RID: 1763
		private static readonly PathTracer.QueueItem[][] TempQueues = new PathTracer.QueueItem[JobsUtility.ThreadIndexCount][];

		// Token: 0x040006E4 RID: 1764
		private static readonly List<GraphNode>[] TempConnectionLists = new List<GraphNode>[JobsUtility.ThreadIndexCount];

		// Token: 0x040006E5 RID: 1765
		[ThreadStatic]
		private static List<GraphNode> scratchList;

		// Token: 0x040006E6 RID: 1766
		private static int[] SplittingCoefficients = new int[]
		{
			0,
			1,
			1,
			2,
			1,
			4,
			3,
			4,
			1,
			8,
			3,
			8,
			5,
			8,
			7,
			8
		};

		// Token: 0x040006E7 RID: 1767
		private static readonly ProfilerMarker MarkerSimplify = new ProfilerMarker("Simplify");

		// Token: 0x02000152 RID: 338
		public enum PartGraphType : byte
		{
			// Token: 0x040006E9 RID: 1769
			Navmesh,
			// Token: 0x040006EA RID: 1770
			Grid,
			// Token: 0x040006EB RID: 1771
			OffMeshLink
		}

		// Token: 0x02000153 RID: 339
		public enum RepairQuality
		{
			// Token: 0x040006ED RID: 1773
			Low,
			// Token: 0x040006EE RID: 1774
			High
		}

		// Token: 0x02000154 RID: 340
		private struct QueueItem
		{
			// Token: 0x040006EF RID: 1775
			public GraphNode node;

			// Token: 0x040006F0 RID: 1776
			public int parent;

			// Token: 0x040006F1 RID: 1777
			public float distance;
		}

		// Token: 0x02000156 RID: 342
		// (Invoke) Token: 0x06000A5D RID: 2653
		internal delegate bool ContainsAndProject_00000992$PostfixBurstDelegate(ref Int3 a, ref Int3 b, ref Int3 c, ref Vector3 p, float height, ref NativeMovementPlane movementPlane, out Vector3 projected);

		// Token: 0x02000157 RID: 343
		internal static class ContainsAndProject_00000992$BurstDirectCall
		{
			// Token: 0x06000A60 RID: 2656 RVA: 0x0003B030 File Offset: 0x00039230
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathTracer.ContainsAndProject_00000992$BurstDirectCall.Pointer == 0)
				{
					PathTracer.ContainsAndProject_00000992$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathTracer.ContainsAndProject_00000992$BurstDirectCall.DeferredCompilation, methodof(PathTracer.ContainsAndProject$BurstManaged(Int3*, Int3*, Int3*, Vector3*, float, NativeMovementPlane*, Vector3*)).MethodHandle, typeof(PathTracer.ContainsAndProject_00000992$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathTracer.ContainsAndProject_00000992$BurstDirectCall.Pointer;
			}

			// Token: 0x06000A61 RID: 2657 RVA: 0x0003B05C File Offset: 0x0003925C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				PathTracer.ContainsAndProject_00000992$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000A62 RID: 2658 RVA: 0x0003B074 File Offset: 0x00039274
			public unsafe static void Constructor()
			{
				PathTracer.ContainsAndProject_00000992$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathTracer.ContainsAndProject(Int3*, Int3*, Int3*, Vector3*, float, NativeMovementPlane*, Vector3*)).MethodHandle);
			}

			// Token: 0x06000A63 RID: 2659 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000A64 RID: 2660 RVA: 0x0003B085 File Offset: 0x00039285
			// Note: this type is marked as 'beforefieldinit'.
			static ContainsAndProject_00000992$BurstDirectCall()
			{
				PathTracer.ContainsAndProject_00000992$BurstDirectCall.Constructor();
			}

			// Token: 0x06000A65 RID: 2661 RVA: 0x0003B08C File Offset: 0x0003928C
			public static bool Invoke(ref Int3 a, ref Int3 b, ref Int3 c, ref Vector3 p, float height, ref NativeMovementPlane movementPlane, out Vector3 projected)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathTracer.ContainsAndProject_00000992$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Pathfinding.Int3&,Pathfinding.Int3&,Pathfinding.Int3&,UnityEngine.Vector3&,System.Single,Pathfinding.Util.NativeMovementPlane&,UnityEngine.Vector3&), ref a, ref b, ref c, ref p, height, ref movementPlane, ref projected, functionPointer);
					}
				}
				return PathTracer.ContainsAndProject$BurstManaged(ref a, ref b, ref c, ref p, height, ref movementPlane, out projected);
			}

			// Token: 0x040006F4 RID: 1780
			private static IntPtr Pointer;

			// Token: 0x040006F5 RID: 1781
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000158 RID: 344
		// (Invoke) Token: 0x06000A67 RID: 2663
		internal delegate float EstimateRemainingPath_000009A5$PostfixBurstDelegate(ref Funnel.FunnelState funnelState, ref Funnel.PathPart part, int maxCorners, ref NativeMovementPlane movementPlane);

		// Token: 0x02000159 RID: 345
		internal static class EstimateRemainingPath_000009A5$BurstDirectCall
		{
			// Token: 0x06000A6A RID: 2666 RVA: 0x0003B0CF File Offset: 0x000392CF
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathTracer.EstimateRemainingPath_000009A5$BurstDirectCall.Pointer == 0)
				{
					PathTracer.EstimateRemainingPath_000009A5$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathTracer.EstimateRemainingPath_000009A5$BurstDirectCall.DeferredCompilation, methodof(PathTracer.EstimateRemainingPath$BurstManaged(Funnel.FunnelState*, Funnel.PathPart*, int, NativeMovementPlane*)).MethodHandle, typeof(PathTracer.EstimateRemainingPath_000009A5$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathTracer.EstimateRemainingPath_000009A5$BurstDirectCall.Pointer;
			}

			// Token: 0x06000A6B RID: 2667 RVA: 0x0003B0FC File Offset: 0x000392FC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				PathTracer.EstimateRemainingPath_000009A5$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000A6C RID: 2668 RVA: 0x0003B114 File Offset: 0x00039314
			public unsafe static void Constructor()
			{
				PathTracer.EstimateRemainingPath_000009A5$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathTracer.EstimateRemainingPath(Funnel.FunnelState*, Funnel.PathPart*, int, NativeMovementPlane*)).MethodHandle);
			}

			// Token: 0x06000A6D RID: 2669 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000A6E RID: 2670 RVA: 0x0003B125 File Offset: 0x00039325
			// Note: this type is marked as 'beforefieldinit'.
			static EstimateRemainingPath_000009A5$BurstDirectCall()
			{
				PathTracer.EstimateRemainingPath_000009A5$BurstDirectCall.Constructor();
			}

			// Token: 0x06000A6F RID: 2671 RVA: 0x0003B12C File Offset: 0x0003932C
			public static float Invoke(ref Funnel.FunnelState funnelState, ref Funnel.PathPart part, int maxCorners, ref NativeMovementPlane movementPlane)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathTracer.EstimateRemainingPath_000009A5$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Pathfinding.Funnel/FunnelState&,Pathfinding.Funnel/PathPart&,System.Int32,Pathfinding.Util.NativeMovementPlane&), ref funnelState, ref part, maxCorners, ref movementPlane, functionPointer);
					}
				}
				return PathTracer.EstimateRemainingPath$BurstManaged(ref funnelState, ref part, maxCorners, ref movementPlane);
			}

			// Token: 0x040006F6 RID: 1782
			private static IntPtr Pointer;

			// Token: 0x040006F7 RID: 1783
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200015A RID: 346
		// (Invoke) Token: 0x06000A71 RID: 2673
		internal delegate float RemainingDistanceLowerBound_000009A9$PostfixBurstDelegate(in UnsafeSpan<float3> nextCorners, in float3 endOfPart, in NativeMovementPlane movementPlane);

		// Token: 0x0200015B RID: 347
		internal static class RemainingDistanceLowerBound_000009A9$BurstDirectCall
		{
			// Token: 0x06000A74 RID: 2676 RVA: 0x0003B163 File Offset: 0x00039363
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathTracer.RemainingDistanceLowerBound_000009A9$BurstDirectCall.Pointer == 0)
				{
					PathTracer.RemainingDistanceLowerBound_000009A9$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathTracer.RemainingDistanceLowerBound_000009A9$BurstDirectCall.DeferredCompilation, methodof(PathTracer.RemainingDistanceLowerBound$BurstManaged(UnsafeSpan<float3>*, float3*, NativeMovementPlane*)).MethodHandle, typeof(PathTracer.RemainingDistanceLowerBound_000009A9$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathTracer.RemainingDistanceLowerBound_000009A9$BurstDirectCall.Pointer;
			}

			// Token: 0x06000A75 RID: 2677 RVA: 0x0003B190 File Offset: 0x00039390
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				PathTracer.RemainingDistanceLowerBound_000009A9$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000A76 RID: 2678 RVA: 0x0003B1A8 File Offset: 0x000393A8
			public unsafe static void Constructor()
			{
				PathTracer.RemainingDistanceLowerBound_000009A9$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathTracer.RemainingDistanceLowerBound(UnsafeSpan<float3>*, float3*, NativeMovementPlane*)).MethodHandle);
			}

			// Token: 0x06000A77 RID: 2679 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000A78 RID: 2680 RVA: 0x0003B1B9 File Offset: 0x000393B9
			// Note: this type is marked as 'beforefieldinit'.
			static RemainingDistanceLowerBound_000009A9$BurstDirectCall()
			{
				PathTracer.RemainingDistanceLowerBound_000009A9$BurstDirectCall.Constructor();
			}

			// Token: 0x06000A79 RID: 2681 RVA: 0x0003B1C0 File Offset: 0x000393C0
			public static float Invoke(in UnsafeSpan<float3> nextCorners, in float3 endOfPart, in NativeMovementPlane movementPlane)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathTracer.RemainingDistanceLowerBound_000009A9$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Pathfinding.Collections.UnsafeSpan`1<Unity.Mathematics.float3>&,Unity.Mathematics.float3&,Pathfinding.Util.NativeMovementPlane&), ref nextCorners, ref endOfPart, ref movementPlane, functionPointer);
					}
				}
				return PathTracer.RemainingDistanceLowerBound$BurstManaged(nextCorners, endOfPart, movementPlane);
			}

			// Token: 0x040006F8 RID: 1784
			private static IntPtr Pointer;

			// Token: 0x040006F9 RID: 1785
			private static IntPtr DeferredCompilation;
		}
	}
}
