using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000220 RID: 544
	[BurstCompile]
	public struct JobPrepareRaycasts : IJob
	{
		// Token: 0x06000D15 RID: 3349 RVA: 0x00052EF4 File Offset: 0x000510F4
		public void Execute()
		{
			Vector3 normalized = this.direction.normalized;
			UnsafeSpan<RaycastCommand> span = this.raycastCommands.AsUnsafeSpan<RaycastCommand>();
			QueryParameters queryParameters = new QueryParameters(this.mask, false, QueryTriggerInteraction.Ignore, false);
			RaycastCommand value = new RaycastCommand(this.physicsScene, Vector3.zero, normalized, queryParameters, this.distance);
			span.Fill(value);
			for (int i = 0; i < this.raycastCommands.Length; i++)
			{
				span[i].from = this.origins[i] + this.originOffset;
			}
		}

		// Token: 0x04000A01 RID: 2561
		public Vector3 direction;

		// Token: 0x04000A02 RID: 2562
		public Vector3 originOffset;

		// Token: 0x04000A03 RID: 2563
		public float distance;

		// Token: 0x04000A04 RID: 2564
		public LayerMask mask;

		// Token: 0x04000A05 RID: 2565
		public PhysicsScene physicsScene;

		// Token: 0x04000A06 RID: 2566
		[ReadOnly]
		public NativeArray<Vector3> origins;

		// Token: 0x04000A07 RID: 2567
		[WriteOnly]
		public NativeArray<RaycastCommand> raycastCommands;
	}
}
