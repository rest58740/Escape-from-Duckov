using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200072D RID: 1837
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.VARDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct VARDESC
	{
		// Token: 0x04002B64 RID: 11108
		public int memid;

		// Token: 0x04002B65 RID: 11109
		public string lpstrSchema;

		// Token: 0x04002B66 RID: 11110
		public ELEMDESC elemdescVar;

		// Token: 0x04002B67 RID: 11111
		public short wVarFlags;

		// Token: 0x04002B68 RID: 11112
		public VarEnum varkind;

		// Token: 0x0200072E RID: 1838
		[ComVisible(false)]
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct DESCUNION
		{
			// Token: 0x04002B69 RID: 11113
			[FieldOffset(0)]
			public int oInst;

			// Token: 0x04002B6A RID: 11114
			[FieldOffset(0)]
			public IntPtr lpvarValue;
		}
	}
}
