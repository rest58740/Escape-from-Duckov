using System;
using System.Collections.Generic;
using System.Text;
using Pathfinding.Pooling;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200012D RID: 301
	public class ABPath : Path
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x00032AAD File Offset: 0x00030CAD
		public GraphNode startNode
		{
			get
			{
				if (this.path.Count <= 0)
				{
					return null;
				}
				return this.path[0];
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x00032ACB File Offset: 0x00030CCB
		public GraphNode endNode
		{
			get
			{
				if (this.path.Count <= 0)
				{
					return null;
				}
				return this.path[this.path.Count - 1];
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x0001797A File Offset: 0x00015B7A
		protected virtual bool hasEndPoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x0001797A File Offset: 0x00015B7A
		public virtual bool endPointKnownBeforeCalculation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00032B0B File Offset: 0x00030D0B
		public static ABPath Construct(Vector3 start, Vector3 end, OnPathDelegate callback = null)
		{
			ABPath path = PathPool.GetPath<ABPath>();
			path.Setup(start, end, callback);
			return path;
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00032B1B File Offset: 0x00030D1B
		protected void Setup(Vector3 start, Vector3 end, OnPathDelegate callbackDelegate)
		{
			this.callback = callbackDelegate;
			this.UpdateStartEnd(start, end);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00032B2C File Offset: 0x00030D2C
		public static ABPath FakePath(List<Vector3> vectorPath, List<GraphNode> nodePath = null)
		{
			ABPath path = PathPool.GetPath<ABPath>();
			for (int i = 0; i < vectorPath.Count; i++)
			{
				path.vectorPath.Add(vectorPath[i]);
			}
			path.completeState = PathCompleteState.Complete;
			((IPathInternals)path).AdvanceState(PathState.Returned);
			if (vectorPath.Count > 0)
			{
				path.UpdateStartEnd(vectorPath[0], vectorPath[vectorPath.Count - 1]);
			}
			if (nodePath != null)
			{
				for (int j = 0; j < nodePath.Count; j++)
				{
					path.path.Add(nodePath[j]);
				}
			}
			return path;
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00032BBB File Offset: 0x00030DBB
		protected void UpdateStartEnd(Vector3 start, Vector3 end)
		{
			this.originalStartPoint = start;
			this.originalEndPoint = end;
			this.startPoint = start;
			this.endPoint = end;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00032BDC File Offset: 0x00030DDC
		protected override void Reset()
		{
			base.Reset();
			this.originalStartPoint = Vector3.zero;
			this.originalEndPoint = Vector3.zero;
			this.startPoint = Vector3.zero;
			this.endPoint = Vector3.zero;
			this.calculatePartial = false;
			this.partialBestTargetPathNodeIndex = 0U;
			this.partialBestTargetHScore = uint.MaxValue;
			this.partialBestTargetGScore = uint.MaxValue;
			this.cost = 0U;
			this.endingCondition = null;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00032C48 File Offset: 0x00030E48
		protected virtual bool EndPointGridGraphSpecialCase(GraphNode closestWalkableEndNode, Vector3 originalEndPoint, int targetIndex)
		{
			GridNode gridNode = closestWalkableEndNode as GridNode;
			if (gridNode != null)
			{
				GridGraph gridGraph = GridNode.GetGridGraph(gridNode.GraphIndex);
				GridNode gridNode2 = gridGraph.GetNearest(originalEndPoint, ABPath.NNConstraintNone).node as GridNode;
				if (gridNode != gridNode2 && gridNode2 != null)
				{
					int num = gridNode.NodeInGridIndex % gridGraph.width;
					int num2 = gridNode.NodeInGridIndex / gridGraph.width;
					int num3 = gridNode2.NodeInGridIndex % gridGraph.width;
					int num4 = gridNode2.NodeInGridIndex / gridGraph.width;
					bool flag = false;
					switch (gridGraph.neighbours)
					{
					case NumNeighbours.Four:
						if ((num == num3 && Math.Abs(num2 - num4) == 1) || (num2 == num4 && Math.Abs(num - num3) == 1))
						{
							flag = true;
						}
						break;
					case NumNeighbours.Eight:
						if (Math.Abs(num - num3) <= 1 && Math.Abs(num2 - num4) <= 1)
						{
							flag = true;
						}
						break;
					case NumNeighbours.Six:
						for (int i = 0; i < 6; i++)
						{
							int num5 = num3 + GridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[i]];
							int num6 = num4 + GridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[i]];
							if (num == num5 && num2 == num6)
							{
								flag = true;
								break;
							}
						}
						break;
					default:
						throw new Exception("Unhandled NumNeighbours");
					}
					if (flag)
					{
						this.AddEndpointsForSurroundingGridNodes(gridNode2, originalEndPoint, targetIndex);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00032DA0 File Offset: 0x00030FA0
		private void AddEndpointsForSurroundingGridNodes(GridNode gridNode, Vector3 desiredPoint, int targetIndex)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(gridNode.GraphIndex);
			int num = (gridGraph.neighbours == NumNeighbours.Four) ? 4 : ((gridGraph.neighbours == NumNeighbours.Eight) ? 8 : 6);
			int num2 = gridNode.NodeInGridIndex % gridGraph.width;
			int num3 = gridNode.NodeInGridIndex / gridGraph.width;
			for (int i = 0; i < num; i++)
			{
				int x;
				int z;
				if (gridGraph.neighbours == NumNeighbours.Six)
				{
					x = num2 + GridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[i]];
					z = num3 + GridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[i]];
				}
				else
				{
					x = num2 + GridGraph.neighbourXOffsets[i];
					z = num3 + GridGraph.neighbourZOffsets[i];
				}
				GridNodeBase node = gridGraph.GetNode(x, z);
				if (node != null)
				{
					this.pathHandler.AddTemporaryNode(new TemporaryNode
					{
						type = TemporaryNodeType.End,
						position = (Int3)node.ClosestPointOnNode(desiredPoint),
						associatedNode = node.NodeIndex,
						targetIndex = targetIndex
					});
				}
			}
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00032EA4 File Offset: 0x000310A4
		protected override void Prepare()
		{
			NNInfo nearest = base.GetNearest(this.startPoint);
			PathNNConstraint pathNNConstraint = this.nnConstraint as PathNNConstraint;
			if (pathNNConstraint != null)
			{
				pathNNConstraint.SetStart(nearest.node);
			}
			this.startPoint = nearest.position;
			if (nearest.node == null)
			{
				base.FailWithError("Couldn't find a node close to the start point");
				return;
			}
			if (!base.CanTraverse(nearest.node))
			{
				base.FailWithError("The node closest to the start point could not be traversed");
				return;
			}
			this.pathHandler.AddTemporaryNode(new TemporaryNode
			{
				associatedNode = nearest.node.NodeIndex,
				position = (Int3)nearest.position,
				type = TemporaryNodeType.Start
			});
			uint targetNodeIndex = 0U;
			if (this.hasEndPoint)
			{
				NNInfo nearest2 = base.GetNearest(this.originalEndPoint);
				this.endPoint = nearest2.position;
				if (nearest2.node == null)
				{
					base.FailWithError("Couldn't find a node close to the end point");
					return;
				}
				if (!base.CanTraverse(nearest2.node))
				{
					base.FailWithError("The node closest to the end point could not be traversed");
					return;
				}
				if (nearest.node.Area != nearest2.node.Area)
				{
					base.FailWithError("There is no valid path to the target");
					return;
				}
				targetNodeIndex = nearest2.node.NodeIndex;
				if (!this.EndPointGridGraphSpecialCase(nearest2.node, this.originalEndPoint, 0))
				{
					this.pathHandler.AddTemporaryNode(new TemporaryNode
					{
						associatedNode = nearest2.node.NodeIndex,
						position = (Int3)nearest2.position,
						type = TemporaryNodeType.End
					});
				}
			}
			int3 mn;
			int3 mx;
			base.TemporaryEndNodesBoundingBox(out mn, out mx);
			this.heuristicObjective = new HeuristicObjective(mn, mx, this.heuristic, this.heuristicScale, targetNodeIndex, AstarPath.active.euclideanEmbedding);
			base.MarkNodesAdjacentToTemporaryEndNodes();
			base.AddStartNodesToHeap();
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00033074 File Offset: 0x00031274
		private void CompletePartial()
		{
			base.CompleteState = PathCompleteState.Partial;
			this.endPoint = this.pathHandler.GetNode(this.partialBestTargetPathNodeIndex).ClosestPointOnNode(this.originalEndPoint);
			this.cost = this.partialBestTargetGScore;
			this.Trace(this.partialBestTargetPathNodeIndex);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x000330C2 File Offset: 0x000312C2
		protected override void OnHeapExhausted()
		{
			if (this.calculatePartial && this.partialBestTargetPathNodeIndex != 0U)
			{
				this.CompletePartial();
				return;
			}
			base.FailWithError("Searched all reachable nodes, but could not find target. This can happen if you have nodes with a different tag blocking the way to the goal. You can enable path.calculatePartial to handle that case as a workaround (though this comes with a performance cost).");
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x000330E8 File Offset: 0x000312E8
		protected unsafe override void OnFoundEndNode(uint pathNode, uint hScore, uint gScore)
		{
			if (this.pathHandler.IsTemporaryNode(pathNode))
			{
				TemporaryNode temporaryNode = *this.pathHandler.GetTemporaryNode(pathNode);
				GraphNode node = this.pathHandler.GetNode(temporaryNode.associatedNode);
				if (this.endingCondition != null && !this.endingCondition.TargetFound(node, this.partialBestTargetHScore, gScore))
				{
					return;
				}
				this.endPoint = (Vector3)temporaryNode.position;
				this.endPoint = node.ClosestPointOnNode(this.endPoint);
			}
			else
			{
				GraphNode node2 = this.pathHandler.GetNode(pathNode);
				this.endPoint = (Vector3)node2.position;
			}
			this.cost = gScore;
			base.CompleteState = PathCompleteState.Complete;
			this.Trace(pathNode);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x000331A0 File Offset: 0x000313A0
		public override void OnVisitNode(uint pathNode, uint hScore, uint gScore)
		{
			if (base.CompleteState != PathCompleteState.NotCalculated)
			{
				return;
			}
			if (this.endingCondition != null)
			{
				GraphNode node = this.pathHandler.GetNode(pathNode);
				if (this.endingCondition.TargetFound(node, hScore, gScore))
				{
					this.OnFoundEndNode(pathNode, hScore, gScore);
					if (base.CompleteState == PathCompleteState.Complete)
					{
						return;
					}
				}
			}
			if (hScore < this.partialBestTargetHScore)
			{
				this.partialBestTargetPathNodeIndex = pathNode;
				this.partialBestTargetHScore = hScore;
				this.partialBestTargetGScore = gScore;
			}
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0003320C File Offset: 0x0003140C
		protected override string DebugString(PathLog logMode)
		{
			if (logMode == PathLog.None || (!base.error && logMode == PathLog.OnlyErrors))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			base.DebugStringPrefix(logMode, stringBuilder);
			if (!base.error)
			{
				stringBuilder.Append(" Path Cost: ");
				stringBuilder.Append(this.cost);
			}
			if (!base.error && logMode == PathLog.Heavy)
			{
				Vector3 vector;
				if (this.hasEndPoint && this.endNode != null)
				{
					stringBuilder.Append("\n\tPoint: ");
					StringBuilder stringBuilder2 = stringBuilder;
					vector = this.endPoint;
					stringBuilder2.Append(vector.ToString());
					stringBuilder.Append("\n\tGraph: ");
					stringBuilder.Append(this.endNode.GraphIndex);
				}
				stringBuilder.Append("\nStart Node");
				stringBuilder.Append("\n\tPoint: ");
				StringBuilder stringBuilder3 = stringBuilder;
				vector = this.startPoint;
				stringBuilder3.Append(vector.ToString());
				stringBuilder.Append("\n\tGraph: ");
				if (this.startNode != null)
				{
					stringBuilder.Append(this.startNode.GraphIndex);
				}
				else
				{
					stringBuilder.Append("< null startNode >");
				}
			}
			base.DebugStringSuffix(logMode, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x04000659 RID: 1625
		public Vector3 originalStartPoint;

		// Token: 0x0400065A RID: 1626
		public Vector3 originalEndPoint;

		// Token: 0x0400065B RID: 1627
		public Vector3 startPoint;

		// Token: 0x0400065C RID: 1628
		public Vector3 endPoint;

		// Token: 0x0400065D RID: 1629
		public uint cost;

		// Token: 0x0400065E RID: 1630
		public bool calculatePartial;

		// Token: 0x0400065F RID: 1631
		protected uint partialBestTargetPathNodeIndex;

		// Token: 0x04000660 RID: 1632
		protected uint partialBestTargetHScore = uint.MaxValue;

		// Token: 0x04000661 RID: 1633
		protected uint partialBestTargetGScore = uint.MaxValue;

		// Token: 0x04000662 RID: 1634
		public PathEndingCondition endingCondition;

		// Token: 0x04000663 RID: 1635
		private static readonly NNConstraint NNConstraintNone = NNConstraint.None;
	}
}
