using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000023 RID: 35
	internal sealed class Distinct<TSource, TKey> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000265 RID: 613 RVA: 0x00009068 File Offset: 0x00007268
		public Distinct(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			this.source = source;
			this.keySelector = keySelector;
			this.comparer = comparer;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00009085 File Offset: 0x00007285
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Distinct<TSource, TKey>._Distinct(this.source, this.keySelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000A5 RID: 165
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000A6 RID: 166
		private readonly Func<TSource, TKey> keySelector;

		// Token: 0x040000A7 RID: 167
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x020000F7 RID: 247
		private class _Distinct : AsyncEnumeratorBase<TSource, TSource>
		{
			// Token: 0x06000595 RID: 1429 RVA: 0x00025CD2 File Offset: 0x00023ED2
			public _Distinct(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.set = new HashSet<TKey>(comparer);
				this.keySelector = keySelector;
			}

			// Token: 0x06000596 RID: 1430 RVA: 0x00025CF0 File Offset: 0x00023EF0
			protected override bool TryMoveNextCore(bool sourceHasCurrent, out bool result)
			{
				if (!sourceHasCurrent)
				{
					result = false;
					return true;
				}
				TSource sourceCurrent = base.SourceCurrent;
				if (this.set.Add(this.keySelector(sourceCurrent)))
				{
					base.Current = sourceCurrent;
					result = true;
					return true;
				}
				result = false;
				return false;
			}

			// Token: 0x04000892 RID: 2194
			private readonly HashSet<TKey> set;

			// Token: 0x04000893 RID: 2195
			private readonly Func<TSource, TKey> keySelector;
		}
	}
}
