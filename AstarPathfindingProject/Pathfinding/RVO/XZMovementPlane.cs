using System;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002AB RID: 683
	public struct XZMovementPlane : IMovementPlaneWrapper
	{
		// Token: 0x0600102F RID: 4143 RVA: 0x0006597F File Offset: 0x00063B7F
		public float2 ToPlane(float3 p)
		{
			return p.xz;
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00065988 File Offset: 0x00063B88
		public float2 ToPlane(float3 p, out float elevation)
		{
			elevation = p.y;
			return p.xz;
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x00065999 File Offset: 0x00063B99
		public float3 ToWorld(float2 p, float elevation = 0f)
		{
			return new float3(p.x, elevation, p.y);
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00025E1F File Offset: 0x0002401F
		public Bounds ToWorld(Bounds bounds)
		{
			return bounds;
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x000035CE File Offset: 0x000017CE
		public void Set(NativeMovementPlane plane)
		{
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x000659AD File Offset: 0x00063BAD
		public float4x4 matrix
		{
			get
			{
				return float4x4.RotateX(math.radians(90f));
			}
		}
	}
}
