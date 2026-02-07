using System;
using Unity.Collections;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001C2 RID: 450
	public struct TileMeshesUnsafe
	{
		// Token: 0x06000BF2 RID: 3058 RVA: 0x0004630C File Offset: 0x0004450C
		public TileMeshesUnsafe(NativeArray<TileMesh.TileMeshUnsafe> tileMeshes, IntRect tileRect, Vector2 tileWorldSize)
		{
			this.tileMeshes = tileMeshes;
			this.tileRect = tileRect;
			this.tileWorldSize = tileWorldSize;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00046324 File Offset: 0x00044524
		public TileMeshes ToManaged()
		{
			TileMesh[] array = new TileMesh[this.tileMeshes.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.tileMeshes[i].ToManaged();
			}
			return new TileMeshes
			{
				tileMeshes = array,
				tileRect = this.tileRect,
				tileWorldSize = this.tileWorldSize
			};
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00046398 File Offset: 0x00044598
		public void Dispose(Allocator allocator)
		{
			if (!this.tileMeshes.IsCreated)
			{
				return;
			}
			for (int i = 0; i < this.tileMeshes.Length; i++)
			{
				this.tileMeshes[i].Dispose(allocator);
			}
			this.tileMeshes.Dispose();
		}

		// Token: 0x0400083D RID: 2109
		public NativeArray<TileMesh.TileMeshUnsafe> tileMeshes;

		// Token: 0x0400083E RID: 2110
		public IntRect tileRect;

		// Token: 0x0400083F RID: 2111
		public Vector2 tileWorldSize;
	}
}
