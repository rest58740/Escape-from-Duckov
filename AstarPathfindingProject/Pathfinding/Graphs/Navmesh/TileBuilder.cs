using System;
using System.Collections.Generic;
using Pathfinding.Graphs.Navmesh.Jobs;
using Pathfinding.Graphs.Navmesh.Voxelization.Burst;
using Pathfinding.Jobs;
using Pathfinding.Sync;
using Unity.Collections;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001AC RID: 428
	public struct TileBuilder
	{
		// Token: 0x06000B9B RID: 2971 RVA: 0x0004222C File Offset: 0x0004042C
		public TileBuilder(RecastGraph graph, TileLayout tileLayout, IntRect tileRect)
		{
			this.tileLayout = tileLayout;
			this.tileRect = tileRect;
			this.walkableClimb = Mathf.Min(graph.walkableClimb, graph.walkableHeight);
			this.collectionSettings = graph.collectionSettings;
			this.dimensionMode = graph.dimensionMode;
			this.backgroundTraversability = graph.backgroundTraversability;
			this.tileBorderSizeInVoxels = graph.TileBorderSizeInVoxels;
			this.walkableHeight = graph.walkableHeight;
			this.maxSlope = graph.maxSlope;
			this.characterRadiusInVoxels = graph.CharacterRadiusInVoxels;
			this.minRegionSize = Mathf.RoundToInt(graph.minRegionSize);
			this.maxEdgeLength = graph.maxEdgeLength;
			this.contourMaxError = graph.contourMaxError;
			this.relevantGraphSurfaceMode = graph.relevantGraphSurfaceMode;
			this.perLayerModifications = graph.perLayerModifications;
			if (this.collectionSettings.physicsScene == null)
			{
				this.collectionSettings.physicsScene = new PhysicsScene?(graph.active.gameObject.scene.GetPhysicsScene());
			}
			if (this.collectionSettings.physicsScene2D == null)
			{
				this.collectionSettings.physicsScene2D = new PhysicsScene2D?(graph.active.gameObject.scene.GetPhysicsScene2D());
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x00042361 File Offset: 0x00040561
		private int TileBorderSizeInVoxels
		{
			get
			{
				return this.characterRadiusInVoxels + 3;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000B9D RID: 2973 RVA: 0x0004236B File Offset: 0x0004056B
		private float TileBorderSizeInWorldUnits
		{
			get
			{
				return (float)this.TileBorderSizeInVoxels * this.tileLayout.cellSize;
			}
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00042380 File Offset: 0x00040580
		public Bounds GetWorldSpaceBounds(float xzPadding = 0f)
		{
			Bounds tileBoundsInGraphSpace = this.tileLayout.GetTileBoundsInGraphSpace(this.tileRect.xmin, this.tileRect.ymin, this.tileRect.Width, this.tileRect.Height);
			tileBoundsInGraphSpace.Expand(new Vector3(2f * xzPadding, 0f, 2f * xzPadding));
			return this.tileLayout.transform.Transform(tileBoundsInGraphSpace);
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x000423F8 File Offset: 0x000405F8
		public RecastMeshGatherer.MeshCollection CollectMeshes(Bounds bounds)
		{
			LayerMask mask = this.collectionSettings.layerMask;
			List<string> tagMask = this.collectionSettings.tagMask;
			if (this.collectionSettings.collectionMode == RecastGraph.CollectionSettings.FilterMode.Layers)
			{
				tagMask = null;
			}
			else
			{
				mask = 0;
			}
			RecastMeshGatherer recastMeshGatherer = new RecastMeshGatherer(this.collectionSettings.physicsScene.Value, this.collectionSettings.physicsScene2D.Value, bounds, this.collectionSettings.terrainHeightmapDownsamplingFactor, mask, tagMask, this.perLayerModifications, this.tileLayout.cellSize / this.collectionSettings.colliderRasterizeDetail);
			if (this.collectionSettings.rasterizeMeshes && this.dimensionMode == RecastGraph.DimensionMode.Dimension3D)
			{
				recastMeshGatherer.CollectSceneMeshes();
			}
			recastMeshGatherer.CollectRecastNavmeshModifiers();
			if (this.collectionSettings.rasterizeTerrain && this.dimensionMode == RecastGraph.DimensionMode.Dimension3D)
			{
				float desiredChunkSize = 0.51f * this.tileLayout.cellSize * (float)(math.max(this.tileLayout.tileSizeInVoxels.x, this.tileLayout.tileSizeInVoxels.y) + 2 * this.TileBorderSizeInVoxels);
				recastMeshGatherer.CollectTerrainMeshes(this.collectionSettings.rasterizeTrees, desiredChunkSize);
			}
			if (this.collectionSettings.rasterizeColliders || this.dimensionMode == RecastGraph.DimensionMode.Dimension2D)
			{
				if (this.dimensionMode == RecastGraph.DimensionMode.Dimension3D)
				{
					recastMeshGatherer.CollectColliderMeshes();
				}
				else
				{
					recastMeshGatherer.Collect2DColliderMeshes();
				}
			}
			if (this.collectionSettings.onCollectMeshes != null)
			{
				this.collectionSettings.onCollectMeshes(recastMeshGatherer);
			}
			RecastMeshGatherer.MeshCollection result = recastMeshGatherer.Finalize();
			if (this.tileRect == new IntRect(0, 0, this.tileLayout.tileCount.x - 1, this.tileLayout.tileCount.y - 1) && result.meshes.Length == 0)
			{
				Debug.LogWarning("No rasterizable objects were found contained in the layers specified by the 'mask' variables");
			}
			return result;
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x000425B8 File Offset: 0x000407B8
		private TileBuilder.BucketMapping PutMeshesIntoTileBuckets(RecastMeshGatherer.MeshCollection meshCollection, IntRect tileBuckets)
		{
			int num = tileBuckets.Width * tileBuckets.Height;
			NativeList<int>[] array = new NativeList<int>[num];
			float tileBorderSizeInWorldUnits = this.TileBorderSizeInWorldUnits;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new NativeList<int>(Allocator.Persistent);
			}
			Vector2Int offset = -tileBuckets.Min;
			IntRect b = new IntRect(0, 0, tileBuckets.Width - 1, tileBuckets.Height - 1);
			NativeArray<RasterizationMesh> meshes = meshCollection.meshes;
			for (int j = 0; j < meshes.Length; j++)
			{
				Bounds bounds = meshes[j].bounds;
				IntRect intRect = IntRect.Intersection(this.tileLayout.GetTouchingTiles(bounds, tileBorderSizeInWorldUnits).Offset(offset), b);
				for (int k = intRect.ymin; k <= intRect.ymax; k++)
				{
					for (int l = intRect.xmin; l <= intRect.xmax; l++)
					{
						array[l + k * tileBuckets.Width].Add(j);
					}
				}
			}
			int num2 = 0;
			for (int m = 0; m < array.Length; m++)
			{
				num2 += array[m].Length;
			}
			NativeArray<int> nativeArray = new NativeArray<int>(num2, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			NativeArray<int> bucketRanges = new NativeArray<int>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			num2 = 0;
			for (int n = 0; n < array.Length; n++)
			{
				if (array[n].Length > 0)
				{
					NativeArray<int>.Copy(array[n].AsArray(), 0, nativeArray, num2, array[n].Length);
				}
				num2 += array[n].Length;
				bucketRanges[n] = num2;
				array[n].Dispose();
			}
			return new TileBuilder.BucketMapping
			{
				meshes = meshCollection.meshes,
				pointers = nativeArray,
				bucketRanges = bucketRanges
			};
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x000427B0 File Offset: 0x000409B0
		public Promise<TileBuilder.TileBuilderOutput> Schedule(DisposeArena arena)
		{
			int area = this.tileRect.Area;
			int width = this.tileRect.Width;
			int height = this.tileRect.Height;
			Bounds worldSpaceBounds = this.GetWorldSpaceBounds(this.TileBorderSizeInWorldUnits);
			if (this.dimensionMode == RecastGraph.DimensionMode.Dimension2D)
			{
				worldSpaceBounds.extents = new Vector3(worldSpaceBounds.extents.x, worldSpaceBounds.extents.y, float.PositiveInfinity);
			}
			RecastMeshGatherer.MeshCollection meshCollection = this.CollectMeshes(worldSpaceBounds);
			TileBuilder.BucketMapping bucketMapping = this.PutMeshesIntoTileBuckets(meshCollection, this.tileRect);
			NativeArray<TileMesh.TileMeshUnsafe> nativeArray = new NativeArray<TileMesh.TileMeshUnsafe>(area, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			int width2 = this.tileLayout.tileSizeInVoxels.x + this.tileBorderSizeInVoxels * 2;
			int depth = this.tileLayout.tileSizeInVoxels.y + this.tileBorderSizeInVoxels * 2;
			float cellHeight = this.tileLayout.CellHeight;
			uint voxelWalkableHeight = (uint)(this.walkableHeight / cellHeight);
			int voxelWalkableClimb = Mathf.RoundToInt(this.walkableClimb / cellHeight);
			NativeArray<Bounds> nativeArray2 = new NativeArray<Bounds>(area, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int index = j + i * width;
					Bounds tileBoundsInGraphSpace = this.tileLayout.GetTileBoundsInGraphSpace(this.tileRect.xmin + j, this.tileRect.ymin + i, 1, 1);
					tileBoundsInGraphSpace.Expand(new Vector3(1f, 0f, 1f) * this.TileBorderSizeInWorldUnits * 2f);
					nativeArray2[index] = tileBoundsInGraphSpace;
				}
			}
			TileBuilderBurst[] array = new TileBuilderBurst[Mathf.Max(1, Mathf.Min(area, JobsUtility.JobWorkerCount + 1))];
			NativeReference<int> nativeReference = new NativeReference<int>(0, Allocator.Persistent);
			JobHandle jobHandle = default(JobHandle);
			NativeList<JobBuildRegions.RelevantGraphSurfaceInfo> data = new NativeList<JobBuildRegions.RelevantGraphSurfaceInfo>(Allocator.Persistent);
			RelevantGraphSurface relevantGraphSurface = RelevantGraphSurface.Root;
			while (relevantGraphSurface != null)
			{
				JobBuildRegions.RelevantGraphSurfaceInfo relevantGraphSurfaceInfo = default(JobBuildRegions.RelevantGraphSurfaceInfo);
				relevantGraphSurfaceInfo.position = relevantGraphSurface.transform.position;
				relevantGraphSurfaceInfo.range = relevantGraphSurface.maxRange;
				data.Add(relevantGraphSurfaceInfo);
				relevantGraphSurface = relevantGraphSurface.Next;
			}
			int num = Mathf.CeilToInt(Mathf.Sqrt((float)area));
			int num2 = num * array.Length;
			int num3 = 2 * (area + num2 - 1) / num2;
			JobBuildTileMeshFromVoxels jobData = new JobBuildTileMeshFromVoxels
			{
				tileBuilder = array[0],
				inputMeshes = bucketMapping,
				tileGraphSpaceBounds = nativeArray2,
				voxelWalkableClimb = voxelWalkableClimb,
				voxelWalkableHeight = voxelWalkableHeight,
				voxelToTileSpace = Matrix4x4.Scale(new Vector3(this.tileLayout.cellSize, cellHeight, this.tileLayout.cellSize)) * Matrix4x4.Translate(-new Vector3(1f, 0f, 1f) * (float)this.TileBorderSizeInVoxels),
				cellSize = this.tileLayout.cellSize,
				cellHeight = cellHeight,
				maxSlope = Mathf.Max(this.maxSlope, 0.0001f),
				dimensionMode = this.dimensionMode,
				backgroundTraversability = this.backgroundTraversability,
				graphToWorldSpace = this.tileLayout.transform.matrix,
				graphSpaceLimits = new Vector2(this.tileLayout.graphSpaceSize.x + (float)(this.characterRadiusInVoxels - 1) * this.tileLayout.cellSize, this.tileLayout.graphSpaceSize.z + (float)(this.characterRadiusInVoxels - 1) * this.tileLayout.cellSize),
				characterRadiusInVoxels = this.characterRadiusInVoxels,
				tileBorderSizeInVoxels = this.tileBorderSizeInVoxels,
				minRegionSize = this.minRegionSize,
				maxEdgeLength = this.maxEdgeLength,
				contourMaxError = this.contourMaxError,
				maxTiles = num,
				relevantGraphSurfaces = data.AsArray(),
				relevantGraphSurfaceMode = this.relevantGraphSurfaceMode
			};
			jobData.SetOutputMeshes(nativeArray);
			jobData.SetCounter(nativeReference);
			int maximumVoxelYCoord = (int)(this.tileLayout.graphSpaceSize.y / cellHeight);
			for (int k = 0; k < array.Length; k++)
			{
				jobData.tileBuilder = (array[k] = new TileBuilderBurst(width2, depth, (int)voxelWalkableHeight, maximumVoxelYCoord));
				JobHandle jobHandle2 = default(JobHandle);
				for (int l = 0; l < num3; l++)
				{
					jobHandle2 = jobData.Schedule(jobHandle2);
				}
				jobHandle = JobHandle.CombineDependencies(jobHandle, jobHandle2);
			}
			JobHandle.ScheduleBatchedJobs();
			arena.Add<Bounds>(nativeArray2);
			arena.Add<JobBuildRegions.RelevantGraphSurfaceInfo>(data);
			arena.Add<int>(bucketMapping.bucketRanges);
			arena.Add<int>(bucketMapping.pointers);
			arena.Add<RecastMeshGatherer.MeshCollection>(meshCollection);
			for (int m = 0; m < array.Length; m++)
			{
				arena.Add<TileBuilderBurst>(array[m]);
			}
			return new Promise<TileBuilder.TileBuilderOutput>(jobHandle, new TileBuilder.TileBuilderOutput
			{
				tileMeshes = new TileMeshesUnsafe(nativeArray, this.tileRect, new Vector2(this.tileLayout.TileWorldSizeX, this.tileLayout.TileWorldSizeZ)),
				currentTileCounter = nativeReference
			});
		}

		// Token: 0x040007EF RID: 2031
		public float walkableClimb;

		// Token: 0x040007F0 RID: 2032
		public RecastGraph.CollectionSettings collectionSettings;

		// Token: 0x040007F1 RID: 2033
		public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;

		// Token: 0x040007F2 RID: 2034
		public RecastGraph.DimensionMode dimensionMode;

		// Token: 0x040007F3 RID: 2035
		public RecastGraph.BackgroundTraversability backgroundTraversability;

		// Token: 0x040007F4 RID: 2036
		public int tileBorderSizeInVoxels;

		// Token: 0x040007F5 RID: 2037
		public float walkableHeight;

		// Token: 0x040007F6 RID: 2038
		public float maxSlope;

		// Token: 0x040007F7 RID: 2039
		public int characterRadiusInVoxels;

		// Token: 0x040007F8 RID: 2040
		public int minRegionSize;

		// Token: 0x040007F9 RID: 2041
		public float maxEdgeLength;

		// Token: 0x040007FA RID: 2042
		public float contourMaxError;

		// Token: 0x040007FB RID: 2043
		public TileLayout tileLayout;

		// Token: 0x040007FC RID: 2044
		public IntRect tileRect;

		// Token: 0x040007FD RID: 2045
		public List<RecastGraph.PerLayerModification> perLayerModifications;

		// Token: 0x020001AD RID: 429
		public class TileBuilderOutput : IProgress, IDisposable
		{
			// Token: 0x170001B1 RID: 433
			// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00042CD4 File Offset: 0x00040ED4
			public float Progress
			{
				get
				{
					int area = this.tileMeshes.tileRect.Area;
					int num = Mathf.Min(area, this.currentTileCounter.Value);
					if (area <= 0)
					{
						return 0f;
					}
					return (float)num / (float)area;
				}
			}

			// Token: 0x06000BA3 RID: 2979 RVA: 0x00042D13 File Offset: 0x00040F13
			public void Dispose()
			{
				this.tileMeshes.Dispose(Allocator.Persistent);
				if (this.currentTileCounter.IsCreated)
				{
					this.currentTileCounter.Dispose();
				}
			}

			// Token: 0x040007FE RID: 2046
			public NativeReference<int> currentTileCounter;

			// Token: 0x040007FF RID: 2047
			public TileMeshesUnsafe tileMeshes;
		}

		// Token: 0x020001AE RID: 430
		public struct BucketMapping
		{
			// Token: 0x04000800 RID: 2048
			public NativeArray<RasterizationMesh> meshes;

			// Token: 0x04000801 RID: 2049
			public NativeArray<int> pointers;

			// Token: 0x04000802 RID: 2050
			public NativeArray<int> bucketRanges;
		}
	}
}
