using System;
using System.Collections.Generic;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x02000181 RID: 385
	public struct JobHandleWithMainThreadWork<T> where T : struct
	{
		// Token: 0x06000AE4 RID: 2788 RVA: 0x0003D779 File Offset: 0x0003B979
		public JobHandleWithMainThreadWork(IEnumerator<ValueTuple<JobHandle, T>> handles, JobDependencyTracker tracker)
		{
			this.coroutine = handles;
			this.tracker = tracker;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0003D78C File Offset: 0x0003B98C
		public void Complete()
		{
			this.tracker.timeSlice = TimeSlice.Infinite;
			while (this.coroutine.MoveNext())
			{
				ValueTuple<JobHandle, T> valueTuple = this.coroutine.Current;
				valueTuple.Item1.Complete();
			}
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0003D7D0 File Offset: 0x0003B9D0
		public IEnumerable<T?> CompleteTimeSliced(float maxMillisPerStep)
		{
			this.tracker.timeSlice = TimeSlice.MillisFromNow(maxMillisPerStep);
			while (this.coroutine.MoveNext())
			{
				ValueTuple<JobHandle, T> valueTuple;
				if (maxMillisPerStep < float.PositiveInfinity)
				{
					for (;;)
					{
						valueTuple = this.coroutine.Current;
						if (valueTuple.Item1.IsCompleted)
						{
							break;
						}
						T? t = null;
						this.tracker.timeSlice = TimeSlice.MillisFromNow(maxMillisPerStep);
					}
				}
				valueTuple = this.coroutine.Current;
				valueTuple.Item1.Complete();
				yield return new T?(this.coroutine.Current.Item2);
				this.tracker.timeSlice = TimeSlice.MillisFromNow(maxMillisPerStep);
			}
			yield break;
			yield break;
		}

		// Token: 0x04000753 RID: 1875
		private JobDependencyTracker tracker;

		// Token: 0x04000754 RID: 1876
		private IEnumerator<ValueTuple<JobHandle, T>> coroutine;
	}
}
