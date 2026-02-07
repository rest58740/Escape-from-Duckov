using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007AE RID: 1966
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct STATSTG
	{
		// Token: 0x04002C4B RID: 11339
		public string pwcsName;

		// Token: 0x04002C4C RID: 11340
		public int type;

		// Token: 0x04002C4D RID: 11341
		public long cbSize;

		// Token: 0x04002C4E RID: 11342
		public FILETIME mtime;

		// Token: 0x04002C4F RID: 11343
		public FILETIME ctime;

		// Token: 0x04002C50 RID: 11344
		public FILETIME atime;

		// Token: 0x04002C51 RID: 11345
		public int grfMode;

		// Token: 0x04002C52 RID: 11346
		public int grfLocksSupported;

		// Token: 0x04002C53 RID: 11347
		public Guid clsid;

		// Token: 0x04002C54 RID: 11348
		public int grfStateBits;

		// Token: 0x04002C55 RID: 11349
		public int reserved;
	}
}
