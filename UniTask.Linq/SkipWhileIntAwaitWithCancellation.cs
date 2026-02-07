using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000067 RID: 103
	internal sealed class SkipWhileIntAwaitWithCancellation<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x0600035C RID: 860 RVA: 0x0000C5E9 File Offset: 0x0000A7E9
		public SkipWhileIntAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<bool>> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000C5FF File Offset: 0x0000A7FF
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SkipWhileIntAwaitWithCancellation<TSource>._SkipWhileIntAwaitWithCancellation(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x0400015C RID: 348
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400015D RID: 349
		private readonly Func<TSource, int, CancellationToken, UniTask<bool>> predicate;

		// Token: 0x020001A2 RID: 418
		private class _SkipWhileIntAwaitWithCancellation : AsyncEnumeratorAwaitSelectorBase<TSource, TSource, bool>
		{
			// Token: 0x060007DD RID: 2013 RVA: 0x00042067 File Offset: 0x00040267
			public _SkipWhileIntAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.predicate = predicate;
			}

			// Token: 0x060007DE RID: 2014 RVA: 0x00042078 File Offset: 0x00040278
			protected override UniTask<bool> TransformAsync(TSource sourceCurrent)
			{
				if (this.predicate == null)
				{
					return CompletedTasks.False;
				}
				Func<TSource, int, CancellationToken, UniTask<bool>> func = this.predicate;
				int num = this.index;
				this.index = checked(num + 1);
				return func(sourceCurrent, num, this.cancellationToken);
			}

			// Token: 0x060007DF RID: 2015 RVA: 0x000420B6 File Offset: 0x000402B6
			protected override bool TrySetCurrentCore(bool awaitResult, out bool terminateIteration)
			{
				terminateIteration = false;
				if (!awaitResult)
				{
					this.predicate = null;
					base.Current = base.SourceCurrent;
					return true;
				}
				return false;
			}

			// Token: 0x04000FA2 RID: 4002
			private Func<TSource, int, CancellationToken, UniTask<bool>> predicate;

			// Token: 0x04000FA3 RID: 4003
			private int index;
		}
	}
}
