using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200005E RID: 94
	public interface IAsyncEndEditEventHandler<T> : IDisposable
	{
		// Token: 0x0600029E RID: 670
		UniTask<T> OnEndEditAsync();
	}
}
