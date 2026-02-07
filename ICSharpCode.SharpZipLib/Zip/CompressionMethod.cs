using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000014 RID: 20
	public enum CompressionMethod
	{
		// Token: 0x040000D5 RID: 213
		Stored,
		// Token: 0x040000D6 RID: 214
		Deflated = 8,
		// Token: 0x040000D7 RID: 215
		Deflate64,
		// Token: 0x040000D8 RID: 216
		BZip2 = 11,
		// Token: 0x040000D9 RID: 217
		WinZipAES = 99
	}
}
