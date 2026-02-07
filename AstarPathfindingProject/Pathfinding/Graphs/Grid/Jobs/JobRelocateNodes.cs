using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000224 RID: 548
	[BurstCompile]
	public struct JobRelocateNodes : IJob, GridIterationUtilities.ICellAction
	{
		// Token: 0x06000D1A RID: 3354 RVA: 0x000531AC File Offset: 0x000513AC
		public void Execute()
		{
			GridIterationUtilities.ForEachCellIn3DArray<JobRelocateNodes>(this.bounds.size, ref this);
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x000531C0 File Offset: 0x000513C0
		public void Execute(uint innerIndex, int x, int y, int z)
		{
			float y2 = this.previousWorldToGraph.MultiplyPoint3x4(this.positions[(int)innerIndex]).y;
			this.positions[(int)innerIndex] = JobNodeGridLayout.NodePosition(this.graphToWorld, x, z, y2);
		}

		// Token: 0x04000A1E RID: 2590
		public Matrix4x4 previousWorldToGraph;

		// Token: 0x04000A1F RID: 2591
		public Matrix4x4 graphToWorld;

		// Token: 0x04000A20 RID: 2592
		public NativeArray<Vector3> positions;

		// Token: 0x04000A21 RID: 2593
		public IntBounds bounds;
	}
}
