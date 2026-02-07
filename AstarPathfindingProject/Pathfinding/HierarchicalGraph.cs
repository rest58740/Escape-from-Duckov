using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Pooling;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000A2 RID: 162
	public class HierarchicalGraph
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00018E65 File Offset: 0x00017065
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x00018E6D File Offset: 0x0001706D
		public int version { get; private set; }

		// Token: 0x06000508 RID: 1288 RVA: 0x00018E78 File Offset: 0x00017078
		internal void OnDisable()
		{
			this.rwLock.WriteSync().Unlock();
			this.navmeshEdges.Dispose();
			if (this.gcHandle.IsAllocated)
			{
				this.gcHandle.Free();
			}
			if (this.connectionAllocator.IsCreated)
			{
				this.numHierarchicalNodes.Dispose();
				this.connectionAllocator.Dispose();
				this.connectionAllocations.Dispose();
				this.bounds.Dispose();
				this.dirtiedHierarchicalNodes.Dispose();
				this.dirtyNodes.Dispose();
				this.children = null;
				this.areas = null;
				this.dirty = null;
				this.versions = null;
				this.freeNodeIndices.Clear();
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00018F31 File Offset: 0x00017131
		public int GetHierarchicalNodeVersion(int index)
		{
			return index * 71237 ^ this.versions[index];
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00018F44 File Offset: 0x00017144
		public HierarchicalGraph.HierarhicalNodeData GetHierarhicalNodeData(out RWLock.ReadLockAsync readLock)
		{
			readLock = this.rwLock.Read();
			return new HierarchicalGraph.HierarhicalNodeData
			{
				connectionAllocator = this.connectionAllocator,
				connectionAllocations = this.connectionAllocations,
				bounds = this.bounds
			};
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00018F94 File Offset: 0x00017194
		internal HierarchicalGraph(GlobalNodeStorage nodeStorage)
		{
			this.nodeStorage = nodeStorage;
			this.navmeshEdges = new NavmeshEdges();
			this.navmeshEdges.hierarchicalGraph = this;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00018FF4 File Offset: 0x000171F4
		public void OnEnable()
		{
			if (!this.connectionAllocator.IsCreated)
			{
				this.gcHandle = GCHandle.Alloc(this);
				this.connectionAllocator = new SlabAllocator<int>(1024, Allocator.Persistent);
				this.connectionAllocations = new NativeList<int>(0, Allocator.Persistent);
				this.bounds = new NativeList<Bounds>(0, Allocator.Persistent);
				this.numHierarchicalNodes = new NativeReference<int>(0, Allocator.Persistent);
				this.dirtiedHierarchicalNodes = new NativeList<int>(0, Allocator.Persistent);
				this.dirtyNodes = new HierarchicalBitset(1024, Allocator.Persistent);
				this.children = new List<GraphNode>[]
				{
					new List<GraphNode>()
				};
				this.areas = new int[1];
				this.dirty = new byte[1];
				this.versions = new int[1];
				this.freeNodeIndices.Clear();
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x000190CF File Offset: 0x000172CF
		internal void OnCreatedNode(GraphNode node)
		{
			this.AddDirtyNode(node);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000190D8 File Offset: 0x000172D8
		internal void OnDestroyedNode(GraphNode node)
		{
			this.dirty[node.HierarchicalNodeIndex] = 1;
			this.dirtyNodes.Reset((int)node.NodeIndex);
			node.IsHierarchicalNodeDirty = false;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00019100 File Offset: 0x00017300
		public void AddDirtyNode(GraphNode node)
		{
			if (!node.IsHierarchicalNodeDirty)
			{
				if (!this.dirtyNodes.IsCreated || node.Destroyed)
				{
					return;
				}
				this.dirtyNodes.Set((int)node.NodeIndex);
				this.dirty[node.HierarchicalNodeIndex] = 1;
				node.IsHierarchicalNodeDirty = true;
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00019151 File Offset: 0x00017351
		public void ReserveNodeIndices(uint nodeIndexCount)
		{
			this.dirtyNodes.Capacity = Mathf.Max(this.dirtyNodes.Capacity, (int)nodeIndexCount);
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0001916F File Offset: 0x0001736F
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x00019177 File Offset: 0x00017377
		public int NumConnectedComponents { get; private set; }

		// Token: 0x06000513 RID: 1299 RVA: 0x00019180 File Offset: 0x00017380
		public uint GetConnectedComponent(int hierarchicalNodeIndex)
		{
			return (uint)this.areas[hierarchicalNodeIndex];
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001918C File Offset: 0x0001738C
		public void RecalculateIfNecessary()
		{
			this.JobRecalculateIfNecessary(default(JobHandle)).Complete();
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000191B0 File Offset: 0x000173B0
		public JobHandle JobRecalculateIfNecessary(JobHandle dependsOn = default(JobHandle))
		{
			if (!this.connectionAllocator.IsCreated)
			{
				throw new InvalidOperationException("The hierarchical graph has not been initialized. Please call OnEnable before using it.");
			}
			if (!this.dirtyNodes.IsEmpty)
			{
				RWLock.WriteLockAsync writeLockAsync = this.rwLock.Write();
				JobHandle jobHandle = new HierarchicalGraph.JobRecalculateComponents
				{
					hGraphGC = this.gcHandle,
					connectionAllocations = this.connectionAllocations,
					bounds = this.bounds,
					dirtiedHierarchicalNodes = this.dirtiedHierarchicalNodes,
					numHierarchicalNodes = this.numHierarchicalNodes
				}.Schedule(JobHandle.CombineDependencies(writeLockAsync.dependency, dependsOn));
				jobHandle = this.navmeshEdges.RecalculateObstacles(this.dirtiedHierarchicalNodes, this.numHierarchicalNodes, jobHandle);
				writeLockAsync.UnlockAfter(jobHandle);
				return jobHandle;
			}
			return dependsOn;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00019274 File Offset: 0x00017474
		public void RecalculateAll()
		{
			RWLock.LockSync lockSync = this.rwLock.WriteSync();
			AstarPath.active.data.GetNodes(new Action<GraphNode>(this.AddDirtyNode));
			lockSync.Unlock();
			this.RecalculateIfNecessary();
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x000192B8 File Offset: 0x000174B8
		public unsafe void OnDrawGizmos(DrawingData gizmos, RedrawScope redrawScope)
		{
			NodeHasher hasher = new NodeHasher(AstarPath.active);
			hasher.Add<int>(this.gizmoVersion);
			if (!gizmos.Draw(hasher, redrawScope))
			{
				RWLock.LockSync lockSync = this.rwLock.ReadSync();
				try
				{
					using (CommandBuilder builder = gizmos.GetBuilder(hasher, redrawScope, false))
					{
						for (int i = 0; i < this.areas.Length; i++)
						{
							if (this.children[i].Count > 0)
							{
								builder.WireBox(this.bounds[i].center, this.bounds[i].size);
								UnsafeSpan<int> span = this.connectionAllocator.GetSpan(this.connectionAllocations[i]);
								for (int j = 0; j < span.Length; j++)
								{
									if (*span[j] > i)
									{
										builder.Line(this.bounds[i].center, this.bounds[*span[j]].center, Color.black);
									}
								}
							}
						}
					}
				}
				finally
				{
					lockSync.Unlock();
				}
			}
		}

		// Token: 0x04000356 RID: 854
		private const int Tiling = 16;

		// Token: 0x04000357 RID: 855
		private const int MaxChildrenPerNode = 256;

		// Token: 0x04000358 RID: 856
		private const int MinChildrenPerNode = 128;

		// Token: 0x04000359 RID: 857
		private GlobalNodeStorage nodeStorage;

		// Token: 0x0400035A RID: 858
		internal List<GraphNode>[] children;

		// Token: 0x0400035B RID: 859
		internal NativeList<int> connectionAllocations;

		// Token: 0x0400035C RID: 860
		internal SlabAllocator<int> connectionAllocator;

		// Token: 0x0400035D RID: 861
		private NativeList<int> dirtiedHierarchicalNodes;

		// Token: 0x0400035E RID: 862
		private int[] areas;

		// Token: 0x0400035F RID: 863
		private byte[] dirty;

		// Token: 0x04000360 RID: 864
		private int[] versions;

		// Token: 0x04000361 RID: 865
		internal NativeList<Bounds> bounds;

		// Token: 0x04000362 RID: 866
		private NativeReference<int> numHierarchicalNodes;

		// Token: 0x04000363 RID: 867
		internal GCHandle gcHandle;

		// Token: 0x04000365 RID: 869
		public NavmeshEdges navmeshEdges;

		// Token: 0x04000366 RID: 870
		private Queue<GraphNode> temporaryQueue = new Queue<GraphNode>();

		// Token: 0x04000367 RID: 871
		private List<int> currentConnections = new List<int>();

		// Token: 0x04000368 RID: 872
		private Stack<int> temporaryStack = new Stack<int>();

		// Token: 0x04000369 RID: 873
		private HierarchicalBitset dirtyNodes;

		// Token: 0x0400036A RID: 874
		private CircularBuffer<int> freeNodeIndices;

		// Token: 0x0400036B RID: 875
		private int gizmoVersion;

		// Token: 0x0400036C RID: 876
		private RWLock rwLock = new RWLock();

		// Token: 0x020000A3 RID: 163
		public struct HierarhicalNodeData
		{
			// Token: 0x0400036E RID: 878
			[ReadOnly]
			public SlabAllocator<int> connectionAllocator;

			// Token: 0x0400036F RID: 879
			[ReadOnly]
			public NativeList<int> connectionAllocations;

			// Token: 0x04000370 RID: 880
			[ReadOnly]
			public NativeList<Bounds> bounds;
		}

		// Token: 0x020000A4 RID: 164
		private struct JobRecalculateComponents : IJob
		{
			// Token: 0x06000518 RID: 1304 RVA: 0x0001943C File Offset: 0x0001763C
			private void Grow(HierarchicalGraph graph)
			{
				List<GraphNode>[] array = new List<GraphNode>[Math.Max(64, graph.children.Length * 2)];
				int[] array2 = new int[array.Length];
				byte[] array3 = new byte[array.Length];
				int[] array4 = new int[array.Length];
				this.numHierarchicalNodes.Value = array.Length;
				graph.children.CopyTo(array, 0);
				graph.areas.CopyTo(array2, 0);
				graph.dirty.CopyTo(array3, 0);
				graph.versions.CopyTo(array4, 0);
				this.bounds.Resize(array.Length, NativeArrayOptions.UninitializedMemory);
				this.connectionAllocations.Resize(array.Length, NativeArrayOptions.ClearMemory);
				for (int i = array.Length - 1; i >= graph.children.Length; i--)
				{
					array[i] = ListPool<GraphNode>.Claim(256);
					this.connectionAllocations[i] = -2;
					if (i > 0)
					{
						graph.freeNodeIndices.PushEnd(i);
					}
				}
				this.connectionAllocations[0] = -2;
				graph.children = array;
				graph.areas = array2;
				graph.dirty = array3;
				graph.versions = array4;
			}

			// Token: 0x06000519 RID: 1305 RVA: 0x0001954F File Offset: 0x0001774F
			private int GetHierarchicalNodeIndex(HierarchicalGraph graph)
			{
				if (graph.freeNodeIndices.Length == 0)
				{
					this.Grow(graph);
				}
				return graph.freeNodeIndices.PopEnd();
			}

			// Token: 0x0600051A RID: 1306 RVA: 0x00019570 File Offset: 0x00017770
			private unsafe void RemoveHierarchicalNode(HierarchicalGraph hGraph, int hierarchicalNode, bool removeAdjacentSmallNodes)
			{
				hGraph.freeNodeIndices.PushEnd(hierarchicalNode);
				hGraph.versions[hierarchicalNode]++;
				int allocatedIndex = this.connectionAllocations[hierarchicalNode];
				UnsafeSpan<int> span = hGraph.connectionAllocator.GetSpan(allocatedIndex);
				for (int i = 0; i < span.Length; i++)
				{
					int num = *span[i];
					if (hGraph.dirty[num] == 0)
					{
						if (removeAdjacentSmallNodes && hGraph.children[num].Count < 128)
						{
							hGraph.dirty[num] = 2;
							this.RemoveHierarchicalNode(hGraph, num, false);
							span = hGraph.connectionAllocator.GetSpan(allocatedIndex);
						}
						else
						{
							SlabAllocator<int>.List list = hGraph.connectionAllocator.GetList(this.connectionAllocations[num]);
							ref list.Remove(hierarchicalNode);
							this.connectionAllocations[num] = list.allocationIndex;
						}
					}
				}
				hGraph.connectionAllocator.Free(allocatedIndex);
				this.connectionAllocations[hierarchicalNode] = -2;
				List<GraphNode> list2 = hGraph.children[hierarchicalNode];
				byte b = hGraph.dirty[hierarchicalNode];
				for (int j = 0; j < list2.Count; j++)
				{
					if (!list2[j].Destroyed)
					{
						hGraph.AddDirtyNode(list2[j]);
					}
				}
				hGraph.dirty[hierarchicalNode] = b;
				list2.ClearFast<GraphNode>();
			}

			// Token: 0x0600051B RID: 1307 RVA: 0x000196C8 File Offset: 0x000178C8
			[Conditional("CHECK_INVARIANTS")]
			private unsafe void CheckConnectionInvariants()
			{
				HierarchicalGraph hierarchicalGraph = (HierarchicalGraph)this.hGraphGC.Target;
				int length = this.connectionAllocations.Length;
				for (int i = 0; i < this.connectionAllocations.Length; i++)
				{
					if (this.connectionAllocations[i] != -2)
					{
						UnsafeSpan<int> span = hierarchicalGraph.connectionAllocator.GetSpan(this.connectionAllocations[i]);
						for (int j = 0; j < span.Length; j++)
						{
							if (!hierarchicalGraph.connectionAllocator.GetSpan(this.connectionAllocations[*span[j]]).Contains(i))
							{
								throw new Exception("Connections are not bidirectional");
							}
						}
					}
				}
			}

			// Token: 0x0600051C RID: 1308 RVA: 0x00019778 File Offset: 0x00017978
			[Conditional("CHECK_INVARIANTS")]
			private void CheckPreUpdateInvariants()
			{
				HierarchicalGraph hierarchicalGraph = (HierarchicalGraph)this.hGraphGC.Target;
				int length = this.connectionAllocations.Length;
				for (int i = 0; i < this.connectionAllocations.Length; i++)
				{
					if (this.connectionAllocations[i] != -2)
					{
						List<GraphNode> list = hierarchicalGraph.children[i];
						for (int j = 0; j < list.Count; j++)
						{
							bool destroyed = list[j].Destroyed;
						}
					}
				}
			}

			// Token: 0x0600051D RID: 1309 RVA: 0x000197F4 File Offset: 0x000179F4
			[Conditional("CHECK_INVARIANTS")]
			private void CheckChildInvariants()
			{
				HierarchicalGraph hierarchicalGraph = (HierarchicalGraph)this.hGraphGC.Target;
				int length = this.connectionAllocations.Length;
				for (int i = 0; i < this.connectionAllocations.Length; i++)
				{
					if (this.connectionAllocations[i] != -2)
					{
						List<GraphNode> list = hierarchicalGraph.children[i];
						for (int j = 0; j < list.Count; j++)
						{
						}
					}
				}
			}

			// Token: 0x0600051E RID: 1310 RVA: 0x00019860 File Offset: 0x00017A60
			private void FindHierarchicalNodeChildren(HierarchicalGraph hGraph, int hierarchicalNode, GraphNode startNode)
			{
				hGraph.versions[hierarchicalNode]++;
				Queue<GraphNode> temporaryQueue = hGraph.temporaryQueue;
				HierarchicalGraph.JobRecalculateComponents.Context context2 = new HierarchicalGraph.JobRecalculateComponents.Context
				{
					children = hGraph.children[hierarchicalNode],
					hierarchicalNodeIndex = hierarchicalNode,
					connections = hGraph.currentConnections,
					graphindex = startNode.GraphIndex,
					queue = temporaryQueue
				};
				context2.connections.Clear();
				context2.children.Add(startNode);
				context2.queue.Enqueue(startNode);
				startNode.HierarchicalNodeIndex = hierarchicalNode;
				GraphNode.GetConnectionsWithData<HierarchicalGraph.JobRecalculateComponents.Context> action = delegate(GraphNode neighbour, ref HierarchicalGraph.JobRecalculateComponents.Context context)
				{
					if (neighbour.Destroyed)
					{
						throw new InvalidOperationException(string.Concat(new string[]
						{
							"A node in a ",
							AstarPath.active.graphs[(int)context.graphindex].GetType().Name,
							" contained a connection to a destroyed ",
							neighbour.GetType().Name,
							"."
						}));
					}
					int hierarchicalNodeIndex = neighbour.HierarchicalNodeIndex;
					if (hierarchicalNodeIndex == 0)
					{
						if (context.children.Count < 256 && neighbour.Walkable && neighbour.GraphIndex == context.graphindex)
						{
							neighbour.HierarchicalNodeIndex = context.hierarchicalNodeIndex;
							context.queue.Enqueue(neighbour);
							context.children.Add(neighbour);
							return;
						}
					}
					else if (hierarchicalNodeIndex != context.hierarchicalNodeIndex && !context.connections.Contains(hierarchicalNodeIndex))
					{
						context.connections.Add(hierarchicalNodeIndex);
					}
				};
				while (temporaryQueue.Count > 0)
				{
					temporaryQueue.Dequeue().GetConnections<HierarchicalGraph.JobRecalculateComponents.Context>(action, ref context2, 48);
				}
				if (hGraph.currentConnections.Count > 4096)
				{
					throw new Exception("Too many connections for a single hierarchical node. Do you have thousands of off-mesh links in a single location?");
				}
				for (int i = 0; i < hGraph.currentConnections.Count; i++)
				{
					int index = hGraph.currentConnections[i];
					int allocatedIndex = this.connectionAllocations[index];
					SlabAllocator<int>.List list = hGraph.connectionAllocator.GetList(allocatedIndex);
					list.Add(hierarchicalNode);
					this.connectionAllocations[index] = list.allocationIndex;
				}
				this.connectionAllocations[hierarchicalNode] = hGraph.connectionAllocator.Allocate(hGraph.currentConnections);
				temporaryQueue.Clear();
			}

			// Token: 0x0600051F RID: 1311 RVA: 0x000199D0 File Offset: 0x00017BD0
			private unsafe void FloodFill(HierarchicalGraph hGraph)
			{
				int[] areas = hGraph.areas;
				for (int i = 0; i < areas.Length; i++)
				{
					areas[i] = 0;
				}
				Stack<int> temporaryStack = hGraph.temporaryStack;
				int num = 0;
				for (int j = 1; j < areas.Length; j++)
				{
					if (areas[j] == 0 && this.connectionAllocations[j] != -2)
					{
						num++;
						areas[j] = num;
						temporaryStack.Push(j);
						while (temporaryStack.Count > 0)
						{
							int index = temporaryStack.Pop();
							UnsafeSpan<int> span = hGraph.connectionAllocator.GetSpan(this.connectionAllocations[index]);
							for (int k = span.Length - 1; k >= 0; k--)
							{
								int num2 = *span[k];
								if (areas[num2] != num)
								{
									areas[num2] = num;
									temporaryStack.Push(num2);
								}
							}
						}
					}
				}
				hGraph.NumConnectedComponents = Math.Max(1, num + 1);
				int version = hGraph.version;
				hGraph.version = version + 1;
			}

			// Token: 0x06000520 RID: 1312 RVA: 0x00019AC8 File Offset: 0x00017CC8
			public unsafe void Execute()
			{
				HierarchicalGraph hierarchicalGraph = this.hGraphGC.Target as HierarchicalGraph;
				byte[] dirty = hierarchicalGraph.dirty;
				int length = hierarchicalGraph.freeNodeIndices.Length;
				for (int i = 1; i < dirty.Length; i++)
				{
					if (dirty[i] == 1)
					{
						this.RemoveHierarchicalNode(hierarchicalGraph, i, true);
					}
				}
				for (int j = 1; j < dirty.Length; j++)
				{
					dirty[j] = 0;
				}
				NativeArray<int> arr = new NativeArray<int>(512, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				GlobalNodeStorage nodeStorage = hierarchicalGraph.nodeStorage;
				foreach (UnsafeSpan<int> unsafeSpan in hierarchicalGraph.dirtyNodes.GetIterator(arr.AsUnsafeSpan<int>()))
				{
					for (int k = 0; k < unsafeSpan.Length; k++)
					{
						GraphNode node = nodeStorage.GetNode((uint)(*unsafeSpan[k]));
						node.IsHierarchicalNodeDirty = false;
						node.HierarchicalNodeIndex = 0;
					}
				}
				this.dirtiedHierarchicalNodes.Clear();
				foreach (UnsafeSpan<int> unsafeSpan2 in hierarchicalGraph.dirtyNodes.GetIterator(arr.AsUnsafeSpan<int>()))
				{
					for (int l = 0; l < unsafeSpan2.Length; l++)
					{
						GraphNode node2 = nodeStorage.GetNode((uint)(*unsafeSpan2[l]));
						if (!node2.Destroyed && node2.HierarchicalNodeIndex == 0 && node2.Walkable)
						{
							int hierarchicalNodeIndex = this.GetHierarchicalNodeIndex(hierarchicalGraph);
							this.FindHierarchicalNodeChildren(hierarchicalGraph, hierarchicalNodeIndex, node2);
							this.dirtiedHierarchicalNodes.Add(hierarchicalNodeIndex);
						}
					}
				}
				for (int m = length; m < hierarchicalGraph.freeNodeIndices.Length; m++)
				{
					int num = hierarchicalGraph.freeNodeIndices[m];
					this.dirtiedHierarchicalNodes.Add(num);
				}
				hierarchicalGraph.dirtyNodes.Clear();
				this.FloodFill(hierarchicalGraph);
				hierarchicalGraph.gizmoVersion++;
			}

			// Token: 0x04000371 RID: 881
			public GCHandle hGraphGC;

			// Token: 0x04000372 RID: 882
			public NativeList<int> connectionAllocations;

			// Token: 0x04000373 RID: 883
			public NativeList<Bounds> bounds;

			// Token: 0x04000374 RID: 884
			public NativeList<int> dirtiedHierarchicalNodes;

			// Token: 0x04000375 RID: 885
			public NativeReference<int> numHierarchicalNodes;

			// Token: 0x020000A5 RID: 165
			private struct Context
			{
				// Token: 0x04000376 RID: 886
				public List<GraphNode> children;

				// Token: 0x04000377 RID: 887
				public int hierarchicalNodeIndex;

				// Token: 0x04000378 RID: 888
				public List<int> connections;

				// Token: 0x04000379 RID: 889
				public uint graphindex;

				// Token: 0x0400037A RID: 890
				public Queue<GraphNode> queue;
			}
		}
	}
}
