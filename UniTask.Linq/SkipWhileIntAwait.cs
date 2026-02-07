using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000065 RID: 101
	internal sealed class SkipWhileIntAwait<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000358 RID: 856 RVA: 0x0000C595 File Offset: 0x0000A795
		public SkipWhileIntAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<bool>> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000C5AB File Offset: 0x0000A7AB
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SkipWhileIntAwait<TSource>._SkipWhileIntAwait(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x04000158 RID: 344
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000159 RID: 345
		private readonly Func<TSource, int, UniTask<bool>> predicate;

		// Token: 0x020001A0 RID: 416
		private class _SkipWhileIntAwait : AsyncEnumeratorAwaitSelectorBase<TSource, TSource, bool>
		{
			// Token: 0x060007D7 RID: 2007 RVA: 0x00041FAF File Offset: 0x000401AF
			public _SkipWhileIntAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<bool>> predicate, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.predicate = predicate;
			}

			// Token: 0x060007D8 RID: 2008 RVA: 0x00041FC0 File Offset: 0x000401C0
			protected override UniTask<bool> TransformAsync(TSource sourceCurrent)
			{
				if (this.predicate == null)
				{
					return CompletedTasks.False;
				}
				Func<TSource, int, UniTask<bool>> func = this.predicate;
				int num = this.index;
				this.index = checked(num + 1);
				return func(sourceCurrent, num);
			}

			// Token: 0x060007D9 RID: 2009 RVA: 0x00041FF8 File Offset: 0x000401F8
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

			// Token: 0x04000F9F RID: 3999
			private Func<TSource, int, UniTask<bool>> predicate;

			// Token: 0x04000FA0 RID: 4000
			private int index;
		}
	}
}
