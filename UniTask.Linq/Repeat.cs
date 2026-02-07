using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000050 RID: 80
	internal class Repeat<TElement> : IUniTaskAsyncEnumerable<TElement>
	{
		// Token: 0x0600032A RID: 810 RVA: 0x0000BFE9 File Offset: 0x0000A1E9
		public Repeat(TElement element, int count)
		{
			this.element = element;
			this.count = count;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000BFFF File Offset: 0x0000A1FF
		public IUniTaskAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Repeat<TElement>._Repeat(this.element, this.count, cancellationToken);
		}

		// Token: 0x0400012D RID: 301
		private readonly TElement element;

		// Token: 0x0400012E RID: 302
		private readonly int count;

		// Token: 0x02000188 RID: 392
		private class _Repeat : IUniTaskAsyncEnumerator<TElement>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000752 RID: 1874 RVA: 0x0003ED62 File Offset: 0x0003CF62
			public _Repeat(TElement element, int count, CancellationToken cancellationToken)
			{
				this.element = element;
				this.count = count;
				this.cancellationToken = cancellationToken;
				this.remaining = count;
			}

			// Token: 0x17000035 RID: 53
			// (get) Token: 0x06000753 RID: 1875 RVA: 0x0003ED86 File Offset: 0x0003CF86
			public TElement Current
			{
				get
				{
					return this.element;
				}
			}

			// Token: 0x06000754 RID: 1876 RVA: 0x0003ED90 File Offset: 0x0003CF90
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				int num = this.remaining;
				this.remaining = num - 1;
				if (num != 0)
				{
					return CompletedTasks.True;
				}
				return CompletedTasks.False;
			}

			// Token: 0x06000755 RID: 1877 RVA: 0x0003EDC8 File Offset: 0x0003CFC8
			public UniTask DisposeAsync()
			{
				return default(UniTask);
			}

			// Token: 0x04000EB5 RID: 3765
			private readonly TElement element;

			// Token: 0x04000EB6 RID: 3766
			private readonly int count;

			// Token: 0x04000EB7 RID: 3767
			private int remaining;

			// Token: 0x04000EB8 RID: 3768
			private CancellationToken cancellationToken;
		}
	}
}
