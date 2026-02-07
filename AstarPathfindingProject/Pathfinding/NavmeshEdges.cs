using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.RVO;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000074 RID: 116
	[BurstCompile]
	public class NavmeshEdges
	{
		// Token: 0x060003D1 RID: 977 RVA: 0x000137EC File Offset: 0x000119EC
		public void Dispose()
		{
			this.rwLock.WriteSync().Unlock();
			this.obstacleData.Dispose();
			this.allocationLock.Dispose();
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00013822 File Offset: 0x00011A22
		private void Init()
		{
			this.obstacleData.Init(Allocator.Persistent);
			if (!this.allocationLock.IsCreated)
			{
				this.allocationLock = new NativeReference<SpinLock>(Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00013850 File Offset: 0x00011A50
		public JobHandle RecalculateObstacles(NativeList<int> dirtyHierarchicalNodes, NativeReference<int> numHierarchicalNodes, JobHandle dependency)
		{
			this.Init();
			RWLock.WriteLockAsync writeLockAsync = this.rwLock.Write();
			JobHandle jobHandle = new NavmeshEdges.JobResizeObstacles
			{
				numHierarchicalNodes = numHierarchicalNodes,
				obstacles = this.obstacleData.obstacles
			}.Schedule(JobHandle.CombineDependencies(dependency, writeLockAsync.dependency));
			jobHandle = new NavmeshEdges.JobCalculateObstacles
			{
				hGraphGC = this.hierarchicalGraph.gcHandle,
				obstacleVertices = this.obstacleData.obstacleVertices,
				obstacleVertexGroups = this.obstacleData.obstacleVertexGroups,
				obstacles = this.obstacleData.obstacles.AsDeferredJobArray(),
				bounds = this.hierarchicalGraph.bounds.AsDeferredJobArray(),
				dirtyHierarchicalNodes = dirtyHierarchicalNodes,
				allocationLock = this.allocationLock
			}.ScheduleBatch(32, 1, jobHandle);
			writeLockAsync.UnlockAfter(jobHandle);
			this.gizmoVersion++;
			return jobHandle;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00013948 File Offset: 0x00011B48
		public unsafe void OnDrawGizmos(DrawingData gizmos, RedrawScope redrawScope)
		{
			if (!this.obstacleData.obstacleVertices.IsCreated)
			{
				return;
			}
			NodeHasher hasher = new NodeHasher(AstarPath.active);
			hasher.Add<int>(12314127);
			hasher.Add<int>(this.gizmoVersion);
			if (!gizmos.Draw(hasher, redrawScope))
			{
				RWLock.LockSync lockSync = this.rwLock.ReadSync();
				try
				{
					using (CommandBuilder builder = gizmos.GetBuilder(hasher, redrawScope, false))
					{
						for (int i = 1; i < this.obstacleData.obstacles.Length; i++)
						{
							UnmanagedObstacle unmanagedObstacle = this.obstacleData.obstacles[i];
							UnsafeSpan<float3> span = this.obstacleData.obstacleVertices.GetSpan(unmanagedObstacle.verticesAllocation);
							UnsafeSpan<ObstacleVertexGroup> span2 = this.obstacleData.obstacleVertexGroups.GetSpan(unmanagedObstacle.groupsAllocation);
							int num = 0;
							for (int j = 0; j < span2.Length; j++)
							{
								ObstacleVertexGroup obstacleVertexGroup = *span2[j];
								builder.PushLineWidth(2f, true);
								for (int k = 0; k < obstacleVertexGroup.vertexCount - 1; k++)
								{
									builder.ArrowRelativeSizeHead(*span[num + k], *span[num + k + 1], new float3(0f, 1f, 0f), 0.05f, Color.black);
								}
								if (obstacleVertexGroup.type == ObstacleType.Loop)
								{
									builder.Arrow(*span[num + obstacleVertexGroup.vertexCount - 1], *span[num], new float3(0f, 1f, 0f), 0.05f, Color.black);
								}
								builder.PopLineWidth();
								num += obstacleVertexGroup.vertexCount;
								builder.WireBox(0.5f * (obstacleVertexGroup.boundsMn + obstacleVertexGroup.boundsMx), obstacleVertexGroup.boundsMx - obstacleVertexGroup.boundsMn, Color.white);
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

		// Token: 0x060003D5 RID: 981 RVA: 0x00013BB0 File Offset: 0x00011DB0
		public NavmeshEdges.NavmeshBorderData GetNavmeshEdgeData(out RWLock.CombinedReadLockAsync readLock)
		{
			this.Init();
			RWLock.ReadLockAsync @lock = this.rwLock.Read();
			RWLock.ReadLockAsync lock2;
			HierarchicalGraph.HierarhicalNodeData hierarhicalNodeData = this.hierarchicalGraph.GetHierarhicalNodeData(out lock2);
			readLock = new RWLock.CombinedReadLockAsync(@lock, lock2);
			return new NavmeshEdges.NavmeshBorderData
			{
				hierarhicalNodeData = hierarhicalNodeData,
				obstacleData = this.obstacleData
			};
		}

		// Token: 0x0400028D RID: 653
		public SimulatorBurst.ObstacleData obstacleData;

		// Token: 0x0400028E RID: 654
		private NativeReference<SpinLock> allocationLock;

		// Token: 0x0400028F RID: 655
		private const int JobRecalculateObstaclesBatchCount = 32;

		// Token: 0x04000290 RID: 656
		private RWLock rwLock = new RWLock();

		// Token: 0x04000291 RID: 657
		public HierarchicalGraph hierarchicalGraph;

		// Token: 0x04000292 RID: 658
		private int gizmoVersion;

		// Token: 0x02000075 RID: 117
		[BurstCompile]
		private struct JobResizeObstacles : IJob
		{
			// Token: 0x060003D7 RID: 983 RVA: 0x00013C1C File Offset: 0x00011E1C
			public void Execute()
			{
				int length = this.obstacles.Length;
				int value = this.numHierarchicalNodes.Value;
				this.obstacles.Resize(value, NativeArrayOptions.UninitializedMemory);
				for (int i = length; i < this.obstacles.Length; i++)
				{
					this.obstacles[i] = new UnmanagedObstacle
					{
						verticesAllocation = -1,
						groupsAllocation = -1
					};
				}
				if (this.obstacles.Length > 0)
				{
					this.obstacles[0] = new UnmanagedObstacle
					{
						verticesAllocation = -2,
						groupsAllocation = -2
					};
				}
			}

			// Token: 0x04000293 RID: 659
			public NativeList<UnmanagedObstacle> obstacles;

			// Token: 0x04000294 RID: 660
			public NativeReference<int> numHierarchicalNodes;
		}

		// Token: 0x02000076 RID: 118
		private struct JobCalculateObstacles : IJobParallelForBatch
		{
			// Token: 0x060003D8 RID: 984 RVA: 0x00013CBC File Offset: 0x00011EBC
			public void Execute(int startIndex, int count)
			{
				HierarchicalGraph hGraph = this.hGraphGC.Target as HierarchicalGraph;
				int num = (this.dirtyHierarchicalNodes.Length + 32 - 1) / 32;
				startIndex *= num;
				count *= num;
				int num2 = math.min(startIndex + count, this.dirtyHierarchicalNodes.Length);
				NativeList<RVOObstacleCache.ObstacleSegment> edgesScratch = new NativeList<RVOObstacleCache.ObstacleSegment>(Allocator.Temp);
				for (int i = startIndex; i < num2; i++)
				{
					edgesScratch.Clear();
					int hierarchicalNode = this.dirtyHierarchicalNodes[i];
					this.CalculateBoundingBox(hGraph, hierarchicalNode);
					this.CalculateObstacles(hGraph, hierarchicalNode, this.obstacleVertexGroups, this.obstacleVertices, this.obstacles, edgesScratch);
				}
			}

			// Token: 0x060003D9 RID: 985 RVA: 0x00013D64 File Offset: 0x00011F64
			private void CalculateBoundingBox(HierarchicalGraph hGraph, int hierarchicalNode)
			{
				List<GraphNode> list = hGraph.children[hierarchicalNode];
				Bounds value = default(Bounds);
				if (list.Count != 0)
				{
					if (list[0] is TriangleMeshNode)
					{
						Int3 @int = new Int3(int.MaxValue, int.MaxValue, int.MaxValue);
						Int3 int2 = new Int3(int.MinValue, int.MinValue, int.MinValue);
						for (int i = 0; i < list.Count; i++)
						{
							Int3 rhs;
							Int3 rhs2;
							Int3 rhs3;
							(list[i] as TriangleMeshNode).GetVertices(out rhs, out rhs2, out rhs3);
							@int = Int3.Min(Int3.Min(Int3.Min(@int, rhs), rhs2), rhs3);
							int2 = Int3.Max(Int3.Max(Int3.Max(int2, rhs), rhs2), rhs3);
						}
						value.SetMinMax((Vector3)@int, (Vector3)int2);
					}
					else
					{
						Int3 int3 = new Int3(int.MaxValue, int.MaxValue, int.MaxValue);
						Int3 int4 = new Int3(int.MinValue, int.MinValue, int.MinValue);
						for (int j = 0; j < list.Count; j++)
						{
							GraphNode graphNode = list[j];
							int3 = Int3.Min(int3, graphNode.position);
							int4 = Int3.Max(int4, graphNode.position);
						}
						if (list[0] is GridNodeBase)
						{
							float nodeSize;
							if (list[0] is LevelGridNode)
							{
								nodeSize = LevelGridNode.GetGridGraph(list[0].GraphIndex).nodeSize;
							}
							else
							{
								nodeSize = GridNode.GetGridGraph(list[0].GraphIndex).nodeSize;
							}
							Vector3 b = nodeSize * 0.70710677f * Vector3.one;
							value.SetMinMax((Vector3)int3 - b, (Vector3)int4 + b);
						}
						else
						{
							value.SetMinMax((Vector3)int3, (Vector3)int4);
						}
					}
				}
				this.bounds[hierarchicalNode] = value;
			}

			// Token: 0x060003DA RID: 986 RVA: 0x00013F54 File Offset: 0x00012154
			private unsafe void CalculateObstacles(HierarchicalGraph hGraph, int hierarchicalNode, SlabAllocator<ObstacleVertexGroup> obstacleVertexGroups, SlabAllocator<float3> obstacleVertices, NativeArray<UnmanagedObstacle> obstacles, NativeList<RVOObstacleCache.ObstacleSegment> edgesScratch)
			{
				RVOObstacleCache.CollectContours(hGraph.children[hierarchicalNode], edgesScratch);
				UnmanagedObstacle unmanagedObstacle = obstacles[hierarchicalNode];
				ref SpinLock ptr = ref UnsafeUtility.AsRef<SpinLock>((void*)this.allocationLock.GetUnsafePtr<SpinLock>());
				if (unmanagedObstacle.groupsAllocation != -1)
				{
					ptr.Lock();
					obstacleVertices.Free(unmanagedObstacle.verticesAllocation);
					obstacleVertexGroups.Free(unmanagedObstacle.groupsAllocation);
					ptr.Unlock();
				}
				List<GraphNode> list = hGraph.children[hierarchicalNode];
				bool simplifyObstacles = true;
				NativeMovementPlane nativeMovementPlane;
				if (list.Count > 0)
				{
					if (list[0] is GridNodeBase)
					{
						nativeMovementPlane = new NativeMovementPlane((list[0].Graph as GridGraph).transform.rotation);
					}
					else if (list[0] is TriangleMeshNode)
					{
						NavmeshBase navmeshBase = list[0].Graph as NavmeshBase;
						nativeMovementPlane = new NativeMovementPlane(navmeshBase.transform.rotation);
						simplifyObstacles = navmeshBase.RecalculateNormals;
					}
					else
					{
						nativeMovementPlane = new NativeMovementPlane(quaternion.identity);
						simplifyObstacles = false;
					}
				}
				else
				{
					nativeMovementPlane = default(NativeMovementPlane);
				}
				UnsafeSpan<RVOObstacleCache.ObstacleSegment> unsafeSpan = edgesScratch.AsUnsafeSpan<RVOObstacleCache.ObstacleSegment>();
				RVOObstacleCache.TraceContours(ref unsafeSpan, ref nativeMovementPlane, hierarchicalNode, (UnmanagedObstacle*)obstacles.GetUnsafePtr<UnmanagedObstacle>(), ref obstacleVertices, ref obstacleVertexGroups, ref ptr, simplifyObstacles);
			}

			// Token: 0x04000295 RID: 661
			public GCHandle hGraphGC;

			// Token: 0x04000296 RID: 662
			public SlabAllocator<float3> obstacleVertices;

			// Token: 0x04000297 RID: 663
			public SlabAllocator<ObstacleVertexGroup> obstacleVertexGroups;

			// Token: 0x04000298 RID: 664
			[NativeDisableParallelForRestriction]
			public NativeArray<UnmanagedObstacle> obstacles;

			// Token: 0x04000299 RID: 665
			[NativeDisableParallelForRestriction]
			public NativeArray<Bounds> bounds;

			// Token: 0x0400029A RID: 666
			[ReadOnly]
			public NativeList<int> dirtyHierarchicalNodes;

			// Token: 0x0400029B RID: 667
			[NativeDisableParallelForRestriction]
			public NativeReference<SpinLock> allocationLock;

			// Token: 0x0400029C RID: 668
			private static readonly ProfilerMarker MarkerBBox = new ProfilerMarker("HierarchicalBBox");

			// Token: 0x0400029D RID: 669
			private static readonly ProfilerMarker MarkerObstacles = new ProfilerMarker("CalculateObstacles");

			// Token: 0x0400029E RID: 670
			private static readonly ProfilerMarker MarkerCollect = new ProfilerMarker("Collect");

			// Token: 0x0400029F RID: 671
			private static readonly ProfilerMarker MarkerTrace = new ProfilerMarker("Trace");
		}

		// Token: 0x02000077 RID: 119
		public struct NavmeshBorderData
		{
			// Token: 0x060003DC RID: 988 RVA: 0x000140C4 File Offset: 0x000122C4
			public static NavmeshEdges.NavmeshBorderData CreateEmpty(Allocator allocator)
			{
				return new NavmeshEdges.NavmeshBorderData
				{
					hierarhicalNodeData = new HierarchicalGraph.HierarhicalNodeData
					{
						connectionAllocator = default(SlabAllocator<int>),
						connectionAllocations = new NativeList<int>(0, allocator),
						bounds = new NativeList<Bounds>(0, allocator)
					},
					obstacleData = new SimulatorBurst.ObstacleData
					{
						obstacleVertexGroups = default(SlabAllocator<ObstacleVertexGroup>),
						obstacleVertices = default(SlabAllocator<float3>),
						obstacles = new NativeList<UnmanagedObstacle>(0, allocator)
					}
				};
			}

			// Token: 0x060003DD RID: 989 RVA: 0x0001415C File Offset: 0x0001235C
			public void DisposeEmpty(JobHandle dependsOn)
			{
				if (this.hierarhicalNodeData.connectionAllocator.IsCreated)
				{
					throw new InvalidOperationException("NavmeshEdgeData was not empty");
				}
				this.hierarhicalNodeData.connectionAllocations.Dispose(dependsOn);
				this.hierarhicalNodeData.bounds.Dispose(dependsOn);
				this.obstacleData.obstacles.Dispose(dependsOn);
			}

			// Token: 0x060003DE RID: 990 RVA: 0x000141BC File Offset: 0x000123BC
			private unsafe static void GetHierarchicalNodesInRangeRec(int hierarchicalNode, Bounds bounds, SlabAllocator<int> connectionAllocator, [NoAlias] NativeList<int> connectionAllocations, NativeList<Bounds> nodeBounds, [NoAlias] NativeList<int> indices)
			{
				indices.Add(hierarchicalNode);
				UnsafeSpan<int> span = connectionAllocator.GetSpan(connectionAllocations[hierarchicalNode]);
				for (int i = 0; i < span.Length; i++)
				{
					int num = *span[i];
					if (nodeBounds[num].Intersects(bounds) && !indices.Contains(num))
					{
						NavmeshEdges.NavmeshBorderData.GetHierarchicalNodesInRangeRec(num, bounds, connectionAllocator, connectionAllocations, nodeBounds, indices);
					}
				}
			}

			// Token: 0x060003DF RID: 991 RVA: 0x00014228 File Offset: 0x00012428
			private unsafe static void ConvertObstaclesToEdges(ref SimulatorBurst.ObstacleData obstacleData, NativeList<int> obstacleIndices, Bounds localBounds, NativeList<float2> edgeBuffer, NativeMovementPlane movementPlane)
			{
				Bounds bounds = movementPlane.ToWorld(localBounds);
				ToPlaneMatrix toPlaneMatrix = movementPlane.AsWorldToPlaneMatrix();
				float3 rhs = bounds.min;
				float3 rhs2 = bounds.max;
				float3 rhs3 = localBounds.min;
				float3 rhs4 = localBounds.max;
				int num = 0;
				for (int i = 0; i < obstacleIndices.Length; i++)
				{
					UnmanagedObstacle unmanagedObstacle = obstacleData.obstacles[obstacleIndices[i]];
					num += obstacleData.obstacleVertices.GetSpan(unmanagedObstacle.verticesAllocation).Length;
				}
				edgeBuffer.ResizeUninitialized(num * 3);
				int length = 0;
				for (int j = 0; j < obstacleIndices.Length; j++)
				{
					UnmanagedObstacle unmanagedObstacle2 = obstacleData.obstacles[obstacleIndices[j]];
					if (unmanagedObstacle2.verticesAllocation != -1)
					{
						UnsafeSpan<float3> span = obstacleData.obstacleVertices.GetSpan(unmanagedObstacle2.verticesAllocation);
						UnsafeSpan<ObstacleVertexGroup> span2 = obstacleData.obstacleVertexGroups.GetSpan(unmanagedObstacle2.groupsAllocation);
						int num2 = 0;
						for (int k = 0; k < span2.Length; k++)
						{
							ObstacleVertexGroup obstacleVertexGroup = *span2[k];
							if (!math.all(obstacleVertexGroup.boundsMx >= rhs & obstacleVertexGroup.boundsMn <= rhs2))
							{
								num2 += obstacleVertexGroup.vertexCount;
							}
							else
							{
								bool flag = obstacleVertexGroup.type == ObstacleType.Loop;
								int index = num2 + (flag ? (obstacleVertexGroup.vertexCount - 1) : 0);
								for (int l = num2 + (flag ? 0 : 1); l < num2 + obstacleVertexGroup.vertexCount; l++)
								{
									float3 @float = *span[index];
									float3 float2 = *span[l];
									float3 lhs = math.min(@float, float2);
									if (math.all(math.max(@float, float2) >= rhs & lhs <= rhs2))
									{
										float3 x = toPlaneMatrix.ToXZPlane(@float);
										float3 y = toPlaneMatrix.ToXZPlane(float2);
										lhs = math.min(x, y);
										if (math.all(math.max(x, y) >= rhs3 & lhs <= rhs4))
										{
											edgeBuffer[length++] = x.xz;
											edgeBuffer[length++] = y.xz;
										}
									}
									index = l;
								}
								num2 += obstacleVertexGroup.vertexCount;
							}
						}
					}
				}
				edgeBuffer.Length = length;
			}

			// Token: 0x060003E0 RID: 992 RVA: 0x000144C7 File Offset: 0x000126C7
			public void GetObstaclesInRange(int hierarchicalNode, Bounds bounds, NativeList<int> obstacleIndexBuffer)
			{
				if (!this.obstacleData.obstacleVertices.IsCreated)
				{
					return;
				}
				NavmeshEdges.NavmeshBorderData.GetHierarchicalNodesInRangeRec(hierarchicalNode, bounds, this.hierarhicalNodeData.connectionAllocator, this.hierarhicalNodeData.connectionAllocations, this.hierarhicalNodeData.bounds, obstacleIndexBuffer);
			}

			// Token: 0x060003E1 RID: 993 RVA: 0x00014505 File Offset: 0x00012705
			public void GetEdgesInRange(int hierarchicalNode, Bounds localBounds, NativeList<float2> edgeBuffer, NativeList<int> scratchBuffer, NativeMovementPlane movementPlane)
			{
				if (!this.obstacleData.obstacleVertices.IsCreated)
				{
					return;
				}
				this.GetObstaclesInRange(hierarchicalNode, movementPlane.ToWorld(localBounds), scratchBuffer);
				NavmeshEdges.NavmeshBorderData.ConvertObstaclesToEdges(ref this.obstacleData, scratchBuffer, localBounds, edgeBuffer, movementPlane);
			}

			// Token: 0x040002A0 RID: 672
			public HierarchicalGraph.HierarhicalNodeData hierarhicalNodeData;

			// Token: 0x040002A1 RID: 673
			public SimulatorBurst.ObstacleData obstacleData;
		}
	}
}
