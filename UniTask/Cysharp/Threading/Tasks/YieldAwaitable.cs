using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200003D RID: 61
	public readonly struct YieldAwaitable
	{
		// Token: 0x06000181 RID: 385 RVA: 0x0000685E File Offset: 0x00004A5E
		public YieldAwaitable(PlayerLoopTiming timing)
		{
			this.timing = timing;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006867 File Offset: 0x00004A67
		public YieldAwaitable.Awaiter GetAwaiter()
		{
			return new YieldAwaitable.Awaiter(this.timing);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006874 File Offset: 0x00004A74
		public UniTask ToUniTask()
		{
			return UniTask.Yield(this.timing, CancellationToken.None, false);
		}

		// Token: 0x04000087 RID: 135
		private readonly PlayerLoopTiming timing;

		// Token: 0x020001BD RID: 445
		public readonly struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x06000A5E RID: 2654 RVA: 0x00024D37 File Offset: 0x00022F37
			public Awaiter(PlayerLoopTiming timing)
			{
				this.timing = timing;
			}

			// Token: 0x1700006D RID: 109
			// (get) Token: 0x06000A5F RID: 2655 RVA: 0x00024D40 File Offset: 0x00022F40
			public bool IsCompleted
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000A60 RID: 2656 RVA: 0x00024D43 File Offset: 0x00022F43
			public void GetResult()
			{
			}

			// Token: 0x06000A61 RID: 2657 RVA: 0x00024D45 File Offset: 0x00022F45
			public void OnCompleted(Action continuation)
			{
				PlayerLoopHelper.AddContinuation(this.timing, continuation);
			}

			// Token: 0x06000A62 RID: 2658 RVA: 0x00024D53 File Offset: 0x00022F53
			public void UnsafeOnCompleted(Action continuation)
			{
				PlayerLoopHelper.AddContinuation(this.timing, continuation);
			}

			// Token: 0x0400039B RID: 923
			private readonly PlayerLoopTiming timing;
		}
	}
}
