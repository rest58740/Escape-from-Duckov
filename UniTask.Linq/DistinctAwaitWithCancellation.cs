using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000025 RID: 37
	internal sealed class DistinctAwaitWithCancellation<TSource, TKey> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000269 RID: 617 RVA: 0x000090D6 File Offset: 0x000072D6
		public DistinctAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			this.source = source;
			this.keySelector = keySelector;
			this.comparer = comparer;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000090F3 File Offset: 0x000072F3
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new DistinctAwaitWithCancellation<TSource, TKey>._DistinctAwaitWithCancellation(this.source, this.keySelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000AB RID: 171
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000AC RID: 172
		private readonly Func<TSource, CancellationToken, UniTask<TKey>> keySelector;

		// Token: 0x040000AD RID: 173
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x020000F9 RID: 249
		private class _DistinctAwaitWithCancellation : AsyncEnumeratorAwaitSelectorBase<TSource, TSource, TKey>
		{
			// Token: 0x0600059A RID: 1434 RVA: 0x00025D86 File Offset: 0x00023F86
			public _DistinctAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.set = new HashSet<TKey>(comparer);
				this.keySelector = keySelector;
			}

			// Token: 0x0600059B RID: 1435 RVA: 0x00025DA4 File Offset: 0x00023FA4
			protected override UniTask<TKey> TransformAsync(TSource sourceCurrent)
			{
				return this.keySelector(sourceCurrent, this.cancellationToken);
			}

			// Token: 0x0600059C RID: 1436 RVA: 0x00025DB8 File Offset: 0x00023FB8
			protected override bool TrySetCurrentCore(TKey awaitResult, out bool terminateIteration)
			{
				if (this.set.Add(awaitResult))
				{
					base.Current = base.SourceCurrent;
					terminateIteration = false;
					return true;
				}
				terminateIteration = false;
				return false;
			}

			// Token: 0x04000896 RID: 2198
			private readonly HashSet<TKey> set;

			// Token: 0x04000897 RID: 2199
			private readonly Func<TSource, CancellationToken, UniTask<TKey>> keySelector;
		}
	}
}
