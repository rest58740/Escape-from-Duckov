using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000042 RID: 66
	internal class Never<T> : IUniTaskAsyncEnumerable<T>
	{
		// Token: 0x06000301 RID: 769 RVA: 0x0000B983 File Offset: 0x00009B83
		private Never()
		{
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000B98B File Offset: 0x00009B8B
		public IUniTaskAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Never<T>._Never(cancellationToken);
		}

		// Token: 0x04000104 RID: 260
		public static readonly IUniTaskAsyncEnumerable<T> Instance = new Never<T>();

		// Token: 0x0200017B RID: 379
		private class _Never : IUniTaskAsyncEnumerator<T>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000719 RID: 1817 RVA: 0x0003DE2A File Offset: 0x0003C02A
			public _Never(CancellationToken cancellationToken)
			{
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x0600071A RID: 1818 RVA: 0x0003DE3C File Offset: 0x0003C03C
			public T Current
			{
				get
				{
					return default(T);
				}
			}

			// Token: 0x0600071B RID: 1819 RVA: 0x0003DE54 File Offset: 0x0003C054
			public UniTask<bool> MoveNextAsync()
			{
				UniTaskCompletionSource<bool> uniTaskCompletionSource = new UniTaskCompletionSource<bool>();
				this.cancellationToken.Register(delegate(object state)
				{
					((UniTaskCompletionSource<bool>)state).TrySetCanceled(this.cancellationToken);
				}, uniTaskCompletionSource);
				return uniTaskCompletionSource.Task;
			}

			// Token: 0x0600071C RID: 1820 RVA: 0x0003DE88 File Offset: 0x0003C088
			public UniTask DisposeAsync()
			{
				return default(UniTask);
			}

			// Token: 0x04000E6C RID: 3692
			private CancellationToken cancellationToken;
		}
	}
}
