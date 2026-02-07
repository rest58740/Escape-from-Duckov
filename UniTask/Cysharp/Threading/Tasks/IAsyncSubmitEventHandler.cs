using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000063 RID: 99
	public interface IAsyncSubmitEventHandler<T> : IDisposable
	{
		// Token: 0x060002A3 RID: 675
		UniTask<T> OnSubmitAsync();
	}
}
