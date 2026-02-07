using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007B5 RID: 1973
	[Flags]
	[Serializable]
	public enum IMPLTYPEFLAGS
	{
		// Token: 0x04002C7B RID: 11387
		IMPLTYPEFLAG_FDEFAULT = 1,
		// Token: 0x04002C7C RID: 11388
		IMPLTYPEFLAG_FSOURCE = 2,
		// Token: 0x04002C7D RID: 11389
		IMPLTYPEFLAG_FRESTRICTED = 4,
		// Token: 0x04002C7E RID: 11390
		IMPLTYPEFLAG_FDEFAULTVTABLE = 8
	}
}
