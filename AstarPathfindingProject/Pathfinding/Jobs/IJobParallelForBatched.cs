using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace Pathfinding.Jobs
{
	// Token: 0x0200017A RID: 378
	[JobProducerType(typeof(JobParallelForBatchedExtensions.ParallelForBatchJobStruct<>))]
	public interface IJobParallelForBatched
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000AD0 RID: 2768
		bool allowBoundsChecks { get; }

		// Token: 0x06000AD1 RID: 2769
		void Execute(int startIndex, int count);
	}
}
