using System;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001EC RID: 492
	public struct JobCreateTiles : IJob
	{
		// Token: 0x06000C82 RID: 3202 RVA: 0x0004DD00 File Offset: 0x0004BF00
		public unsafe void Execute()
		{
			NavmeshTile[] array = (NavmeshTile[])this.tiles.Target;
			int width = this.tileRect.Width;
			int height = this.tileRect.Height;
			bool isCreated = this.preCutTileMeshes.IsCreated;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int num = i * width + j;
					int tileIndex = (i + this.tileRect.ymin) * this.graphTileCount.x + (j + this.tileRect.xmin);
					TileMesh.TileMeshUnsafe tileMeshUnsafe = this.tileMeshes[num];
					UnsafeSpan<Int3> unsafeSpan = tileMeshUnsafe.verticesInTileSpace.Clone(Allocator.Persistent);
					UnsafeSpan<Int3> verts = unsafeSpan.Clone(Allocator.Persistent);
					Int3 rhs = (Int3)new Vector3(this.tileWorldSize.x * (float)(j + this.tileRect.xmin), 0f, this.tileWorldSize.y * (float)(i + this.tileRect.ymin));
					for (int k = 0; k < unsafeSpan.Length; k++)
					{
						Int3 @int = *unsafeSpan[k] + rhs;
						*unsafeSpan[k] = @int;
						*verts[k] = (Int3)this.graphToWorldSpace.MultiplyPoint3x4((Vector3)@int);
					}
					UnsafeSpan<int> unsafeSpan2 = tileMeshUnsafe.triangles.Clone(Allocator.Persistent);
					NavmeshTile navmeshTile = new NavmeshTile
					{
						x = j + this.tileRect.xmin,
						z = i + this.tileRect.ymin,
						w = 1,
						d = 1,
						tris = unsafeSpan2,
						vertsInGraphSpace = unsafeSpan,
						verts = verts,
						bbTree = new BBTree(unsafeSpan2, unsafeSpan),
						nodes = new TriangleMeshNode[unsafeSpan2.Length / 3],
						graph = null,
						isCut = false
					};
					if (isCreated)
					{
						TileMesh.TileMeshUnsafe tileMeshUnsafe2 = this.preCutTileMeshes[num];
						navmeshTile.preCutVertsInTileSpace = tileMeshUnsafe2.verticesInTileSpace.Clone(Allocator.Persistent);
						navmeshTile.preCutTris = tileMeshUnsafe2.triangles.Clone(Allocator.Persistent);
						navmeshTile.preCutTags = tileMeshUnsafe2.tags.Clone(Allocator.Persistent);
						navmeshTile.isCut = true;
					}
					NavmeshBase.CreateNodes(navmeshTile, navmeshTile.tris, tileIndex, this.graphIndex, tileMeshUnsafe.tags, false, null, this.initialPenalty, false);
					array[num] = navmeshTile;
				}
			}
		}

		// Token: 0x04000918 RID: 2328
		[ReadOnly]
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<TileMesh.TileMeshUnsafe> preCutTileMeshes;

		// Token: 0x04000919 RID: 2329
		[ReadOnly]
		public NativeArray<TileMesh.TileMeshUnsafe> tileMeshes;

		// Token: 0x0400091A RID: 2330
		public GCHandle tiles;

		// Token: 0x0400091B RID: 2331
		public uint graphIndex;

		// Token: 0x0400091C RID: 2332
		public Vector2Int graphTileCount;

		// Token: 0x0400091D RID: 2333
		public IntRect tileRect;

		// Token: 0x0400091E RID: 2334
		public uint initialPenalty;

		// Token: 0x0400091F RID: 2335
		public bool recalculateNormals;

		// Token: 0x04000920 RID: 2336
		public Vector2 tileWorldSize;

		// Token: 0x04000921 RID: 2337
		public Matrix4x4 graphToWorldSpace;
	}
}
