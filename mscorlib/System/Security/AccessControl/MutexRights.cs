using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000533 RID: 1331
	[Flags]
	public enum MutexRights
	{
		// Token: 0x040024B1 RID: 9393
		Modify = 1,
		// Token: 0x040024B2 RID: 9394
		Delete = 65536,
		// Token: 0x040024B3 RID: 9395
		ReadPermissions = 131072,
		// Token: 0x040024B4 RID: 9396
		ChangePermissions = 262144,
		// Token: 0x040024B5 RID: 9397
		TakeOwnership = 524288,
		// Token: 0x040024B6 RID: 9398
		Synchronize = 1048576,
		// Token: 0x040024B7 RID: 9399
		FullControl = 2031617
	}
}
