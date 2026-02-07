using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x0200021B RID: 539
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobNodeGridLayout : IJob, GridIterationUtilities.ICellAction
	{
		// Token: 0x06000D0E RID: 3342 RVA: 0x00052A70 File Offset: 0x00050C70
		public static Vector3 NodePosition(Matrix4x4 graphToWorld, int x, int z, float height = 0f)
		{
			return graphToWorld.MultiplyPoint3x4(new Vector3((float)x + 0.5f, height, (float)z + 0.5f));
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00052A8F File Offset: 0x00050C8F
		public void Execute()
		{
			GridIterationUtilities.ForEachCellIn3DArray<JobNodeGridLayout>(this.bounds.size, ref this);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00052AA4 File Offset: 0x00050CA4
		public void Execute(uint innerIndex, int x, int y, int z)
		{
			this.nodePositions[(int)innerIndex] = JobNodeGridLayout.NodePosition(this.graphToWorld, x + this.bounds.min.x, z + this.bounds.min.z, 0f);
		}

		// Token: 0x040009DF RID: 2527
		public Matrix4x4 graphToWorld;

		// Token: 0x040009E0 RID: 2528
		public IntBounds bounds;

		// Token: 0x040009E1 RID: 2529
		[WriteOnly]
		public NativeArray<Vector3> nodePositions;
	}
}
