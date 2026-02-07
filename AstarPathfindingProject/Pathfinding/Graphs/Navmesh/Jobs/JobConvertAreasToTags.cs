using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001EB RID: 491
	[BurstCompile]
	public struct JobConvertAreasToTags : IJob
	{
		// Token: 0x06000C81 RID: 3201 RVA: 0x0004DCAC File Offset: 0x0004BEAC
		public void Execute()
		{
			for (int i = 0; i < this.areas.Length; i++)
			{
				int num = this.areas[i];
				this.areas[i] = (((num & 16384) != 0) ? ((num & 16383) - 1) : 0);
			}
		}

		// Token: 0x04000917 RID: 2327
		public NativeList<int> areas;
	}
}
