using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007B6 RID: 1974
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPEATTR
	{
		// Token: 0x04002C7F RID: 11391
		public const int MEMBER_ID_NIL = -1;

		// Token: 0x04002C80 RID: 11392
		public Guid guid;

		// Token: 0x04002C81 RID: 11393
		public int lcid;

		// Token: 0x04002C82 RID: 11394
		public int dwReserved;

		// Token: 0x04002C83 RID: 11395
		public int memidConstructor;

		// Token: 0x04002C84 RID: 11396
		public int memidDestructor;

		// Token: 0x04002C85 RID: 11397
		public IntPtr lpstrSchema;

		// Token: 0x04002C86 RID: 11398
		public int cbSizeInstance;

		// Token: 0x04002C87 RID: 11399
		public TYPEKIND typekind;

		// Token: 0x04002C88 RID: 11400
		public short cFuncs;

		// Token: 0x04002C89 RID: 11401
		public short cVars;

		// Token: 0x04002C8A RID: 11402
		public short cImplTypes;

		// Token: 0x04002C8B RID: 11403
		public short cbSizeVft;

		// Token: 0x04002C8C RID: 11404
		public short cbAlignment;

		// Token: 0x04002C8D RID: 11405
		public TYPEFLAGS wTypeFlags;

		// Token: 0x04002C8E RID: 11406
		public short wMajorVerNum;

		// Token: 0x04002C8F RID: 11407
		public short wMinorVerNum;

		// Token: 0x04002C90 RID: 11408
		public TYPEDESC tdescAlias;

		// Token: 0x04002C91 RID: 11409
		public IDLDESC idldescType;
	}
}
