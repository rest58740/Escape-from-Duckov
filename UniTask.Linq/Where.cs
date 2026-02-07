using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000085 RID: 133
	internal sealed class Where<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x060003CA RID: 970 RVA: 0x0000DF5D File Offset: 0x0000C15D
		public Where(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000DF73 File Offset: 0x0000C173
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Where<TSource>._Where(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x04000191 RID: 401
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000192 RID: 402
		private readonly Func<TSource, bool> predicate;

		// Token: 0x020001F9 RID: 505
		private sealed class _Where : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x060008DF RID: 2271 RVA: 0x0004DD3B File Offset: 0x0004BF3B
			public _Where(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
			{
				this.source = source;
				this.predicate = predicate;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x060008E0 RID: 2272 RVA: 0x0004DD71 File Offset: 0x0004BF71
			// (set) Token: 0x060008E1 RID: 2273 RVA: 0x0004DD79 File Offset: 0x0004BF79
			public TSource Current { get; private set; }

			// Token: 0x060008E2 RID: 2274 RVA: 0x0004DD84 File Offset: 0x0004BF84
			public UniTask<bool> MoveNextAsync()
			{
				if (this.state == -2)
				{
					return default(UniTask<bool>);
				}
				this.completionSource.Reset();
				this.MoveNext();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060008E3 RID: 2275 RVA: 0x0004DDC8 File Offset: 0x0004BFC8
			private void MoveNext()
			{
				for (;;)
				{
					try
					{
						switch (this.state)
						{
						case -1:
							this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
							break;
						case 0:
							break;
						case 1:
							goto IL_7B;
						default:
							goto IL_BA;
						}
						this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
						if (!this.awaiter.IsCompleted)
						{
							this.state = 1;
							this.awaiter.UnsafeOnCompleted(this.moveNextAction);
							return;
						}
						IL_7B:
						if (this.awaiter.GetResult())
						{
							this.Current = this.enumerator.Current;
							if (this.predicate(this.Current))
							{
								goto IL_EA;
							}
							this.state = 0;
							continue;
						}
						IL_BA:;
					}
					catch (Exception ex)
					{
						this.state = -2;
						this.completionSource.TrySetException(ex);
						return;
					}
					break;
				}
				this.state = -2;
				this.completionSource.TrySetResult(false);
				return;
				IL_EA:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x060008E4 RID: 2276 RVA: 0x0004DEE4 File Offset: 0x0004C0E4
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x0400131B RID: 4891
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x0400131C RID: 4892
			private readonly Func<TSource, bool> predicate;

			// Token: 0x0400131D RID: 4893
			private readonly CancellationToken cancellationToken;

			// Token: 0x0400131E RID: 4894
			private int state = -1;

			// Token: 0x0400131F RID: 4895
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04001320 RID: 4896
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04001321 RID: 4897
			private Action moveNextAction;
		}
	}
}
