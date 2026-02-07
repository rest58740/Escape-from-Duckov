using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x0200021D RID: 541
	[BurstCompile]
	public struct JobPrepareCapsuleCommands : IJob
	{
		// Token: 0x06000D12 RID: 3346 RVA: 0x00052C44 File Offset: 0x00050E44
		public void Execute()
		{
			UnsafeSpan<OverlapCapsuleCommand> span = this.commands.AsUnsafeSpan<OverlapCapsuleCommand>();
			QueryParameters queryParameters = new QueryParameters(this.mask, false, QueryTriggerInteraction.Ignore, false);
			span.Fill(new OverlapCapsuleCommand(this.physicsScene, Vector3.zero, Vector3.zero, this.radius, queryParameters));
			for (int i = 0; i < span.Length; i++)
			{
				Vector3 vector = this.origins[i] + this.originOffset;
				span[i].point0 = vector;
				span[i].point1 = vector + this.direction;
			}
		}

		// Token: 0x040009EB RID: 2539
		public Vector3 direction;

		// Token: 0x040009EC RID: 2540
		public Vector3 originOffset;

		// Token: 0x040009ED RID: 2541
		public float radius;

		// Token: 0x040009EE RID: 2542
		public LayerMask mask;

		// Token: 0x040009EF RID: 2543
		public PhysicsScene physicsScene;

		// Token: 0x040009F0 RID: 2544
		[ReadOnly]
		public NativeArray<Vector3> origins;

		// Token: 0x040009F1 RID: 2545
		[WriteOnly]
		public NativeArray<OverlapCapsuleCommand> commands;
	}
}
