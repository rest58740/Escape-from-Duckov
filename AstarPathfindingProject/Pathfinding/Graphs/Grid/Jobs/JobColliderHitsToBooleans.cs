using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000216 RID: 534
	[BurstCompile]
	public struct JobColliderHitsToBooleans : IJob
	{
		// Token: 0x06000D07 RID: 3335 RVA: 0x00051E5C File Offset: 0x0005005C
		public void Execute()
		{
			for (int i = 0; i < this.hits.Length; i++)
			{
				this.result[i] = (this.hits[i].instanceID == 0);
			}
		}

		// Token: 0x040009C6 RID: 2502
		[ReadOnly]
		public NativeArray<ColliderHit> hits;

		// Token: 0x040009C7 RID: 2503
		[WriteOnly]
		public NativeArray<bool> result;
	}
}
