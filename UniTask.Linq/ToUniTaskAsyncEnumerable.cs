using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200007C RID: 124
	internal class ToUniTaskAsyncEnumerable<T> : IUniTaskAsyncEnumerable<T>
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x0000DD7B File Offset: 0x0000BF7B
		public ToUniTaskAsyncEnumerable(IEnumerable<T> source)
		{
			this.source = source;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000DD8A File Offset: 0x0000BF8A
		public IUniTaskAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new ToUniTaskAsyncEnumerable<T>._ToUniTaskAsyncEnumerable(this.source, cancellationToken);
		}

		// Token: 0x04000178 RID: 376
		private readonly IEnumerable<T> source;

		// Token: 0x020001F0 RID: 496
		private class _ToUniTaskAsyncEnumerable : IUniTaskAsyncEnumerator<T>, IUniTaskAsyncDisposable
		{
			// Token: 0x060008B1 RID: 2225 RVA: 0x0004CD26 File Offset: 0x0004AF26
			public _ToUniTaskAsyncEnumerable(IEnumerable<T> source, CancellationToken cancellationToken)
			{
				this.source = source;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0004CD3C File Offset: 0x0004AF3C
			public T Current
			{
				get
				{
					return this.enumerator.Current;
				}
			}

			// Token: 0x060008B3 RID: 2227 RVA: 0x0004CD49 File Offset: 0x0004AF49
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.enumerator == null)
				{
					this.enumerator = this.source.GetEnumerator();
				}
				if (this.enumerator.MoveNext())
				{
					return CompletedTasks.True;
				}
				return CompletedTasks.False;
			}

			// Token: 0x060008B4 RID: 2228 RVA: 0x0004CD88 File Offset: 0x0004AF88
			public UniTask DisposeAsync()
			{
				this.enumerator.Dispose();
				return default(UniTask);
			}

			// Token: 0x040012DD RID: 4829
			private readonly IEnumerable<T> source;

			// Token: 0x040012DE RID: 4830
			private CancellationToken cancellationToken;

			// Token: 0x040012DF RID: 4831
			private IEnumerator<T> enumerator;
		}
	}
}
