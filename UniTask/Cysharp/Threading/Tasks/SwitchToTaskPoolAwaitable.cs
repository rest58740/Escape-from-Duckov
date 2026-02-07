using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000042 RID: 66
	public struct SwitchToTaskPoolAwaitable
	{
		// Token: 0x0600018A RID: 394 RVA: 0x0000693C File Offset: 0x00004B3C
		public SwitchToTaskPoolAwaitable.Awaiter GetAwaiter()
		{
			return default(SwitchToTaskPoolAwaitable.Awaiter);
		}

		// Token: 0x020001C1 RID: 449
		public struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x17000071 RID: 113
			// (get) Token: 0x06000A74 RID: 2676 RVA: 0x00024E74 File Offset: 0x00023074
			public bool IsCompleted
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000A75 RID: 2677 RVA: 0x00024E77 File Offset: 0x00023077
			public void GetResult()
			{
			}

			// Token: 0x06000A76 RID: 2678 RVA: 0x00024E79 File Offset: 0x00023079
			public void OnCompleted(Action continuation)
			{
				Task.Factory.StartNew(SwitchToTaskPoolAwaitable.Awaiter.switchToCallback, continuation, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
			}

			// Token: 0x06000A77 RID: 2679 RVA: 0x00024E97 File Offset: 0x00023097
			public void UnsafeOnCompleted(Action continuation)
			{
				Task.Factory.StartNew(SwitchToTaskPoolAwaitable.Awaiter.switchToCallback, continuation, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
			}

			// Token: 0x06000A78 RID: 2680 RVA: 0x00024EB5 File Offset: 0x000230B5
			private static void Callback(object state)
			{
				((Action)state)();
			}

			// Token: 0x040003A1 RID: 929
			private static readonly Action<object> switchToCallback = new Action<object>(SwitchToTaskPoolAwaitable.Awaiter.Callback);
		}
	}
}
