using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007C0 RID: 1984
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct VARDESC
	{
		// Token: 0x04002CBC RID: 11452
		public int memid;

		// Token: 0x04002CBD RID: 11453
		public string lpstrSchema;

		// Token: 0x04002CBE RID: 11454
		public VARDESC.DESCUNION desc;

		// Token: 0x04002CBF RID: 11455
		public ELEMDESC elemdescVar;

		// Token: 0x04002CC0 RID: 11456
		public short wVarFlags;

		// Token: 0x04002CC1 RID: 11457
		public VARKIND varkind;

		// Token: 0x020007C1 RID: 1985
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct DESCUNION
		{
			// Token: 0x04002CC2 RID: 11458
			[FieldOffset(0)]
			public int oInst;

			// Token: 0x04002CC3 RID: 11459
			[FieldOffset(0)]
			public IntPtr lpvarValue;
		}
	}
}
