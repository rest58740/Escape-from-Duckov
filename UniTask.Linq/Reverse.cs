using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000052 RID: 82
	internal sealed class Reverse<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x0600032E RID: 814 RVA: 0x0000C030 File Offset: 0x0000A230
		public Reverse(IUniTaskAsyncEnumerable<TSource> source)
		{
			this.source = source;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000C03F File Offset: 0x0000A23F
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Reverse<TSource>._Reverse(this.source, cancellationToken);
		}

		// Token: 0x04000130 RID: 304
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0200018A RID: 394
		private sealed class _Reverse : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600075A RID: 1882 RVA: 0x0003EE42 File Offset: 0x0003D042
			public _Reverse(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
			{
				this.source = source;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000037 RID: 55
			// (get) Token: 0x0600075B RID: 1883 RVA: 0x0003EE58 File Offset: 0x0003D058
			// (set) Token: 0x0600075C RID: 1884 RVA: 0x0003EE60 File Offset: 0x0003D060
			public TSource Current { get; private set; }

			// Token: 0x0600075D RID: 1885 RVA: 0x0003EE6C File Offset: 0x0003D06C
			public UniTask<bool> MoveNextAsync()
			{
				Reverse<TSource>._Reverse.<MoveNextAsync>d__9 <MoveNextAsync>d__;
				<MoveNextAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
				<MoveNextAsync>d__.<>4__this = this;
				<MoveNextAsync>d__.<>1__state = -1;
				<MoveNextAsync>d__.<>t__builder.Start<Reverse<TSource>._Reverse.<MoveNextAsync>d__9>(ref <MoveNextAsync>d__);
				return <MoveNextAsync>d__.<>t__builder.Task;
			}

			// Token: 0x0600075E RID: 1886 RVA: 0x0003EEB0 File Offset: 0x0003D0B0
			public UniTask DisposeAsync()
			{
				return default(UniTask);
			}

			// Token: 0x04000EBC RID: 3772
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000EBD RID: 3773
			private CancellationToken cancellationToken;

			// Token: 0x04000EBE RID: 3774
			private TSource[] array;

			// Token: 0x04000EBF RID: 3775
			private int index;
		}
	}
}
