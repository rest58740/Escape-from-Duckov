using System;

namespace System.Security.AccessControl
{
	// Token: 0x0200051A RID: 1306
	[Flags]
	public enum ControlFlags
	{
		// Token: 0x04002461 RID: 9313
		None = 0,
		// Token: 0x04002462 RID: 9314
		OwnerDefaulted = 1,
		// Token: 0x04002463 RID: 9315
		GroupDefaulted = 2,
		// Token: 0x04002464 RID: 9316
		DiscretionaryAclPresent = 4,
		// Token: 0x04002465 RID: 9317
		DiscretionaryAclDefaulted = 8,
		// Token: 0x04002466 RID: 9318
		SystemAclPresent = 16,
		// Token: 0x04002467 RID: 9319
		SystemAclDefaulted = 32,
		// Token: 0x04002468 RID: 9320
		DiscretionaryAclUntrusted = 64,
		// Token: 0x04002469 RID: 9321
		ServerSecurity = 128,
		// Token: 0x0400246A RID: 9322
		DiscretionaryAclAutoInheritRequired = 256,
		// Token: 0x0400246B RID: 9323
		SystemAclAutoInheritRequired = 512,
		// Token: 0x0400246C RID: 9324
		DiscretionaryAclAutoInherited = 1024,
		// Token: 0x0400246D RID: 9325
		SystemAclAutoInherited = 2048,
		// Token: 0x0400246E RID: 9326
		DiscretionaryAclProtected = 4096,
		// Token: 0x0400246F RID: 9327
		SystemAclProtected = 8192,
		// Token: 0x04002470 RID: 9328
		RMControlValid = 16384,
		// Token: 0x04002471 RID: 9329
		SelfRelative = 32768
	}
}
