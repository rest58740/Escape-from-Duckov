using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000021 RID: 33
	[StructLayout(LayoutKind.Auto)]
	public readonly struct UniTaskCancelableAsyncEnumerable<T>
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00002EF4 File Offset: 0x000010F4
		internal UniTaskCancelableAsyncEnumerable(IUniTaskAsyncEnumerable<T> enumerable, CancellationToken cancellationToken)
		{
			this.enumerable = enumerable;
			this.cancellationToken = cancellationToken;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002F04 File Offset: 0x00001104
		public UniTaskCancelableAsyncEnumerable<T>.Enumerator GetAsyncEnumerator()
		{
			return new UniTaskCancelableAsyncEnumerable<T>.Enumerator(this.enumerable.GetAsyncEnumerator(this.cancellationToken));
		}

		// Token: 0x04000026 RID: 38
		private readonly IUniTaskAsyncEnumerable<T> enumerable;

		// Token: 0x04000027 RID: 39
		private readonly CancellationToken cancellationToken;

		// Token: 0x0200013D RID: 317
		[StructLayout(LayoutKind.Auto)]
		public readonly struct Enumerator
		{
			// Token: 0x06000766 RID: 1894 RVA: 0x000113C7 File Offset: 0x0000F5C7
			internal Enumerator(IUniTaskAsyncEnumerator<T> enumerator)
			{
				this.enumerator = enumerator;
			}

			// Token: 0x17000059 RID: 89
			// (get) Token: 0x06000767 RID: 1895 RVA: 0x000113D0 File Offset: 0x0000F5D0
			public T Current
			{
				get
				{
					return this.enumerator.Current;
				}
			}

			// Token: 0x06000768 RID: 1896 RVA: 0x000113DD File Offset: 0x0000F5DD
			public UniTask<bool> MoveNextAsync()
			{
				return this.enumerator.MoveNextAsync();
			}

			// Token: 0x06000769 RID: 1897 RVA: 0x000113EA File Offset: 0x0000F5EA
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x040001C5 RID: 453
			private readonly IUniTaskAsyncEnumerator<T> enumerator;
		}
	}
}
