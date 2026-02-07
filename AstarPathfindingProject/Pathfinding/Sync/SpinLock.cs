using System;
using System.Threading;
using Unity.Burst.Intrinsics;

namespace Pathfinding.Sync
{
	// Token: 0x02000230 RID: 560
	internal struct SpinLock
	{
		// Token: 0x06000D4B RID: 3403 RVA: 0x00053C63 File Offset: 0x00051E63
		public void Lock()
		{
			while (Interlocked.CompareExchange(ref this.locked, 1, 0) != 0)
			{
				Common.Pause();
			}
			Thread.MemoryBarrier();
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00053C80 File Offset: 0x00051E80
		public void Unlock()
		{
			Thread.MemoryBarrier();
			if (Interlocked.Exchange(ref this.locked, 0) == 0)
			{
				throw new InvalidOperationException("Trying to unlock a lock which is not locked");
			}
		}

		// Token: 0x04000A45 RID: 2629
		private volatile int locked;
	}
}
