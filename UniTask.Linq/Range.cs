using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200004F RID: 79
	internal class Range : IUniTaskAsyncEnumerable<int>
	{
		// Token: 0x06000328 RID: 808 RVA: 0x0000BFBD File Offset: 0x0000A1BD
		public Range(int start, int count)
		{
			this.start = start;
			this.end = start + count;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000BFD5 File Offset: 0x0000A1D5
		public IUniTaskAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Range._Range(this.start, this.end, cancellationToken);
		}

		// Token: 0x0400012B RID: 299
		private readonly int start;

		// Token: 0x0400012C RID: 300
		private readonly int end;

		// Token: 0x02000187 RID: 391
		private class _Range : IUniTaskAsyncEnumerator<int>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600074E RID: 1870 RVA: 0x0003ECE7 File Offset: 0x0003CEE7
			public _Range(int start, int end, CancellationToken cancellationToken)
			{
				this.start = start;
				this.end = end;
				this.cancellationToken = cancellationToken;
				this.current = start - 1;
			}

			// Token: 0x17000034 RID: 52
			// (get) Token: 0x0600074F RID: 1871 RVA: 0x0003ED0D File Offset: 0x0003CF0D
			public int Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x06000750 RID: 1872 RVA: 0x0003ED15 File Offset: 0x0003CF15
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				this.current++;
				if (this.current != this.end)
				{
					return CompletedTasks.True;
				}
				return CompletedTasks.False;
			}

			// Token: 0x06000751 RID: 1873 RVA: 0x0003ED4C File Offset: 0x0003CF4C
			public UniTask DisposeAsync()
			{
				return default(UniTask);
			}

			// Token: 0x04000EB1 RID: 3761
			private readonly int start;

			// Token: 0x04000EB2 RID: 3762
			private readonly int end;

			// Token: 0x04000EB3 RID: 3763
			private int current;

			// Token: 0x04000EB4 RID: 3764
			private CancellationToken cancellationToken;
		}
	}
}
