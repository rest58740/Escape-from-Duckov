using System;

namespace System.Globalization
{
	// Token: 0x0200096D RID: 2413
	[Flags]
	public enum NumberStyles
	{
		// Token: 0x040034D1 RID: 13521
		None = 0,
		// Token: 0x040034D2 RID: 13522
		AllowLeadingWhite = 1,
		// Token: 0x040034D3 RID: 13523
		AllowTrailingWhite = 2,
		// Token: 0x040034D4 RID: 13524
		AllowLeadingSign = 4,
		// Token: 0x040034D5 RID: 13525
		AllowTrailingSign = 8,
		// Token: 0x040034D6 RID: 13526
		AllowParentheses = 16,
		// Token: 0x040034D7 RID: 13527
		AllowDecimalPoint = 32,
		// Token: 0x040034D8 RID: 13528
		AllowThousands = 64,
		// Token: 0x040034D9 RID: 13529
		AllowExponent = 128,
		// Token: 0x040034DA RID: 13530
		AllowCurrencySymbol = 256,
		// Token: 0x040034DB RID: 13531
		AllowHexSpecifier = 512,
		// Token: 0x040034DC RID: 13532
		Integer = 7,
		// Token: 0x040034DD RID: 13533
		HexNumber = 515,
		// Token: 0x040034DE RID: 13534
		Number = 111,
		// Token: 0x040034DF RID: 13535
		Float = 167,
		// Token: 0x040034E0 RID: 13536
		Currency = 383,
		// Token: 0x040034E1 RID: 13537
		Any = 511
	}
}
