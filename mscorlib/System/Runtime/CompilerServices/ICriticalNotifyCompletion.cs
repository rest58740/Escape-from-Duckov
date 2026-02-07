using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007F7 RID: 2039
	public interface ICriticalNotifyCompletion : INotifyCompletion
	{
		// Token: 0x06004609 RID: 17929
		void UnsafeOnCompleted(Action continuation);
	}
}
