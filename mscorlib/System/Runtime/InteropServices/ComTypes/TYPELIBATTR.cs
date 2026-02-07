using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007CD RID: 1997
	[Serializable]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPELIBATTR
	{
		// Token: 0x04002D0D RID: 11533
		public Guid guid;

		// Token: 0x04002D0E RID: 11534
		public int lcid;

		// Token: 0x04002D0F RID: 11535
		public SYSKIND syskind;

		// Token: 0x04002D10 RID: 11536
		public short wMajorVerNum;

		// Token: 0x04002D11 RID: 11537
		public short wMinorVerNum;

		// Token: 0x04002D12 RID: 11538
		public LIBFLAGS wLibFlags;
	}
}
