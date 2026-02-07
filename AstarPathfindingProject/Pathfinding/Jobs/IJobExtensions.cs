using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x0200018D RID: 397
	public static class IJobExtensions
	{
		// Token: 0x06000B0A RID: 2826 RVA: 0x0003DF18 File Offset: 0x0003C118
		public static JobHandle Schedule<T>(this T data, JobDependencyTracker tracker) where T : struct, IJob
		{
			if (tracker.forceLinearDependencies)
			{
				data.Run<T>();
				return default(JobHandle);
			}
			JobHandle jobHandle = data.Schedule(JobDependencyAnalyzer<T>.GetDependencies(ref data, tracker));
			JobDependencyAnalyzer<T>.Scheduled(ref data, tracker, jobHandle);
			return jobHandle;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0003DF58 File Offset: 0x0003C158
		public static JobHandle ScheduleBatch<T>(this T data, int arrayLength, int minIndicesPerJobCount, JobDependencyTracker tracker, JobHandle additionalDependency = default(JobHandle)) where T : struct, IJobParallelForBatched
		{
			if (tracker.forceLinearDependencies)
			{
				additionalDependency.Complete();
				data.RunBatch(arrayLength);
				return default(JobHandle);
			}
			JobHandle jobHandle = data.ScheduleBatch(arrayLength, minIndicesPerJobCount, JobDependencyAnalyzer<T>.GetDependencies(ref data, tracker, additionalDependency));
			JobDependencyAnalyzer<T>.Scheduled(ref data, tracker, jobHandle);
			return jobHandle;
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0003DFA4 File Offset: 0x0003C1A4
		public static JobHandle ScheduleManaged<T>(this T data, JobHandle dependsOn) where T : struct, IJob
		{
			return new IJobExtensions.ManagedJob
			{
				handle = GCHandle.Alloc(data)
			}.Schedule(dependsOn);
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0003DFD4 File Offset: 0x0003C1D4
		public static JobHandle ScheduleManaged(this Action data, JobHandle dependsOn)
		{
			return new IJobExtensions.ManagedActionJob
			{
				handle = GCHandle.Alloc(data)
			}.Schedule(dependsOn);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0003E000 File Offset: 0x0003C200
		public static JobHandle GetDependencies<T>(this T data, JobDependencyTracker tracker) where T : struct, IJob
		{
			if (tracker.forceLinearDependencies)
			{
				return default(JobHandle);
			}
			return JobDependencyAnalyzer<T>.GetDependencies(ref data, tracker);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0003E027 File Offset: 0x0003C227
		public static IEnumerator<JobHandle> ExecuteMainThreadJob<T>(this T data, JobDependencyTracker tracker) where T : struct, IJobTimeSliced
		{
			if (tracker.forceLinearDependencies)
			{
				data.Execute();
				yield break;
			}
			JobHandle dependencies = JobDependencyAnalyzer<T>.GetDependencies(ref data, tracker);
			yield return dependencies;
			while (!data.Execute(tracker.timeSlice))
			{
				JobHandle jobHandle = default(JobHandle);
			}
			yield break;
			yield break;
		}

		// Token: 0x0200018E RID: 398
		private struct ManagedJob : IJob
		{
			// Token: 0x06000B10 RID: 2832 RVA: 0x0003E03D File Offset: 0x0003C23D
			public void Execute()
			{
				((IJob)this.handle.Target).Execute();
				this.handle.Free();
			}

			// Token: 0x04000776 RID: 1910
			public GCHandle handle;
		}

		// Token: 0x0200018F RID: 399
		private struct ManagedActionJob : IJob
		{
			// Token: 0x06000B11 RID: 2833 RVA: 0x0003E05F File Offset: 0x0003C25F
			public void Execute()
			{
				((Action)this.handle.Target)();
				this.handle.Free();
			}

			// Token: 0x04000777 RID: 1911
			public GCHandle handle;
		}
	}
}
