using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000073 RID: 115
	internal sealed class TakeWhileAwaitWithCancellation<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x060003A0 RID: 928 RVA: 0x0000D70B File Offset: 0x0000B90B
		public TakeWhileAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000D721 File Offset: 0x0000B921
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new TakeWhileAwaitWithCancellation<TSource>._TakeWhileAwaitWithCancellation(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x04000172 RID: 370
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000173 RID: 371
		private readonly Func<TSource, CancellationToken, UniTask<bool>> predicate;

		// Token: 0x020001DA RID: 474
		private class _TakeWhileAwaitWithCancellation : AsyncEnumeratorAwaitSelectorBase<TSource, TSource, bool>
		{
			// Token: 0x0600086E RID: 2158 RVA: 0x00049B44 File Offset: 0x00047D44
			public _TakeWhileAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.predicate = predicate;
			}

			// Token: 0x0600086F RID: 2159 RVA: 0x00049B55 File Offset: 0x00047D55
			protected override UniTask<bool> TransformAsync(TSource sourceCurrent)
			{
				return this.predicate(sourceCurrent, this.cancellationToken);
			}

			// Token: 0x06000870 RID: 2160 RVA: 0x00049B69 File Offset: 0x00047D69
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

			// Token: 0x040011EF RID: 4591
			private Func<TSource, CancellationToken, UniTask<bool>> predicate;
		}
	}
}
