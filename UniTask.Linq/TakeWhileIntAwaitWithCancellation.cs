using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000074 RID: 116
	internal sealed class TakeWhileIntAwaitWithCancellation<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x0000D735 File Offset: 0x0000B935
		public TakeWhileIntAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<bool>> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000D74B File Offset: 0x0000B94B
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new TakeWhileIntAwaitWithCancellation<TSource>._TakeWhileIntAwaitWithCancellation(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x04000174 RID: 372
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000175 RID: 373
		private readonly Func<TSource, int, CancellationToken, UniTask<bool>> predicate;

		// Token: 0x020001DB RID: 475
		private class _TakeWhileIntAwaitWithCancellation : AsyncEnumeratorAwaitSelectorBase<TSource, TSource, bool>
		{
			// Token: 0x06000871 RID: 2161 RVA: 0x00049B83 File Offset: 0x00047D83
			public _TakeWhileIntAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.predicate = predicate;
			}

			// Token: 0x06000872 RID: 2162 RVA: 0x00049B94 File Offset: 0x00047D94
			protected override UniTask<bool> TransformAsync(TSource sourceCurrent)
			{
				Func<TSource, int, CancellationToken, UniTask<bool>> func = this.predicate;
				int num = this.index;
				this.index = checked(num + 1);
				return func(sourceCurrent, num, this.cancellationToken);
			}

			// Token: 0x06000873 RID: 2163 RVA: 0x00049BC4 File Offset: 0x00047DC4
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

			// Token: 0x040011F0 RID: 4592
			private readonly Func<TSource, int, CancellationToken, UniTask<bool>> predicate;

			// Token: 0x040011F1 RID: 4593
			private int index;
		}
	}
}
