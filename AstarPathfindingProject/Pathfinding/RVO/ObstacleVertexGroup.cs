using System;
using Unity.Mathematics;

namespace Pathfinding.RVO
{
	// Token: 0x020002B0 RID: 688
	public struct ObstacleVertexGroup
	{
		// Token: 0x04000C0F RID: 3087
		public ObstacleType type;

		// Token: 0x04000C10 RID: 3088
		public int vertexCount;

		// Token: 0x04000C11 RID: 3089
		public float3 boundsMn;

		// Token: 0x04000C12 RID: 3090
		public float3 boundsMx;
	}
}
