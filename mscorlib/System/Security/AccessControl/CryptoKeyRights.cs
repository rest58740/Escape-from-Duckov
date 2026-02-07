using System;

namespace System.Security.AccessControl
{
	// Token: 0x0200051D RID: 1309
	[Flags]
	public enum CryptoKeyRights
	{
		// Token: 0x04002473 RID: 9331
		ReadData = 1,
		// Token: 0x04002474 RID: 9332
		WriteData = 2,
		// Token: 0x04002475 RID: 9333
		ReadExtendedAttributes = 8,
		// Token: 0x04002476 RID: 9334
		WriteExtendedAttributes = 16,
		// Token: 0x04002477 RID: 9335
		ReadAttributes = 128,
		// Token: 0x04002478 RID: 9336
		WriteAttributes = 256,
		// Token: 0x04002479 RID: 9337
		Delete = 65536,
		// Token: 0x0400247A RID: 9338
		ReadPermissions = 131072,
		// Token: 0x0400247B RID: 9339
		ChangePermissions = 262144,
		// Token: 0x0400247C RID: 9340
		TakeOwnership = 524288,
		// Token: 0x0400247D RID: 9341
		Synchronize = 1048576,
		// Token: 0x0400247E RID: 9342
		FullControl = 2032027,
		// Token: 0x0400247F RID: 9343
		GenericAll = 268435456,
		// Token: 0x04002480 RID: 9344
		GenericExecute = 536870912,
		// Token: 0x04002481 RID: 9345
		GenericWrite = 1073741824,
		// Token: 0x04002482 RID: 9346
		GenericRead = -2147483648
	}
}
