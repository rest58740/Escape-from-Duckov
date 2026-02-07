using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000022 RID: 34
	internal sealed class Distinct<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000263 RID: 611 RVA: 0x0000903E File Offset: 0x0000723E
		public Distinct(IUniTaskAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			this.source = source;
			this.comparer = comparer;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009054 File Offset: 0x00007254
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Distinct<TSource>._Distinct(this.source, this.comparer, cancellationToken);
		}

		// Token: 0x040000A3 RID: 163
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000A4 RID: 164
		private readonly IEqualityComparer<TSource> comparer;

		// Token: 0x020000F6 RID: 246
		private class _Distinct : AsyncEnumeratorBase<TSource, TSource>
		{
			// Token: 0x06000593 RID: 1427 RVA: 0x00025C81 File Offset: 0x00023E81
			public _Distinct(IUniTaskAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.set = new HashSet<TSource>(comparer);
			}

			// Token: 0x06000594 RID: 1428 RVA: 0x00025C98 File Offset: 0x00023E98
			protected override bool TryMoveNextCore(bool sourceHasCurrent, out bool result)
			{
				if (!sourceHasCurrent)
				{
					result = false;
					return true;
				}
				TSource sourceCurrent = base.SourceCurrent;
				if (this.set.Add(sourceCurrent))
				{
					base.Current = sourceCurrent;
					result = true;
					return true;
				}
				result = false;
				return false;
			}

			// Token: 0x04000891 RID: 2193
			private readonly HashSet<TSource> set;
		}
	}
}
