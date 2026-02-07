using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000016 RID: 22
	[Flags]
	public enum GeneralBitFlags
	{
		// Token: 0x040000EA RID: 234
		Encrypted = 1,
		// Token: 0x040000EB RID: 235
		Method = 6,
		// Token: 0x040000EC RID: 236
		Descriptor = 8,
		// Token: 0x040000ED RID: 237
		ReservedPKware4 = 16,
		// Token: 0x040000EE RID: 238
		Patched = 32,
		// Token: 0x040000EF RID: 239
		StrongEncryption = 64,
		// Token: 0x040000F0 RID: 240
		Unused7 = 128,
		// Token: 0x040000F1 RID: 241
		Unused8 = 256,
		// Token: 0x040000F2 RID: 242
		Unused9 = 512,
		// Token: 0x040000F3 RID: 243
		Unused10 = 1024,
		// Token: 0x040000F4 RID: 244
		UnicodeText = 2048,
		// Token: 0x040000F5 RID: 245
		EnhancedCompress = 4096,
		// Token: 0x040000F6 RID: 246
		HeaderMasked = 8192,
		// Token: 0x040000F7 RID: 247
		ReservedPkware14 = 16384,
		// Token: 0x040000F8 RID: 248
		ReservedPkware15 = 32768
	}
}
