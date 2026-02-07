using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000131 RID: 305
	public class FloodPath : Path
	{
		// Token: 0x06000957 RID: 2391 RVA: 0x00033534 File Offset: 0x00031734
		public bool HasPathTo(GraphNode node)
		{
			if (this.parents != null)
			{
				uint num = 0U;
				while ((ulong)num < (ulong)((long)node.PathNodeVariants))
				{
					if (this.parents.ContainsKey(node.NodeIndex + num))
					{
						return true;
					}
					num += 1U;
				}
			}
			return false;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x00033574 File Offset: 0x00031774
		internal bool IsValid(GlobalNodeStorage nodeStorage)
		{
			return nodeStorage.destroyedNodesVersion == this.validationHash;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x00033584 File Offset: 0x00031784
		public uint GetParent(uint node)
		{
			uint result;
			if (!this.parents.TryGetValue(node, out result))
			{
				return 0U;
			}
			return result;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x000335B3 File Offset: 0x000317B3
		public static FloodPath Construct(Vector3 start, OnPathDelegate callback = null)
		{
			FloodPath path = PathPool.GetPath<FloodPath>();
			path.Setup(start, callback);
			return path;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x000335C2 File Offset: 0x000317C2
		public static FloodPath Construct(GraphNode start, OnPathDelegate callback = null)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			FloodPath path = PathPool.GetPath<FloodPath>();
			path.Setup(start, callback);
			return path;
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x000335DF File Offset: 0x000317DF
		protected void Setup(Vector3 start, OnPathDelegate callback)
		{
			this.callback = callback;
			this.originalStartPoint = start;
			this.startPoint = start;
			this.heuristic = Heuristic.None;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x000335FD File Offset: 0x000317FD
		protected void Setup(GraphNode start, OnPathDelegate callback)
		{
			this.callback = callback;
			this.originalStartPoint = (Vector3)start.position;
			this.startNode = start;
			this.startPoint = (Vector3)start.position;
			this.heuristic = Heuristic.None;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00033636 File Offset: 0x00031836
		protected override void Reset()
		{
			base.Reset();
			this.originalStartPoint = Vector3.zero;
			this.startPoint = Vector3.zero;
			this.startNode = null;
			this.parents = new Dictionary<uint, uint>();
			this.saveParents = true;
			this.validationHash = 0U;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00033674 File Offset: 0x00031874
		protected override void Prepare()
		{
			if (this.startNode == null)
			{
				NNInfo nearest = base.GetNearest(this.originalStartPoint);
				this.startPoint = nearest.position;
				this.startNode = nearest.node;
			}
			else
			{
				if (this.startNode.Destroyed)
				{
					base.FailWithError("Start node has been destroyed");
					return;
				}
				this.startPoint = (Vector3)this.startNode.position;
			}
			if (this.startNode == null)
			{
				base.FailWithError("Couldn't find a close node to the start point");
				return;
			}
			if (!base.CanTraverse(this.startNode))
			{
				base.FailWithError("The node closest to the start point could not be traversed");
				return;
			}
			this.pathHandler.AddTemporaryNode(new TemporaryNode
			{
				type = TemporaryNodeType.Start,
				position = (Int3)this.startPoint,
				associatedNode = this.startNode.NodeIndex
			});
			this.heuristicObjective = new HeuristicObjective(int3.zero, Heuristic.None, 0f);
			base.AddStartNodesToHeap();
			this.validationHash = this.pathHandler.nodeStorage.destroyedNodesVersion;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0003346F File Offset: 0x0003166F
		protected override void OnHeapExhausted()
		{
			base.CompleteState = PathCompleteState.Complete;
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x00033780 File Offset: 0x00031980
		protected override void OnFoundEndNode(uint pathNode, uint hScore, uint gScore)
		{
			throw new InvalidOperationException("FloodPaths do not have any end nodes");
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0003378C File Offset: 0x0003198C
		public override void OnVisitNode(uint pathNode, uint hScore, uint gScore)
		{
			if (this.saveParents)
			{
				uint parentIndex = this.pathHandler.pathNodes[pathNode].parentIndex;
				this.parents[pathNode] = (parentIndex | (this.pathHandler.IsTemporaryNode(parentIndex) ? 2147483648U : 0U));
			}
		}

		// Token: 0x0400066A RID: 1642
		public Vector3 originalStartPoint;

		// Token: 0x0400066B RID: 1643
		public Vector3 startPoint;

		// Token: 0x0400066C RID: 1644
		public GraphNode startNode;

		// Token: 0x0400066D RID: 1645
		public bool saveParents = true;

		// Token: 0x0400066E RID: 1646
		protected Dictionary<uint, uint> parents;

		// Token: 0x0400066F RID: 1647
		private uint validationHash;

		// Token: 0x04000670 RID: 1648
		public const uint TemporaryNodeBit = 2147483648U;
	}
}
