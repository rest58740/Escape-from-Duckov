using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000063 RID: 99
	internal sealed class SkipWhileInt<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000354 RID: 852 RVA: 0x0000C541 File Offset: 0x0000A741
		public SkipWhileInt(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			this.source = source;
			this.predicate = predicate;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000C557 File Offset: 0x0000A757
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SkipWhileInt<TSource>._SkipWhileInt(this.source, this.predicate, cancellationToken);
		}

		// Token: 0x04000154 RID: 340
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000155 RID: 341
		private readonly Func<TSource, int, bool> predicate;

		// Token: 0x0200019E RID: 414
		private class _SkipWhileInt : AsyncEnumeratorBase<TSource, TSource>
		{
			// Token: 0x060007D2 RID: 2002 RVA: 0x00041EF0 File Offset: 0x000400F0
			public _SkipWhileInt(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.predicate = predicate;
			}

			// Token: 0x060007D3 RID: 2003 RVA: 0x00041F04 File Offset: 0x00040104
			protected override bool TryMoveNextCore(bool sourceHasCurrent, out bool result)
			{
				if (sourceHasCurrent)
				{
					if (this.predicate != null)
					{
						Func<TSource, int, bool> func = this.predicate;
						TSource sourceCurrent = base.SourceCurrent;
						int num = this.index;
						this.index = checked(num + 1);
						if (func(sourceCurrent, num))
						{
							result = false;
							return false;
						}
					}
					this.predicate = null;
					base.Current = base.SourceCurrent;
					result = true;
					return true;
				}
				result = false;
				return true;
			}

			// Token: 0x04000F9C RID: 3996
			private Func<TSource, int, bool> predicate;

			// Token: 0x04000F9D RID: 3997
			private int index;
		}
	}
}
