using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x0200021E RID: 542
	[BurstCompile]
	public struct JobPrepareGridRaycast : IJob
	{
		// Token: 0x06000D13 RID: 3347 RVA: 0x00052CE8 File Offset: 0x00050EE8
		public void Execute()
		{
			float magnitude = this.raycastDirection.magnitude;
			int3 size = this.bounds.size;
			Vector3 normalized = this.raycastDirection.normalized;
			UnsafeSpan<RaycastCommand> span = this.raycastCommands.AsUnsafeSpan<RaycastCommand>();
			QueryParameters queryParameters = new QueryParameters(this.raycastMask, false, QueryTriggerInteraction.Ignore, false);
			span.Fill(new RaycastCommand(this.physicsScene, Vector3.zero, normalized, queryParameters, magnitude));
			for (int i = 0; i < size.z; i++)
			{
				int num = i * size.x;
				for (int j = 0; j < size.x; j++)
				{
					int index = num + j;
					Vector3 a = JobNodeGridLayout.NodePosition(this.graphToWorld, j + this.bounds.min.x, i + this.bounds.min.z, 0f);
					span[index].from = a + this.raycastOffset;
				}
			}
		}

		// Token: 0x040009F2 RID: 2546
		public Matrix4x4 graphToWorld;

		// Token: 0x040009F3 RID: 2547
		public IntBounds bounds;

		// Token: 0x040009F4 RID: 2548
		public Vector3 raycastOffset;

		// Token: 0x040009F5 RID: 2549
		public Vector3 raycastDirection;

		// Token: 0x040009F6 RID: 2550
		public LayerMask raycastMask;

		// Token: 0x040009F7 RID: 2551
		public PhysicsScene physicsScene;

		// Token: 0x040009F8 RID: 2552
		[WriteOnly]
		public NativeArray<RaycastCommand> raycastCommands;
	}
}
