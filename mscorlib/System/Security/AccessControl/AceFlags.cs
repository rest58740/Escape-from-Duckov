using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000506 RID: 1286
	[Flags]
	public enum AceFlags : byte
	{
		// Token: 0x04002412 RID: 9234
		None = 0,
		// Token: 0x04002413 RID: 9235
		ObjectInherit = 1,
		// Token: 0x04002414 RID: 9236
		ContainerInherit = 2,
		// Token: 0x04002415 RID: 9237
		NoPropagateInherit = 4,
		// Token: 0x04002416 RID: 9238
		InheritOnly = 8,
		// Token: 0x04002417 RID: 9239
		InheritanceFlags = 15,
		// Token: 0x04002418 RID: 9240
		Inherited = 16,
		// Token: 0x04002419 RID: 9241
		SuccessfulAccess = 64,
		// Token: 0x0400241A RID: 9242
		FailedAccess = 128,
		// Token: 0x0400241B RID: 9243
		AuditFlags = 192
	}
}
