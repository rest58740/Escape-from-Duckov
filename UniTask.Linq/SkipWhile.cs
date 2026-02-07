using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000062 RID: 98
	internal sealed class SkipWhile<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000352 RID: 850 RVA: 0x0000C517 File Offset: 0x0000A717
		public SkipWhile(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000C52D File Offset: 0x0000A72D
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SkipWhile<TSource>._SkipWhile(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x04000152 RID: 338
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000153 RID: 339
		private readonly Func<TSource, bool> predicate;

		// Token: 0x0200019D RID: 413
		private class _SkipWhile : AsyncEnumeratorBase<TSource, TSource>
		{
			// Token: 0x060007D0 RID: 2000 RVA: 0x00041E91 File Offset: 0x00040091
			public _SkipWhile(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.predicate = predicate;
			}

			// Token: 0x060007D1 RID: 2001 RVA: 0x00041EA4 File Offset: 0x000400A4
			protected override bool TryMoveNextCore(bool sourceHasCurrent, out bool result)
			{
				if (!sourceHasCurrent)
				{
					result = false;
					return true;
				}
				if (this.predicate == null || !this.predicate(base.SourceCurrent))
				{
					this.predicate = null;
					base.Current = base.SourceCurrent;
					result = true;
					return true;
				}
				result = false;
				return false;
			}

			// Token: 0x04000F9B RID: 3995
			private Func<TSource, bool> predicate;
		}
	}
}
