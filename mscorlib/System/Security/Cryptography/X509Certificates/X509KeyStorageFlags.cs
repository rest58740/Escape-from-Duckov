using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020004D8 RID: 1240
	[Flags]
	public enum X509KeyStorageFlags
	{
		// Token: 0x04002291 RID: 8849
		DefaultKeySet = 0,
		// Token: 0x04002292 RID: 8850
		UserKeySet = 1,
		// Token: 0x04002293 RID: 8851
		MachineKeySet = 2,
		// Token: 0x04002294 RID: 8852
		Exportable = 4,
		// Token: 0x04002295 RID: 8853
		UserProtected = 8,
		// Token: 0x04002296 RID: 8854
		PersistKeySet = 16,
		// Token: 0x04002297 RID: 8855
		EphemeralKeySet = 32
	}
}
