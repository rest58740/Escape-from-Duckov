using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Sync;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001AF RID: 431
	public struct TileCutter
	{
		// Token: 0x06000BA5 RID: 2981 RVA: 0x00042D39 File Offset: 0x00040F39
		public TileCutter(NavmeshBase graph, GridLookup<NavmeshClipper> cuts, TileLayout tileLayout)
		{
			this.graph = graph;
			this.cuts = cuts;
			this.tileLayout = tileLayout;
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00042D50 File Offset: 0x00040F50
		private static void DisposeTileData(UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices, UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles, UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags, Allocator allocator, bool skipFirst)
		{
			for (int i = 0; i < tileVertices.Length; i++)
			{
				for (int j = skipFirst ? 1 : 0; j < tileVertices[i].Length; j++)
				{
					tileVertices[i][j].Free(allocator);
					tileTriangles[i][j].Free(allocator);
					tileTags[i][j].Free(allocator);
				}
				tileVertices[i].Dispose();
				tileTriangles[i].Dispose();
				tileTags[i].Dispose();
			}
			tileVertices.Free(allocator);
			tileTriangles.Free(allocator);
			tileTags.Free(allocator);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00042E18 File Offset: 0x00041018
		public unsafe static void EnsurePreCutDataExists(NavmeshBase graph, NavmeshTile tile)
		{
			if (!tile.isCut)
			{
				tile.preCutTris = tile.tris.Clone(Allocator.Persistent);
				tile.preCutVertsInTileSpace = tile.vertsInGraphSpace.Clone(Allocator.Persistent);
				Int3 rhs = (Int3)graph.GetTileBoundsInGraphSpace(tile.x, tile.z, 1, 1).min;
				for (int i = 0; i < tile.preCutVertsInTileSpace.Length; i++)
				{
					*tile.preCutVertsInTileSpace[i] -= rhs;
				}
				tile.preCutTags = new UnsafeSpan<uint>(Allocator.Persistent, tile.nodes.Length);
				for (int j = 0; j < tile.nodes.Length; j++)
				{
					*tile.preCutTags[j] = tile.nodes[j].Tag;
				}
				tile.isCut = true;
			}
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0001797A File Offset: 0x00015B7A
		private static bool CheckVersion()
		{
			return true;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00042EF4 File Offset: 0x000410F4
		public unsafe Promise<TileCutter.TileCutterOutput> Schedule(List<Vector2Int> tileCoordinates)
		{
			if (this.cuts == null)
			{
				return new Promise<TileCutter.TileCutterOutput>(default(JobHandle), default(TileCutter.TileCutterOutput));
			}
			int count = tileCoordinates.Count;
			UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices = new UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>>(Allocator.Persistent, count);
			UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles = new UnsafeSpan<UnsafeList<UnsafeSpan<int>>>(Allocator.Persistent, count);
			UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags = new UnsafeSpan<UnsafeList<UnsafeSpan<int>>>(Allocator.Persistent, count);
			for (int i = 0; i < tileVertices.Length; i++)
			{
				*tileVertices[i] = new UnsafeList<UnsafeSpan<Int3>>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				*tileTriangles[i] = new UnsafeList<UnsafeSpan<int>>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				*tileTags[i] = new UnsafeList<UnsafeSpan<int>>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			}
			TileHandler.CutCollection cutCollection = TileHandler.CollectCuts(this.cuts, tileCoordinates, this.graph.NavmeshCuttingCharacterRadius, this.tileLayout, ref tileVertices, ref tileTriangles, ref tileTags);
			if (!cutCollection.cuttingRequired || !TileCutter.CheckVersion())
			{
				TileCutter.DisposeTileData(tileVertices, tileTriangles, tileTags, Allocator.Persistent, false);
				cutCollection.Dispose();
				return new Promise<TileCutter.TileCutterOutput>(default(JobHandle), default(TileCutter.TileCutterOutput));
			}
			NativeArray<TileMesh.TileMeshUnsafe> tileMeshes = new NativeArray<TileMesh.TileMeshUnsafe>(count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int j = 0; j < tileCoordinates.Count; j++)
			{
				NavmeshTile tile = this.graph.GetTile(tileCoordinates[j].x, tileCoordinates[j].y);
				TileCutter.EnsurePreCutDataExists(this.graph, tile);
				tileMeshes[j] = new TileMesh.TileMeshUnsafe
				{
					triangles = tile.preCutTris,
					verticesInTileSpace = tile.preCutVertsInTileSpace,
					tags = tile.preCutTags
				};
			}
			Vector2 tileWorldSize = new Vector2(this.graph.TileWorldSizeX, this.graph.TileWorldSizeZ);
			TileMeshesUnsafe inputTileMeshes = new TileMeshesUnsafe(tileMeshes, new IntRect(0, 0, -1, -1), tileWorldSize);
			NativeArray<TileMesh.TileMeshUnsafe> nativeArray = new NativeArray<TileMesh.TileMeshUnsafe>(count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			TileMeshesUnsafe tileMeshes2 = new TileMeshesUnsafe(nativeArray, new IntRect(0, 0, -1, -1), tileWorldSize);
			TileCutter.TileCutterOutput result = new TileCutter.TileCutterOutput
			{
				tileMeshes = tileMeshes2
			};
			TileHandler.InitDelegates();
			JobHandle jobHandle = new TileCutter.JobCutTiles
			{
				tileVertices = tileVertices,
				tileTriangles = tileTriangles,
				tileTags = tileTags,
				cutCollection = cutCollection,
				inputTileMeshes = inputTileMeshes,
				outputTileMeshes = nativeArray
			}.Schedule(default(JobHandle));
			tileMeshes.Dispose(jobHandle);
			return new Promise<TileCutter.TileCutterOutput>(jobHandle, result);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00043174 File Offset: 0x00041374
		public unsafe Promise<TileCutter.TileCutterOutput> Schedule(Promise<TileBuilder.TileBuilderOutput> builderOutput)
		{
			if (this.cuts == null)
			{
				return new Promise<TileCutter.TileCutterOutput>(builderOutput.handle, default(TileCutter.TileCutterOutput));
			}
			TileBuilder.TileBuilderOutput value = builderOutput.GetValue();
			IntRect tileRect = value.tileMeshes.tileRect;
			List<Vector2Int> innerCoordinates = tileRect.GetInnerCoordinates();
			int count = innerCoordinates.Count;
			UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices = new UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>>(Allocator.Persistent, count);
			UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles = new UnsafeSpan<UnsafeList<UnsafeSpan<int>>>(Allocator.Persistent, count);
			UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags = new UnsafeSpan<UnsafeList<UnsafeSpan<int>>>(Allocator.Persistent, count);
			for (int i = 0; i < tileVertices.Length; i++)
			{
				*tileVertices[i] = new UnsafeList<UnsafeSpan<Int3>>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				*tileTriangles[i] = new UnsafeList<UnsafeSpan<int>>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				*tileTags[i] = new UnsafeList<UnsafeSpan<int>>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			}
			TileHandler.CutCollection cutCollection = TileHandler.CollectCuts(this.cuts, innerCoordinates, this.graph.NavmeshCuttingCharacterRadius, this.tileLayout, ref tileVertices, ref tileTriangles, ref tileTags);
			if (!cutCollection.cuttingRequired || !TileCutter.CheckVersion())
			{
				TileCutter.DisposeTileData(tileVertices, tileTriangles, tileTags, Allocator.Persistent, false);
				cutCollection.Dispose();
				return new Promise<TileCutter.TileCutterOutput>(builderOutput.handle, default(TileCutter.TileCutterOutput));
			}
			NativeArray<TileMesh.TileMeshUnsafe> nativeArray = new NativeArray<TileMesh.TileMeshUnsafe>(value.tileMeshes.tileMeshes.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			TileMeshesUnsafe tileMeshes = new TileMeshesUnsafe(nativeArray, value.tileMeshes.tileRect, value.tileMeshes.tileWorldSize);
			TileCutter.TileCutterOutput result = new TileCutter.TileCutterOutput
			{
				tileMeshes = tileMeshes
			};
			TileHandler.InitDelegates();
			return new Promise<TileCutter.TileCutterOutput>(new TileCutter.JobCutTiles
			{
				tileVertices = tileVertices,
				tileTriangles = tileTriangles,
				tileTags = tileTags,
				cutCollection = cutCollection,
				inputTileMeshes = value.tileMeshes,
				outputTileMeshes = nativeArray
			}.Schedule(builderOutput.handle), result);
		}

		// Token: 0x04000803 RID: 2051
		private NavmeshBase graph;

		// Token: 0x04000804 RID: 2052
		private GridLookup<NavmeshClipper> cuts;

		// Token: 0x04000805 RID: 2053
		private TileLayout tileLayout;

		// Token: 0x020001B0 RID: 432
		public struct TileCutterOutput : IProgress, IDisposable
		{
			// Token: 0x170001B2 RID: 434
			// (get) Token: 0x06000BAB RID: 2987 RVA: 0x000059E1 File Offset: 0x00003BE1
			public float Progress
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x06000BAC RID: 2988 RVA: 0x0004334B File Offset: 0x0004154B
			public void Dispose()
			{
				this.tileMeshes.Dispose(Allocator.Persistent);
			}

			// Token: 0x04000806 RID: 2054
			public TileMeshesUnsafe tileMeshes;
		}

		// Token: 0x020001B1 RID: 433
		[BurstCompile]
		private struct JobCutTiles : IJob
		{
			// Token: 0x06000BAD RID: 2989 RVA: 0x0004335C File Offset: 0x0004155C
			public void Execute()
			{
				int length = this.inputTileMeshes.tileMeshes.Length;
				for (int i = 0; i < length; i++)
				{
					this.tileVertices[i].InsertRange(0, 1);
					this.tileTriangles[i].InsertRange(0, 1);
					this.tileTags[i].InsertRange(0, 1);
					this.tileVertices[i][0] = this.inputTileMeshes.tileMeshes[i].verticesInTileSpace;
					this.tileTriangles[i][0] = this.inputTileMeshes.tileMeshes[i].triangles;
					this.tileTags[i][0] = this.inputTileMeshes.tileMeshes[i].tags.Reinterpret<int>();
				}
				UnsafeSpan<TileMesh.TileMeshUnsafe> unsafeSpan = this.outputTileMeshes.AsUnsafeSpan<TileMesh.TileMeshUnsafe>();
				Vector2Int vector2Int = new Vector2Int(Mathf.RoundToInt(this.inputTileMeshes.tileWorldSize.x * 1000f), Mathf.RoundToInt(this.inputTileMeshes.tileWorldSize.y * 1000f));
				TileHandler.CutTiles(ref this.tileVertices, ref this.tileTriangles, ref this.tileTags, ref vector2Int, ref this.cutCollection, ref unsafeSpan, Allocator.Persistent);
				TileCutter.DisposeTileData(this.tileVertices, this.tileTriangles, this.tileTags, Allocator.Persistent, true);
				this.cutCollection.Dispose();
			}

			// Token: 0x04000807 RID: 2055
			public UnsafeSpan<UnsafeList<UnsafeSpan<Int3>>> tileVertices;

			// Token: 0x04000808 RID: 2056
			public UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTriangles;

			// Token: 0x04000809 RID: 2057
			public UnsafeSpan<UnsafeList<UnsafeSpan<int>>> tileTags;

			// Token: 0x0400080A RID: 2058
			public TileHandler.CutCollection cutCollection;

			// Token: 0x0400080B RID: 2059
			public TileMeshesUnsafe inputTileMeshes;

			// Token: 0x0400080C RID: 2060
			public NativeArray<TileMesh.TileMeshUnsafe> outputTileMeshes;
		}
	}
}
