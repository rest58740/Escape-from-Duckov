using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002BD RID: 701
	[BurstCompile]
	public static class RVOObstacleCache
	{
		// Token: 0x060010B7 RID: 4279 RVA: 0x0006749C File Offset: 0x0006569C
		private static ulong HashKey(GraphNode sourceNode, int traversableTags, SimpleMovementPlane movementPlane)
		{
			return (((((ulong)sourceNode.NodeIndex * 786433UL ^ (ulong)((long)traversableTags)) * 786433UL ^ (ulong)(movementPlane.rotation.x * 4f)) * 786433UL ^ (ulong)(movementPlane.rotation.y * 4f)) * 786433UL ^ (ulong)(movementPlane.rotation.z * 4f)) * 786433UL ^ (ulong)(movementPlane.rotation.w * 4f);
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x00067524 File Offset: 0x00065724
		public unsafe static void CollectContours(List<GraphNode> nodes, NativeList<RVOObstacleCache.ObstacleSegment> obstacles)
		{
			if (nodes.Count == 0)
			{
				return;
			}
			if (nodes[0] is TriangleMeshNode)
			{
				for (int i = 0; i < nodes.Count; i++)
				{
					TriangleMeshNode triangleMeshNode = nodes[i] as TriangleMeshNode;
					int num = 0;
					if (triangleMeshNode.connections != null)
					{
						for (int j = 0; j < triangleMeshNode.connections.Length; j++)
						{
							Connection connection = triangleMeshNode.connections[j];
							if (connection.isEdgeShared)
							{
								num |= 1 << connection.shapeEdge;
							}
						}
					}
					Int3 @int;
					Int3 int2;
					Int3 int3;
					triangleMeshNode.GetVertices(out @int, out int2, out int3);
					for (int k = 0; k < 3; k++)
					{
						if ((num & 1 << k) == 0)
						{
							Int3 ob;
							Int3 ob2;
							switch (k)
							{
							case 0:
								ob = @int;
								ob2 = int2;
								break;
							case 1:
								ob = int2;
								ob2 = int3;
								break;
							case 2:
								goto IL_C0;
							default:
								goto IL_C0;
							}
							IL_C7:
							int hashCode = ob.GetHashCode();
							int hashCode2 = ob2.GetHashCode();
							RVOObstacleCache.ObstacleSegment obstacleSegment = default(RVOObstacleCache.ObstacleSegment);
							obstacleSegment.vertex1 = (Vector3)ob;
							obstacleSegment.vertex2 = (Vector3)ob2;
							obstacleSegment.vertex1LinkId = hashCode;
							obstacleSegment.vertex2LinkId = hashCode2;
							obstacles.Add(obstacleSegment);
							goto IL_12E;
							IL_C0:
							ob = int3;
							ob2 = @int;
							goto IL_C7;
						}
						IL_12E:;
					}
				}
				return;
			}
			if (nodes[0] is GridNodeBase)
			{
				GridGraph gridGraph;
				if (nodes[0] is LevelGridNode)
				{
					gridGraph = LevelGridNode.GetGridGraph(nodes[0].GraphIndex);
				}
				else
				{
					gridGraph = GridNode.GetGridGraph(nodes[0].GraphIndex);
				}
				Vector3* ptr = stackalloc Vector3[checked(unchecked((UIntPtr)4) * (UIntPtr)sizeof(Vector3))];
				for (int l = 0; l < 4; l++)
				{
					int num2 = (l + 1) % 4;
					ptr[l] = gridGraph.transform.TransformVector(0.5f * new Vector3((float)(GridGraph.neighbourXOffsets[l] + GridGraph.neighbourXOffsets[num2]), 0f, (float)(GridGraph.neighbourZOffsets[l] + GridGraph.neighbourZOffsets[num2])));
				}
				for (int m = 0; m < nodes.Count; m++)
				{
					GridNodeBase gridNodeBase = nodes[m] as GridNodeBase;
					if (!gridNodeBase.HasConnectionsToAllAxisAlignedNeighbours)
					{
						for (int n = 0; n < 4; n++)
						{
							if (!gridNodeBase.HasConnectionInDirection(n))
							{
								int direction = (n + 1) % 4;
								int num3 = (n - 1 + 4) % 4;
								GridNodeBase neighbourAlongDirection = gridNodeBase.GetNeighbourAlongDirection(direction);
								GridNodeBase neighbourAlongDirection2 = gridNodeBase.GetNeighbourAlongDirection(num3);
								uint vertex1LinkId;
								if (neighbourAlongDirection != null)
								{
									GridNodeBase neighbourAlongDirection3 = neighbourAlongDirection.GetNeighbourAlongDirection(n);
									if (neighbourAlongDirection3 != null)
									{
										uint nodeIndex = gridNodeBase.NodeIndex;
										uint nodeIndex2 = neighbourAlongDirection.NodeIndex;
										uint nodeIndex3 = neighbourAlongDirection3.NodeIndex;
										if (nodeIndex > nodeIndex2)
										{
											Memory.Swap<uint>(ref nodeIndex, ref nodeIndex2);
										}
										if (nodeIndex2 > nodeIndex3)
										{
											Memory.Swap<uint>(ref nodeIndex2, ref nodeIndex3);
										}
										if (nodeIndex > nodeIndex2)
										{
											Memory.Swap<uint>(ref nodeIndex, ref nodeIndex2);
										}
										vertex1LinkId = math.hash(new uint3(nodeIndex, nodeIndex2, nodeIndex3));
									}
									else
									{
										uint nodeIndex4 = gridNodeBase.NodeIndex;
										uint nodeIndex5 = neighbourAlongDirection.NodeIndex;
										if (nodeIndex4 > nodeIndex5)
										{
											Memory.Swap<uint>(ref nodeIndex4, ref nodeIndex5);
										}
										vertex1LinkId = math.hash(new uint3(nodeIndex4, nodeIndex5, (uint)n));
									}
								}
								else
								{
									int y = n + 4;
									vertex1LinkId = math.hash(new uint2(gridNodeBase.NodeIndex, (uint)y));
								}
								uint vertex2LinkId;
								if (neighbourAlongDirection2 != null)
								{
									GridNodeBase neighbourAlongDirection4 = neighbourAlongDirection2.GetNeighbourAlongDirection(n);
									if (neighbourAlongDirection4 != null)
									{
										uint nodeIndex6 = gridNodeBase.NodeIndex;
										uint nodeIndex7 = neighbourAlongDirection2.NodeIndex;
										uint nodeIndex8 = neighbourAlongDirection4.NodeIndex;
										if (nodeIndex6 > nodeIndex7)
										{
											Memory.Swap<uint>(ref nodeIndex6, ref nodeIndex7);
										}
										if (nodeIndex7 > nodeIndex8)
										{
											Memory.Swap<uint>(ref nodeIndex7, ref nodeIndex8);
										}
										if (nodeIndex6 > nodeIndex7)
										{
											Memory.Swap<uint>(ref nodeIndex6, ref nodeIndex7);
										}
										vertex2LinkId = math.hash(new uint3(nodeIndex6, nodeIndex7, nodeIndex8));
									}
									else
									{
										uint nodeIndex9 = gridNodeBase.NodeIndex;
										uint nodeIndex10 = neighbourAlongDirection2.NodeIndex;
										if (nodeIndex9 > nodeIndex10)
										{
											Memory.Swap<uint>(ref nodeIndex9, ref nodeIndex10);
										}
										vertex2LinkId = math.hash(new uint3(nodeIndex9, nodeIndex10, (uint)n));
									}
								}
								else
								{
									int y2 = num3 + 4;
									vertex2LinkId = math.hash(new uint2(gridNodeBase.NodeIndex, (uint)y2));
								}
								Vector3 a = (Vector3)gridNodeBase.position;
								RVOObstacleCache.ObstacleSegment obstacleSegment = default(RVOObstacleCache.ObstacleSegment);
								obstacleSegment.vertex1 = a + ptr[n];
								obstacleSegment.vertex2 = a + ptr[num3];
								obstacleSegment.vertex1LinkId = (int)vertex1LinkId;
								obstacleSegment.vertex2LinkId = (int)vertex2LinkId;
								obstacles.Add(obstacleSegment);
							}
						}
					}
				}
			}
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x000679C1 File Offset: 0x00065BC1
		[BurstCompile]
		internal unsafe static void TraceContours(ref UnsafeSpan<RVOObstacleCache.ObstacleSegment> obstaclesSpan, ref NativeMovementPlane movementPlane, int obstacleId, UnmanagedObstacle* outputObstacles, ref SlabAllocator<float3> verticesAllocator, ref SlabAllocator<ObstacleVertexGroup> obstaclesAllocator, ref SpinLock spinLock, bool simplifyObstacles)
		{
			RVOObstacleCache.TraceContours_00000F8F$BurstDirectCall.Invoke(ref obstaclesSpan, ref movementPlane, obstacleId, outputObstacles, ref verticesAllocator, ref obstaclesAllocator, ref spinLock, simplifyObstacles);
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x000679E8 File Offset: 0x00065BE8
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void TraceContours$BurstManaged(ref UnsafeSpan<RVOObstacleCache.ObstacleSegment> obstaclesSpan, ref NativeMovementPlane movementPlane, int obstacleId, UnmanagedObstacle* outputObstacles, ref SlabAllocator<float3> verticesAllocator, ref SlabAllocator<ObstacleVertexGroup> obstaclesAllocator, ref SpinLock spinLock, bool simplifyObstacles)
		{
			UnsafeSpan<RVOObstacleCache.ObstacleSegment> unsafeSpan = obstaclesSpan;
			if (unsafeSpan.Length == 0)
			{
				outputObstacles[obstacleId] = new UnmanagedObstacle
				{
					verticesAllocation = -1,
					groupsAllocation = -1
				};
				return;
			}
			NativeHashMap<int, int> nativeHashMap = new NativeHashMap<int, int>(unsafeSpan.Length, Allocator.Temp);
			NativeArray<byte> nativeArray = new NativeArray<byte>(unsafeSpan.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < unsafeSpan.Length; i++)
			{
				if (nativeHashMap.TryAdd(unsafeSpan[i].vertex1LinkId, i))
				{
					nativeArray[i] = 2;
				}
				else
				{
					nativeArray[i] = 0;
				}
			}
			for (int j = 0; j < unsafeSpan.Length; j++)
			{
				int index;
				if (nativeHashMap.TryGetValue(unsafeSpan[j].vertex2LinkId, out index) && nativeArray[index] > 0)
				{
					nativeArray[index] = 1;
				}
			}
			NativeList<ObstacleVertexGroup> values = new NativeList<ObstacleVertexGroup>(16, Allocator.Temp);
			NativeList<float3> values2 = new NativeList<float3>(16, Allocator.Temp);
			ToPlaneMatrix toPlaneMatrix = movementPlane.AsWorldToPlaneMatrix();
			for (int k = 0; k <= 1; k++)
			{
				int num = (k == 1) ? 1 : 2;
				for (int l = 0; l < unsafeSpan.Length; l++)
				{
					if ((int)nativeArray[l] >= num)
					{
						int length = values2.Length;
						values2.Add(unsafeSpan[l].vertex1);
						float3 @float = unsafeSpan[l].vertex1;
						float3 float2 = unsafeSpan[l].vertex2;
						int index2 = l;
						ObstacleType type = ObstacleType.Chain;
						float3 float3 = @float;
						float3 float4 = @float;
						while (nativeArray[index2] != 0)
						{
							nativeArray[index2] = 0;
							int num2;
							float3 float5;
							if (nativeHashMap.TryGetValue(unsafeSpan[index2].vertex2LinkId, out num2))
							{
								float5 = 0.5f * (unsafeSpan[index2].vertex2 + unsafeSpan[num2].vertex1);
							}
							else
							{
								float5 = unsafeSpan[index2].vertex2;
								num2 = -1;
							}
							float3 rhs = @float;
							float3 lhs = float5;
							float3 float6 = float2;
							float2 c = toPlaneMatrix.ToPlane(lhs - rhs);
							float2 c2 = toPlaneMatrix.ToPlane(float6 - rhs);
							if (math.abs(VectorMath.Determinant(c, c2)) >= 0.01f || !simplifyObstacles)
							{
								values2.Add(float2);
								float3 = math.min(float3, float2);
								float4 = math.max(float4, float2);
								@float = float6;
							}
							if (num2 == l)
							{
								values2[length] = float5;
								type = ObstacleType.Loop;
								break;
							}
							if (num2 == -1)
							{
								values2.Add(float5);
								float3 = math.min(float3, float5);
								float4 = math.max(float4, float5);
								break;
							}
							index2 = num2;
							float2 = float5;
						}
						ObstacleVertexGroup obstacleVertexGroup = default(ObstacleVertexGroup);
						obstacleVertexGroup.type = type;
						obstacleVertexGroup.vertexCount = values2.Length - length;
						obstacleVertexGroup.boundsMn = float3;
						obstacleVertexGroup.boundsMx = float4;
						values.Add(obstacleVertexGroup);
					}
				}
			}
			int groupsAllocation;
			int verticesAllocation;
			if (values.Length > 0)
			{
				spinLock.Lock();
				groupsAllocation = obstaclesAllocator.Allocate(values);
				verticesAllocation = verticesAllocator.Allocate(values2);
				spinLock.Unlock();
			}
			else
			{
				groupsAllocation = -1;
				verticesAllocation = -1;
			}
			outputObstacles[obstacleId] = new UnmanagedObstacle
			{
				verticesAllocation = verticesAllocation,
				groupsAllocation = groupsAllocation
			};
		}

		// Token: 0x04000C7C RID: 3196
		private static readonly ProfilerMarker MarkerAllocate = new ProfilerMarker("Allocate");

		// Token: 0x020002BE RID: 702
		public struct ObstacleSegment
		{
			// Token: 0x04000C7D RID: 3197
			public float3 vertex1;

			// Token: 0x04000C7E RID: 3198
			public float3 vertex2;

			// Token: 0x04000C7F RID: 3199
			public int vertex1LinkId;

			// Token: 0x04000C80 RID: 3200
			public int vertex2LinkId;
		}

		// Token: 0x020002BF RID: 703
		// (Invoke) Token: 0x060010BD RID: 4285
		internal unsafe delegate void TraceContours_00000F8F$PostfixBurstDelegate(ref UnsafeSpan<RVOObstacleCache.ObstacleSegment> obstaclesSpan, ref NativeMovementPlane movementPlane, int obstacleId, UnmanagedObstacle* outputObstacles, ref SlabAllocator<float3> verticesAllocator, ref SlabAllocator<ObstacleVertexGroup> obstaclesAllocator, ref SpinLock spinLock, bool simplifyObstacles);

		// Token: 0x020002C0 RID: 704
		internal static class TraceContours_00000F8F$BurstDirectCall
		{
			// Token: 0x060010C0 RID: 4288 RVA: 0x00067D61 File Offset: 0x00065F61
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RVOObstacleCache.TraceContours_00000F8F$BurstDirectCall.Pointer == 0)
				{
					RVOObstacleCache.TraceContours_00000F8F$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RVOObstacleCache.TraceContours_00000F8F$BurstDirectCall.DeferredCompilation, methodof(RVOObstacleCache.TraceContours$BurstManaged(UnsafeSpan<RVOObstacleCache.ObstacleSegment>*, NativeMovementPlane*, int, UnmanagedObstacle*, SlabAllocator<float3>*, SlabAllocator<ObstacleVertexGroup>*, SpinLock*, bool)).MethodHandle, typeof(RVOObstacleCache.TraceContours_00000F8F$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RVOObstacleCache.TraceContours_00000F8F$BurstDirectCall.Pointer;
			}

			// Token: 0x060010C1 RID: 4289 RVA: 0x00067D90 File Offset: 0x00065F90
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				RVOObstacleCache.TraceContours_00000F8F$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x060010C2 RID: 4290 RVA: 0x00067DA8 File Offset: 0x00065FA8
			public unsafe static void Constructor()
			{
				RVOObstacleCache.TraceContours_00000F8F$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RVOObstacleCache.TraceContours(UnsafeSpan<RVOObstacleCache.ObstacleSegment>*, NativeMovementPlane*, int, UnmanagedObstacle*, SlabAllocator<float3>*, SlabAllocator<ObstacleVertexGroup>*, SpinLock*, bool)).MethodHandle);
			}

			// Token: 0x060010C3 RID: 4291 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x060010C4 RID: 4292 RVA: 0x00067DB9 File Offset: 0x00065FB9
			// Note: this type is marked as 'beforefieldinit'.
			static TraceContours_00000F8F$BurstDirectCall()
			{
				RVOObstacleCache.TraceContours_00000F8F$BurstDirectCall.Constructor();
			}

			// Token: 0x060010C5 RID: 4293 RVA: 0x00067DC0 File Offset: 0x00065FC0
			public unsafe static void Invoke(ref UnsafeSpan<RVOObstacleCache.ObstacleSegment> obstaclesSpan, ref NativeMovementPlane movementPlane, int obstacleId, UnmanagedObstacle* outputObstacles, ref SlabAllocator<float3> verticesAllocator, ref SlabAllocator<ObstacleVertexGroup> obstaclesAllocator, ref SpinLock spinLock, bool simplifyObstacles)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RVOObstacleCache.TraceContours_00000F8F$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<Pathfinding.RVO.RVOObstacleCache/ObstacleSegment>&,Pathfinding.Util.NativeMovementPlane&,System.Int32,Pathfinding.RVO.UnmanagedObstacle*,Pathfinding.Collections.SlabAllocator`1<Unity.Mathematics.float3>&,Pathfinding.Collections.SlabAllocator`1<Pathfinding.RVO.ObstacleVertexGroup>&,Pathfinding.Sync.SpinLock&,System.Boolean), ref obstaclesSpan, ref movementPlane, obstacleId, outputObstacles, ref verticesAllocator, ref obstaclesAllocator, ref spinLock, simplifyObstacles, functionPointer);
						return;
					}
				}
				RVOObstacleCache.TraceContours$BurstManaged(ref obstaclesSpan, ref movementPlane, obstacleId, outputObstacles, ref verticesAllocator, ref obstaclesAllocator, ref spinLock, simplifyObstacles);
			}

			// Token: 0x04000C81 RID: 3201
			private static IntPtr Pointer;

			// Token: 0x04000C82 RID: 3202
			private static IntPtr DeferredCompilation;
		}
	}
}
