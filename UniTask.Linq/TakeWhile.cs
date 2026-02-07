using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200006F RID: 111
	internal sealed class TakeWhile<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000398 RID: 920 RVA: 0x0000D663 File Offset: 0x0000B863
		public TakeWhile(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000D679 File Offset: 0x0000B879
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new TakeWhile<TSource>._TakeWhile(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x0400016A RID: 362
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400016B RID: 363
		private readonly Func<TSource, bool> predicate;

		// Token: 0x020001D6 RID: 470
		private class _TakeWhile : AsyncEnumeratorBase<TSource, TSource>
		{
			// Token: 0x06000864 RID: 2148 RVA: 0x00049A1D File Offset: 0x00047C1D
			public _TakeWhile(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.predicate = predicate;
			}

			// Token: 0x06000865 RID: 2149 RVA: 0x00049A2E File Offset: 0x00047C2E
			protected override bool TryMoveNextCore(bool sourceHasCurrent, out bool result)
			{
				if (sourceHasCurrent && this.predicate(base.SourceCurrent))
				{
					base.Current = base.SourceCurrent;
					result = true;
					return true;
				}
				result = false;
				return true;
			}

			// Token: 0x040011E9 RID: 4585
			private Func<TSource, bool> predicate;
		}
	}
}
