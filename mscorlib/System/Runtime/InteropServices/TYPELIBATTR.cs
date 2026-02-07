using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000751 RID: 1873
	[Obsolete]
	[Serializable]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPELIBATTR
	{
		// Token: 0x04002C01 RID: 11265
		public Guid guid;

		// Token: 0x04002C02 RID: 11266
		public int lcid;

		// Token: 0x04002C03 RID: 11267
		public SYSKIND syskind;

		// Token: 0x04002C04 RID: 11268
		public short wMajorVerNum;

		// Token: 0x04002C05 RID: 11269
		public short wMinorVerNum;

		// Token: 0x04002C06 RID: 11270
		public LIBFLAGS wLibFlags;
	}
}
