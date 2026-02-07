using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000721 RID: 1825
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.TYPEKIND instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum TYPEKIND
	{
		// Token: 0x04002B0E RID: 11022
		TKIND_ENUM,
		// Token: 0x04002B0F RID: 11023
		TKIND_RECORD,
		// Token: 0x04002B10 RID: 11024
		TKIND_MODULE,
		// Token: 0x04002B11 RID: 11025
		TKIND_INTERFACE,
		// Token: 0x04002B12 RID: 11026
		TKIND_DISPATCH,
		// Token: 0x04002B13 RID: 11027
		TKIND_COCLASS,
		// Token: 0x04002B14 RID: 11028
		TKIND_ALIAS,
		// Token: 0x04002B15 RID: 11029
		TKIND_UNION,
		// Token: 0x04002B16 RID: 11030
		TKIND_MAX
	}
}
