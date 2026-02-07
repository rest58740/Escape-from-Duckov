using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000087 RID: 135
	internal sealed class WhereAwait<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x060003CE RID: 974 RVA: 0x0000DFB1 File Offset: 0x0000C1B1
		public WhereAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000DFC7 File Offset: 0x0000C1C7
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new WhereAwait<TSource>._WhereAwait(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x04000195 RID: 405
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000196 RID: 406
		private readonly Func<TSource, UniTask<bool>> predicate;

		// Token: 0x020001FB RID: 507
		private sealed class _WhereAwait : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x060008EB RID: 2283 RVA: 0x0004E0B9 File Offset: 0x0004C2B9
			public _WhereAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken)
			{
				this.source = source;
				this.predicate = predicate;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x17000058 RID: 88
			// (get) Token: 0x060008EC RID: 2284 RVA: 0x0004E0EF File Offset: 0x0004C2EF
			// (set) Token: 0x060008ED RID: 2285 RVA: 0x0004E0F7 File Offset: 0x0004C2F7
			public TSource Current { get; private set; }

			// Token: 0x060008EE RID: 2286 RVA: 0x0004E100 File Offset: 0x0004C300
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

			// Token: 0x060008EF RID: 2287 RVA: 0x0004E144 File Offset: 0x0004C344
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
							goto IL_7F;
						case 2:
							goto IL_E5;
						default:
							goto IL_11A;
						}
						this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
						if (!this.awaiter.IsCompleted)
						{
							this.state = 1;
							this.awaiter.UnsafeOnCompleted(this.moveNextAction);
							return;
						}
						IL_7F:
						if (!this.awaiter.GetResult())
						{
							break;
						}
						this.Current = this.enumerator.Current;
						this.awaiter2 = this.predicate(this.Current).GetAwaiter();
						if (!this.awaiter2.IsCompleted)
						{
							this.state = 2;
							this.awaiter2.UnsafeOnCompleted(this.moveNextAction);
							return;
						}
						IL_E5:
						if (this.awaiter2.GetResult())
						{
							goto IL_130;
						}
						this.state = 0;
						continue;
					}
					catch (Exception ex)
					{
						this.state = -2;
						this.completionSource.TrySetException(ex);
						return;
					}
					break;
				}
				IL_11A:
				this.state = -2;
				this.completionSource.TrySetResult(false);
				return;
				IL_130:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x060008F0 RID: 2288 RVA: 0x0004E2B4 File Offset: 0x0004C4B4
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x0400132C RID: 4908
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x0400132D RID: 4909
			private readonly Func<TSource, UniTask<bool>> predicate;

			// Token: 0x0400132E RID: 4910
			private readonly CancellationToken cancellationToken;

			// Token: 0x0400132F RID: 4911
			private int state = -1;

			// Token: 0x04001330 RID: 4912
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04001331 RID: 4913
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04001332 RID: 4914
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x04001333 RID: 4915
			private Action moveNextAction;
		}
	}
}
