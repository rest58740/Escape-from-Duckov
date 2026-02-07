using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000723 RID: 1827
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Flags]
	[Serializable]
	public enum IMPLTYPEFLAGS
	{
		// Token: 0x04002B28 RID: 11048
		IMPLTYPEFLAG_FDEFAULT = 1,
		// Token: 0x04002B29 RID: 11049
		IMPLTYPEFLAG_FSOURCE = 2,
		// Token: 0x04002B2A RID: 11050
		IMPLTYPEFLAG_FRESTRICTED = 4,
		// Token: 0x04002B2B RID: 11051
		IMPLTYPEFLAG_FDEFAULTVTABLE = 8
	}
}
