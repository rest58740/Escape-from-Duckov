using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000725 RID: 1829
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.FUNCDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	public struct FUNCDESC
	{
		// Token: 0x04002B3F RID: 11071
		public int memid;

		// Token: 0x04002B40 RID: 11072
		public IntPtr lprgscode;

		// Token: 0x04002B41 RID: 11073
		public IntPtr lprgelemdescParam;

		// Token: 0x04002B42 RID: 11074
		public FUNCKIND funckind;

		// Token: 0x04002B43 RID: 11075
		public INVOKEKIND invkind;

		// Token: 0x04002B44 RID: 11076
		public CALLCONV callconv;

		// Token: 0x04002B45 RID: 11077
		public short cParams;

		// Token: 0x04002B46 RID: 11078
		public short cParamsOpt;

		// Token: 0x04002B47 RID: 11079
		public short oVft;

		// Token: 0x04002B48 RID: 11080
		public short cScodes;

		// Token: 0x04002B49 RID: 11081
		public ELEMDESC elemdescFunc;

		// Token: 0x04002B4A RID: 11082
		public short wFuncFlags;
	}
}
