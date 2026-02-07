using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000B8 RID: 184
	internal class PathReturnQueue
	{
		// Token: 0x060005CC RID: 1484 RVA: 0x0001C0FE File Offset: 0x0001A2FE
		public PathReturnQueue(object pathsClaimedSilentlyBy, Action OnReturnedPaths)
		{
			this.pathsClaimedSilentlyBy = pathsClaimedSilentlyBy;
			this.OnReturnedPaths = OnReturnedPaths;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001C120 File Offset: 0x0001A320
		public void Enqueue(Path path)
		{
			Queue<Path> obj = this.pathReturnQueue;
			lock (obj)
			{
				this.pathReturnQueue.Enqueue(path);
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001C168 File Offset: 0x0001A368
		public void ReturnPaths(bool timeSlice)
		{
			long num = timeSlice ? (DateTime.UtcNow.Ticks + 10000L) : 0L;
			int num2 = 0;
			int num3 = 0;
			for (;;)
			{
				Queue<Path> obj = this.pathReturnQueue;
				Path path;
				lock (obj)
				{
					if (this.pathReturnQueue.Count == 0)
					{
						break;
					}
					path = this.pathReturnQueue.Dequeue();
				}
				((IPathInternals)path).AdvanceState(PathState.Returning);
				try
				{
					((IPathInternals)path).ReturnPath();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
				((IPathInternals)path).AdvanceState(PathState.Returned);
				path.Release(this.pathsClaimedSilentlyBy, true);
				num2++;
				num3++;
				if (num2 > 5 && timeSlice)
				{
					num2 = 0;
					if (DateTime.UtcNow.Ticks >= num)
					{
						break;
					}
				}
			}
			if (num3 > 0)
			{
				this.OnReturnedPaths();
			}
		}

		// Token: 0x040003E4 RID: 996
		private readonly Queue<Path> pathReturnQueue = new Queue<Path>();

		// Token: 0x040003E5 RID: 997
		private readonly object pathsClaimedSilentlyBy;

		// Token: 0x040003E6 RID: 998
		private readonly Action OnReturnedPaths;
	}
}
