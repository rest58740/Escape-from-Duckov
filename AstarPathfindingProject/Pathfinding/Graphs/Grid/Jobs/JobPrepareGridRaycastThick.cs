using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x0200021F RID: 543
	[BurstCompile]
	public struct JobPrepareGridRaycastThick : IJob
	{
		// Token: 0x06000D14 RID: 3348 RVA: 0x00052DEC File Offset: 0x00050FEC
		public void Execute()
		{
			float magnitude = this.raycastDirection.magnitude;
			int3 size = this.bounds.size;
			Vector3 normalized = this.raycastDirection.normalized;
			UnsafeSpan<SpherecastCommand> span = this.raycastCommands.AsUnsafeSpan<SpherecastCommand>();
			QueryParameters queryParameters = new QueryParameters(this.raycastMask, false, QueryTriggerInteraction.Ignore, false);
			span.Fill(new SpherecastCommand(this.physicsScene, Vector3.zero, this.radius, normalized, queryParameters, magnitude));
			for (int i = 0; i < size.z; i++)
			{
				int num = i * size.x;
				for (int j = 0; j < size.x; j++)
				{
					int index = num + j;
					Vector3 a = JobNodeGridLayout.NodePosition(this.graphToWorld, j + this.bounds.min.x, i + this.bounds.min.z, 0f);
					span[index].origin = a + this.raycastOffset;
				}
			}
		}

		// Token: 0x040009F9 RID: 2553
		public Matrix4x4 graphToWorld;

		// Token: 0x040009FA RID: 2554
		public IntBounds bounds;

		// Token: 0x040009FB RID: 2555
		public Vector3 raycastOffset;

		// Token: 0x040009FC RID: 2556
		public Vector3 raycastDirection;

		// Token: 0x040009FD RID: 2557
		public LayerMask raycastMask;

		// Token: 0x040009FE RID: 2558
		public PhysicsScene physicsScene;

		// Token: 0x040009FF RID: 2559
		public float radius;

		// Token: 0x04000A00 RID: 2560
		[WriteOnly]
		public NativeArray<SpherecastCommand> raycastCommands;
	}
}
