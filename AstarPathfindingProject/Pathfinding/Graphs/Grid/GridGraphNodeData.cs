using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Pathfinding.Graphs.Grid.Jobs;
using Pathfinding.Jobs;
using Unity.Collections;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid
{
	// Token: 0x020001F5 RID: 501
	public struct GridGraphNodeData
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0004E8D4 File Offset: 0x0004CAD4
		public int layers
		{
			get
			{
				return this.bounds.size.y;
			}
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0004E8E8 File Offset: 0x0004CAE8
		public void AllocateBuffers(JobDependencyTracker dependencyTracker)
		{
			if (dependencyTracker != null)
			{
				this.positions = dependencyTracker.NewNativeArray<Vector3>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
				this.normals = dependencyTracker.NewNativeArray<float4>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
				this.connections = dependencyTracker.NewNativeArray<ulong>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
				this.penalties = dependencyTracker.NewNativeArray<uint>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
				this.walkable = dependencyTracker.NewNativeArray<bool>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
				this.walkableWithErosion = dependencyTracker.NewNativeArray<bool>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
				this.tags = dependencyTracker.NewNativeArray<int>(this.numNodes, this.allocationMethod, NativeArrayOptions.ClearMemory);
				return;
			}
			this.positions = new NativeArray<Vector3>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.normals = new NativeArray<float4>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.connections = new NativeArray<ulong>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.penalties = new NativeArray<uint>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.walkable = new NativeArray<bool>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.walkableWithErosion = new NativeArray<bool>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.tags = new NativeArray<int>(this.numNodes, this.allocationMethod, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x0004EA54 File Offset: 0x0004CC54
		public void TrackBuffers(JobDependencyTracker dependencyTracker)
		{
			if (this.positions.IsCreated)
			{
				dependencyTracker.Track<Vector3>(this.positions, true);
			}
			if (this.normals.IsCreated)
			{
				dependencyTracker.Track<float4>(this.normals, true);
			}
			if (this.connections.IsCreated)
			{
				dependencyTracker.Track<ulong>(this.connections, true);
			}
			if (this.penalties.IsCreated)
			{
				dependencyTracker.Track<uint>(this.penalties, true);
			}
			if (this.walkable.IsCreated)
			{
				dependencyTracker.Track<bool>(this.walkable, true);
			}
			if (this.walkableWithErosion.IsCreated)
			{
				dependencyTracker.Track<bool>(this.walkableWithErosion, true);
			}
			if (this.tags.IsCreated)
			{
				dependencyTracker.Track<int>(this.tags, true);
			}
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0004EB18 File Offset: 0x0004CD18
		public void PersistBuffers(JobDependencyTracker dependencyTracker)
		{
			dependencyTracker.Persist<Vector3>(this.positions);
			dependencyTracker.Persist<float4>(this.normals);
			dependencyTracker.Persist<ulong>(this.connections);
			dependencyTracker.Persist<uint>(this.penalties);
			dependencyTracker.Persist<bool>(this.walkable);
			dependencyTracker.Persist<bool>(this.walkableWithErosion);
			dependencyTracker.Persist<int>(this.tags);
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0004EB7C File Offset: 0x0004CD7C
		public void Dispose()
		{
			this.bounds = default(IntBounds);
			this.numNodes = 0;
			if (this.positions.IsCreated)
			{
				this.positions.Dispose();
			}
			if (this.normals.IsCreated)
			{
				this.normals.Dispose();
			}
			if (this.connections.IsCreated)
			{
				this.connections.Dispose();
			}
			if (this.penalties.IsCreated)
			{
				this.penalties.Dispose();
			}
			if (this.walkable.IsCreated)
			{
				this.walkable.Dispose();
			}
			if (this.walkableWithErosion.IsCreated)
			{
				this.walkableWithErosion.Dispose();
			}
			if (this.tags.IsCreated)
			{
				this.tags.Dispose();
			}
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0004EC44 File Offset: 0x0004CE44
		public unsafe JobHandle Rotate2D(int dx, int dz, JobHandle dependency)
		{
			int3 size = this.bounds.size;
			IntPtr intPtr = stackalloc byte[checked(unchecked((UIntPtr)7) * (UIntPtr)sizeof(JobHandle))];
			*intPtr = this.positions.Rotate3D(size, dx, dz).Schedule(dependency);
			*(intPtr + (IntPtr)sizeof(JobHandle)) = this.normals.Rotate3D(size, dx, dz).Schedule(dependency);
			*(intPtr + (IntPtr)2 * (IntPtr)sizeof(JobHandle)) = this.connections.Rotate3D(size, dx, dz).Schedule(dependency);
			*(intPtr + (IntPtr)3 * (IntPtr)sizeof(JobHandle)) = this.penalties.Rotate3D(size, dx, dz).Schedule(dependency);
			*(intPtr + (IntPtr)4 * (IntPtr)sizeof(JobHandle)) = this.walkable.Rotate3D(size, dx, dz).Schedule(dependency);
			*(intPtr + (IntPtr)5 * (IntPtr)sizeof(JobHandle)) = this.walkableWithErosion.Rotate3D(size, dx, dz).Schedule(dependency);
			*(intPtr + (IntPtr)6 * (IntPtr)sizeof(JobHandle)) = this.tags.Rotate3D(size, dx, dz).Schedule(dependency);
			return JobHandleUnsafeUtility.CombineDependencies(intPtr, 7);
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x0004ED60 File Offset: 0x0004CF60
		public void ResizeLayerCount(int layerCount, JobDependencyTracker dependencyTracker)
		{
			if (layerCount > this.layers)
			{
				GridGraphNodeData gridGraphNodeData = this;
				this.bounds.max.y = layerCount;
				this.numNodes = this.bounds.volume;
				this.AllocateBuffers(dependencyTracker);
				this.normals.MemSet(float4.zero).Schedule(dependencyTracker);
				this.walkable.MemSet(false).Schedule(dependencyTracker);
				this.walkableWithErosion.MemSet(false).Schedule(dependencyTracker);
				new JobCopyBuffers
				{
					input = gridGraphNodeData,
					output = this,
					copyPenaltyAndTags = true,
					bounds = gridGraphNodeData.bounds
				}.Schedule(dependencyTracker);
			}
			if (layerCount < this.layers)
			{
				throw new ArgumentException("Cannot reduce the number of layers");
			}
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0004EE34 File Offset: 0x0004D034
		public void ReadFromNodesForConnectionCalculations(GridNodeBase[] nodes, Slice3D slice, JobHandle nodesDependsOn, NativeArray<float4> graphNodeNormals, JobDependencyTracker dependencyTracker)
		{
			this.bounds = slice.slice;
			this.numNodes = slice.slice.volume;
			this.positions = new NativeArray<Vector3>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.normals = new NativeArray<float4>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.connections = new NativeArray<ulong>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			this.walkableWithErosion = new NativeArray<bool>(this.numNodes, this.allocationMethod, NativeArrayOptions.UninitializedMemory);
			GridGraphNodeData.LightReader lightReader = new GridGraphNodeData.LightReader
			{
				nodes = nodes,
				nodePositions = this.positions.AsUnsafeSpan<Vector3>(),
				nodeWalkable = this.walkableWithErosion.AsUnsafeSpan<bool>()
			};
			GridIterationUtilities.ForEachCellIn3DSlice<GridGraphNodeData.LightReader>(slice, ref lightReader);
			this.ReadNodeNormals(slice, graphNodeNormals, dependencyTracker);
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0004EF0C File Offset: 0x0004D10C
		private void ReadNodeNormals(Slice3D slice, NativeArray<float4> graphNodeNormals, JobDependencyTracker dependencyTracker)
		{
			if (dependencyTracker != null)
			{
				this.normals.MemSet(float4.zero).Schedule(dependencyTracker);
				new JobCopyRectangle<float4>
				{
					input = graphNodeNormals,
					output = this.normals,
					inputSlice = slice,
					outputSlice = new Slice3D(this.bounds, slice.slice)
				}.Schedule(dependencyTracker);
				return;
			}
			this.normals.AsUnsafeSpan<float4>().FillZeros<float4>();
			JobCopyRectangle<float4>.Copy(graphNodeNormals, this.normals, slice, new Slice3D(this.bounds, slice.slice));
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0004EFA8 File Offset: 0x0004D1A8
		public static GridGraphNodeData ReadFromNodes(GridNodeBase[] nodes, Slice3D slice, JobHandle nodesDependsOn, NativeArray<float4> graphNodeNormals, Allocator allocator, bool layeredDataLayout, JobDependencyTracker dependencyTracker)
		{
			GridGraphNodeData gridGraphNodeData = new GridGraphNodeData
			{
				allocationMethod = allocator,
				numNodes = slice.slice.volume,
				bounds = slice.slice,
				layeredDataLayout = layeredDataLayout
			};
			gridGraphNodeData.AllocateBuffers(dependencyTracker);
			GCHandle gchandle = GCHandle.Alloc(nodes);
			JobHandle dependsOn = new JobReadNodeData
			{
				nodesHandle = gchandle,
				nodePositions = gridGraphNodeData.positions,
				nodePenalties = gridGraphNodeData.penalties,
				nodeTags = gridGraphNodeData.tags,
				nodeConnections = gridGraphNodeData.connections,
				nodeWalkableWithErosion = gridGraphNodeData.walkableWithErosion,
				nodeWalkable = gridGraphNodeData.walkable,
				slice = slice
			}.ScheduleBatch(gridGraphNodeData.numNodes, math.max(2000, gridGraphNodeData.numNodes / 16), dependencyTracker, nodesDependsOn);
			dependencyTracker.DeferFree(gchandle, dependsOn);
			if (graphNodeNormals.IsCreated)
			{
				gridGraphNodeData.ReadNodeNormals(slice, graphNodeNormals, dependencyTracker);
			}
			return gridGraphNodeData;
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0004F0AC File Offset: 0x0004D2AC
		public GridGraphNodeData ReadFromNodesAndCopy(GridNodeBase[] nodes, Slice3D slice, JobHandle nodesDependsOn, NativeArray<float4> graphNodeNormals, bool copyPenaltyAndTags, JobDependencyTracker dependencyTracker)
		{
			GridGraphNodeData result = GridGraphNodeData.ReadFromNodes(nodes, slice, nodesDependsOn, graphNodeNormals, this.allocationMethod, this.layeredDataLayout, dependencyTracker);
			result.CopyFrom(this, copyPenaltyAndTags, dependencyTracker);
			return result;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0004F0E4 File Offset: 0x0004D2E4
		public void CopyFrom(GridGraphNodeData other, bool copyPenaltyAndTags, JobDependencyTracker dependencyTracker)
		{
			this.CopyFrom(other, IntBounds.Intersection(this.bounds, other.bounds), copyPenaltyAndTags, dependencyTracker);
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0004F100 File Offset: 0x0004D300
		public void CopyFrom(GridGraphNodeData other, IntBounds bounds, bool copyPenaltyAndTags, JobDependencyTracker dependencyTracker)
		{
			JobCopyBuffers data = new JobCopyBuffers
			{
				input = other,
				output = this,
				copyPenaltyAndTags = copyPenaltyAndTags,
				bounds = bounds
			};
			if (dependencyTracker != null)
			{
				data.Schedule(dependencyTracker);
				return;
			}
			ref data.RunByRef<JobCopyBuffers>();
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0004F154 File Offset: 0x0004D354
		public JobHandle AssignToNodes(GridNodeBase[] nodes, int3 nodeArrayBounds, IntBounds writeMask, uint graphIndex, JobHandle nodesDependsOn, JobDependencyTracker dependencyTracker)
		{
			GCHandle gchandle = GCHandle.Alloc(nodes);
			JobHandle jobHandle = new JobWriteNodeData
			{
				nodesHandle = gchandle,
				graphIndex = graphIndex,
				nodePositions = this.positions,
				nodePenalties = this.penalties,
				nodeTags = this.tags,
				nodeConnections = this.connections,
				nodeWalkableWithErosion = this.walkableWithErosion,
				nodeWalkable = this.walkable,
				nodeArrayBounds = nodeArrayBounds,
				dataBounds = this.bounds,
				writeMask = writeMask
			}.ScheduleBatch(writeMask.volume, math.max(1000, writeMask.volume / 16), dependencyTracker, nodesDependsOn);
			dependencyTracker.DeferFree(gchandle, jobHandle);
			return jobHandle;
		}

		// Token: 0x04000944 RID: 2372
		public Allocator allocationMethod;

		// Token: 0x04000945 RID: 2373
		public int numNodes;

		// Token: 0x04000946 RID: 2374
		public IntBounds bounds;

		// Token: 0x04000947 RID: 2375
		public NativeArray<Vector3> positions;

		// Token: 0x04000948 RID: 2376
		public NativeArray<ulong> connections;

		// Token: 0x04000949 RID: 2377
		public NativeArray<uint> penalties;

		// Token: 0x0400094A RID: 2378
		public NativeArray<int> tags;

		// Token: 0x0400094B RID: 2379
		public NativeArray<float4> normals;

		// Token: 0x0400094C RID: 2380
		public NativeArray<bool> walkable;

		// Token: 0x0400094D RID: 2381
		public NativeArray<bool> walkableWithErosion;

		// Token: 0x0400094E RID: 2382
		public bool layeredDataLayout;

		// Token: 0x020001F6 RID: 502
		private struct LightReader : GridIterationUtilities.ISliceAction
		{
			// Token: 0x06000CA7 RID: 3239 RVA: 0x0004F21C File Offset: 0x0004D41C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe void Execute(uint outerIdx, uint innerIdx)
			{
				if ((ulong)outerIdx < (ulong)((long)this.nodes.Length))
				{
					GridNodeBase gridNodeBase = this.nodes[(int)outerIdx];
					if (gridNodeBase != null)
					{
						*this.nodePositions[innerIdx] = (Vector3)gridNodeBase.position;
						*this.nodeWalkable[innerIdx] = gridNodeBase.Walkable;
						return;
					}
				}
				*this.nodePositions[innerIdx] = Vector3.zero;
				*this.nodeWalkable[innerIdx] = false;
			}

			// Token: 0x0400094F RID: 2383
			public GridNodeBase[] nodes;

			// Token: 0x04000950 RID: 2384
			public UnsafeSpan<Vector3> nodePositions;

			// Token: 0x04000951 RID: 2385
			public UnsafeSpan<bool> nodeWalkable;
		}
	}
}
