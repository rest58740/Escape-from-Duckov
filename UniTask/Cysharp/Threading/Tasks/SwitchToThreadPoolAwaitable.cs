using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000041 RID: 65
	public struct SwitchToThreadPoolAwaitable
	{
		// Token: 0x06000189 RID: 393 RVA: 0x00006924 File Offset: 0x00004B24
		public SwitchToThreadPoolAwaitable.Awaiter GetAwaiter()
		{
			return default(SwitchToThreadPoolAwaitable.Awaiter);
		}

		// Token: 0x020001C0 RID: 448
		public struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x17000070 RID: 112
			// (get) Token: 0x06000A6E RID: 2670 RVA: 0x00024E33 File Offset: 0x00023033
			public bool IsCompleted
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000A6F RID: 2671 RVA: 0x00024E36 File Offset: 0x00023036
			public void GetResult()
			{
			}

			// Token: 0x06000A70 RID: 2672 RVA: 0x00024E38 File Offset: 0x00023038
			public void OnCompleted(Action continuation)
			{
				ThreadPool.QueueUserWorkItem(SwitchToThreadPoolAwaitable.Awaiter.switchToCallback, continuation);
			}

			// Token: 0x06000A71 RID: 2673 RVA: 0x00024E46 File Offset: 0x00023046
			public void UnsafeOnCompleted(Action continuation)
			{
				ThreadPool.UnsafeQueueUserWorkItem(SwitchToThreadPoolAwaitable.Awaiter.switchToCallback, continuation);
			}

			// Token: 0x06000A72 RID: 2674 RVA: 0x00024E54 File Offset: 0x00023054
			private static void Callback(object state)
			{
				((Action)state)();
			}

			// Token: 0x040003A0 RID: 928
			private static readonly WaitCallback switchToCallback = new WaitCallback(SwitchToThreadPoolAwaitable.Awaiter.Callback);
		}
	}
}
