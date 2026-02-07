using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000525 RID: 1317
	[Flags]
	public enum EventWaitHandleRights
	{
		// Token: 0x04002486 RID: 9350
		Modify = 2,
		// Token: 0x04002487 RID: 9351
		Delete = 65536,
		// Token: 0x04002488 RID: 9352
		ReadPermissions = 131072,
		// Token: 0x04002489 RID: 9353
		ChangePermissions = 262144,
		// Token: 0x0400248A RID: 9354
		TakeOwnership = 524288,
		// Token: 0x0400248B RID: 9355
		Synchronize = 1048576,
		// Token: 0x0400248C RID: 9356
		FullControl = 2031619
	}
}
