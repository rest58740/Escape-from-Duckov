using System;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000CD RID: 205
	public interface ICacheNotificationReceiver
	{
		// Token: 0x060005C3 RID: 1475
		void OnFreed();

		// Token: 0x060005C4 RID: 1476
		void OnClaimed();
	}
}
