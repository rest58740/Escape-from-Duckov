using System;

namespace Duckov.Tasks
{
	// Token: 0x02000387 RID: 903
	public interface ITaskBehaviour
	{
		// Token: 0x06001F6F RID: 8047
		void Begin();

		// Token: 0x06001F70 RID: 8048
		bool IsPending();

		// Token: 0x06001F71 RID: 8049
		bool IsComplete();

		// Token: 0x06001F72 RID: 8050 RVA: 0x0006F4D8 File Offset: 0x0006D6D8
		void Skip()
		{
		}
	}
}
