using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200004E RID: 78
	internal sealed class QueueOperator<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000326 RID: 806 RVA: 0x0000BFA0 File Offset: 0x0000A1A0
		public QueueOperator(IUniTaskAsyncEnumerable<TSource> source)
		{
			this.source = source;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000BFAF File Offset: 0x0000A1AF
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new QueueOperator<TSource>._Queue(this.source, cancellationToken);
		}

		// Token: 0x0400012A RID: 298
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x02000186 RID: 390
		private sealed class _Queue : IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000749 RID: 1865 RVA: 0x0003EB92 File Offset: 0x0003CD92
			public _Queue(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
			{
				this.source = source;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000033 RID: 51
			// (get) Token: 0x0600074A RID: 1866 RVA: 0x0003EBA8 File Offset: 0x0003CDA8
			public TSource Current
			{
				get
				{
					return this.channelEnumerator.Current;
				}
			}

			// Token: 0x0600074B RID: 1867 RVA: 0x0003EBB8 File Offset: 0x0003CDB8
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.sourceEnumerator == null)
				{
					this.sourceEnumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
					this.channel = Channel.CreateSingleConsumerUnbounded<TSource>();
					this.channelEnumerator = this.channel.Reader.ReadAllAsync(default(CancellationToken)).GetAsyncEnumerator(this.cancellationToken);
					QueueOperator<TSource>._Queue.ConsumeAll(this, this.sourceEnumerator, this.channel).Forget();
				}
				return this.channelEnumerator.MoveNextAsync();
			}

			// Token: 0x0600074C RID: 1868 RVA: 0x0003EC50 File Offset: 0x0003CE50
			private static UniTaskVoid ConsumeAll(QueueOperator<TSource>._Queue self, IUniTaskAsyncEnumerator<TSource> enumerator, ChannelWriter<TSource> writer)
			{
				QueueOperator<TSource>._Queue.<ConsumeAll>d__10 <ConsumeAll>d__;
				<ConsumeAll>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<ConsumeAll>d__.self = self;
				<ConsumeAll>d__.enumerator = enumerator;
				<ConsumeAll>d__.writer = writer;
				<ConsumeAll>d__.<>1__state = -1;
				<ConsumeAll>d__.<>t__builder.Start<QueueOperator<TSource>._Queue.<ConsumeAll>d__10>(ref <ConsumeAll>d__);
				return <ConsumeAll>d__.<>t__builder.Task;
			}

			// Token: 0x0600074D RID: 1869 RVA: 0x0003ECA4 File Offset: 0x0003CEA4
			public UniTask DisposeAsync()
			{
				QueueOperator<TSource>._Queue.<DisposeAsync>d__11 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<QueueOperator<TSource>._Queue.<DisposeAsync>d__11>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x04000EAB RID: 3755
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000EAC RID: 3756
			private CancellationToken cancellationToken;

			// Token: 0x04000EAD RID: 3757
			private Channel<TSource> channel;

			// Token: 0x04000EAE RID: 3758
			private IUniTaskAsyncEnumerator<TSource> channelEnumerator;

			// Token: 0x04000EAF RID: 3759
			private IUniTaskAsyncEnumerator<TSource> sourceEnumerator;

			// Token: 0x04000EB0 RID: 3760
			private bool channelClosed;
		}
	}
}
