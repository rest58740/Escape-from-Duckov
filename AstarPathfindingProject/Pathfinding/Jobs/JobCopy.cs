using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x02000172 RID: 370
	[BurstCompile]
	public struct JobCopy<T> : IJob where T : struct
	{
		// Token: 0x06000AC7 RID: 2759 RVA: 0x0003D01D File Offset: 0x0003B21D
		public void Execute()
		{
			this.from.CopyTo(this.to);
		}

		// Token: 0x04000738 RID: 1848
		[ReadOnly]
		public NativeArray<T> from;

		// Token: 0x04000739 RID: 1849
		[WriteOnly]
		public NativeArray<T> to;
	}
}
