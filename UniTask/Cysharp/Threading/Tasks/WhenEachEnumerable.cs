using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;
using Cysharp.Threading.Tasks.Internal;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000047 RID: 71
	internal sealed class WhenEachEnumerable<T> : IUniTaskAsyncEnumerable<WhenEachResult<T>>
	{
		// Token: 0x06000198 RID: 408 RVA: 0x00006A96 File Offset: 0x00004C96
		public WhenEachEnumerable(IEnumerable<UniTask<T>> source)
		{
			this.source = source;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00006AA5 File Offset: 0x00004CA5
		public IUniTaskAsyncEnumerator<WhenEachResult<T>> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new WhenEachEnumerable<T>.Enumerator(this.source, cancellationToken);
		}

		// Token: 0x0400009D RID: 157
		private IEnumerable<UniTask<T>> source;

		// Token: 0x020001C4 RID: 452
		private sealed class Enumerator : IUniTaskAsyncEnumerator<WhenEachResult<T>>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000A89 RID: 2697 RVA: 0x00024FE5 File Offset: 0x000231E5
			public Enumerator(IEnumerable<UniTask<T>> source, CancellationToken cancellationToken)
			{
				this.source = source;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000074 RID: 116
			// (get) Token: 0x06000A8A RID: 2698 RVA: 0x00024FFB File Offset: 0x000231FB
			public WhenEachResult<T> Current
			{
				get
				{
					return this.channelEnumerator.Current;
				}
			}

			// Token: 0x06000A8B RID: 2699 RVA: 0x00025008 File Offset: 0x00023208
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.state == WhenEachState.NotRunning)
				{
					this.state = WhenEachState.Running;
					this.channel = Channel.CreateSingleConsumerUnbounded<WhenEachResult<T>>();
					this.channelEnumerator = this.channel.Reader.ReadAllAsync(default(CancellationToken)).GetAsyncEnumerator(this.cancellationToken);
					UniTask<T>[] array = this.source as UniTask<T>[];
					if (array != null)
					{
						WhenEachEnumerable<T>.Enumerator.ConsumeAll(this, array, array.Length);
					}
					else
					{
						using (ArrayPoolUtil.RentArray<UniTask<T>> rentArray = ArrayPoolUtil.Materialize<UniTask<T>>(this.source))
						{
							WhenEachEnumerable<T>.Enumerator.ConsumeAll(this, rentArray.Array, rentArray.Length);
						}
					}
				}
				return this.channelEnumerator.MoveNextAsync();
			}

			// Token: 0x06000A8C RID: 2700 RVA: 0x000250CC File Offset: 0x000232CC
			private static void ConsumeAll(WhenEachEnumerable<T>.Enumerator self, UniTask<T>[] array, int length)
			{
				for (int i = 0; i < length; i++)
				{
					WhenEachEnumerable<T>.Enumerator.RunWhenEachTask(self, array[i], length).Forget();
				}
			}

			// Token: 0x06000A8D RID: 2701 RVA: 0x000250FC File Offset: 0x000232FC
			private static UniTaskVoid RunWhenEachTask(WhenEachEnumerable<T>.Enumerator self, UniTask<T> task, int length)
			{
				WhenEachEnumerable<T>.Enumerator.<RunWhenEachTask>d__11 <RunWhenEachTask>d__;
				<RunWhenEachTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<RunWhenEachTask>d__.self = self;
				<RunWhenEachTask>d__.task = task;
				<RunWhenEachTask>d__.length = length;
				<RunWhenEachTask>d__.<>1__state = -1;
				<RunWhenEachTask>d__.<>t__builder.Start<WhenEachEnumerable<T>.Enumerator.<RunWhenEachTask>d__11>(ref <RunWhenEachTask>d__);
				return <RunWhenEachTask>d__.<>t__builder.Task;
			}

			// Token: 0x06000A8E RID: 2702 RVA: 0x00025150 File Offset: 0x00023350
			public UniTask DisposeAsync()
			{
				WhenEachEnumerable<T>.Enumerator.<DisposeAsync>d__12 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<WhenEachEnumerable<T>.Enumerator.<DisposeAsync>d__12>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x040003A9 RID: 937
			private readonly IEnumerable<UniTask<T>> source;

			// Token: 0x040003AA RID: 938
			private CancellationToken cancellationToken;

			// Token: 0x040003AB RID: 939
			private Channel<WhenEachResult<T>> channel;

			// Token: 0x040003AC RID: 940
			private IUniTaskAsyncEnumerator<WhenEachResult<T>> channelEnumerator;

			// Token: 0x040003AD RID: 941
			private int completeCount;

			// Token: 0x040003AE RID: 942
			private WhenEachState state;
		}
	}
}
