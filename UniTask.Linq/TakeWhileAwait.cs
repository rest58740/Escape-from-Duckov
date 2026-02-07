using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000071 RID: 113
	internal sealed class TakeWhileAwait<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x0600039C RID: 924 RVA: 0x0000D6B7 File Offset: 0x0000B8B7
		public TakeWhileAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000D6CD File Offset: 0x0000B8CD
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new TakeWhileAwait<TSource>._TakeWhileAwait(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x0400016E RID: 366
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400016F RID: 367
		private readonly Func<TSource, UniTask<bool>> predicate;

		// Token: 0x020001D8 RID: 472
		private class _TakeWhileAwait : AsyncEnumeratorAwaitSelectorBase<TSource, TSource, bool>
		{
			// Token: 0x06000868 RID: 2152 RVA: 0x00049AB5 File Offset: 0x00047CB5
			public _TakeWhileAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.predicate = predicate;
			}

			// Token: 0x06000869 RID: 2153 RVA: 0x00049AC6 File Offset: 0x00047CC6
			protected override UniTask<bool> TransformAsync(TSource sourceCurrent)
			{
				return this.predicate(sourceCurrent);
			}

			// Token: 0x0600086A RID: 2154 RVA: 0x00049AD4 File Offset: 0x00047CD4
			protected override bool TrySetCurrentCore(bool awaitResult, out bool terminateIteration)
			{
				if (awaitResult)
				{
					base.Current = base.SourceCurrent;
					terminateIteration = false;
					return true;
				}
				terminateIteration = true;
				return false;
			}

			// Token: 0x040011EC RID: 4588
			private Func<TSource, UniTask<bool>> predicate;
		}
	}
}
