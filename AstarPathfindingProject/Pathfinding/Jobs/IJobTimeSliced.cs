using System;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x0200018C RID: 396
	public interface IJobTimeSliced : IJob
	{
		// Token: 0x06000B09 RID: 2825
		bool Execute(TimeSlice timeSlice);
	}
}
