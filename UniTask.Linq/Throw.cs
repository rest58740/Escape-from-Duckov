using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000075 RID: 117
	internal class Throw<TValue> : IUniTaskAsyncEnumerable<TValue>
	{
		// Token: 0x060003A4 RID: 932 RVA: 0x0000D75F File Offset: 0x0000B95F
		public Throw(Exception exception)
		{
			this.exception = exception;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000D76E File Offset: 0x0000B96E
		public IUniTaskAsyncEnumerator<TValue> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Throw<TValue>._Throw(this.exception, cancellationToken);
		}

		// Token: 0x04000176 RID: 374
		private readonly Exception exception;

		// Token: 0x020001DC RID: 476
		private class _Throw : IUniTaskAsyncEnumerator<TValue>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000874 RID: 2164 RVA: 0x00049BDE File Offset: 0x00047DDE
			public _Throw(Exception exception, CancellationToken cancellationToken)
			{
				this.exception = exception;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000048 RID: 72
			// (get) Token: 0x06000875 RID: 2165 RVA: 0x00049BF4 File Offset: 0x00047DF4
			public TValue Current
			{
				get
				{
					return default(TValue);
				}
			}

			// Token: 0x06000876 RID: 2166 RVA: 0x00049C0A File Offset: 0x00047E0A
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				return UniTask.FromException<bool>(this.exception);
			}

			// Token: 0x06000877 RID: 2167 RVA: 0x00049C24 File Offset: 0x00047E24
			public UniTask DisposeAsync()
			{
				return default(UniTask);
			}

			// Token: 0x040011F2 RID: 4594
			private readonly Exception exception;

			// Token: 0x040011F3 RID: 4595
			private CancellationToken cancellationToken;
		}
	}
}
