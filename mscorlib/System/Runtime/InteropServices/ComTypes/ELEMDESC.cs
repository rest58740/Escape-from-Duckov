using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007BD RID: 1981
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct ELEMDESC
	{
		// Token: 0x04002CB3 RID: 11443
		public TYPEDESC tdesc;

		// Token: 0x04002CB4 RID: 11444
		public ELEMDESC.DESCUNION desc;

		// Token: 0x020007BE RID: 1982
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct DESCUNION
		{
			// Token: 0x04002CB5 RID: 11445
			[FieldOffset(0)]
			public IDLDESC idldesc;

			// Token: 0x04002CB6 RID: 11446
			[FieldOffset(0)]
			public PARAMDESC paramdesc;
		}
	}
}
