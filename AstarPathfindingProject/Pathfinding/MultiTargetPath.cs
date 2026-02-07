using System;
using System.Collections.Generic;
using System.Text;
using Pathfinding.Pooling;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000134 RID: 308
	public class MultiTargetPath : ABPath
	{
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x00033A4A File Offset: 0x00031C4A
		// (set) Token: 0x0600096F RID: 2415 RVA: 0x00033A52 File Offset: 0x00031C52
		public bool inverted { get; protected set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x000185BF File Offset: 0x000167BF
		public override bool endPointKnownBeforeCalculation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00033A71 File Offset: 0x00031C71
		public static MultiTargetPath Construct(Vector3[] startPoints, Vector3 target, OnPathDelegate[] callbackDelegates, OnPathDelegate callback = null)
		{
			MultiTargetPath multiTargetPath = MultiTargetPath.Construct(target, startPoints, callbackDelegates, callback);
			multiTargetPath.inverted = true;
			return multiTargetPath;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00033A83 File Offset: 0x00031C83
		public static MultiTargetPath Construct(Vector3 start, Vector3[] targets, OnPathDelegate[] callbackDelegates, OnPathDelegate callback = null)
		{
			MultiTargetPath path = PathPool.GetPath<MultiTargetPath>();
			path.Setup(start, targets, callbackDelegates, callback);
			return path;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00033A94 File Offset: 0x00031C94
		protected void Setup(Vector3 start, Vector3[] targets, OnPathDelegate[] callbackDelegates, OnPathDelegate callback)
		{
			this.inverted = false;
			this.callback = callback;
			this.callbacks = callbackDelegates;
			if (this.callbacks != null && this.callbacks.Length != targets.Length)
			{
				throw new ArgumentException("The targets array must have the same length as the callbackDelegates array");
			}
			this.targetPoints = targets;
			this.originalStartPoint = start;
			this.startPoint = start;
			if (targets.Length == 0)
			{
				base.FailWithError("No targets were assigned to the MultiTargetPath");
				return;
			}
			this.endPoint = targets[0];
			this.originalTargetPoints = new Vector3[this.targetPoints.Length];
			for (int i = 0; i < this.targetPoints.Length; i++)
			{
				this.originalTargetPoints[i] = this.targetPoints[i];
			}
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00033B47 File Offset: 0x00031D47
		protected override void Reset()
		{
			base.Reset();
			this.pathsForAll = true;
			this.chosenTarget = -1;
			this.inverted = true;
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00033B64 File Offset: 0x00031D64
		protected override void OnEnterPool()
		{
			if (this.vectorPaths != null)
			{
				for (int i = 0; i < this.vectorPaths.Length; i++)
				{
					if (this.vectorPaths[i] != null)
					{
						ListPool<Vector3>.Release(this.vectorPaths[i]);
					}
				}
			}
			this.vectorPaths = null;
			this.vectorPath = null;
			if (this.nodePaths != null)
			{
				for (int j = 0; j < this.nodePaths.Length; j++)
				{
					if (this.nodePaths[j] != null)
					{
						ListPool<GraphNode>.Release(this.nodePaths[j]);
					}
				}
			}
			this.nodePaths = null;
			this.path = null;
			this.callbacks = null;
			this.targetNodes = null;
			this.targetsFound = null;
			this.targetPathCosts = null;
			this.targetPoints = null;
			this.originalTargetPoints = null;
			base.OnEnterPool();
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00033C24 File Offset: 0x00031E24
		private void ChooseShortestPath()
		{
			this.chosenTarget = -1;
			if (this.nodePaths != null)
			{
				uint num = uint.MaxValue;
				for (int i = 0; i < this.nodePaths.Length; i++)
				{
					if (this.nodePaths[i] != null)
					{
						uint num2 = this.targetPathCosts[i];
						if (num2 < num)
						{
							this.chosenTarget = i;
							num = num2;
						}
					}
				}
			}
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00033C78 File Offset: 0x00031E78
		private void SetPathParametersForReturn(int target)
		{
			this.path = this.nodePaths[target];
			this.vectorPath = this.vectorPaths[target];
			if (this.inverted)
			{
				this.startPoint = this.targetPoints[target];
				this.originalStartPoint = this.originalTargetPoints[target];
			}
			else
			{
				this.endPoint = this.targetPoints[target];
				this.originalEndPoint = this.originalTargetPoints[target];
			}
			this.cost = ((this.path != null) ? this.targetPathCosts[target] : 0U);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00033D0C File Offset: 0x00031F0C
		protected override void ReturnPath()
		{
			if (base.error)
			{
				if (this.callbacks != null)
				{
					for (int i = 0; i < this.callbacks.Length; i++)
					{
						if (this.callbacks[i] != null)
						{
							this.callbacks[i](this);
						}
					}
				}
				if (this.callback != null)
				{
					this.callback(this);
				}
				return;
			}
			bool flag = false;
			if (this.inverted)
			{
				this.endPoint = this.startPoint;
				this.originalEndPoint = this.originalStartPoint;
			}
			for (int j = 0; j < this.nodePaths.Length; j++)
			{
				if (this.nodePaths[j] != null)
				{
					this.completeState = PathCompleteState.Complete;
					flag = true;
				}
				else
				{
					this.completeState = PathCompleteState.Error;
				}
				if (this.callbacks != null && this.callbacks[j] != null)
				{
					this.SetPathParametersForReturn(j);
					this.callbacks[j](this);
					this.vectorPaths[j] = this.vectorPath;
				}
			}
			if (flag)
			{
				this.completeState = PathCompleteState.Complete;
				this.SetPathParametersForReturn(this.chosenTarget);
			}
			else
			{
				this.completeState = PathCompleteState.Error;
			}
			if (this.callback != null)
			{
				this.callback(this);
			}
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00033E24 File Offset: 0x00032024
		protected void RebuildOpenList()
		{
			BinaryHeap heap = this.pathHandler.heap;
			if (heap.tieBreaking != BinaryHeap.TieBreaking.HScore)
			{
				return;
			}
			for (int i = 0; i < heap.numberOfItems; i++)
			{
				uint pathNodeIndex = heap.GetPathNodeIndex(i);
				Int3 ob;
				if (this.pathHandler.IsTemporaryNode(pathNodeIndex))
				{
					ob = this.pathHandler.GetTemporaryNode(pathNodeIndex).position;
				}
				else
				{
					ob = this.pathHandler.GetNode(pathNodeIndex).DecodeVariantPosition(pathNodeIndex, this.pathHandler.pathNodes[pathNodeIndex].fractionAlongEdge);
				}
				uint h = (uint)this.heuristicObjective.Calculate((int3)ob, 0U);
				heap.SetH(i, h);
			}
			this.pathHandler.heap.Rebuild(this.pathHandler.pathNodes);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00033EE4 File Offset: 0x000320E4
		protected override void Prepare()
		{
			NNInfo nearest = base.GetNearest(this.startPoint);
			GraphNode node = nearest.node;
			if (this.endingCondition != null)
			{
				base.FailWithError("Multi target paths cannot use custom ending conditions");
				return;
			}
			if (node == null)
			{
				base.FailWithError("Could not find start node for multi target path");
				return;
			}
			if (!base.CanTraverse(node))
			{
				base.FailWithError("The node closest to the start point could not be traversed");
				return;
			}
			PathNNConstraint pathNNConstraint = this.nnConstraint as PathNNConstraint;
			if (pathNNConstraint != null)
			{
				pathNNConstraint.SetStart(nearest.node);
			}
			this.pathHandler.AddTemporaryNode(new TemporaryNode
			{
				associatedNode = nearest.node.NodeIndex,
				position = (Int3)nearest.position,
				type = TemporaryNodeType.Start
			});
			this.vectorPaths = new List<Vector3>[this.targetPoints.Length];
			this.nodePaths = new List<GraphNode>[this.targetPoints.Length];
			this.targetNodes = new GraphNode[this.targetPoints.Length];
			this.targetsFound = new bool[this.targetPoints.Length];
			this.targetPathCosts = new uint[this.targetPoints.Length];
			this.targetNodeCount = 0;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = 0; i < this.targetPoints.Length; i++)
			{
				Vector3 vector = this.targetPoints[i];
				NNInfo nearest2 = base.GetNearest(vector);
				this.targetNodes[i] = nearest2.node;
				this.targetPoints[i] = nearest2.position;
				if (this.targetNodes[i] != null)
				{
					flag3 = true;
				}
				bool flag4 = false;
				if (nearest2.node != null && base.CanTraverse(nearest2.node))
				{
					flag = true;
				}
				else
				{
					flag4 = true;
				}
				if (nearest2.node != null && nearest2.node.Area == node.Area)
				{
					flag2 = true;
				}
				else
				{
					flag4 = true;
				}
				if (flag4)
				{
					this.targetsFound[i] = true;
				}
				else
				{
					this.targetNodeCount++;
					if (!this.EndPointGridGraphSpecialCase(nearest2.node, vector, i))
					{
						this.pathHandler.AddTemporaryNode(new TemporaryNode
						{
							associatedNode = nearest2.node.NodeIndex,
							position = (Int3)nearest2.position,
							targetIndex = i,
							type = TemporaryNodeType.End
						});
					}
				}
			}
			this.startPoint = nearest.position;
			if (!flag3)
			{
				base.FailWithError("Couldn't find a valid node close to the any of the end points");
				return;
			}
			if (!flag)
			{
				base.FailWithError("No target nodes could be traversed");
				return;
			}
			if (!flag2)
			{
				base.FailWithError("There's no valid path to any of the given targets");
				return;
			}
			base.MarkNodesAdjacentToTemporaryEndNodes();
			base.AddStartNodesToHeap();
			this.RecalculateHTarget();
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00034184 File Offset: 0x00032384
		private void RecalculateHTarget()
		{
			if (this.pathsForAll)
			{
				int3 @int = base.FirstTemporaryEndNode();
				this.heuristicObjective = new HeuristicObjective(@int, @int, this.heuristic, this.heuristicScale, 0U, null);
			}
			else
			{
				int3 mn;
				int3 mx;
				base.TemporaryEndNodesBoundingBox(out mn, out mx);
				this.heuristicObjective = new HeuristicObjective(mn, mx, this.heuristic, this.heuristicScale, 0U, null);
			}
			this.RebuildOpenList();
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x000341E8 File Offset: 0x000323E8
		protected override void Cleanup()
		{
			this.ChooseShortestPath();
			base.Cleanup();
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0003346F File Offset: 0x0003166F
		protected override void OnHeapExhausted()
		{
			base.CompleteState = PathCompleteState.Complete;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x000341F8 File Offset: 0x000323F8
		protected override void OnFoundEndNode(uint pathNode, uint hScore, uint gScore)
		{
			if (!this.pathHandler.IsTemporaryNode(pathNode))
			{
				base.FailWithError("Expected the end node to be a temporary node. Cannot determine which path it belongs to. This could happen if you are using a custom ending condition for the path.");
				return;
			}
			int targetIndex = this.pathHandler.GetTemporaryNode(pathNode).targetIndex;
			if (this.targetsFound[targetIndex])
			{
				throw new ArgumentException("This target has already been found");
			}
			this.Trace(pathNode);
			this.vectorPaths[targetIndex] = this.vectorPath;
			this.nodePaths[targetIndex] = this.path;
			this.vectorPath = ListPool<Vector3>.Claim();
			this.path = ListPool<GraphNode>.Claim();
			this.targetsFound[targetIndex] = true;
			this.targetPathCosts[targetIndex] = gScore;
			this.targetNodeCount--;
			uint num = 0U;
			while ((ulong)num < (ulong)((long)this.pathHandler.numTemporaryNodes))
			{
				uint nodeIndex = this.pathHandler.temporaryNodeStartIndex + num;
				ref TemporaryNode temporaryNode = ref this.pathHandler.GetTemporaryNode(nodeIndex);
				if (temporaryNode.type == TemporaryNodeType.End && temporaryNode.targetIndex == targetIndex)
				{
					temporaryNode.type = TemporaryNodeType.Ignore;
				}
				num += 1U;
			}
			if (!this.pathsForAll)
			{
				base.CompleteState = PathCompleteState.Complete;
				this.targetNodeCount = 0;
				return;
			}
			if (this.targetNodeCount <= 0)
			{
				base.CompleteState = PathCompleteState.Complete;
				return;
			}
			this.RecalculateHTarget();
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00034319 File Offset: 0x00032519
		protected override void Trace(uint pathNodeIndex)
		{
			base.Trace(pathNodeIndex, !this.inverted);
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0003432C File Offset: 0x0003252C
		protected override string DebugString(PathLog logMode)
		{
			if (logMode == PathLog.None || (!base.error && logMode == PathLog.OnlyErrors))
			{
				return "";
			}
			StringBuilder debugStringBuilder = this.pathHandler.DebugStringBuilder;
			debugStringBuilder.Length = 0;
			base.DebugStringPrefix(logMode, debugStringBuilder);
			if (!base.error)
			{
				debugStringBuilder.Append("\nShortest path was ");
				debugStringBuilder.Append((this.chosenTarget == -1) ? "undefined" : this.nodePaths[this.chosenTarget].Count.ToString());
				debugStringBuilder.Append(" nodes long");
				if (logMode == PathLog.Heavy)
				{
					debugStringBuilder.Append("\nPaths (").Append(this.targetsFound.Length).Append("):");
					for (int i = 0; i < this.targetsFound.Length; i++)
					{
						debugStringBuilder.Append("\n\n\tPath ").Append(i).Append(" Found: ").Append(this.targetsFound[i]);
						if (this.nodePaths[i] != null)
						{
							debugStringBuilder.Append("\n\t\tLength: ");
							debugStringBuilder.Append(this.nodePaths[i].Count);
						}
					}
					debugStringBuilder.Append("\nStart Node");
					debugStringBuilder.Append("\n\tPoint: ");
					StringBuilder stringBuilder = debugStringBuilder;
					Vector3 endPoint = this.endPoint;
					stringBuilder.Append(endPoint.ToString());
					debugStringBuilder.Append("\n\tGraph: ");
					debugStringBuilder.Append(base.startNode.GraphIndex);
					debugStringBuilder.Append("\nBinary Heap size at completion: ");
					debugStringBuilder.AppendLine((this.pathHandler.heap.numberOfItems - 2).ToString());
				}
			}
			base.DebugStringSuffix(logMode, debugStringBuilder);
			return debugStringBuilder.ToString();
		}

		// Token: 0x04000673 RID: 1651
		public OnPathDelegate[] callbacks;

		// Token: 0x04000674 RID: 1652
		public GraphNode[] targetNodes;

		// Token: 0x04000675 RID: 1653
		protected int targetNodeCount;

		// Token: 0x04000676 RID: 1654
		public bool[] targetsFound;

		// Token: 0x04000677 RID: 1655
		public uint[] targetPathCosts;

		// Token: 0x04000678 RID: 1656
		public Vector3[] targetPoints;

		// Token: 0x04000679 RID: 1657
		public Vector3[] originalTargetPoints;

		// Token: 0x0400067A RID: 1658
		public List<Vector3>[] vectorPaths;

		// Token: 0x0400067B RID: 1659
		public List<GraphNode>[] nodePaths;

		// Token: 0x0400067C RID: 1660
		public bool pathsForAll = true;

		// Token: 0x0400067D RID: 1661
		public int chosenTarget = -1;
	}
}
