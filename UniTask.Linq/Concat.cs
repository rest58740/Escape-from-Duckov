using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200001C RID: 28
	internal sealed class Concat<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000257 RID: 599 RVA: 0x00008E27 File Offset: 0x00007027
		public Concat(IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second)
		{
			this.first = first;
			this.second = second;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00008E3D File Offset: 0x0000703D
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Concat<TSource>._Concat(this.first, this.second, cancellationToken);
		}

		// Token: 0x0400009E RID: 158
		private readonly IUniTaskAsyncEnumerable<TSource> first;

		// Token: 0x0400009F RID: 159
		private readonly IUniTaskAsyncEnumerable<TSource> second;

		// Token: 0x020000ED RID: 237
		private sealed class _Concat : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000569 RID: 1385 RVA: 0x00024B2C File Offset: 0x00022D2C
			public _Concat(IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second, CancellationToken cancellationToken)
			{
				this.first = first;
				this.second = second;
				this.cancellationToken = cancellationToken;
				this.iteratingState = Concat<TSource>._Concat.IteratingState.IteratingFirst;
			}

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x0600056A RID: 1386 RVA: 0x00024B50 File Offset: 0x00022D50
			// (set) Token: 0x0600056B RID: 1387 RVA: 0x00024B58 File Offset: 0x00022D58
			public TSource Current { get; private set; }

			// Token: 0x0600056C RID: 1388 RVA: 0x00024B61 File Offset: 0x00022D61
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.iteratingState == Concat<TSource>._Concat.IteratingState.Complete)
				{
					return CompletedTasks.False;
				}
				this.completionSource.Reset();
				this.StartIterate();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x0600056D RID: 1389 RVA: 0x00024BA0 File Offset: 0x00022DA0
			private void StartIterate()
			{
				if (this.enumerator == null)
				{
					if (this.iteratingState == Concat<TSource>._Concat.IteratingState.IteratingFirst)
					{
						this.enumerator = this.first.GetAsyncEnumerator(this.cancellationToken);
					}
					else if (this.iteratingState == Concat<TSource>._Concat.IteratingState.IteratingSecond)
					{
						this.enumerator = this.second.GetAsyncEnumerator(this.cancellationToken);
					}
				}
				try
				{
					this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
					return;
				}
				if (this.awaiter.IsCompleted)
				{
					Concat<TSource>._Concat.MoveNextCoreDelegate(this);
					return;
				}
				this.awaiter.SourceOnCompleted(Concat<TSource>._Concat.MoveNextCoreDelegate, this);
			}

			// Token: 0x0600056E RID: 1390 RVA: 0x00024C5C File Offset: 0x00022E5C
			private static void MoveNextCore(object state)
			{
				Concat<TSource>._Concat concat = (Concat<TSource>._Concat)state;
				bool flag;
				if (concat.TryGetResult<bool>(concat.awaiter, ref flag))
				{
					if (flag)
					{
						concat.Current = concat.enumerator.Current;
						concat.completionSource.TrySetResult(true);
						return;
					}
					if (concat.iteratingState == Concat<TSource>._Concat.IteratingState.IteratingFirst)
					{
						concat.RunSecondAfterDisposeAsync().Forget();
						return;
					}
					concat.iteratingState = Concat<TSource>._Concat.IteratingState.Complete;
					concat.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x0600056F RID: 1391 RVA: 0x00024CD0 File Offset: 0x00022ED0
			private UniTaskVoid RunSecondAfterDisposeAsync()
			{
				Concat<TSource>._Concat.<RunSecondAfterDisposeAsync>d__16 <RunSecondAfterDisposeAsync>d__;
				<RunSecondAfterDisposeAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<RunSecondAfterDisposeAsync>d__.<>4__this = this;
				<RunSecondAfterDisposeAsync>d__.<>1__state = -1;
				<RunSecondAfterDisposeAsync>d__.<>t__builder.Start<Concat<TSource>._Concat.<RunSecondAfterDisposeAsync>d__16>(ref <RunSecondAfterDisposeAsync>d__);
				return <RunSecondAfterDisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x06000570 RID: 1392 RVA: 0x00024D14 File Offset: 0x00022F14
			public UniTask DisposeAsync()
			{
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x04000843 RID: 2115
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(Concat<TSource>._Concat.MoveNextCore);

			// Token: 0x04000844 RID: 2116
			private readonly IUniTaskAsyncEnumerable<TSource> first;

			// Token: 0x04000845 RID: 2117
			private readonly IUniTaskAsyncEnumerable<TSource> second;

			// Token: 0x04000846 RID: 2118
			private CancellationToken cancellationToken;

			// Token: 0x04000847 RID: 2119
			private Concat<TSource>._Concat.IteratingState iteratingState;

			// Token: 0x04000848 RID: 2120
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04000849 RID: 2121
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x02000213 RID: 531
			private enum IteratingState
			{
				// Token: 0x040013B5 RID: 5045
				IteratingFirst,
				// Token: 0x040013B6 RID: 5046
				IteratingSecond,
				// Token: 0x040013B7 RID: 5047
				Complete
			}
		}
	}
}
