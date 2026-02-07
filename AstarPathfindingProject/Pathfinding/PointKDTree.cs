using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000EE RID: 238
	public class PointKDTree
	{
		// Token: 0x060007DF RID: 2015 RVA: 0x00029484 File Offset: 0x00027684
		public PointKDTree()
		{
			this.tree[1] = new PointKDTree.Node
			{
				data = this.GetOrCreateList()
			};
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x000294DC File Offset: 0x000276DC
		public void Add(GraphNode node)
		{
			this.numNodes++;
			this.Add(node, 1, 0);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000294F5 File Offset: 0x000276F5
		public void Remove(GraphNode node)
		{
			if (!this.Remove(node, 1, 0))
			{
				throw new ArgumentException("The node is not in the lookup tree. Has it been moved?");
			}
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00029510 File Offset: 0x00027710
		public void Rebuild(GraphNode[] nodes, int start, int end)
		{
			if (start < 0 || end < start || end > nodes.Length)
			{
				throw new ArgumentException();
			}
			for (int i = 0; i < this.tree.Length; i++)
			{
				GraphNode[] data = this.tree[i].data;
				if (data != null)
				{
					for (int j = 0; j < 21; j++)
					{
						data[j] = null;
					}
					this.arrayCache.Push(data);
					this.tree[i].data = null;
				}
			}
			this.numNodes = end - start;
			this.Build(1, new List<GraphNode>(nodes), start, end);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x000295A0 File Offset: 0x000277A0
		private GraphNode[] GetOrCreateList()
		{
			if (this.arrayCache.Count <= 0)
			{
				return new GraphNode[21];
			}
			return this.arrayCache.Pop();
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000295C3 File Offset: 0x000277C3
		private int Size(int index)
		{
			if (this.tree[index].data == null)
			{
				return this.Size(2 * index) + this.Size(2 * index + 1);
			}
			return (int)this.tree[index].count;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00029600 File Offset: 0x00027800
		private void CollectAndClear(int index, List<GraphNode> buffer)
		{
			GraphNode[] data = this.tree[index].data;
			ushort count = this.tree[index].count;
			if (data != null)
			{
				this.tree[index] = default(PointKDTree.Node);
				for (int i = 0; i < (int)count; i++)
				{
					buffer.Add(data[i]);
					data[i] = null;
				}
				this.arrayCache.Push(data);
				return;
			}
			this.CollectAndClear(index * 2, buffer);
			this.CollectAndClear(index * 2 + 1, buffer);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00029682 File Offset: 0x00027882
		private static int MaxAllowedSize(int numNodes, int depth)
		{
			return Math.Min(5 * numNodes / 2 >> depth, 3 * numNodes / 4);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00029698 File Offset: 0x00027898
		private void Rebalance(int index)
		{
			this.CollectAndClear(index, this.largeList);
			this.Build(index, this.largeList, 0, this.largeList.Count);
			this.largeList.ClearFast<GraphNode>();
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x000296CC File Offset: 0x000278CC
		private void EnsureSize(int index)
		{
			if (index >= this.tree.Length)
			{
				PointKDTree.Node[] array = new PointKDTree.Node[Math.Max(index + 1, this.tree.Length * 2)];
				this.tree.CopyTo(array, 0);
				this.tree = array;
			}
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00029710 File Offset: 0x00027910
		private void Build(int index, List<GraphNode> nodes, int start, int end)
		{
			this.EnsureSize(index);
			if (end - start <= 10)
			{
				GraphNode[] array = this.tree[index].data = this.GetOrCreateList();
				this.tree[index].count = (ushort)(end - start);
				for (int i = start; i < end; i++)
				{
					array[i - start] = nodes[i];
				}
				return;
			}
			Int3 position;
			Int3 @int = position = nodes[start].position;
			for (int j = start; j < end; j++)
			{
				Int3 position2 = nodes[j].position;
				position = new Int3(Math.Min(position.x, position2.x), Math.Min(position.y, position2.y), Math.Min(position.z, position2.z));
				@int = new Int3(Math.Max(@int.x, position2.x), Math.Max(@int.y, position2.y), Math.Max(@int.z, position2.z));
			}
			Int3 int2 = @int - position;
			int num = (int2.x > int2.y) ? ((int2.x > int2.z) ? 0 : 2) : ((int2.y > int2.z) ? 1 : 2);
			nodes.Sort(start, end - start, PointKDTree.comparers[num]);
			int num2 = (start + end) / 2;
			this.tree[index].split = (nodes[num2 - 1].position[num] + nodes[num2].position[num]) / 2;
			this.tree[index].splitAxis = (byte)num;
			this.Build(index * 2, nodes, start, num2);
			this.Build(index * 2 + 1, nodes, num2, end);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000298FC File Offset: 0x00027AFC
		private void Add(GraphNode point, int index, int depth = 0)
		{
			while (this.tree[index].data == null)
			{
				index = 2 * index + ((point.position[(int)this.tree[index].splitAxis] < this.tree[index].split) ? 0 : 1);
				depth++;
			}
			GraphNode[] data = this.tree[index].data;
			PointKDTree.Node[] array = this.tree;
			int num = index;
			ushort count = array[num].count;
			array[num].count = count + 1;
			data[(int)count] = point;
			if (this.tree[index].count >= 21)
			{
				int num2 = 0;
				while (depth - num2 > 0 && this.Size(index >> num2) > PointKDTree.MaxAllowedSize(this.numNodes, depth - num2))
				{
					num2++;
				}
				this.Rebalance(index >> num2);
			}
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x000299D4 File Offset: 0x00027BD4
		private bool Remove(GraphNode point, int index, int depth = 0)
		{
			Int3 position = point.position;
			while (this.tree[index].data == null)
			{
				int num = position[(int)this.tree[index].splitAxis];
				if (num == this.tree[index].split)
				{
					return this.Remove(point, 2 * index, depth + 1) || this.Remove(point, 2 * index + 1, depth + 1);
				}
				index = 2 * index + ((num < this.tree[index].split) ? 0 : 1);
				depth++;
			}
			int num2 = Array.IndexOf<GraphNode>(this.tree[index].data, point);
			if (num2 == -1)
			{
				return false;
			}
			PointKDTree.Node node = this.tree[index];
			node.count -= 1;
			node.data[num2] = node.data[(int)node.count];
			node.data[(int)node.count] = null;
			this.tree[index] = node;
			this.numNodes--;
			if (node.count == 0 && index != 1)
			{
				this.Rebalance(index >> 1);
			}
			return true;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00029B00 File Offset: 0x00027D00
		public GraphNode GetNearest(Int3 point, NNConstraint constraint, ref float distanceSqr)
		{
			GraphNode graphNode = null;
			long num = (distanceSqr < float.PositiveInfinity) ? ((long)(1000000f * distanceSqr)) : long.MaxValue;
			this.GetNearestInternal(1, point, constraint, ref graphNode, ref num);
			distanceSqr = ((graphNode != null) ? (1.0000001E-06f * (float)num) : float.PositiveInfinity);
			return graphNode;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00029B50 File Offset: 0x00027D50
		private void GetNearestInternal(int index, Int3 point, NNConstraint constraint, ref GraphNode best, ref long bestSqrDist)
		{
			GraphNode[] data = this.tree[index].data;
			if (data != null)
			{
				for (int i = (int)(this.tree[index].count - 1); i >= 0; i--)
				{
					long sqrMagnitudeLong = (data[i].position - point).sqrMagnitudeLong;
					if (sqrMagnitudeLong < bestSqrDist && (constraint == null || constraint.Suitable(data[i])))
					{
						bestSqrDist = sqrMagnitudeLong;
						best = data[i];
					}
				}
				return;
			}
			long num = (long)(point[(int)this.tree[index].splitAxis] - this.tree[index].split);
			int num2 = 2 * index + ((num < 0L) ? 0 : 1);
			this.GetNearestInternal(num2, point, constraint, ref best, ref bestSqrDist);
			if (num * num < bestSqrDist)
			{
				this.GetNearestInternal(num2 ^ 1, point, constraint, ref best, ref bestSqrDist);
			}
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00029C2C File Offset: 0x00027E2C
		public GraphNode GetNearestConnection(Int3 point, NNConstraint constraint, long maximumSqrConnectionLength)
		{
			GraphNode result = null;
			long maxValue = long.MaxValue;
			long distanceThresholdOffset = (maximumSqrConnectionLength + 3L) / 4L;
			this.GetNearestConnectionInternal(1, point, constraint, ref result, ref maxValue, distanceThresholdOffset);
			return result;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00029C5C File Offset: 0x00027E5C
		private void GetNearestConnectionInternal(int index, Int3 point, NNConstraint constraint, ref GraphNode best, ref long bestSqrDist, long distanceThresholdOffset)
		{
			GraphNode[] data = this.tree[index].data;
			if (data != null)
			{
				Vector3 p = (Vector3)point;
				for (int i = (int)(this.tree[index].count - 1); i >= 0; i--)
				{
					long sqrMagnitudeLong = (data[i].position - point).sqrMagnitudeLong;
					if (sqrMagnitudeLong - distanceThresholdOffset < bestSqrDist && (constraint == null || constraint.Suitable(data[i])))
					{
						Connection[] connections = (data[i] as PointNode).connections;
						if (connections != null)
						{
							Vector3 vector = (Vector3)data[i].position;
							for (int j = 0; j < connections.Length; j++)
							{
								Vector3 b = ((Vector3)connections[j].node.position + vector) * 0.5f;
								long num = (long)(VectorMath.SqrDistancePointSegment(vector, b, p) * 1000f * 1000f);
								if (num < bestSqrDist)
								{
									bestSqrDist = num;
									best = data[i];
								}
							}
						}
						if (sqrMagnitudeLong < bestSqrDist)
						{
							bestSqrDist = sqrMagnitudeLong;
							best = data[i];
						}
					}
				}
				return;
			}
			long num2 = (long)(point[(int)this.tree[index].splitAxis] - this.tree[index].split);
			int num3 = 2 * index + ((num2 < 0L) ? 0 : 1);
			this.GetNearestConnectionInternal(num3, point, constraint, ref best, ref bestSqrDist, distanceThresholdOffset);
			if (num2 * num2 - distanceThresholdOffset < bestSqrDist)
			{
				this.GetNearestConnectionInternal(num3 ^ 1, point, constraint, ref best, ref bestSqrDist, distanceThresholdOffset);
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00029DE9 File Offset: 0x00027FE9
		public void GetInRange(Int3 point, long sqrRadius, List<GraphNode> buffer)
		{
			this.GetInRangeInternal(1, point, sqrRadius, buffer);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00029DF8 File Offset: 0x00027FF8
		private void GetInRangeInternal(int index, Int3 point, long sqrRadius, List<GraphNode> buffer)
		{
			GraphNode[] data = this.tree[index].data;
			if (data != null)
			{
				for (int i = (int)(this.tree[index].count - 1); i >= 0; i--)
				{
					if ((data[i].position - point).sqrMagnitudeLong < sqrRadius)
					{
						buffer.Add(data[i]);
					}
				}
				return;
			}
			long num = (long)(point[(int)this.tree[index].splitAxis] - this.tree[index].split);
			int num2 = 2 * index + ((num < 0L) ? 0 : 1);
			this.GetInRangeInternal(num2, point, sqrRadius, buffer);
			if (num * num < sqrRadius)
			{
				this.GetInRangeInternal(num2 ^ 1, point, sqrRadius, buffer);
			}
		}

		// Token: 0x040004F3 RID: 1267
		public const int LeafSize = 10;

		// Token: 0x040004F4 RID: 1268
		public const int LeafArraySize = 21;

		// Token: 0x040004F5 RID: 1269
		private PointKDTree.Node[] tree = new PointKDTree.Node[16];

		// Token: 0x040004F6 RID: 1270
		private int numNodes;

		// Token: 0x040004F7 RID: 1271
		private readonly List<GraphNode> largeList = new List<GraphNode>();

		// Token: 0x040004F8 RID: 1272
		private readonly Stack<GraphNode[]> arrayCache = new Stack<GraphNode[]>();

		// Token: 0x040004F9 RID: 1273
		private static readonly IComparer<GraphNode>[] comparers = new IComparer<GraphNode>[]
		{
			new PointKDTree.CompareX(),
			new PointKDTree.CompareY(),
			new PointKDTree.CompareZ()
		};

		// Token: 0x020000EF RID: 239
		private struct Node
		{
			// Token: 0x040004FA RID: 1274
			public GraphNode[] data;

			// Token: 0x040004FB RID: 1275
			public int split;

			// Token: 0x040004FC RID: 1276
			public ushort count;

			// Token: 0x040004FD RID: 1277
			public byte splitAxis;
		}

		// Token: 0x020000F0 RID: 240
		private class CompareX : IComparer<GraphNode>
		{
			// Token: 0x060007F3 RID: 2035 RVA: 0x00029EDC File Offset: 0x000280DC
			public int Compare(GraphNode lhs, GraphNode rhs)
			{
				return lhs.position.x.CompareTo(rhs.position.x);
			}
		}

		// Token: 0x020000F1 RID: 241
		private class CompareY : IComparer<GraphNode>
		{
			// Token: 0x060007F5 RID: 2037 RVA: 0x00029EF9 File Offset: 0x000280F9
			public int Compare(GraphNode lhs, GraphNode rhs)
			{
				return lhs.position.y.CompareTo(rhs.position.y);
			}
		}

		// Token: 0x020000F2 RID: 242
		private class CompareZ : IComparer<GraphNode>
		{
			// Token: 0x060007F7 RID: 2039 RVA: 0x00029F16 File Offset: 0x00028116
			public int Compare(GraphNode lhs, GraphNode rhs)
			{
				return lhs.position.z.CompareTo(rhs.position.z);
			}
		}
	}
}
