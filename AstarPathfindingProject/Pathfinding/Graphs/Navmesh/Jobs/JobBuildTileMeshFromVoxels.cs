using System;
using System.Threading;
using Pathfinding.Collections;
using Pathfinding.Graphs.Navmesh.Voxelization.Burst;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001E6 RID: 486
	[BurstCompile(CompileSynchronously = true)]
	public struct JobBuildTileMeshFromVoxels : IJob
	{
		// Token: 0x06000C77 RID: 3191 RVA: 0x0004D091 File Offset: 0x0004B291
		public unsafe void SetOutputMeshes(NativeArray<TileMesh.TileMeshUnsafe> arr)
		{
			this.outputMeshes = (TileMesh.TileMeshUnsafe*)arr.GetUnsafeReadOnlyPtr<TileMesh.TileMeshUnsafe>();
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0004D09F File Offset: 0x0004B29F
		public void SetCounter(NativeReference<int> counter)
		{
			this.currentTileCounter = counter.GetUnsafePtr<int>();
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0004D0B0 File Offset: 0x0004B2B0
		public unsafe void Execute()
		{
			for (int i = 0; i < this.maxTiles; i++)
			{
				int num = Interlocked.Increment(UnsafeUtility.AsRef<int>((void*)this.currentTileCounter)) - 1;
				if (num >= this.tileGraphSpaceBounds.Length)
				{
					return;
				}
				this.tileBuilder.linkedVoxelField.ResetLinkedVoxelSpans();
				if (this.dimensionMode == RecastGraph.DimensionMode.Dimension2D && this.backgroundTraversability == RecastGraph.BackgroundTraversability.Walkable)
				{
					this.tileBuilder.linkedVoxelField.SetWalkableBackground();
				}
				int num2 = (num > 0) ? this.inputMeshes.bucketRanges[num - 1] : 0;
				int num3 = this.inputMeshes.bucketRanges[num];
				JobVoxelize jobVoxelize = default(JobVoxelize);
				jobVoxelize.inputMeshes = this.inputMeshes.meshes;
				jobVoxelize.bucket = this.inputMeshes.pointers.GetSubArray(num2, num3 - num2);
				jobVoxelize.voxelWalkableClimb = this.voxelWalkableClimb;
				jobVoxelize.voxelWalkableHeight = this.voxelWalkableHeight;
				jobVoxelize.cellSize = this.cellSize;
				jobVoxelize.cellHeight = this.cellHeight;
				jobVoxelize.maxSlope = this.maxSlope;
				jobVoxelize.graphTransform = this.graphToWorldSpace;
				jobVoxelize.graphSpaceBounds = this.tileGraphSpaceBounds[num];
				jobVoxelize.graphSpaceLimits = this.graphSpaceLimits;
				jobVoxelize.voxelArea = this.tileBuilder.linkedVoxelField;
				jobVoxelize.Execute();
				JobFilterLedges jobFilterLedges = default(JobFilterLedges);
				jobFilterLedges.field = this.tileBuilder.linkedVoxelField;
				jobFilterLedges.voxelWalkableClimb = this.voxelWalkableClimb;
				jobFilterLedges.voxelWalkableHeight = this.voxelWalkableHeight;
				jobFilterLedges.cellSize = this.cellSize;
				jobFilterLedges.cellHeight = this.cellHeight;
				jobFilterLedges.Execute();
				JobFilterLowHeightSpans jobFilterLowHeightSpans = default(JobFilterLowHeightSpans);
				jobFilterLowHeightSpans.field = this.tileBuilder.linkedVoxelField;
				jobFilterLowHeightSpans.voxelWalkableHeight = this.voxelWalkableHeight;
				jobFilterLowHeightSpans.Execute();
				JobBuildCompactField jobBuildCompactField = default(JobBuildCompactField);
				jobBuildCompactField.input = this.tileBuilder.linkedVoxelField;
				jobBuildCompactField.output = this.tileBuilder.compactVoxelField;
				jobBuildCompactField.Execute();
				JobBuildConnections jobBuildConnections = default(JobBuildConnections);
				jobBuildConnections.field = this.tileBuilder.compactVoxelField;
				jobBuildConnections.voxelWalkableHeight = (int)this.voxelWalkableHeight;
				jobBuildConnections.voxelWalkableClimb = this.voxelWalkableClimb;
				jobBuildConnections.Execute();
				JobErodeWalkableArea jobErodeWalkableArea = default(JobErodeWalkableArea);
				jobErodeWalkableArea.field = this.tileBuilder.compactVoxelField;
				jobErodeWalkableArea.radius = this.characterRadiusInVoxels;
				jobErodeWalkableArea.Execute();
				JobBuildDistanceField jobBuildDistanceField = default(JobBuildDistanceField);
				jobBuildDistanceField.field = this.tileBuilder.compactVoxelField;
				jobBuildDistanceField.output = this.tileBuilder.distanceField;
				jobBuildDistanceField.Execute();
				JobBuildRegions jobBuildRegions = default(JobBuildRegions);
				jobBuildRegions.field = this.tileBuilder.compactVoxelField;
				jobBuildRegions.distanceField = this.tileBuilder.distanceField;
				jobBuildRegions.borderSize = this.tileBorderSizeInVoxels;
				jobBuildRegions.minRegionSize = Mathf.RoundToInt((float)this.minRegionSize);
				jobBuildRegions.srcQue = this.tileBuilder.tmpQueue1;
				jobBuildRegions.dstQue = this.tileBuilder.tmpQueue2;
				jobBuildRegions.relevantGraphSurfaces = this.relevantGraphSurfaces;
				jobBuildRegions.relevantGraphSurfaceMode = this.relevantGraphSurfaceMode;
				jobBuildRegions.cellSize = this.cellSize;
				jobBuildRegions.cellHeight = this.cellHeight;
				jobBuildRegions.graphTransform = this.graphToWorldSpace;
				jobBuildRegions.graphSpaceBounds = this.tileGraphSpaceBounds[num];
				jobBuildRegions.Execute();
				JobBuildContours jobBuildContours = default(JobBuildContours);
				jobBuildContours.field = this.tileBuilder.compactVoxelField;
				jobBuildContours.maxError = this.contourMaxError;
				jobBuildContours.maxEdgeLength = this.maxEdgeLength;
				jobBuildContours.buildFlags = 5;
				jobBuildContours.cellSize = this.cellSize;
				jobBuildContours.outputContours = this.tileBuilder.contours;
				jobBuildContours.outputVerts = this.tileBuilder.contourVertices;
				jobBuildContours.Execute();
				JobBuildMesh jobBuildMesh = default(JobBuildMesh);
				jobBuildMesh.contours = this.tileBuilder.contours;
				jobBuildMesh.contourVertices = this.tileBuilder.contourVertices;
				jobBuildMesh.mesh = this.tileBuilder.voxelMesh;
				jobBuildMesh.field = this.tileBuilder.compactVoxelField;
				jobBuildMesh.Execute();
				ref TileMesh.TileMeshUnsafe ptr = ref this.outputMeshes[num];
				JobConvertAreasToTags jobConvertAreasToTags = default(JobConvertAreasToTags);
				jobConvertAreasToTags.areas = this.tileBuilder.voxelMesh.areas;
				jobConvertAreasToTags.Execute();
				MeshUtility.JobMergeNearbyVertices jobMergeNearbyVertices = default(MeshUtility.JobMergeNearbyVertices);
				jobMergeNearbyVertices.vertices = this.tileBuilder.voxelMesh.verts;
				jobMergeNearbyVertices.triangles = this.tileBuilder.voxelMesh.tris;
				jobMergeNearbyVertices.mergeRadiusSq = 0;
				jobMergeNearbyVertices.Execute();
				MeshUtility.JobRemoveDegenerateTriangles jobRemoveDegenerateTriangles = default(MeshUtility.JobRemoveDegenerateTriangles);
				jobRemoveDegenerateTriangles.vertices = this.tileBuilder.voxelMesh.verts;
				jobRemoveDegenerateTriangles.triangles = this.tileBuilder.voxelMesh.tris;
				jobRemoveDegenerateTriangles.tags = this.tileBuilder.voxelMesh.areas;
				jobRemoveDegenerateTriangles.Execute();
				JobTransformTileCoordinates jobTransformTileCoordinates = default(JobTransformTileCoordinates);
				jobTransformTileCoordinates.vertices = this.tileBuilder.voxelMesh.verts.AsUnsafeSpan<Int3>();
				jobTransformTileCoordinates.matrix = this.voxelToTileSpace;
				jobTransformTileCoordinates.Execute();
				ptr = new TileMesh.TileMeshUnsafe
				{
					verticesInTileSpace = this.tileBuilder.voxelMesh.verts.AsUnsafeSpan<Int3>().Clone(Allocator.Persistent),
					triangles = this.tileBuilder.voxelMesh.tris.AsUnsafeSpan<int>().Clone(Allocator.Persistent),
					tags = this.tileBuilder.voxelMesh.areas.AsUnsafeSpan<int>().Reinterpret<uint>().Clone(Allocator.Persistent)
				};
			}
		}

		// Token: 0x040008E1 RID: 2273
		public TileBuilderBurst tileBuilder;

		// Token: 0x040008E2 RID: 2274
		[ReadOnly]
		public TileBuilder.BucketMapping inputMeshes;

		// Token: 0x040008E3 RID: 2275
		[ReadOnly]
		public NativeArray<Bounds> tileGraphSpaceBounds;

		// Token: 0x040008E4 RID: 2276
		public Matrix4x4 voxelToTileSpace;

		// Token: 0x040008E5 RID: 2277
		public Vector2 graphSpaceLimits;

		// Token: 0x040008E6 RID: 2278
		[NativeDisableUnsafePtrRestriction]
		public unsafe TileMesh.TileMeshUnsafe* outputMeshes;

		// Token: 0x040008E7 RID: 2279
		public int maxTiles;

		// Token: 0x040008E8 RID: 2280
		public int voxelWalkableClimb;

		// Token: 0x040008E9 RID: 2281
		public uint voxelWalkableHeight;

		// Token: 0x040008EA RID: 2282
		public float cellSize;

		// Token: 0x040008EB RID: 2283
		public float cellHeight;

		// Token: 0x040008EC RID: 2284
		public float maxSlope;

		// Token: 0x040008ED RID: 2285
		public RecastGraph.DimensionMode dimensionMode;

		// Token: 0x040008EE RID: 2286
		public RecastGraph.BackgroundTraversability backgroundTraversability;

		// Token: 0x040008EF RID: 2287
		public Matrix4x4 graphToWorldSpace;

		// Token: 0x040008F0 RID: 2288
		public int characterRadiusInVoxels;

		// Token: 0x040008F1 RID: 2289
		public int tileBorderSizeInVoxels;

		// Token: 0x040008F2 RID: 2290
		public int minRegionSize;

		// Token: 0x040008F3 RID: 2291
		public float maxEdgeLength;

		// Token: 0x040008F4 RID: 2292
		public float contourMaxError;

		// Token: 0x040008F5 RID: 2293
		[ReadOnly]
		public NativeArray<JobBuildRegions.RelevantGraphSurfaceInfo> relevantGraphSurfaces;

		// Token: 0x040008F6 RID: 2294
		public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;

		// Token: 0x040008F7 RID: 2295
		[NativeDisableUnsafePtrRestriction]
		public unsafe int* currentTileCounter;

		// Token: 0x040008F8 RID: 2296
		private static readonly ProfilerMarker MarkerVoxelize = new ProfilerMarker("Voxelize");

		// Token: 0x040008F9 RID: 2297
		private static readonly ProfilerMarker MarkerFilterLedges = new ProfilerMarker("FilterLedges");

		// Token: 0x040008FA RID: 2298
		private static readonly ProfilerMarker MarkerFilterLowHeightSpans = new ProfilerMarker("FilterLowHeightSpans");

		// Token: 0x040008FB RID: 2299
		private static readonly ProfilerMarker MarkerBuildCompactField = new ProfilerMarker("BuildCompactField");

		// Token: 0x040008FC RID: 2300
		private static readonly ProfilerMarker MarkerBuildConnections = new ProfilerMarker("BuildConnections");

		// Token: 0x040008FD RID: 2301
		private static readonly ProfilerMarker MarkerErodeWalkableArea = new ProfilerMarker("ErodeWalkableArea");

		// Token: 0x040008FE RID: 2302
		private static readonly ProfilerMarker MarkerBuildDistanceField = new ProfilerMarker("BuildDistanceField");

		// Token: 0x040008FF RID: 2303
		private static readonly ProfilerMarker MarkerBuildRegions = new ProfilerMarker("BuildRegions");

		// Token: 0x04000900 RID: 2304
		private static readonly ProfilerMarker MarkerBuildContours = new ProfilerMarker("BuildContours");

		// Token: 0x04000901 RID: 2305
		private static readonly ProfilerMarker MarkerBuildMesh = new ProfilerMarker("BuildMesh");

		// Token: 0x04000902 RID: 2306
		private static readonly ProfilerMarker MarkerConvertAreasToTags = new ProfilerMarker("ConvertAreasToTags");

		// Token: 0x04000903 RID: 2307
		private static readonly ProfilerMarker MarkerRemoveDuplicateVertices = new ProfilerMarker("RemoveDuplicateVertices");

		// Token: 0x04000904 RID: 2308
		private static readonly ProfilerMarker MarkerTransformTileCoordinates = new ProfilerMarker("TransformTileCoordinates");
	}
}
