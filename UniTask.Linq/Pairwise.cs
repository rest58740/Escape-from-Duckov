using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200004C RID: 76
	internal sealed class Pairwise<TSource> : IUniTaskAsyncEnumerable<ValueTuple<TSource, TSource>>
	{
		// Token: 0x06000320 RID: 800 RVA: 0x0000BEB2 File Offset: 0x0000A0B2
		public Pairwise(IUniTaskAsyncEnumerable<TSource> source)
		{
			this.source = source;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000BEC1 File Offset: 0x0000A0C1
		public IUniTaskAsyncEnumerator<ValueTuple<TSource, TSource>> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Pairwise<TSource>._Pairwise(this.source, cancellationToken);
		}

		// Token: 0x04000123 RID: 291
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x02000182 RID: 386
		private sealed class _Pairwise : MoveNextSource, IUniTaskAsyncEnumerator<ValueTuple<TSource, TSource>>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600072E RID: 1838 RVA: 0x0003E616 File Offset: 0x0003C816
			public _Pairwise(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
			{
				this.source = source;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700002F RID: 47
			// (get) Token: 0x0600072F RID: 1839 RVA: 0x0003E62C File Offset: 0x0003C82C
			// (set) Token: 0x06000730 RID: 1840 RVA: 0x0003E634 File Offset: 0x0003C834
			public ValueTuple<TSource, TSource> Current { get; private set; }

			// Token: 0x06000731 RID: 1841 RVA: 0x0003E640 File Offset: 0x0003C840
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.enumerator == null)
				{
					this.isFirst = true;
					this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
				}
				this.completionSource.Reset();
				this.SourceMoveNext();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000732 RID: 1842 RVA: 0x0003E6A0 File Offset: 0x0003C8A0
			private void SourceMoveNext()
			{
				try
				{
					this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
					if (this.awaiter.IsCompleted)
					{
						Pairwise<TSource>._Pairwise.MoveNextCore(this);
					}
					else
					{
						this.awaiter.SourceOnCompleted(Pairwise<TSource>._Pairwise.MoveNextCoreDelegate, this);
					}
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x06000733 RID: 1843 RVA: 0x0003E710 File Offset: 0x0003C910
			private static void MoveNextCore(object state)
			{
				Pairwise<TSource>._Pairwise pairwise = (Pairwise<TSource>._Pairwise)state;
				bool flag;
				if (pairwise.TryGetResult<bool>(pairwise.awaiter, ref flag))
				{
					if (flag)
					{
						if (pairwise.isFirst)
						{
							pairwise.isFirst = false;
							pairwise.prev = pairwise.enumerator.Current;
							pairwise.SourceMoveNext();
							return;
						}
						TSource item = pairwise.prev;
						pairwise.prev = pairwise.enumerator.Current;
						pairwise.Current = new ValueTuple<TSource, TSource>(item, pairwise.prev);
						pairwise.completionSource.TrySetResult(true);
						return;
					}
					else
					{
						pairwise.completionSource.TrySetResult(false);
					}
				}
			}

			// Token: 0x06000734 RID: 1844 RVA: 0x0003E7A4 File Offset: 0x0003C9A4
			public UniTask DisposeAsync()
			{
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x04000E93 RID: 3731
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(Pairwise<TSource>._Pairwise.MoveNextCore);

			// Token: 0x04000E94 RID: 3732
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000E95 RID: 3733
			private CancellationToken cancellationToken;

			// Token: 0x04000E96 RID: 3734
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04000E97 RID: 3735
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04000E98 RID: 3736
			private TSource prev;

			// Token: 0x04000E99 RID: 3737
			private bool isFirst;
		}
	}
}
