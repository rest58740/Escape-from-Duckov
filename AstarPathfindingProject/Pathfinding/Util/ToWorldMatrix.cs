using System;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x02000287 RID: 647
	public readonly struct ToWorldMatrix
	{
		// Token: 0x06000F60 RID: 3936 RVA: 0x0005EE89 File Offset: 0x0005D089
		public ToWorldMatrix(NativeMovementPlane plane)
		{
			this.matrix = new float3x3(plane.rotation);
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0005EE9C File Offset: 0x0005D09C
		public ToWorldMatrix(float3x3 matrix)
		{
			this.matrix = matrix;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0005EEA5 File Offset: 0x0005D0A5
		public float3 ToWorld(float2 p, float elevation = 0f)
		{
			return math.mul(this.matrix, new float3(p.x, elevation, p.y));
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0005EEC4 File Offset: 0x0005D0C4
		public Bounds ToWorld(Bounds bounds)
		{
			return new Bounds
			{
				center = math.mul(this.matrix, bounds.center),
				extents = math.mul(new float3x3(math.abs(this.matrix.c0), math.abs(this.matrix.c1), math.abs(this.matrix.c2)), bounds.extents)
			};
		}

		// Token: 0x04000B60 RID: 2912
		public readonly float3x3 matrix;
	}
}
