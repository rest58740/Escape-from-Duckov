using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x0200021A RID: 538
	[BurstCompile]
	public struct JobMergeRaycastCollisionHits : IJob
	{
		// Token: 0x06000D0D RID: 3341 RVA: 0x00052A00 File Offset: 0x00050C00
		public void Execute()
		{
			for (int i = 0; i < this.hit1.Length; i++)
			{
				this.result[i] = (this.hit1[i].normal == Vector3.zero && this.hit2[i].normal == Vector3.zero);
			}
		}

		// Token: 0x040009DC RID: 2524
		[ReadOnly]
		public NativeArray<RaycastHit> hit1;

		// Token: 0x040009DD RID: 2525
		[ReadOnly]
		public NativeArray<RaycastHit> hit2;

		// Token: 0x040009DE RID: 2526
		[WriteOnly]
		public NativeArray<bool> result;
	}
}
