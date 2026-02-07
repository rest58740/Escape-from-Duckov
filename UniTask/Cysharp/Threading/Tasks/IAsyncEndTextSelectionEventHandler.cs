using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200005F RID: 95
	public interface IAsyncEndTextSelectionEventHandler<T> : IDisposable
	{
		// Token: 0x0600029F RID: 671
		UniTask<T> OnEndTextSelectionAsync();
	}
}
