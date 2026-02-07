using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007F5 RID: 2037
	public interface IAsyncStateMachine
	{
		// Token: 0x06004606 RID: 17926
		void MoveNext();

		// Token: 0x06004607 RID: 17927
		void SetStateMachine(IAsyncStateMachine stateMachine);
	}
}
