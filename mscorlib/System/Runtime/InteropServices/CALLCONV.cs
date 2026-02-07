using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000733 RID: 1843
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.CALLCONV instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum CALLCONV
	{
		// Token: 0x04002B83 RID: 11139
		CC_CDECL = 1,
		// Token: 0x04002B84 RID: 11140
		CC_MSCPASCAL,
		// Token: 0x04002B85 RID: 11141
		CC_PASCAL = 2,
		// Token: 0x04002B86 RID: 11142
		CC_MACPASCAL,
		// Token: 0x04002B87 RID: 11143
		CC_STDCALL,
		// Token: 0x04002B88 RID: 11144
		CC_RESERVED,
		// Token: 0x04002B89 RID: 11145
		CC_SYSCALL,
		// Token: 0x04002B8A RID: 11146
		CC_MPWCDECL,
		// Token: 0x04002B8B RID: 11147
		CC_MPWPASCAL,
		// Token: 0x04002B8C RID: 11148
		CC_MAX
	}
}
