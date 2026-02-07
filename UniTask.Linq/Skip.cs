using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200005E RID: 94
	internal sealed class Skip<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x0600034A RID: 842 RVA: 0x0000C447 File Offset: 0x0000A647
		public Skip(IUniTaskAsyncEnumerable<TSource> source, int count)
		{
			this.source = source;
			this.count = count;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000C45D File Offset: 0x0000A65D
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Skip<TSource>._Skip(this.source, this.count, cancellationToken);
		}

		// Token: 0x04000149 RID: 329
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400014A RID: 330
		private readonly int count;

		// Token: 0x02000199 RID: 409
		private sealed class _Skip : AsyncEnumeratorBase<TSource, TSource>
		{
			// Token: 0x060007B2 RID: 1970 RVA: 0x000416C2 File Offset: 0x0003F8C2
			public _Skip(IUniTaskAsyncEnumerable<TSource> source, int count, CancellationToken cancellationToken) : base(source, cancellationToken)
			{
				this.count = count;
			}

			// Token: 0x060007B3 RID: 1971 RVA: 0x000416D4 File Offset: 0x0003F8D4
			protected override bool TryMoveNextCore(bool sourceHasCurrent, out bool result)
			{
				if (!sourceHasCurrent)
				{
					result = false;
					return true;
				}
				int num = this.count;
				int num2 = this.index;
				this.index = checked(num2 + 1);
				if (num <= num2)
				{
					base.Current = base.SourceCurrent;
					result = true;
					return true;
				}
				result = false;
				return false;
			}

			// Token: 0x04000F78 RID: 3960
			private readonly int count;

			// Token: 0x04000F79 RID: 3961
			private int index;
		}
	}
}
