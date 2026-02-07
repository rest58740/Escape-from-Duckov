using System;
using Pathfinding.Collections;
using Pathfinding.Jobs;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Pathfinding
{
	// Token: 0x02000098 RID: 152
	internal class GlobalNodeStorage
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x0001806B File Offset: 0x0001626B
		// (set) Token: 0x060004D1 RID: 1233 RVA: 0x00018073 File Offset: 0x00016273
		public uint destroyedNodesVersion { get; private set; }

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001807C File Offset: 0x0001627C
		public GlobalNodeStorage(AstarPath astar)
		{
			this.astar = astar;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x000180E4 File Offset: 0x000162E4
		public GraphNode GetNode(uint nodeIndex)
		{
			return this.nodes[(int)nodeIndex];
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000180F0 File Offset: 0x000162F0
		private void DisposeThreadData()
		{
			if (this.pathfindingThreadData.Length != 0)
			{
				for (int i = 0; i < this.pathfindingThreadData.Length; i++)
				{
					this.pathfindingThreadData[i].pathNodes.Free(Allocator.Persistent);
				}
				this.pathfindingThreadData = new GlobalNodeStorage.PathfindingThreadData[0];
			}
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001813C File Offset: 0x0001633C
		public void SetThreadCount(int threadCount)
		{
			if (this.pathfindingThreadData.Length != threadCount)
			{
				this.DisposeThreadData();
				this.pathfindingThreadData = new GlobalNodeStorage.PathfindingThreadData[threadCount];
				for (int i = 0; i < this.pathfindingThreadData.Length; i++)
				{
					this.pathfindingThreadData[i].pathNodes = new UnsafeSpan<PathNode>(Allocator.Persistent, (int)(this.reservedPathNodeData + (uint)this.temporaryNodeCount));
					this.pathfindingThreadData[i].pathNodes.Fill(PathNode.Default);
				}
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x000181B8 File Offset: 0x000163B8
		public void GrowTemporaryNodeStorage(int threadID)
		{
			GlobalNodeStorage.PathfindingThreadData pathfindingThreadData = this.pathfindingThreadData[threadID];
			int num = pathfindingThreadData.pathNodes.Length - (int)this.reservedPathNodeData;
			this.temporaryNodeCount = Math.Max(this.temporaryNodeCount, num * 8);
			int length = pathfindingThreadData.pathNodes.Length;
			pathfindingThreadData.pathNodes = pathfindingThreadData.pathNodes.Reallocate(Allocator.Persistent, (int)(this.reservedPathNodeData + (uint)this.temporaryNodeCount));
			pathfindingThreadData.pathNodes.Slice(length).Fill(PathNode.Default);
			this.pathfindingThreadData[threadID] = pathfindingThreadData;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001824C File Offset: 0x0001644C
		public void InitializeNode(GraphNode node)
		{
			int pathNodeVariants = node.PathNodeVariants;
			lock (this)
			{
				if (this.nodeIndexPools[pathNodeVariants - 1].Count > 0)
				{
					node.NodeIndex = this.nodeIndexPools[pathNodeVariants - 1].Pop();
				}
				else
				{
					node.NodeIndex = this.nextNodeIndex;
					this.nextNodeIndex += (uint)pathNodeVariants;
					this.ReserveNodeIndices(this.nextNodeIndex);
				}
				for (int i = 0; i < pathNodeVariants; i++)
				{
					this.nodes[(int)(checked((IntPtr)(unchecked((ulong)node.NodeIndex + (ulong)((long)i)))))] = node;
				}
				this.astar.hierarchicalGraph.OnCreatedNode(node);
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00018308 File Offset: 0x00016508
		private void ReserveNodeIndices(uint nextNodeIndex)
		{
			if (nextNodeIndex <= this.reservedPathNodeData)
			{
				return;
			}
			this.reservedPathNodeData = math.ceilpow2(nextNodeIndex);
			this.astar.hierarchicalGraph.ReserveNodeIndices(this.reservedPathNodeData);
			int threadCount = this.pathfindingThreadData.Length;
			this.DisposeThreadData();
			this.SetThreadCount(threadCount);
			Memory.Realloc<GraphNode>(ref this.nodes, (int)this.reservedPathNodeData);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00018368 File Offset: 0x00016568
		public void DestroyNode(GraphNode node)
		{
			uint nodeIndex = node.NodeIndex;
			if (nodeIndex == 268435454U)
			{
				return;
			}
			uint destroyedNodesVersion = this.destroyedNodesVersion;
			this.destroyedNodesVersion = destroyedNodesVersion + 1U;
			int pathNodeVariants = node.PathNodeVariants;
			this.nodeIndexPools[pathNodeVariants - 1].Push(nodeIndex);
			for (int i = 0; i < pathNodeVariants; i++)
			{
				this.nodes[(int)(checked((IntPtr)(unchecked((ulong)nodeIndex + (ulong)((long)i)))))] = null;
			}
			for (int j = 0; j < this.pathfindingThreadData.Length; j++)
			{
				GlobalNodeStorage.PathfindingThreadData pathfindingThreadData = this.pathfindingThreadData[j];
				uint num = 0U;
				while ((ulong)num < (ulong)((long)pathNodeVariants))
				{
					pathfindingThreadData.pathNodes[nodeIndex + num].pathID = 0;
					num += 1U;
				}
			}
			this.astar.hierarchicalGraph.OnDestroyedNode(node);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00018428 File Offset: 0x00016628
		public void OnDisable()
		{
			this.lastAllocationJob.Complete();
			this.nextNodeIndex = 1U;
			this.reservedPathNodeData = 0U;
			for (int i = 0; i < this.nodeIndexPools.Length; i++)
			{
				this.nodeIndexPools[i].Clear();
			}
			this.nodes = new GraphNode[0];
			this.DisposeThreadData();
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00018480 File Offset: 0x00016680
		public JobHandle AllocateNodesJob<T>(T[] result, int count, Func<T> createNode, uint variantsPerNode) where T : GraphNode
		{
			this.lastAllocationJob = new GlobalNodeStorage.JobAllocateNodes<T>
			{
				result = result,
				count = count,
				nodeStorage = this,
				variantsPerNode = variantsPerNode,
				createNode = createNode
			}.ScheduleManaged(this.lastAllocationJob);
			return this.lastAllocationJob;
		}

		// Token: 0x04000333 RID: 819
		private readonly AstarPath astar;

		// Token: 0x04000334 RID: 820
		private JobHandle lastAllocationJob;

		// Token: 0x04000335 RID: 821
		public uint nextNodeIndex = 1U;

		// Token: 0x04000336 RID: 822
		public uint reservedPathNodeData;

		// Token: 0x04000338 RID: 824
		private const int InitialTemporaryNodes = 256;

		// Token: 0x04000339 RID: 825
		private int temporaryNodeCount = 256;

		// Token: 0x0400033A RID: 826
		private readonly GlobalNodeStorage.IndexedStack<uint>[] nodeIndexPools = new GlobalNodeStorage.IndexedStack<uint>[]
		{
			new GlobalNodeStorage.IndexedStack<uint>(),
			new GlobalNodeStorage.IndexedStack<uint>(),
			new GlobalNodeStorage.IndexedStack<uint>()
		};

		// Token: 0x0400033B RID: 827
		public GlobalNodeStorage.PathfindingThreadData[] pathfindingThreadData = new GlobalNodeStorage.PathfindingThreadData[0];

		// Token: 0x0400033C RID: 828
		private GraphNode[] nodes = new GraphNode[0];

		// Token: 0x02000099 RID: 153
		public struct PathfindingThreadData
		{
			// Token: 0x0400033D RID: 829
			public UnsafeSpan<PathNode> pathNodes;
		}

		// Token: 0x0200009A RID: 154
		private class IndexedStack<T>
		{
			// Token: 0x170000DD RID: 221
			// (get) Token: 0x060004DC RID: 1244 RVA: 0x000184D6 File Offset: 0x000166D6
			// (set) Token: 0x060004DD RID: 1245 RVA: 0x000184DE File Offset: 0x000166DE
			public int Count { get; private set; }

			// Token: 0x060004DE RID: 1246 RVA: 0x000184E8 File Offset: 0x000166E8
			public void Push(T v)
			{
				if (this.Count == this.buffer.Length)
				{
					Memory.Realloc<T>(ref this.buffer, this.buffer.Length * 2);
				}
				this.buffer[this.Count] = v;
				int count = this.Count;
				this.Count = count + 1;
			}

			// Token: 0x060004DF RID: 1247 RVA: 0x0001853C File Offset: 0x0001673C
			public void Clear()
			{
				this.Count = 0;
			}

			// Token: 0x060004E0 RID: 1248 RVA: 0x00018548 File Offset: 0x00016748
			public T Pop()
			{
				int count = this.Count;
				this.Count = count - 1;
				return this.buffer[this.Count];
			}

			// Token: 0x060004E1 RID: 1249 RVA: 0x00018576 File Offset: 0x00016776
			public void PopMany(T[] resultBuffer, int popCount)
			{
				if (popCount > this.Count)
				{
					throw new IndexOutOfRangeException();
				}
				Array.Copy(this.buffer, this.Count - popCount, resultBuffer, 0, popCount);
				this.Count -= popCount;
			}

			// Token: 0x0400033E RID: 830
			private T[] buffer = new T[4];
		}

		// Token: 0x0200009B RID: 155
		private struct JobAllocateNodes<T> : IJob where T : GraphNode
		{
			// Token: 0x170000DE RID: 222
			// (get) Token: 0x060004E3 RID: 1251 RVA: 0x000185BF File Offset: 0x000167BF
			public bool allowBoundsChecks
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060004E4 RID: 1252 RVA: 0x000185C4 File Offset: 0x000167C4
			public void Execute()
			{
				HierarchicalGraph hierarchicalGraph = this.nodeStorage.astar.hierarchicalGraph;
				GlobalNodeStorage obj = this.nodeStorage;
				lock (obj)
				{
					GlobalNodeStorage.IndexedStack<uint> indexedStack = this.nodeStorage.nodeIndexPools[(int)(this.variantsPerNode - 1U)];
					uint num = this.nodeStorage.nextNodeIndex;
					uint num2 = 0U;
					while ((ulong)num2 < (ulong)((long)this.count))
					{
						T t = this.result[(int)num2] = this.createNode();
						if (indexedStack.Count > 0)
						{
							t.NodeIndex = indexedStack.Pop();
						}
						else
						{
							t.NodeIndex = num;
							num += this.variantsPerNode;
						}
						num2 += 1U;
					}
					this.nodeStorage.ReserveNodeIndices(num);
					this.nodeStorage.nextNodeIndex = num;
					for (int i = 0; i < this.count; i++)
					{
						T t2 = this.result[i];
						hierarchicalGraph.AddDirtyNode(t2);
						this.nodeStorage.nodes[(int)t2.NodeIndex] = t2;
					}
				}
			}

			// Token: 0x04000340 RID: 832
			public T[] result;

			// Token: 0x04000341 RID: 833
			public int count;

			// Token: 0x04000342 RID: 834
			public GlobalNodeStorage nodeStorage;

			// Token: 0x04000343 RID: 835
			public uint variantsPerNode;

			// Token: 0x04000344 RID: 836
			public Func<T> createNode;
		}
	}
}
