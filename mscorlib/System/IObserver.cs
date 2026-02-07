using System;

namespace System
{
	// Token: 0x02000142 RID: 322
	public interface IObserver<in T>
	{
		// Token: 0x06000C02 RID: 3074
		void OnNext(T value);

		// Token: 0x06000C03 RID: 3075
		void OnError(Exception error);

		// Token: 0x06000C04 RID: 3076
		void OnCompleted();
	}
}
