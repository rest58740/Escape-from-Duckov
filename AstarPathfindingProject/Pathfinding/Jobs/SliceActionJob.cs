using System;
using Pathfinding.Graphs.Grid;
using Unity.Burst;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x02000174 RID: 372
	[BurstCompile]
	public struct SliceActionJob<T> : IJob where T : struct, GridIterationUtilities.ISliceAction
	{
		// Token: 0x06000AC9 RID: 2761 RVA: 0x0003D061 File Offset: 0x0003B261
		public void Execute()
		{
			GridIterationUtilities.ForEachCellIn3DSlice<T>(this.slice, ref this.action);
		}

		// Token: 0x0400073C RID: 1852
		public T action;

		// Token: 0x0400073D RID: 1853
		public Slice3D slice;
	}
}
