using System;
using System.Runtime.InteropServices;
using Pathfinding.Jobs;
using Pathfinding.Sync;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001E1 RID: 481
	public struct JobBuildNodes
	{
		// Token: 0x06000C6D RID: 3181 RVA: 0x0004CA40 File Offset: 0x0004AC40
		internal JobBuildNodes(RecastGraph graph, TileLayout tileLayout)
		{
			this.tileLayout = tileLayout;
			this.graphIndex = graph.graphIndex;
			this.initialPenalty = graph.initialPenalty;
			this.recalculateNormals = graph.RecalculateNormals;
			this.maxTileConnectionEdgeDistance = graph.MaxTileConnectionEdgeDistance;
			this.graphToWorldSpace = tileLayout.transform.matrix;
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x0004CA98 File Offset: 0x0004AC98
		public Promise<JobBuildNodes.BuildNodeTilesOutput> Schedule(DisposeArena arena, Promise<TileBuilder.TileBuilderOutput> preCutDependency, Promise<TileCutter.TileCutterOutput> postCutDependency)
		{
			TileCutter.TileCutterOutput value = postCutDependency.GetValue();
			TileBuilder.TileBuilderOutput value2 = preCutDependency.GetValue();
			IntRect tileRect = value2.tileMeshes.tileRect;
			NativeArray<TileMesh.TileMeshUnsafe> tileMeshes;
			if (value.tileMeshes.tileMeshes.IsCreated)
			{
				tileMeshes = value.tileMeshes.tileMeshes;
			}
			else
			{
				tileMeshes = value2.tileMeshes.tileMeshes;
			}
			NavmeshTile[] array = new NavmeshTile[tileRect.Area];
			GCHandle gchandle = GCHandle.Alloc(array);
			NativeArray<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe> nativeArray = new NativeArray<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe>(tileRect.Area, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			JobHandle job = new JobCalculateTriangleConnections
			{
				tileMeshes = tileMeshes,
				nodeConnections = nativeArray
			}.Schedule(postCutDependency.handle);
			Vector2 tileWorldSize = new Vector2(this.tileLayout.TileWorldSizeX, this.tileLayout.TileWorldSizeZ);
			JobHandle job2 = new JobCreateTiles
			{
				preCutTileMeshes = (value.tileMeshes.tileMeshes.IsCreated ? value2.tileMeshes.tileMeshes : default(NativeArray<TileMesh.TileMeshUnsafe>)),
				tileMeshes = tileMeshes,
				tiles = gchandle,
				tileRect = tileRect,
				graphTileCount = this.tileLayout.tileCount,
				graphIndex = this.graphIndex,
				initialPenalty = this.initialPenalty,
				recalculateNormals = this.recalculateNormals,
				graphToWorldSpace = this.graphToWorldSpace,
				tileWorldSize = tileWorldSize
			}.Schedule(postCutDependency.handle);
			JobHandle dependency = new JobWriteNodeConnections
			{
				nodeConnections = nativeArray,
				tiles = gchandle
			}.Schedule(JobHandle.CombineDependencies(job, job2));
			JobHandle handle = JobConnectTiles.ScheduleBatch(gchandle, dependency, tileRect, tileWorldSize, this.maxTileConnectionEdgeDistance);
			arena.Add(gchandle);
			arena.Add<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe>(nativeArray);
			return new Promise<JobBuildNodes.BuildNodeTilesOutput>(handle, new JobBuildNodes.BuildNodeTilesOutput
			{
				progressSource = value2,
				tiles = array
			});
		}

		// Token: 0x040008C9 RID: 2249
		private uint graphIndex;

		// Token: 0x040008CA RID: 2250
		public uint initialPenalty;

		// Token: 0x040008CB RID: 2251
		public bool recalculateNormals;

		// Token: 0x040008CC RID: 2252
		public float maxTileConnectionEdgeDistance;

		// Token: 0x040008CD RID: 2253
		private Matrix4x4 graphToWorldSpace;

		// Token: 0x040008CE RID: 2254
		private TileLayout tileLayout;

		// Token: 0x020001E2 RID: 482
		public class BuildNodeTilesOutput : IProgress, IDisposable
		{
			// Token: 0x170001BF RID: 447
			// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0004CC71 File Offset: 0x0004AE71
			public float Progress
			{
				get
				{
					return this.progressSource.Progress;
				}
			}

			// Token: 0x06000C70 RID: 3184 RVA: 0x000035CE File Offset: 0x000017CE
			public void Dispose()
			{
			}

			// Token: 0x040008CF RID: 2255
			public TileBuilder.TileBuilderOutput progressSource;

			// Token: 0x040008D0 RID: 2256
			public NavmeshTile[] tiles;
		}
	}
}
