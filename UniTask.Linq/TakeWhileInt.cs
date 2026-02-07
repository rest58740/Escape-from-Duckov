using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000070 RID: 112
	internal sealed class TakeWhileInt<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x0600039A RID: 922 RVA: 0x0000D68D File Offset: 0x0000B88D
		public TakeWhileInt(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000D6A3 File Offset: 0x0000B8A3
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new TakeWhileInt<TSource>._TakeWhileInt(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x0400016C RID: 364
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400016D RID: 365
		private readonly Func<TSource, int, bool> predicate;

		// Token: 0x020001D7 RID: 471
		private class _TakeWhileInt : AsyncEnumeratorBase<TSource, TSource>
		{
			// Token: 0x06000866 RID: 2150 RVA: 0x00049A5B File Offset: 0x00047C5B
			public _TakeWhileInt(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.predicate = predicate;
			}

			// Token: 0x06000867 RID: 2151 RVA: 0x00049A6C File Offset: 0x00047C6C
			protected override bool TryMoveNextCore(bool sourceHasCurrent, out bool result)
			{
				if (sourceHasCurrent)
				{
					Func<TSource, int, bool> func = this.predicate;
					TSource sourceCurrent = base.SourceCurrent;
					int num = this.index;
					this.index = checked(num + 1);
					if (func(sourceCurrent, num))
					{
						base.Current = base.SourceCurrent;
						result = true;
						return true;
					}
				}
				result = false;
				return true;
			}

			// Token: 0x040011EA RID: 4586
			private readonly Func<TSource, int, bool> predicate;

			// Token: 0x040011EB RID: 4587
			private int index;
		}
	}
}
