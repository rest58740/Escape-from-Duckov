using System;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002A9 RID: 681
	public interface IMovementPlaneWrapper
	{
		// Token: 0x06001023 RID: 4131
		float2 ToPlane(float3 p);

		// Token: 0x06001024 RID: 4132
		float2 ToPlane(float3 p, out float elevation);

		// Token: 0x06001025 RID: 4133
		float3 ToWorld(float2 p, float elevation = 0f);

		// Token: 0x06001026 RID: 4134
		Bounds ToWorld(Bounds bounds);

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06001027 RID: 4135
		float4x4 matrix { get; }

		// Token: 0x06001028 RID: 4136
		void Set(NativeMovementPlane plane);
	}
}
