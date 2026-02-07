using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000060 RID: 96
	public interface IAsyncTextSelectionEventHandler<T> : IDisposable
	{
		// Token: 0x060002A0 RID: 672
		UniTask<T> OnTextSelectionAsync();
	}
}
