using System;

namespace System
{
	// Token: 0x02000141 RID: 321
	public interface IObservable<out T>
	{
		// Token: 0x06000C01 RID: 3073
		IDisposable Subscribe(IObserver<T> observer);
	}
}
