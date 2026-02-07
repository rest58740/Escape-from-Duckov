using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200001C RID: 28
	public interface IUniTaskAsyncEnumerator<out T> : IUniTaskAsyncDisposable
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600007A RID: 122
		T Current { get; }

		// Token: 0x0600007B RID: 123
		UniTask<bool> MoveNextAsync();
	}
}
