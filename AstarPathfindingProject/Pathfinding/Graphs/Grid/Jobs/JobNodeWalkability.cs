using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x0200021C RID: 540
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobNodeWalkability : IJob
	{
		// Token: 0x06000D11 RID: 3345 RVA: 0x00052AF4 File Offset: 0x00050CF4
		public void Execute()
		{
			float num = math.cos(math.radians(this.maxSlope));
			float4 @float = new float4(this.up.x, this.up.y, this.up.z, 0f);
			float3 xyz = @float.xyz;
			for (int i = 0; i < this.nodeNormals.Length; i++)
			{
				bool flag = math.any(this.nodeNormals[i]);
				bool flag2 = flag;
				if (!flag && !this.unwalkableWhenNoGround && i < this.layerStride)
				{
					flag2 = true;
					this.nodeNormals[i] = @float;
				}
				if (flag2 && this.useRaycastNormal && flag && math.dot(this.nodeNormals[i], @float) < num)
				{
					flag2 = false;
				}
				if (flag2 && i + this.layerStride < this.nodeNormals.Length && math.any(this.nodeNormals[i + this.layerStride]))
				{
					flag2 = (math.dot(xyz, this.nodePositions[i + this.layerStride] - this.nodePositions[i]) >= this.characterHeight);
				}
				this.nodeWalkable[i] = flag2;
			}
		}

		// Token: 0x040009E2 RID: 2530
		public bool useRaycastNormal;

		// Token: 0x040009E3 RID: 2531
		public float maxSlope;

		// Token: 0x040009E4 RID: 2532
		public Vector3 up;

		// Token: 0x040009E5 RID: 2533
		public bool unwalkableWhenNoGround;

		// Token: 0x040009E6 RID: 2534
		public float characterHeight;

		// Token: 0x040009E7 RID: 2535
		public int layerStride;

		// Token: 0x040009E8 RID: 2536
		[ReadOnly]
		public NativeArray<float3> nodePositions;

		// Token: 0x040009E9 RID: 2537
		public NativeArray<float4> nodeNormals;

		// Token: 0x040009EA RID: 2538
		[WriteOnly]
		public NativeArray<bool> nodeWalkable;
	}
}
