using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000072 RID: 114
	internal sealed class TakeWhileIntAwait<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x0600039E RID: 926 RVA: 0x0000D6E1 File Offset: 0x0000B8E1
		public TakeWhileIntAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<bool>> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000D6F7 File Offset: 0x0000B8F7
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new TakeWhileIntAwait<TSource>._TakeWhileIntAwait(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x04000170 RID: 368
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000171 RID: 369
		private readonly Func<TSource, int, UniTask<bool>> predicate;

		// Token: 0x020001D9 RID: 473
		private class _TakeWhileIntAwait : AsyncEnumeratorAwaitSelectorBase<TSource, TSource, bool>
		{
			// Token: 0x0600086B RID: 2155 RVA: 0x00049AEE File Offset: 0x00047CEE
			public _TakeWhileIntAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<bool>> predicate, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.predicate = predicate;
			}

			// Token: 0x0600086C RID: 2156 RVA: 0x00049B00 File Offset: 0x00047D00
			protected override UniTask<bool> TransformAsync(TSource sourceCurrent)
			{
				Func<TSource, int, UniTask<bool>> func = this.predicate;
				int num = this.index;
				this.index = checked(num + 1);
				return func(sourceCurrent, num);
			}

			// Token: 0x0600086D RID: 2157 RVA: 0x00049B2A File Offset: 0x00047D2A
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

			// Token: 0x040011ED RID: 4589
			private readonly Func<TSource, int, UniTask<bool>> predicate;

			// Token: 0x040011EE RID: 4590
			private int index;
		}
	}
}
