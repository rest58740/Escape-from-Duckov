using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200004D RID: 77
	internal sealed class Publish<TSource> : IConnectableUniTaskAsyncEnumerable<TSource>, IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0000BECF File Offset: 0x0000A0CF
		public Publish(IUniTaskAsyncEnumerable<TSource> source)
		{
			this.source = source;
			this.cancellationTokenSource = new CancellationTokenSource();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000BEEC File Offset: 0x0000A0EC
		public IDisposable Connect()
		{
			if (this.connectedDisposable != null)
			{
				return this.connectedDisposable;
			}
			if (this.enumerator == null)
			{
				this.enumerator = this.source.GetAsyncEnumerator(this.cancellationTokenSource.Token);
			}
			this.ConsumeEnumerator().Forget();
			this.connectedDisposable = new Publish<TSource>.ConnectDisposable(this.cancellationTokenSource);
			return this.connectedDisposable;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000BF54 File Offset: 0x0000A154
		private UniTaskVoid ConsumeEnumerator()
		{
			Publish<TSource>.<ConsumeEnumerator>d__8 <ConsumeEnumerator>d__;
			<ConsumeEnumerator>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<ConsumeEnumerator>d__.<>4__this = this;
			<ConsumeEnumerator>d__.<>1__state = -1;
			<ConsumeEnumerator>d__.<>t__builder.Start<Publish<TSource>.<ConsumeEnumerator>d__8>(ref <ConsumeEnumerator>d__);
			return <ConsumeEnumerator>d__.<>t__builder.Task;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000BF97 File Offset: 0x0000A197
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Publish<TSource>._Publish(this, cancellationToken);
		}

		// Token: 0x04000124 RID: 292
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000125 RID: 293
		private readonly CancellationTokenSource cancellationTokenSource;

		// Token: 0x04000126 RID: 294
		private TriggerEvent<TSource> trigger;

		// Token: 0x04000127 RID: 295
		private IUniTaskAsyncEnumerator<TSource> enumerator;

		// Token: 0x04000128 RID: 296
		private IDisposable connectedDisposable;

		// Token: 0x04000129 RID: 297
		private bool isCompleted;

		// Token: 0x02000183 RID: 387
		private sealed class ConnectDisposable : IDisposable
		{
			// Token: 0x06000736 RID: 1846 RVA: 0x0003E7E1 File Offset: 0x0003C9E1
			public ConnectDisposable(CancellationTokenSource cancellationTokenSource)
			{
				this.cancellationTokenSource = cancellationTokenSource;
			}

			// Token: 0x06000737 RID: 1847 RVA: 0x0003E7F0 File Offset: 0x0003C9F0
			public void Dispose()
			{
				this.cancellationTokenSource.Cancel();
			}

			// Token: 0x04000E9B RID: 3739
			private readonly CancellationTokenSource cancellationTokenSource;
		}

		// Token: 0x02000184 RID: 388
		private sealed class _Publish : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable, ITriggerHandler<TSource>
		{
			// Token: 0x06000738 RID: 1848 RVA: 0x0003E800 File Offset: 0x0003CA00
			public _Publish(Publish<TSource> parent, CancellationToken cancellationToken)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}
				this.parent = parent;
				this.cancellationToken = cancellationToken;
				if (cancellationToken.CanBeCanceled)
				{
					this.cancellationTokenRegistration = CancellationTokenExtensions.RegisterWithoutCaptureExecutionContext(cancellationToken, Publish<TSource>._Publish.CancelDelegate, this);
				}
				parent.trigger.Add(this);
			}

			// Token: 0x17000030 RID: 48
			// (get) Token: 0x06000739 RID: 1849 RVA: 0x0003E852 File Offset: 0x0003CA52
			// (set) Token: 0x0600073A RID: 1850 RVA: 0x0003E85A File Offset: 0x0003CA5A
			public TSource Current { get; private set; }

			// Token: 0x17000031 RID: 49
			// (get) Token: 0x0600073B RID: 1851 RVA: 0x0003E863 File Offset: 0x0003CA63
			// (set) Token: 0x0600073C RID: 1852 RVA: 0x0003E86B File Offset: 0x0003CA6B
			ITriggerHandler<TSource> ITriggerHandler<!0>.Prev { get; set; }

			// Token: 0x17000032 RID: 50
			// (get) Token: 0x0600073D RID: 1853 RVA: 0x0003E874 File Offset: 0x0003CA74
			// (set) Token: 0x0600073E RID: 1854 RVA: 0x0003E87C File Offset: 0x0003CA7C
			ITriggerHandler<TSource> ITriggerHandler<!0>.Next { get; set; }

			// Token: 0x0600073F RID: 1855 RVA: 0x0003E885 File Offset: 0x0003CA85
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.parent.isCompleted)
				{
					return CompletedTasks.False;
				}
				this.completionSource.Reset();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000740 RID: 1856 RVA: 0x0003E8C4 File Offset: 0x0003CAC4
			private static void OnCanceled(object state)
			{
				Publish<TSource>._Publish publish = (Publish<TSource>._Publish)state;
				publish.completionSource.TrySetCanceled(publish.cancellationToken);
				UniTaskExtensions.Forget(publish.DisposeAsync());
			}

			// Token: 0x06000741 RID: 1857 RVA: 0x0003E8F8 File Offset: 0x0003CAF8
			public UniTask DisposeAsync()
			{
				if (!this.isDisposed)
				{
					this.isDisposed = true;
					this.cancellationTokenRegistration.Dispose();
					this.parent.trigger.Remove(this);
				}
				return default(UniTask);
			}

			// Token: 0x06000742 RID: 1858 RVA: 0x0003E939 File Offset: 0x0003CB39
			public void OnNext(TSource value)
			{
				this.Current = value;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x06000743 RID: 1859 RVA: 0x0003E94F File Offset: 0x0003CB4F
			public void OnCanceled(CancellationToken cancellationToken)
			{
				this.completionSource.TrySetCanceled(cancellationToken);
			}

			// Token: 0x06000744 RID: 1860 RVA: 0x0003E95E File Offset: 0x0003CB5E
			public void OnCompleted()
			{
				this.completionSource.TrySetResult(false);
			}

			// Token: 0x06000745 RID: 1861 RVA: 0x0003E96D File Offset: 0x0003CB6D
			public void OnError(Exception ex)
			{
				this.completionSource.TrySetException(ex);
			}

			// Token: 0x04000E9C RID: 3740
			private static readonly Action<object> CancelDelegate = new Action<object>(Publish<TSource>._Publish.OnCanceled);

			// Token: 0x04000E9D RID: 3741
			private readonly Publish<TSource> parent;

			// Token: 0x04000E9E RID: 3742
			private CancellationToken cancellationToken;

			// Token: 0x04000E9F RID: 3743
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x04000EA0 RID: 3744
			private bool isDisposed;
		}
	}
}
