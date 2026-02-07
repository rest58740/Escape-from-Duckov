using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000051 RID: 81
	internal class Return<TValue> : IUniTaskAsyncEnumerable<TValue>
	{
		// Token: 0x0600032C RID: 812 RVA: 0x0000C013 File Offset: 0x0000A213
		public Return(TValue value)
		{
			this.value = value;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000C022 File Offset: 0x0000A222
		public IUniTaskAsyncEnumerator<TValue> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Return<TValue>._Return(this.value, cancellationToken);
		}

		// Token: 0x0400012F RID: 303
		private readonly TValue value;

		// Token: 0x02000189 RID: 393
		private class _Return : IUniTaskAsyncEnumerator<TValue>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000756 RID: 1878 RVA: 0x0003EDDE File Offset: 0x0003CFDE
			public _Return(TValue value, CancellationToken cancellationToken)
			{
				this.value = value;
				this.cancellationToken = cancellationToken;
				this.called = false;
			}

			// Token: 0x17000036 RID: 54
			// (get) Token: 0x06000757 RID: 1879 RVA: 0x0003EDFB File Offset: 0x0003CFFB
			public TValue Current
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x06000758 RID: 1880 RVA: 0x0003EE03 File Offset: 0x0003D003
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (!this.called)
				{
					this.called = true;
					return CompletedTasks.True;
				}
				return CompletedTasks.False;
			}

			// Token: 0x06000759 RID: 1881 RVA: 0x0003EE2C File Offset: 0x0003D02C
			public UniTask DisposeAsync()
			{
				return default(UniTask);
			}

			// Token: 0x04000EB9 RID: 3769
			private readonly TValue value;

			// Token: 0x04000EBA RID: 3770
			private CancellationToken cancellationToken;

			// Token: 0x04000EBB RID: 3771
			private bool called;
		}
	}
}
