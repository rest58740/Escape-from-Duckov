using System;
using Pathfinding.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001D6 RID: 470
	public struct RasterizationMesh
	{
		// Token: 0x0400088B RID: 2187
		public UnsafeSpan<float3> vertices;

		// Token: 0x0400088C RID: 2188
		public UnsafeSpan<int> triangles;

		// Token: 0x0400088D RID: 2189
		public int area;

		// Token: 0x0400088E RID: 2190
		public Bounds bounds;

		// Token: 0x0400088F RID: 2191
		public Matrix4x4 matrix;

		// Token: 0x04000890 RID: 2192
		public bool solid;

		// Token: 0x04000891 RID: 2193
		public bool doubleSided;

		// Token: 0x04000892 RID: 2194
		public bool areaIsTag;

		// Token: 0x04000893 RID: 2195
		public bool flatten;
	}
}
