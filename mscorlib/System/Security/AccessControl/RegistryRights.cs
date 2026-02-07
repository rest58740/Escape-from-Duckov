using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000547 RID: 1351
	[Flags]
	public enum RegistryRights
	{
		// Token: 0x040024DE RID: 9438
		QueryValues = 1,
		// Token: 0x040024DF RID: 9439
		SetValue = 2,
		// Token: 0x040024E0 RID: 9440
		CreateSubKey = 4,
		// Token: 0x040024E1 RID: 9441
		EnumerateSubKeys = 8,
		// Token: 0x040024E2 RID: 9442
		Notify = 16,
		// Token: 0x040024E3 RID: 9443
		CreateLink = 32,
		// Token: 0x040024E4 RID: 9444
		Delete = 65536,
		// Token: 0x040024E5 RID: 9445
		ReadPermissions = 131072,
		// Token: 0x040024E6 RID: 9446
		WriteKey = 131078,
		// Token: 0x040024E7 RID: 9447
		ReadKey = 131097,
		// Token: 0x040024E8 RID: 9448
		ExecuteKey = 131097,
		// Token: 0x040024E9 RID: 9449
		ChangePermissions = 262144,
		// Token: 0x040024EA RID: 9450
		TakeOwnership = 524288,
		// Token: 0x040024EB RID: 9451
		FullControl = 983103
	}
}
