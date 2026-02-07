using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000062 RID: 98
	public interface IAsyncSelectEventHandler<T> : IDisposable
	{
		// Token: 0x060002A2 RID: 674
		UniTask<T> OnSelectAsync();
	}
}
