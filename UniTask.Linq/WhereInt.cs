using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000086 RID: 134
	internal sealed class WhereInt<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x060003CC RID: 972 RVA: 0x0000DF87 File Offset: 0x0000C187
		public WhereInt(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000DF9D File Offset: 0x0000C19D
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new WhereInt<TSource>._Where(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x04000193 RID: 403
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000194 RID: 404
		private readonly Func<TSource, int, bool> predicate;

		// Token: 0x020001FA RID: 506
		private sealed class _Where : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x060008E5 RID: 2277 RVA: 0x0004DEF1 File Offset: 0x0004C0F1
			public _Where(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate, CancellationToken cancellationToken)
			{
				this.source = source;
				this.predicate = predicate;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x17000057 RID: 87
			// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0004DF27 File Offset: 0x0004C127
			// (set) Token: 0x060008E7 RID: 2279 RVA: 0x0004DF2F File Offset: 0x0004C12F
			public TSource Current { get; private set; }

			// Token: 0x060008E8 RID: 2280 RVA: 0x0004DF38 File Offset: 0x0004C138
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

			// Token: 0x060008E9 RID: 2281 RVA: 0x0004DF7C File Offset: 0x0004C17C
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
							goto IL_CB;
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
							Func<TSource, int, bool> func = this.predicate;
							TSource arg = this.Current;
							int num = this.index;
							this.index = checked(num + 1);
							if (func(arg, num))
							{
								goto IL_FB;
							}
							this.state = 0;
							continue;
						}
						IL_CB:;
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
				IL_FB:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x060008EA RID: 2282 RVA: 0x0004E0AC File Offset: 0x0004C2AC
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x04001323 RID: 4899
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04001324 RID: 4900
			private readonly Func<TSource, int, bool> predicate;

			// Token: 0x04001325 RID: 4901
			private readonly CancellationToken cancellationToken;

			// Token: 0x04001326 RID: 4902
			private int state = -1;

			// Token: 0x04001327 RID: 4903
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04001328 RID: 4904
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04001329 RID: 4905
			private Action moveNextAction;

			// Token: 0x0400132A RID: 4906
			private int index;
		}
	}
}
