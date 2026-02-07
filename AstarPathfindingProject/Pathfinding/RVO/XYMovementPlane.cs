using System;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002AA RID: 682
	public struct XYMovementPlane : IMovementPlaneWrapper
	{
		// Token: 0x06001029 RID: 4137 RVA: 0x000658F7 File Offset: 0x00063AF7
		public float2 ToPlane(float3 p)
		{
			return p.xy;
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00065900 File Offset: 0x00063B00
		public float2 ToPlane(float3 p, out float elevation)
		{
			elevation = p.z;
			return p.xy;
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00065911 File Offset: 0x00063B11
		public float3 ToWorld(float2 p, float elevation = 0f)
		{
			return new float3(p.x, p.y, elevation);
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x00065928 File Offset: 0x00063B28
		public Bounds ToWorld(Bounds bounds)
		{
			Vector3 center = bounds.center;
			Vector3 size = bounds.size;
			return new Bounds(new Vector3(center.x, center.z, center.y), new Vector3(size.x, size.z, size.y));
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x00065978 File Offset: 0x00063B78
		public float4x4 matrix
		{
			get
			{
				return float4x4.identity;
			}
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x000035CE File Offset: 0x000017CE
		public void Set(NativeMovementPlane plane)
		{
		}
	}
}
