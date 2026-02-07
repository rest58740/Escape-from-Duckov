using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200005D RID: 93
	public interface IAsyncValueChangedEventHandler<T> : IDisposable
	{
		// Token: 0x0600029D RID: 669
		UniTask<T> OnValueChangedAsync();
	}
}
