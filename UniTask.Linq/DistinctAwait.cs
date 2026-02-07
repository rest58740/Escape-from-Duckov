using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000024 RID: 36
	internal sealed class DistinctAwait<TSource, TKey> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000267 RID: 615 RVA: 0x0000909F File Offset: 0x0000729F
		public DistinctAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			this.source = source;
			this.keySelector = keySelector;
			this.comparer = comparer;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x000090BC File Offset: 0x000072BC
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new DistinctAwait<TSource, TKey>._DistinctAwait(this.source, this.keySelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000A8 RID: 168
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000A9 RID: 169
		private readonly Func<TSource, UniTask<TKey>> keySelector;

		// Token: 0x040000AA RID: 170
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x020000F8 RID: 248
		private class _DistinctAwait : AsyncEnumeratorAwaitSelectorBase<TSource, TSource, TKey>
		{
			// Token: 0x06000597 RID: 1431 RVA: 0x00025D35 File Offset: 0x00023F35
			public _DistinctAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.set = new HashSet<TKey>(comparer);
				this.keySelector = keySelector;
			}

			// Token: 0x06000598 RID: 1432 RVA: 0x00025D53 File Offset: 0x00023F53
			protected override UniTask<TKey> TransformAsync(TSource sourceCurrent)
			{
				return this.keySelector(sourceCurrent);
			}

			// Token: 0x06000599 RID: 1433 RVA: 0x00025D61 File Offset: 0x00023F61
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

			// Token: 0x04000894 RID: 2196
			private readonly HashSet<TKey> set;

			// Token: 0x04000895 RID: 2197
			private readonly Func<TSource, UniTask<TKey>> keySelector;
		}
	}
}
