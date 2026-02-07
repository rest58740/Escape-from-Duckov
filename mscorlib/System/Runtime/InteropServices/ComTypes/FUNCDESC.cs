using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007B7 RID: 1975
	public struct FUNCDESC
	{
		// Token: 0x04002C92 RID: 11410
		public int memid;

		// Token: 0x04002C93 RID: 11411
		public IntPtr lprgscode;

		// Token: 0x04002C94 RID: 11412
		public IntPtr lprgelemdescParam;

		// Token: 0x04002C95 RID: 11413
		public FUNCKIND funckind;

		// Token: 0x04002C96 RID: 11414
		public INVOKEKIND invkind;

		// Token: 0x04002C97 RID: 11415
		public CALLCONV callconv;

		// Token: 0x04002C98 RID: 11416
		public short cParams;

		// Token: 0x04002C99 RID: 11417
		public short cParamsOpt;

		// Token: 0x04002C9A RID: 11418
		public short oVft;

		// Token: 0x04002C9B RID: 11419
		public short cScodes;

		// Token: 0x04002C9C RID: 11420
		public ELEMDESC elemdescFunc;

		// Token: 0x04002C9D RID: 11421
		public short wFuncFlags;
	}
}
