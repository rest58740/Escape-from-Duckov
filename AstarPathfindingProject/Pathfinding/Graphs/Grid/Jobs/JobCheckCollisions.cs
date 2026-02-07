using System;
using Pathfinding.Jobs;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000215 RID: 533
	internal struct JobCheckCollisions : IJobTimeSliced, IJob
	{
		// Token: 0x06000D05 RID: 3333 RVA: 0x00051DD5 File Offset: 0x0004FFD5
		public void Execute()
		{
			this.Execute(TimeSlice.Infinite);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00051DE4 File Offset: 0x0004FFE4
		public bool Execute(TimeSlice timeSlice)
		{
			for (int i = this.startIndex; i < this.nodePositions.Length; i++)
			{
				this.collisionResult[i] = (this.collisionResult[i] && this.collision.Check(this.nodePositions[i]));
				if ((i & 127) == 0 && timeSlice.expired)
				{
					this.startIndex = i + 1;
					return false;
				}
			}
			return true;
		}

		// Token: 0x040009C2 RID: 2498
		[ReadOnly]
		public NativeArray<Vector3> nodePositions;

		// Token: 0x040009C3 RID: 2499
		public NativeArray<bool> collisionResult;

		// Token: 0x040009C4 RID: 2500
		public GraphCollision collision;

		// Token: 0x040009C5 RID: 2501
		private int startIndex;
	}
}
