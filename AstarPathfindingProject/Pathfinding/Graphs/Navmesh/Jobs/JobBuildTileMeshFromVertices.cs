using System;
using Pathfinding.Collections;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001E3 RID: 483
	[BurstCompile(FloatMode = FloatMode.Default)]
	public struct JobBuildTileMeshFromVertices : IJob
	{
		// Token: 0x06000C72 RID: 3186 RVA: 0x0004CC80 File Offset: 0x0004AE80
		public static Promise<TileBuilder.TileBuilderOutput> Schedule(NativeArray<Vector3> vertices, NativeArray<int> indices, Matrix4x4 meshToGraph, bool recalculateNormals)
		{
			if (vertices.Length > 4095)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Too many vertices in the navmesh graph. Provided ",
					vertices.Length.ToString(),
					", but the maximum number of vertices per tile is ",
					4095.ToString(),
					". You can raise this limit by enabling ASTAR_RECAST_LARGER_TILES in the A* Inspector Optimizations tab"
				}));
			}
			NativeArray<TileMesh.TileMeshUnsafe> tileMeshes = new NativeArray<TileMesh.TileMeshUnsafe>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			return new Promise<TileBuilder.TileBuilderOutput>(new JobBuildTileMeshFromVertices
			{
				vertices = vertices,
				indices = indices,
				meshToGraph = meshToGraph,
				outputBuffers = tileMeshes,
				recalculateNormals = recalculateNormals
			}.Schedule(default(JobHandle)), new TileBuilder.TileBuilderOutput
			{
				tileMeshes = new TileMeshesUnsafe(tileMeshes, new IntRect(0, 0, 0, 0), new Vector2(100000f, 100000f))
			});
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0004CD60 File Offset: 0x0004AF60
		public unsafe void Execute()
		{
			NativeList<Int3> list = new NativeList<Int3>(this.vertices.Length, Allocator.Temp);
			list.Length = this.vertices.Length;
			NativeList<int> nativeList = new NativeList<int>(this.indices.Length / 3, Allocator.Temp);
			nativeList.Length = this.indices.Length / 3;
			NativeList<int> nativeList2 = new NativeList<int>(this.indices.Length, Allocator.Temp);
			nativeList2.AddRange(this.indices);
			JobBuildTileMeshFromVertices.JobTransformTileCoordinates jobTransformTileCoordinates = default(JobBuildTileMeshFromVertices.JobTransformTileCoordinates);
			jobTransformTileCoordinates.vertices = this.vertices;
			jobTransformTileCoordinates.outputVertices = list.AsArray();
			jobTransformTileCoordinates.matrix = this.meshToGraph;
			jobTransformTileCoordinates.Execute();
			TileMesh.TileMeshUnsafe* unsafePtr = (TileMesh.TileMeshUnsafe*)this.outputBuffers.GetUnsafePtr<TileMesh.TileMeshUnsafe>();
			MeshUtility.JobMergeNearbyVertices jobMergeNearbyVertices = default(MeshUtility.JobMergeNearbyVertices);
			jobMergeNearbyVertices.vertices = list;
			jobMergeNearbyVertices.triangles = nativeList2;
			jobMergeNearbyVertices.mergeRadiusSq = 0;
			jobMergeNearbyVertices.Execute();
			MeshUtility.JobRemoveDegenerateTriangles jobRemoveDegenerateTriangles = default(MeshUtility.JobRemoveDegenerateTriangles);
			jobRemoveDegenerateTriangles.vertices = list;
			jobRemoveDegenerateTriangles.triangles = nativeList2;
			jobRemoveDegenerateTriangles.tags = nativeList;
			jobRemoveDegenerateTriangles.Execute();
			unsafePtr->verticesInTileSpace = list.AsUnsafeSpan<Int3>().Clone(Allocator.Persistent);
			unsafePtr->triangles = nativeList2.AsUnsafeSpan<int>().Clone(Allocator.Persistent);
			unsafePtr->tags = nativeList.AsUnsafeSpan<int>().Reinterpret<uint>().Clone(Allocator.Persistent);
			if (this.recalculateNormals)
			{
				MeshUtility.MakeTrianglesClockwise(ref unsafePtr->verticesInTileSpace, ref unsafePtr->triangles);
			}
			list.Dispose();
		}

		// Token: 0x040008D1 RID: 2257
		public NativeArray<Vector3> vertices;

		// Token: 0x040008D2 RID: 2258
		public NativeArray<int> indices;

		// Token: 0x040008D3 RID: 2259
		public Matrix4x4 meshToGraph;

		// Token: 0x040008D4 RID: 2260
		public NativeArray<TileMesh.TileMeshUnsafe> outputBuffers;

		// Token: 0x040008D5 RID: 2261
		public bool recalculateNormals;

		// Token: 0x020001E4 RID: 484
		[BurstCompile(FloatMode = FloatMode.Fast)]
		public struct JobTransformTileCoordinates : IJob
		{
			// Token: 0x06000C74 RID: 3188 RVA: 0x0004CEEC File Offset: 0x0004B0EC
			public void Execute()
			{
				if (this.vertices.Length != this.outputVertices.Length)
				{
					throw new ArgumentException("Input and output arrays must have the same length");
				}
				for (int i = 0; i < this.vertices.Length; i++)
				{
					this.outputVertices[i] = (Int3)this.matrix.MultiplyPoint3x4(this.vertices[i]);
				}
			}

			// Token: 0x040008D6 RID: 2262
			public NativeArray<Vector3> vertices;

			// Token: 0x040008D7 RID: 2263
			public NativeArray<Int3> outputVertices;

			// Token: 0x040008D8 RID: 2264
			public Matrix4x4 matrix;
		}
	}
}
