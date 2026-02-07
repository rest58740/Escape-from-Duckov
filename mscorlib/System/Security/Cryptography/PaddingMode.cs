using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000481 RID: 1153
	[ComVisible(true)]
	[Serializable]
	public enum PaddingMode
	{
		// Token: 0x0400213A RID: 8506
		None = 1,
		// Token: 0x0400213B RID: 8507
		PKCS7,
		// Token: 0x0400213C RID: 8508
		Zeros,
		// Token: 0x0400213D RID: 8509
		ANSIX923,
		// Token: 0x0400213E RID: 8510
		ISO10126
	}
}
