using System;
using Unity.Mathematics;

namespace Pathfinding.Util
{
	// Token: 0x02000286 RID: 646
	public readonly struct ToPlaneMatrix
	{
		// Token: 0x06000F5C RID: 3932 RVA: 0x0005EE16 File Offset: 0x0005D016
		public ToPlaneMatrix(NativeMovementPlane plane)
		{
			this.matrix = new float3x3(math.conjugate(plane.rotation));
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0005EE30 File Offset: 0x0005D030
		public float2 ToPlane(float3 p)
		{
			return math.mul(this.matrix, p).xz;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0005EE51 File Offset: 0x0005D051
		public float3 ToXZPlane(float3 p)
		{
			return math.mul(this.matrix, p);
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0005EE60 File Offset: 0x0005D060
		public float2 ToPlane(float3 p, out float elevation)
		{
			float3 @float = math.mul(this.matrix, p);
			elevation = @float.y;
			return @float.xz;
		}

		// Token: 0x04000B5F RID: 2911
		public readonly float3x3 matrix;
	}
}
