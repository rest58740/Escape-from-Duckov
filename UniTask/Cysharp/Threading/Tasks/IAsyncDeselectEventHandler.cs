using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000061 RID: 97
	public interface IAsyncDeselectEventHandler<T> : IDisposable
	{
		// Token: 0x060002A1 RID: 673
		UniTask<T> OnDeselectAsync();
	}
}
