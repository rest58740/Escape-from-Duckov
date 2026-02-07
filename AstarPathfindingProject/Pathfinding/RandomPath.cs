using System;
using Pathfinding.Pooling;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000135 RID: 309
	public class RandomPath : ABPath
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x000185BF File Offset: 0x000167BF
		protected override bool hasEndPoint
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x000185BF File Offset: 0x000167BF
		public override bool endPointKnownBeforeCalculation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x000344DC File Offset: 0x000326DC
		protected override void Reset()
		{
			base.Reset();
			this.searchLength = 5000;
			this.spread = 5000;
			this.aimStrength = 0f;
			this.chosenPathNodeIndex = uint.MaxValue;
			this.maxGScorePathNodeIndex = uint.MaxValue;
			this.chosenPathNodeGScore = 0U;
			this.maxGScore = 0U;
			this.aim = Vector3.zero;
			this.nodesEvaluatedRep = 0;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0003455C File Offset: 0x0003275C
		public static RandomPath Construct(Vector3 start, int length, OnPathDelegate callback = null)
		{
			RandomPath path = PathPool.GetPath<RandomPath>();
			path.Setup(start, length, callback);
			return path;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0003456D File Offset: 0x0003276D
		protected RandomPath Setup(Vector3 start, int length, OnPathDelegate callback)
		{
			this.callback = callback;
			this.searchLength = length;
			this.originalStartPoint = start;
			this.originalEndPoint = Vector3.zero;
			this.startPoint = start;
			this.endPoint = Vector3.zero;
			return this;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x000345A2 File Offset: 0x000327A2
		protected override void ReturnPath()
		{
			if (this.path != null && this.path.Count > 0)
			{
				this.originalEndPoint = this.endPoint;
			}
			if (this.callback != null)
			{
				this.callback(this);
			}
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x000345DC File Offset: 0x000327DC
		protected override void Prepare()
		{
			NNInfo nearest = base.GetNearest(this.startPoint);
			this.startPoint = nearest.position;
			this.endPoint = this.startPoint;
			if (nearest.node == null)
			{
				base.FailWithError("Couldn't find close nodes to the start point");
				return;
			}
			if (!base.CanTraverse(nearest.node))
			{
				base.FailWithError("The node closest to the start point could not be traversed");
				return;
			}
			this.heuristicScale = this.aimStrength;
			this.pathHandler.AddTemporaryNode(new TemporaryNode
			{
				type = TemporaryNodeType.Start,
				position = (Int3)nearest.position,
				associatedNode = nearest.node.NodeIndex
			});
			this.heuristicObjective = new HeuristicObjective((int3)((Int3)this.aim), this.heuristic, this.heuristicScale);
			base.AddStartNodesToHeap();
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x000346B8 File Offset: 0x000328B8
		protected override void OnHeapExhausted()
		{
			if (this.chosenPathNodeIndex == 4294967295U && this.maxGScorePathNodeIndex != 4294967295U)
			{
				this.chosenPathNodeIndex = this.maxGScorePathNodeIndex;
				this.chosenPathNodeGScore = this.maxGScore;
			}
			if (this.chosenPathNodeIndex != 4294967295U)
			{
				this.OnFoundEndNode(this.chosenPathNodeIndex, 0U, this.chosenPathNodeGScore);
				return;
			}
			base.FailWithError("Not a single node found to search");
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x00034718 File Offset: 0x00032918
		protected override void OnFoundEndNode(uint pathNode, uint hScore, uint gScore)
		{
			if (this.pathHandler.IsTemporaryNode(pathNode))
			{
				base.OnFoundEndNode(pathNode, hScore, gScore);
				return;
			}
			GraphNode node = this.pathHandler.GetNode(pathNode);
			this.endPoint = node.RandomPointOnSurface();
			this.cost = gScore;
			base.CompleteState = PathCompleteState.Complete;
			this.Trace(pathNode);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0003476C File Offset: 0x0003296C
		public override void OnVisitNode(uint pathNode, uint hScore, uint gScore)
		{
			if (base.CompleteState != PathCompleteState.NotCalculated)
			{
				return;
			}
			if ((ulong)gScore >= (ulong)((long)this.searchLength))
			{
				if ((ulong)gScore > (ulong)((long)(this.searchLength + this.spread)))
				{
					if (this.chosenPathNodeIndex == 4294967295U)
					{
						this.chosenPathNodeIndex = pathNode;
						this.chosenPathNodeGScore = gScore;
					}
					this.OnFoundEndNode(this.chosenPathNodeIndex, 0U, this.chosenPathNodeGScore);
					return;
				}
				this.nodesEvaluatedRep++;
				if (this.rnd.NextDouble() <= (double)(1f / (float)this.nodesEvaluatedRep))
				{
					this.chosenPathNodeIndex = pathNode;
					this.chosenPathNodeGScore = gScore;
					return;
				}
			}
			else if (gScore > this.maxGScore)
			{
				this.maxGScore = gScore;
				this.maxGScorePathNodeIndex = pathNode;
			}
		}

		// Token: 0x0400067F RID: 1663
		public int searchLength;

		// Token: 0x04000680 RID: 1664
		public int spread = 5000;

		// Token: 0x04000681 RID: 1665
		public float aimStrength;

		// Token: 0x04000682 RID: 1666
		private uint chosenPathNodeIndex;

		// Token: 0x04000683 RID: 1667
		private uint chosenPathNodeGScore;

		// Token: 0x04000684 RID: 1668
		private uint maxGScorePathNodeIndex;

		// Token: 0x04000685 RID: 1669
		private uint maxGScore;

		// Token: 0x04000686 RID: 1670
		public Vector3 aim;

		// Token: 0x04000687 RID: 1671
		private int nodesEvaluatedRep;

		// Token: 0x04000688 RID: 1672
		private readonly System.Random rnd = new System.Random();
	}
}
