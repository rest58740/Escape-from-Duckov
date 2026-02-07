using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000724 RID: 1828
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.TYPEATTR instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPEATTR
	{
		// Token: 0x04002B2C RID: 11052
		public const int MEMBER_ID_NIL = -1;

		// Token: 0x04002B2D RID: 11053
		public Guid guid;

		// Token: 0x04002B2E RID: 11054
		public int lcid;

		// Token: 0x04002B2F RID: 11055
		public int dwReserved;

		// Token: 0x04002B30 RID: 11056
		public int memidConstructor;

		// Token: 0x04002B31 RID: 11057
		public int memidDestructor;

		// Token: 0x04002B32 RID: 11058
		public IntPtr lpstrSchema;

		// Token: 0x04002B33 RID: 11059
		public int cbSizeInstance;

		// Token: 0x04002B34 RID: 11060
		public TYPEKIND typekind;

		// Token: 0x04002B35 RID: 11061
		public short cFuncs;

		// Token: 0x04002B36 RID: 11062
		public short cVars;

		// Token: 0x04002B37 RID: 11063
		public short cImplTypes;

		// Token: 0x04002B38 RID: 11064
		public short cbSizeVft;

		// Token: 0x04002B39 RID: 11065
		public short cbAlignment;

		// Token: 0x04002B3A RID: 11066
		public TYPEFLAGS wTypeFlags;

		// Token: 0x04002B3B RID: 11067
		public short wMajorVerNum;

		// Token: 0x04002B3C RID: 11068
		public short wMinorVerNum;

		// Token: 0x04002B3D RID: 11069
		public TYPEDESC tdescAlias;

		// Token: 0x04002B3E RID: 11070
		public IDLDESC idldescType;
	}
}
