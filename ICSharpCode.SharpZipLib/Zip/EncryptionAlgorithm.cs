using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000015 RID: 21
	public enum EncryptionAlgorithm
	{
		// Token: 0x040000DB RID: 219
		None,
		// Token: 0x040000DC RID: 220
		PkzipClassic,
		// Token: 0x040000DD RID: 221
		Des = 26113,
		// Token: 0x040000DE RID: 222
		RC2,
		// Token: 0x040000DF RID: 223
		TripleDes168,
		// Token: 0x040000E0 RID: 224
		TripleDes112 = 26121,
		// Token: 0x040000E1 RID: 225
		Aes128 = 26126,
		// Token: 0x040000E2 RID: 226
		Aes192,
		// Token: 0x040000E3 RID: 227
		Aes256,
		// Token: 0x040000E4 RID: 228
		RC2Corrected = 26370,
		// Token: 0x040000E5 RID: 229
		Blowfish = 26400,
		// Token: 0x040000E6 RID: 230
		Twofish,
		// Token: 0x040000E7 RID: 231
		RC4 = 26625,
		// Token: 0x040000E8 RID: 232
		Unknown = 65535
	}
}
