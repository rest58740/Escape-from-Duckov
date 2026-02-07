using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000066 RID: 102
	internal sealed class SkipWhileAwaitWithCancellation<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x0600035A RID: 858 RVA: 0x0000C5BF File Offset: 0x0000A7BF
		public SkipWhileAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000C5D5 File Offset: 0x0000A7D5
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SkipWhileAwaitWithCancellation<TSource>._SkipWhileAwaitWithCancellation(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x0400015A RID: 346
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400015B RID: 347
		private readonly Func<TSource, CancellationToken, UniTask<bool>> predicate;

		// Token: 0x020001A1 RID: 417
		private class _SkipWhileAwaitWithCancellation : AsyncEnumeratorAwaitSelectorBase<TSource, TSource, bool>
		{
			// Token: 0x060007DA RID: 2010 RVA: 0x00042016 File Offset: 0x00040216
			public _SkipWhileAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.predicate = predicate;
			}

			// Token: 0x060007DB RID: 2011 RVA: 0x00042027 File Offset: 0x00040227
			protected override UniTask<bool> TransformAsync(TSource sourceCurrent)
			{
				if (this.predicate == null)
				{
					return CompletedTasks.False;
				}
				return this.predicate(sourceCurrent, this.cancellationToken);
			}

			// Token: 0x060007DC RID: 2012 RVA: 0x00042049 File Offset: 0x00040249
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

			// Token: 0x04000FA1 RID: 4001
			private Func<TSource, CancellationToken, UniTask<bool>> predicate;
		}
	}
}
