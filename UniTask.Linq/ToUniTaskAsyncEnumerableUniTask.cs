using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200007E RID: 126
	internal class ToUniTaskAsyncEnumerableUniTask<T> : IUniTaskAsyncEnumerable<T>
	{
		// Token: 0x060003BC RID: 956 RVA: 0x0000DDB5 File Offset: 0x0000BFB5
		public ToUniTaskAsyncEnumerableUniTask(UniTask<T> source)
		{
			this.source = source;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000DDC4 File Offset: 0x0000BFC4
		public IUniTaskAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new ToUniTaskAsyncEnumerableUniTask<T>._ToUniTaskAsyncEnumerableUniTask(this.source, cancellationToken);
		}

		// Token: 0x0400017A RID: 378
		private readonly UniTask<T> source;

		// Token: 0x020001F2 RID: 498
		private class _ToUniTaskAsyncEnumerableUniTask : IUniTaskAsyncEnumerator<T>, IUniTaskAsyncDisposable
		{
			// Token: 0x060008B9 RID: 2233 RVA: 0x0004CE2A File Offset: 0x0004B02A
			public _ToUniTaskAsyncEnumerableUniTask(UniTask<T> source, CancellationToken cancellationToken)
			{
				this.source = source;
				this.cancellationToken = cancellationToken;
				this.called = false;
			}

			// Token: 0x1700004F RID: 79
			// (get) Token: 0x060008BA RID: 2234 RVA: 0x0004CE47 File Offset: 0x0004B047
			public T Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x060008BB RID: 2235 RVA: 0x0004CE50 File Offset: 0x0004B050
			public UniTask<bool> MoveNextAsync()
			{
				ToUniTaskAsyncEnumerableUniTask<T>._ToUniTaskAsyncEnumerableUniTask.<MoveNextAsync>d__7 <MoveNextAsync>d__;
				<MoveNextAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
				<MoveNextAsync>d__.<>4__this = this;
				<MoveNextAsync>d__.<>1__state = -1;
				<MoveNextAsync>d__.<>t__builder.Start<ToUniTaskAsyncEnumerableUniTask<T>._ToUniTaskAsyncEnumerableUniTask.<MoveNextAsync>d__7>(ref <MoveNextAsync>d__);
				return <MoveNextAsync>d__.<>t__builder.Task;
			}

			// Token: 0x060008BC RID: 2236 RVA: 0x0004CE94 File Offset: 0x0004B094
			public UniTask DisposeAsync()
			{
				return default(UniTask);
			}

			// Token: 0x040012E4 RID: 4836
			private readonly UniTask<T> source;

			// Token: 0x040012E5 RID: 4837
			private CancellationToken cancellationToken;

			// Token: 0x040012E6 RID: 4838
			private T current;

			// Token: 0x040012E7 RID: 4839
			private bool called;
		}
	}
}
