using System;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002AC RID: 684
	public struct ArbitraryMovementPlane : IMovementPlaneWrapper
	{
		// Token: 0x06001035 RID: 4149 RVA: 0x000659BE File Offset: 0x00063BBE
		public float2 ToPlane(float3 p)
		{
			return this.plane.ToPlane(p);
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x000659CC File Offset: 0x00063BCC
		public float2 ToPlane(float3 p, out float elevation)
		{
			return this.plane.ToPlane(p, out elevation);
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x000659DB File Offset: 0x00063BDB
		public float3 ToWorld(float2 p, float elevation = 0f)
		{
			return this.plane.ToWorld(p, elevation);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x000659EA File Offset: 0x00063BEA
		public Bounds ToWorld(Bounds bounds)
		{
			return this.plane.ToWorld(bounds);
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x000659F8 File Offset: 0x00063BF8
		public void Set(NativeMovementPlane plane)
		{
			this.plane = plane;
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x00065A04 File Offset: 0x00063C04
		public float4x4 matrix
		{
			get
			{
				return math.mul(float4x4.TRS(0, this.plane.rotation, 1), new float4x4(new float4(1f, 0f, 0f, 0f), new float4(0f, 0f, 1f, 0f), new float4(0f, 1f, 0f, 0f), new float4(0f, 0f, 0f, 1f)));
			}
		}

		// Token: 0x04000C02 RID: 3074
		private NativeMovementPlane plane;
	}
}
