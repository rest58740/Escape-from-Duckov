using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000731 RID: 1841
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.FUNCKIND instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum FUNCKIND
	{
		// Token: 0x04002B78 RID: 11128
		FUNC_VIRTUAL,
		// Token: 0x04002B79 RID: 11129
		FUNC_PUREVIRTUAL,
		// Token: 0x04002B7A RID: 11130
		FUNC_NONVIRTUAL,
		// Token: 0x04002B7B RID: 11131
		FUNC_STATIC,
		// Token: 0x04002B7C RID: 11132
		FUNC_DISPATCH
	}
}
