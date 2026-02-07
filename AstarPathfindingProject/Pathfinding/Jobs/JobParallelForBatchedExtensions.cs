using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace Pathfinding.Jobs
{
	// Token: 0x0200017B RID: 379
	internal static class JobParallelForBatchedExtensions
	{
		// Token: 0x06000AD2 RID: 2770 RVA: 0x0003D470 File Offset: 0x0003B670
		public static JobHandle ScheduleBatch<T>(this T jobData, int arrayLength, int minIndicesPerJobCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelForBatched
		{
			ScheduleMode i_scheduleMode = ScheduleMode.Batched;
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.Initialize(), dependsOn, i_scheduleMode);
			return JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, minIndicesPerJobCount);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0003D4A0 File Offset: 0x0003B6A0
		public static void RunBatch<T>(this T jobData, int arrayLength) where T : struct, IJobParallelForBatched
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.Initialize(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, arrayLength);
		}

		// Token: 0x0200017C RID: 380
		internal struct ParallelForBatchJobStruct<T> where T : struct, IJobParallelForBatched
		{
			// Token: 0x06000AD4 RID: 2772 RVA: 0x0003D4D4 File Offset: 0x0003B6D4
			public static IntPtr Initialize()
			{
				if (JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.jobReflectionData == IntPtr.Zero)
				{
					JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.jobReflectionData = JobsUtility.CreateJobReflectionData(typeof(T), new JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.ExecuteJobFunction(JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.Execute), null, null);
				}
				return JobParallelForBatchedExtensions.ParallelForBatchJobStruct<T>.jobReflectionData;
			}

			// Token: 0x06000AD5 RID: 2773 RVA: 0x0003D510 File Offset: 0x0003B710
			public static void Execute(ref T jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
			{
				int num;
				int num2;
				while (JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out num, out num2))
				{
					jobData.Execute(num, num2 - num);
				}
			}

			// Token: 0x0400074E RID: 1870
			public static IntPtr jobReflectionData;

			// Token: 0x0200017D RID: 381
			// (Invoke) Token: 0x06000AD7 RID: 2775
			public delegate void ExecuteJobFunction(ref T data, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);
		}
	}
}
