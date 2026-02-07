using System;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200001B RID: 27
	public interface IUniTaskAsyncEnumerable<out T>
	{
		// Token: 0x06000079 RID: 121
		IUniTaskAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken));
	}
}
