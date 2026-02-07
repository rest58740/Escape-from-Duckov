using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000064 RID: 100
	internal sealed class SkipWhileAwait<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000356 RID: 854 RVA: 0x0000C56B File Offset: 0x0000A76B
		public SkipWhileAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000C581 File Offset: 0x0000A781
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SkipWhileAwait<TSource>._SkipWhileAwait(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x04000156 RID: 342
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000157 RID: 343
		private readonly Func<TSource, UniTask<bool>> predicate;

		// Token: 0x0200019F RID: 415
		private class _SkipWhileAwait : AsyncEnumeratorAwaitSelectorBase<TSource, TSource, bool>
		{
			// Token: 0x060007D4 RID: 2004 RVA: 0x00041F61 File Offset: 0x00040161
			public _SkipWhileAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.predicate = predicate;
			}

			// Token: 0x060007D5 RID: 2005 RVA: 0x00041F72 File Offset: 0x00040172
			protected override UniTask<bool> TransformAsync(TSource sourceCurrent)
			{
				if (this.predicate == null)
				{
					return CompletedTasks.False;
				}
				return this.predicate(sourceCurrent);
			}

			// Token: 0x060007D6 RID: 2006 RVA: 0x00041F8E File Offset: 0x0004018E
			protected override bool TrySetCurrentCore(bool awaitResult, out bool terminateIteration)
			{
				if (!awaitResult)
				{
					this.predicate = null;
					base.Current = base.SourceCurrent;
					terminateIteration = false;
					return true;
				}
				terminateIteration = false;
				return false;
			}

			// Token: 0x04000F9E RID: 3998
			private Func<TSource, UniTask<bool>> predicate;
		}
	}
}
