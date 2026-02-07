using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000485 RID: 1157
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum CspProviderFlags
	{
		// Token: 0x04002146 RID: 8518
		NoFlags = 0,
		// Token: 0x04002147 RID: 8519
		UseMachineKeyStore = 1,
		// Token: 0x04002148 RID: 8520
		UseDefaultKeyContainer = 2,
		// Token: 0x04002149 RID: 8521
		UseNonExportableKey = 4,
		// Token: 0x0400214A RID: 8522
		UseExistingKey = 8,
		// Token: 0x0400214B RID: 8523
		UseArchivableKey = 16,
		// Token: 0x0400214C RID: 8524
		UseUserProtectedKey = 32,
		// Token: 0x0400214D RID: 8525
		NoPrompt = 64,
		// Token: 0x0400214E RID: 8526
		CreateEphemeralKey = 128
	}
}
