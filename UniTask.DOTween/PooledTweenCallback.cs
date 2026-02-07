using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using DG.Tweening;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000005 RID: 5
	internal sealed class PooledTweenCallback
	{
		// Token: 0x0600000B RID: 11 RVA: 0x0000224B File Offset: 0x0000044B
		private PooledTweenCallback()
		{
			this.runDelegate = new TweenCallback(this.Run);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002268 File Offset: 0x00000468
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TweenCallback Create(Action continuation)
		{
			PooledTweenCallback pooledTweenCallback;
			if (!PooledTweenCallback.pool.TryDequeue(out pooledTweenCallback))
			{
				pooledTweenCallback = new PooledTweenCallback();
			}
			pooledTweenCallback.continuation = continuation;
			return pooledTweenCallback.runDelegate;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002298 File Offset: 0x00000498
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Run()
		{
			Action action = this.continuation;
			this.continuation = null;
			if (action != null)
			{
				PooledTweenCallback.pool.Enqueue(this);
				action();
			}
		}

		// Token: 0x0400000B RID: 11
		private static readonly ConcurrentQueue<PooledTweenCallback> pool = new ConcurrentQueue<PooledTweenCallback>();

		// Token: 0x0400000C RID: 12
		private readonly TweenCallback runDelegate;

		// Token: 0x0400000D RID: 13
		private Action continuation;
	}
}
