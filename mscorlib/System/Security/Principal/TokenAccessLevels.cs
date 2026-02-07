using System;

namespace System.Security.Principal
{
	// Token: 0x020004DD RID: 1245
	[Flags]
	public enum TokenAccessLevels
	{
		// Token: 0x040022A6 RID: 8870
		AssignPrimary = 1,
		// Token: 0x040022A7 RID: 8871
		Duplicate = 2,
		// Token: 0x040022A8 RID: 8872
		Impersonate = 4,
		// Token: 0x040022A9 RID: 8873
		Query = 8,
		// Token: 0x040022AA RID: 8874
		QuerySource = 16,
		// Token: 0x040022AB RID: 8875
		AdjustPrivileges = 32,
		// Token: 0x040022AC RID: 8876
		AdjustGroups = 64,
		// Token: 0x040022AD RID: 8877
		AdjustDefault = 128,
		// Token: 0x040022AE RID: 8878
		AdjustSessionId = 256,
		// Token: 0x040022AF RID: 8879
		Read = 131080,
		// Token: 0x040022B0 RID: 8880
		Write = 131296,
		// Token: 0x040022B1 RID: 8881
		AllAccess = 983551,
		// Token: 0x040022B2 RID: 8882
		MaximumAllowed = 33554432
	}
}
