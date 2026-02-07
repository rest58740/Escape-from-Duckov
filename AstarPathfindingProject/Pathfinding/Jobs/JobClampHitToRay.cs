using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Jobs
{
	// Token: 0x02000177 RID: 375
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobClampHitToRay : IJob
	{
		// Token: 0x06000ACC RID: 2764 RVA: 0x0003D120 File Offset: 0x0003B320
		public void Execute()
		{
			for (int i = 0; i < this.hits.Length; i++)
			{
				RaycastHit value = this.hits[i];
				value.point = VectorMath.ClosestPointOnLine(this.commands[i].origin, this.commands[i].origin + this.commands[i].direction, value.point);
				this.hits[i] = value;
			}
		}

		// Token: 0x04000744 RID: 1860
		[ReadOnly]
		public NativeArray<SpherecastCommand> commands;

		// Token: 0x04000745 RID: 1861
		public NativeArray<RaycastHit> hits;
	}
}
