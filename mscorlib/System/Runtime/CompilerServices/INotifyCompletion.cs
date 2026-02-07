using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007F6 RID: 2038
	public interface INotifyCompletion
	{
		// Token: 0x06004608 RID: 17928
		void OnCompleted(Action continuation);
	}
}
