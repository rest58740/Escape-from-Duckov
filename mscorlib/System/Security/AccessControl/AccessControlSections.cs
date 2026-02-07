using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000501 RID: 1281
	[Flags]
	public enum AccessControlSections
	{
		// Token: 0x04002405 RID: 9221
		None = 0,
		// Token: 0x04002406 RID: 9222
		Audit = 1,
		// Token: 0x04002407 RID: 9223
		Access = 2,
		// Token: 0x04002408 RID: 9224
		Owner = 4,
		// Token: 0x04002409 RID: 9225
		Group = 8,
		// Token: 0x0400240A RID: 9226
		All = 15
	}
}
