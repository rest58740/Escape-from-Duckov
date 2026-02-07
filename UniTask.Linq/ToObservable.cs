using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200007B RID: 123
	internal sealed class ToObservable<T> : IObservable<T>
	{
		// Token: 0x060003B5 RID: 949 RVA: 0x0000DCE8 File Offset: 0x0000BEE8
		public ToObservable(IUniTaskAsyncEnumerable<T> source)
		{
			this.source = source;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000DCF8 File Offset: 0x0000BEF8
		public IDisposable Subscribe(IObserver<T> observer)
		{
			ToObservable<T>.CancellationTokenDisposable cancellationTokenDisposable = new ToObservable<T>.CancellationTokenDisposable();
			ToObservable<T>.RunAsync(this.source, observer, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000DD28 File Offset: 0x0000BF28
		private static UniTaskVoid RunAsync(IUniTaskAsyncEnumerable<T> src, IObserver<T> observer, CancellationToken cancellationToken)
		{
			ToObservable<T>.<RunAsync>d__3 <RunAsync>d__;
			<RunAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<RunAsync>d__.src = src;
			<RunAsync>d__.observer = observer;
			<RunAsync>d__.cancellationToken = cancellationToken;
			<RunAsync>d__.<>1__state = -1;
			<RunAsync>d__.<>t__builder.Start<ToObservable<T>.<RunAsync>d__3>(ref <RunAsync>d__);
			return <RunAsync>d__.<>t__builder.Task;
		}

		// Token: 0x04000177 RID: 375
		private readonly IUniTaskAsyncEnumerable<T> source;

		// Token: 0x020001EE RID: 494
		internal sealed class CancellationTokenDisposable : IDisposable
		{
			// Token: 0x1700004C RID: 76
			// (get) Token: 0x060008AC RID: 2220 RVA: 0x0004CA8E File Offset: 0x0004AC8E
			public CancellationToken Token
			{
				get
				{
					return this.cts.Token;
				}
			}

			// Token: 0x060008AD RID: 2221 RVA: 0x0004CA9B File Offset: 0x0004AC9B
			public void Dispose()
			{
				if (!this.cts.IsCancellationRequested)
				{
					this.cts.Cancel();
				}
			}

			// Token: 0x040012D2 RID: 4818
			private readonly CancellationTokenSource cts = new CancellationTokenSource();
		}
	}
}
