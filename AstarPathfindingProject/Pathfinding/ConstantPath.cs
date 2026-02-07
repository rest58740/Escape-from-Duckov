using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200012E RID: 302
	public class ConstantPath : Path
	{
		// Token: 0x06000949 RID: 2377 RVA: 0x00033345 File Offset: 0x00031545
		public static ConstantPath Construct(Vector3 start, int maxGScore, OnPathDelegate callback = null)
		{
			ConstantPath path = PathPool.GetPath<ConstantPath>();
			path.Setup(start, maxGScore, callback);
			return path;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00033355 File Offset: 0x00031555
		protected void Setup(Vector3 start, int maxGScore, OnPathDelegate callback)
		{
			this.callback = callback;
			this.startPoint = start;
			this.originalStartPoint = this.startPoint;
			this.endingCondition = new EndingConditionDistance(this, maxGScore);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0003337E File Offset: 0x0003157E
		protected override void OnEnterPool()
		{
			base.OnEnterPool();
			if (this.allNodes != null)
			{
				ListPool<GraphNode>.Release(ref this.allNodes);
			}
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00033399 File Offset: 0x00031599
		protected override void Reset()
		{
			base.Reset();
			this.allNodes = ListPool<GraphNode>.Claim();
			this.endingCondition = null;
			this.originalStartPoint = Vector3.zero;
			this.startPoint = Vector3.zero;
			this.startNode = null;
			this.heuristic = Heuristic.None;
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x000333D8 File Offset: 0x000315D8
		protected override void Prepare()
		{
			NNInfo nearest = base.GetNearest(this.startPoint);
			this.startNode = nearest.node;
			if (this.startNode == null)
			{
				base.FailWithError("Could not find close node to the start point");
				return;
			}
			this.pathHandler.AddTemporaryNode(new TemporaryNode
			{
				type = TemporaryNodeType.Start,
				position = (Int3)nearest.position,
				associatedNode = this.startNode.NodeIndex
			});
			this.heuristicObjective = new HeuristicObjective(int3.zero, Heuristic.None, 0f);
			base.AddStartNodesToHeap();
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0003346F File Offset: 0x0003166F
		protected override void OnHeapExhausted()
		{
			base.CompleteState = PathCompleteState.Complete;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00033478 File Offset: 0x00031678
		protected override void OnFoundEndNode(uint pathNode, uint hScore, uint gScore)
		{
			throw new InvalidOperationException("ConstantPaths do not have any end nodes");
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00033484 File Offset: 0x00031684
		public override void OnVisitNode(uint pathNode, uint hScore, uint gScore)
		{
			GraphNode node = this.pathHandler.GetNode(pathNode);
			if (this.endingCondition.TargetFound(node, hScore, gScore))
			{
				base.CompleteState = PathCompleteState.Complete;
				return;
			}
			this.allNodes.Add(node);
		}

		// Token: 0x04000664 RID: 1636
		public GraphNode startNode;

		// Token: 0x04000665 RID: 1637
		public Vector3 startPoint;

		// Token: 0x04000666 RID: 1638
		public Vector3 originalStartPoint;

		// Token: 0x04000667 RID: 1639
		public List<GraphNode> allNodes;

		// Token: 0x04000668 RID: 1640
		public PathEndingCondition endingCondition;
	}
}
