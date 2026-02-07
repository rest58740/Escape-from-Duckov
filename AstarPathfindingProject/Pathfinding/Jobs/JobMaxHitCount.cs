using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Jobs
{
	// Token: 0x02000176 RID: 374
	[BurstCompile]
	public struct JobMaxHitCount : IJob
	{
		// Token: 0x06000ACB RID: 2763 RVA: 0x0003D0A8 File Offset: 0x0003B2A8
		public void Execute()
		{
			int i;
			for (i = 0; i < this.maxHits; i++)
			{
				int num = i * this.layerStride;
				bool flag = false;
				for (int j = num; j < num + this.layerStride; j++)
				{
					if (math.any(this.hits[j].normal))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			this.maxHitCount[0] = math.max(1, i);
		}

		// Token: 0x04000740 RID: 1856
		[ReadOnly]
		public NativeArray<RaycastHit> hits;

		// Token: 0x04000741 RID: 1857
		public int maxHits;

		// Token: 0x04000742 RID: 1858
		public int layerStride;

		// Token: 0x04000743 RID: 1859
		[WriteOnly]
		public NativeArray<int> maxHitCount;
	}
}
