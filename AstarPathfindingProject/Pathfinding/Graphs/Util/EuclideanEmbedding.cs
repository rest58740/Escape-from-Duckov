using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Pooling;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Util
{
	// Token: 0x02000195 RID: 405
	[Serializable]
	public class EuclideanEmbedding
	{
		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x0003E793 File Offset: 0x0003C993
		// (set) Token: 0x06000B23 RID: 2851 RVA: 0x0003E79B File Offset: 0x0003C99B
		public NativeArray<uint> costs { get; private set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x0003E7A4 File Offset: 0x0003C9A4
		// (set) Token: 0x06000B25 RID: 2853 RVA: 0x0003E7AC File Offset: 0x0003C9AC
		public int pivotCount { get; private set; }

		// Token: 0x06000B26 RID: 2854 RVA: 0x0003E7B5 File Offset: 0x0003C9B5
		private uint GetRandom()
		{
			this.rval = 12820163U * this.rval + 1140671485U;
			return this.rval;
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0003E7D8 File Offset: 0x0003C9D8
		public void OnDisable()
		{
			if (this.costs.IsCreated)
			{
				this.costs.Dispose();
			}
			this.costs = default(NativeArray<uint>);
			this.pivotCount = 0;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0003E81C File Offset: 0x0003CA1C
		public unsafe static uint GetHeuristic(UnsafeSpan<uint> costs, uint pivotCount, uint nodeIndex1, uint nodeIndex2)
		{
			uint num = 0U;
			if ((ulong)nodeIndex1 < (ulong)((long)costs.Length) && (ulong)nodeIndex2 < (ulong)((long)costs.Length))
			{
				for (uint num2 = 0U; num2 < pivotCount; num2 += 1U)
				{
					uint num3 = *costs[nodeIndex1 * pivotCount + num2];
					uint num4 = *costs[nodeIndex2 * pivotCount + num2];
					if (num3 != 4294967295U && num4 != 4294967295U)
					{
						uint num5 = (uint)math.abs((int)(num3 - num4));
						if (num5 > num)
						{
							num = num5;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0003E888 File Offset: 0x0003CA88
		private void GetClosestWalkableNodesToChildrenRecursively(Transform tr, List<GraphNode> nodes)
		{
			NNConstraint walkable = NNConstraint.Walkable;
			foreach (object obj in tr)
			{
				Transform transform = (Transform)obj;
				NNInfo nearest = AstarPath.active.GetNearest(transform.position, walkable);
				if (nearest.node != null && nearest.node.Walkable)
				{
					nodes.Add(nearest.node);
				}
				this.GetClosestWalkableNodesToChildrenRecursively(transform, nodes);
			}
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0003E91C File Offset: 0x0003CB1C
		private void PickNRandomNodes(int count, List<GraphNode> buffer)
		{
			int n = 0;
			NavGraph[] graphs = AstarPath.active.graphs;
			if (graphs == null)
			{
				return;
			}
			Action<GraphNode> <>9__0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null)
				{
					NavGraph navGraph = graphs[i];
					Action<GraphNode> action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate(GraphNode node)
						{
							if (!node.Destroyed && node.Walkable)
							{
								int n = n;
								n++;
								if ((ulong)this.GetRandom() % (ulong)((long)n) < (ulong)((long)count))
								{
									if (buffer.Count < count)
									{
										buffer.Add(node);
										return;
									}
									buffer[n % buffer.Count] = node;
								}
							}
						});
					}
					navGraph.GetNodes(action);
				}
			}
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0003E994 File Offset: 0x0003CB94
		private GraphNode PickAnyWalkableNode()
		{
			NavGraph[] graphs = AstarPath.active.graphs;
			GraphNode first = null;
			Action<GraphNode> <>9__0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null)
				{
					NavGraph navGraph = graphs[i];
					Action<GraphNode> action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate(GraphNode node)
						{
							if (node != null && node.Walkable && first == null)
							{
								first = node;
							}
						});
					}
					navGraph.GetNodes(action);
				}
			}
			return first;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0003E9FC File Offset: 0x0003CBFC
		public void RecalculatePivots()
		{
			if (this.mode == HeuristicOptimizationMode.None)
			{
				this.pivotCount = 0;
				this.pivots = null;
				return;
			}
			this.rval = (uint)this.seed;
			List<GraphNode> list = ListPool<GraphNode>.Claim();
			switch (this.mode)
			{
			case HeuristicOptimizationMode.Random:
				this.PickNRandomNodes(this.spreadOutCount, list);
				break;
			case HeuristicOptimizationMode.RandomSpreadOut:
			{
				if (this.pivotPointRoot != null)
				{
					this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, list);
				}
				if (list.Count == 0)
				{
					GraphNode graphNode = this.PickAnyWalkableNode();
					if (graphNode == null)
					{
						ListPool<GraphNode>.Release(ref list);
						this.pivots = new GraphNode[0];
						return;
					}
					list.Add(graphNode);
				}
				int num = this.spreadOutCount - list.Count;
				for (int i = 0; i < num; i++)
				{
					list.Add(null);
				}
				break;
			}
			case HeuristicOptimizationMode.Custom:
				if (this.pivotPointRoot == null)
				{
					throw new Exception("heuristicOptimizationMode is HeuristicOptimizationMode.Custom, but no 'customHeuristicOptimizationPivotsRoot' is set");
				}
				this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, list);
				break;
			default:
				throw new Exception("Invalid HeuristicOptimizationMode: " + this.mode.ToString());
			}
			this.pivots = list.ToArray();
			ListPool<GraphNode>.Release(ref list);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0003EB34 File Offset: 0x0003CD34
		public void RecalculateCosts()
		{
			if (this.pivots == null)
			{
				this.RecalculatePivots();
			}
			if (this.mode == HeuristicOptimizationMode.None)
			{
				return;
			}
			this.RecalculateCostsInner();
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0003EB54 File Offset: 0x0003CD54
		private void RecalculateCostsInner()
		{
			this.pivotCount = 0;
			for (int i = 0; i < this.pivots.Length; i++)
			{
				if (this.pivots[i] != null && (this.pivots[i].Destroyed || !this.pivots[i].Walkable))
				{
					throw new Exception("Invalid pivot nodes (destroyed or unwalkable)");
				}
			}
			if (this.mode != HeuristicOptimizationMode.RandomSpreadOut)
			{
				for (int j = 0; j < this.pivots.Length; j++)
				{
					if (this.pivots[j] == null)
					{
						throw new Exception("Invalid pivot nodes (null)");
					}
				}
			}
			this.pivotCount = this.pivots.Length;
			Action<int> startCostCalculation = null;
			int numComplete = 0;
			uint nextNodeIndex = AstarPath.active.nodeStorage.nextNodeIndex;
			if (this.costs.IsCreated)
			{
				this.costs.Dispose();
			}
			this.costs = new NativeArray<uint>((int)(nextNodeIndex * (uint)this.pivotCount), Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.costs.AsUnsafeSpan<uint>().Fill(uint.MaxValue);
			startCostCalculation = delegate(int pivotIndex)
			{
				GraphNode startNode = this.pivots[pivotIndex];
				EuclideanEmbedding.EuclideanEmbeddingSearchPath path = EuclideanEmbedding.EuclideanEmbeddingSearchPath.Construct(this.costs.AsUnsafeSpan<uint>(), (uint)this.pivotCount, (uint)pivotIndex, startNode);
				path.immediateCallback = delegate(Path _)
				{
					if (this.mode == HeuristicOptimizationMode.RandomSpreadOut && pivotIndex < this.pivots.Length - 1)
					{
						if (this.pivots[pivotIndex + 1] == null)
						{
							this.pivots[pivotIndex + 1] = path.furthestNode;
							if (path.furthestNode == null)
							{
								Debug.LogError("Failed generating random pivot points for heuristic optimizations");
								return;
							}
						}
						startCostCalculation(pivotIndex + 1);
					}
					int numComplete = numComplete;
					numComplete++;
					if (numComplete == this.pivotCount)
					{
						this.ApplyGridGraphEndpointSpecialCase();
					}
				};
				AstarPath.StartPath(path, true, true);
			};
			if (this.mode != HeuristicOptimizationMode.RandomSpreadOut)
			{
				for (int k = 0; k < this.pivots.Length; k++)
				{
					startCostCalculation(k);
				}
			}
			else if (this.pivots.Length != 0)
			{
				startCostCalculation(0);
			}
			this.dirty = false;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0003ECBC File Offset: 0x0003CEBC
		private unsafe void ApplyGridGraphEndpointSpecialCase()
		{
			UnsafeSpan<uint> unsafeSpan = this.costs.AsUnsafeSpan<uint>();
			NavGraph[] graphs = AstarPath.active.graphs;
			for (int i = 0; i < graphs.Length; i++)
			{
				GridGraph gridGraph = graphs[i] as GridGraph;
				if (gridGraph != null)
				{
					GridNodeBase[] nodes = gridGraph.nodes;
					int num = (gridGraph.neighbours == NumNeighbours.Four) ? 4 : ((gridGraph.neighbours == NumNeighbours.Eight) ? 8 : 6);
					for (int j = 0; j < gridGraph.depth; j++)
					{
						for (int k = 0; k < gridGraph.width; k++)
						{
							GridNodeBase gridNodeBase = nodes[j * gridGraph.width + k];
							if (!gridNodeBase.Walkable)
							{
								uint num2 = gridNodeBase.NodeIndex * (uint)this.pivotCount;
								for (int l = 0; l < this.pivotCount; l++)
								{
									*unsafeSpan[num2 + (uint)l] = uint.MaxValue;
								}
								for (int m = 0; m < num; m++)
								{
									int num3;
									int num4;
									if (gridGraph.neighbours == NumNeighbours.Six)
									{
										num3 = k + GridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[m]];
										num4 = j + GridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[m]];
									}
									else
									{
										num3 = k + GridGraph.neighbourXOffsets[m];
										num4 = j + GridGraph.neighbourZOffsets[m];
									}
									if (num3 >= 0 && num4 >= 0 && num3 < gridGraph.width && num4 < gridGraph.depth)
									{
										GridNodeBase gridNodeBase2 = gridGraph.nodes[num4 * gridGraph.width + num3];
										if (gridNodeBase2.Walkable)
										{
											uint num5 = 0U;
											while ((ulong)num5 < (ulong)((long)this.pivotCount))
											{
												uint val = *unsafeSpan[gridNodeBase2.NodeIndex * (uint)this.pivotCount + num5] + gridGraph.neighbourCosts[m];
												*unsafeSpan[num2 + num5] = Math.Min(*unsafeSpan[num2 + num5], val);
												num5 += 1U;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0003EEB0 File Offset: 0x0003D0B0
		public void OnDrawGizmos()
		{
			if (this.pivots != null)
			{
				for (int i = 0; i < this.pivots.Length; i++)
				{
					if (this.pivots[i] != null && !this.pivots[i].Destroyed)
					{
						Draw.SolidBox((Vector3)this.pivots[i].position, Vector3.one, new Color(0.62352943f, 0.36862746f, 0.7607843f, 0.8f));
					}
				}
			}
		}

		// Token: 0x0400078A RID: 1930
		public HeuristicOptimizationMode mode;

		// Token: 0x0400078B RID: 1931
		public int seed;

		// Token: 0x0400078C RID: 1932
		public Transform pivotPointRoot;

		// Token: 0x0400078D RID: 1933
		public int spreadOutCount = 10;

		// Token: 0x0400078E RID: 1934
		[NonSerialized]
		public bool dirty;

		// Token: 0x04000791 RID: 1937
		private GraphNode[] pivots;

		// Token: 0x04000792 RID: 1938
		private const uint ra = 12820163U;

		// Token: 0x04000793 RID: 1939
		private const uint rc = 1140671485U;

		// Token: 0x04000794 RID: 1940
		private uint rval;

		// Token: 0x02000196 RID: 406
		private class EuclideanEmbeddingSearchPath : Path
		{
			// Token: 0x06000B32 RID: 2866 RVA: 0x0003EF40 File Offset: 0x0003D140
			public static EuclideanEmbedding.EuclideanEmbeddingSearchPath Construct(UnsafeSpan<uint> costs, uint costIndexStride, uint pivotIndex, GraphNode startNode)
			{
				EuclideanEmbedding.EuclideanEmbeddingSearchPath path = PathPool.GetPath<EuclideanEmbedding.EuclideanEmbeddingSearchPath>();
				path.costs = costs;
				path.costIndexStride = costIndexStride;
				path.pivotIndex = pivotIndex;
				path.startNode = startNode;
				path.furthestNodeScore = 0U;
				path.furthestNode = null;
				return path;
			}

			// Token: 0x06000B33 RID: 2867 RVA: 0x0003EF71 File Offset: 0x0003D171
			protected override void OnFoundEndNode(uint pathNode, uint hScore, uint gScore)
			{
				throw new InvalidOperationException();
			}

			// Token: 0x06000B34 RID: 2868 RVA: 0x0003346F File Offset: 0x0003166F
			protected override void OnHeapExhausted()
			{
				base.CompleteState = PathCompleteState.Complete;
			}

			// Token: 0x06000B35 RID: 2869 RVA: 0x0003EF78 File Offset: 0x0003D178
			public unsafe override void OnVisitNode(uint pathNode, uint hScore, uint gScore)
			{
				if (!this.pathHandler.IsTemporaryNode(pathNode))
				{
					GraphNode node = this.pathHandler.GetNode(pathNode);
					uint num = node.NodeIndex * this.costIndexStride;
					*this.costs[num + this.pivotIndex] = math.min(*this.costs[num + this.pivotIndex], gScore);
					uint num2 = uint.MaxValue;
					int num3 = 0;
					while ((long)num3 <= (long)((ulong)this.pivotIndex))
					{
						num2 = math.min(num2, *this.costs[num + (uint)num3]);
						num3++;
					}
					if (num2 > this.furthestNodeScore || this.furthestNode == null)
					{
						this.furthestNodeScore = num2;
						this.furthestNode = node;
					}
				}
			}

			// Token: 0x06000B36 RID: 2870 RVA: 0x0003F02C File Offset: 0x0003D22C
			protected override void Prepare()
			{
				this.pathHandler.AddTemporaryNode(new TemporaryNode
				{
					associatedNode = this.startNode.NodeIndex,
					position = this.startNode.position,
					type = TemporaryNodeType.Start
				});
				this.heuristicObjective = new HeuristicObjective(0, Heuristic.None, 0f);
				base.MarkNodesAdjacentToTemporaryEndNodes();
				base.AddStartNodesToHeap();
			}

			// Token: 0x04000795 RID: 1941
			public UnsafeSpan<uint> costs;

			// Token: 0x04000796 RID: 1942
			public uint costIndexStride;

			// Token: 0x04000797 RID: 1943
			public uint pivotIndex;

			// Token: 0x04000798 RID: 1944
			public GraphNode startNode;

			// Token: 0x04000799 RID: 1945
			public uint furthestNodeScore;

			// Token: 0x0400079A RID: 1946
			public GraphNode furthestNode;
		}
	}
}
