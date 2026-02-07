using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200007D RID: 125
	internal class ToUniTaskAsyncEnumerableTask<T> : IUniTaskAsyncEnumerable<T>
	{
		// Token: 0x060003BA RID: 954 RVA: 0x0000DD98 File Offset: 0x0000BF98
		public ToUniTaskAsyncEnumerableTask(Task<T> source)
		{
			this.source = source;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000DDA7 File Offset: 0x0000BFA7
		public IUniTaskAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new ToUniTaskAsyncEnumerableTask<T>._ToUniTaskAsyncEnumerableTask(this.source, cancellationToken);
		}

		// Token: 0x04000179 RID: 377
		private readonly Task<T> source;

		// Token: 0x020001F1 RID: 497
		private class _ToUniTaskAsyncEnumerableTask : IUniTaskAsyncEnumerator<T>, IUniTaskAsyncDisposable
		{
			// Token: 0x060008B5 RID: 2229 RVA: 0x0004CDA9 File Offset: 0x0004AFA9
			public _ToUniTaskAsyncEnumerableTask(Task<T> source, CancellationToken cancellationToken)
			{
				this.source = source;
				this.cancellationToken = cancellationToken;
				this.called = false;
			}

			// Token: 0x1700004E RID: 78
			// (get) Token: 0x060008B6 RID: 2230 RVA: 0x0004CDC6 File Offset: 0x0004AFC6
			public T Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x060008B7 RID: 2231 RVA: 0x0004CDD0 File Offset: 0x0004AFD0
			public UniTask<bool> MoveNextAsync()
			{
				ToUniTaskAsyncEnumerableTask<T>._ToUniTaskAsyncEnumerableTask.<MoveNextAsync>d__7 <MoveNextAsync>d__;
				<MoveNextAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
				<MoveNextAsync>d__.<>4__this = this;
				<MoveNextAsync>d__.<>1__state = -1;
				<MoveNextAsync>d__.<>t__builder.Start<ToUniTaskAsyncEnumerableTask<T>._ToUniTaskAsyncEnumerableTask.<MoveNextAsync>d__7>(ref <MoveNextAsync>d__);
				return <MoveNextAsync>d__.<>t__builder.Task;
			}

			// Token: 0x060008B8 RID: 2232 RVA: 0x0004CE14 File Offset: 0x0004B014
			public UniTask DisposeAsync()
			{
				return default(UniTask);
			}

			// Token: 0x040012E0 RID: 4832
			private readonly Task<T> source;

			// Token: 0x040012E1 RID: 4833
			private CancellationToken cancellationToken;

			// Token: 0x040012E2 RID: 4834
			private T current;

			// Token: 0x040012E3 RID: 4835
			private bool called;
		}
	}
}
