using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000221 RID: 545
	[BurstCompile]
	public struct JobPrepareSphereCommands : IJob
	{
		// Token: 0x06000D16 RID: 3350 RVA: 0x00052F90 File Offset: 0x00051190
		public void Execute()
		{
			UnsafeSpan<OverlapSphereCommand> span = this.commands.AsUnsafeSpan<OverlapSphereCommand>();
			QueryParameters queryParameters = new QueryParameters(this.mask, false, QueryTriggerInteraction.Ignore, false);
			span.Fill(new OverlapSphereCommand(this.physicsScene, Vector3.zero, this.radius, queryParameters));
			for (int i = 0; i < span.Length; i++)
			{
				Vector3 point = this.origins[i] + this.originOffset;
				span[i].point = point;
			}
		}

		// Token: 0x04000A08 RID: 2568
		public Vector3 originOffset;

		// Token: 0x04000A09 RID: 2569
		public float radius;

		// Token: 0x04000A0A RID: 2570
		public LayerMask mask;

		// Token: 0x04000A0B RID: 2571
		public PhysicsScene physicsScene;

		// Token: 0x04000A0C RID: 2572
		[ReadOnly]
		public NativeArray<Vector3> origins;

		// Token: 0x04000A0D RID: 2573
		[WriteOnly]
		public NativeArray<OverlapSphereCommand> commands;
	}
}
