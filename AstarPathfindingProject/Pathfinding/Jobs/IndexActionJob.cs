using System;
using Pathfinding.Graphs.Grid;
using Unity.Burst;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x02000173 RID: 371
	[BurstCompile]
	public struct IndexActionJob<T> : IJob where T : struct, GridIterationUtilities.ISliceAction
	{
		// Token: 0x06000AC8 RID: 2760 RVA: 0x0003D030 File Offset: 0x0003B230
		public void Execute()
		{
			for (int i = 0; i < this.length; i++)
			{
				this.action.Execute((uint)i, (uint)i);
			}
		}

		// Token: 0x0400073A RID: 1850
		public T action;

		// Token: 0x0400073B RID: 1851
		public int length;
	}
}
