using System;
using Pathfinding.Collections;
using Unity.Collections;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001BF RID: 447
	public struct TileMesh
	{
		// Token: 0x04000834 RID: 2100
		public int[] triangles;

		// Token: 0x04000835 RID: 2101
		public Int3[] verticesInTileSpace;

		// Token: 0x04000836 RID: 2102
		public uint[] tags;

		// Token: 0x020001C0 RID: 448
		public struct TileMeshUnsafe
		{
			// Token: 0x06000BED RID: 3053 RVA: 0x00045CF1 File Offset: 0x00043EF1
			public void Dispose(Allocator allocator)
			{
				this.triangles.Free(allocator);
				this.verticesInTileSpace.Free(allocator);
				this.tags.Free(allocator);
			}

			// Token: 0x06000BEE RID: 3054 RVA: 0x00045D18 File Offset: 0x00043F18
			public TileMesh ToManaged()
			{
				return new TileMesh
				{
					triangles = this.triangles.ToArray(),
					verticesInTileSpace = this.verticesInTileSpace.ToArray(),
					tags = this.tags.ToArray()
				};
			}

			// Token: 0x04000837 RID: 2103
			public UnsafeSpan<int> triangles;

			// Token: 0x04000838 RID: 2104
			public UnsafeSpan<Int3> verticesInTileSpace;

			// Token: 0x04000839 RID: 2105
			public UnsafeSpan<uint> tags;
		}
	}
}
