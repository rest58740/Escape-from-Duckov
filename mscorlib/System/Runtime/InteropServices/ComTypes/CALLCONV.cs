using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007C6 RID: 1990
	[Serializable]
	public enum CALLCONV
	{
		// Token: 0x04002CDD RID: 11485
		CC_CDECL = 1,
		// Token: 0x04002CDE RID: 11486
		CC_MSCPASCAL,
		// Token: 0x04002CDF RID: 11487
		CC_PASCAL = 2,
		// Token: 0x04002CE0 RID: 11488
		CC_MACPASCAL,
		// Token: 0x04002CE1 RID: 11489
		CC_STDCALL,
		// Token: 0x04002CE2 RID: 11490
		CC_RESERVED,
		// Token: 0x04002CE3 RID: 11491
		CC_SYSCALL,
		// Token: 0x04002CE4 RID: 11492
		CC_MPWCDECL,
		// Token: 0x04002CE5 RID: 11493
		CC_MPWPASCAL,
		// Token: 0x04002CE6 RID: 11494
		CC_MAX
	}
}
