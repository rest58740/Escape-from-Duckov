using System;
using Pathfinding.Graphs.Grid;
using Unity.Collections;

namespace Pathfinding.Jobs
{
	// Token: 0x02000175 RID: 373
	public struct JobAND : GridIterationUtilities.ISliceAction
	{
		// Token: 0x06000ACA RID: 2762 RVA: 0x0003D074 File Offset: 0x0003B274
		public void Execute(uint outerIdx, uint innerIdx)
		{
			ref NativeArray<bool> ptr = ref this.result;
			ptr[(int)outerIdx] = (ptr[(int)outerIdx] & this.data[(int)outerIdx]);
		}

		// Token: 0x0400073E RID: 1854
		public NativeArray<bool> result;

		// Token: 0x0400073F RID: 1855
		[ReadOnly]
		public NativeArray<bool> data;
	}
}
