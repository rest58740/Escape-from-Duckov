using System;

namespace System.Globalization
{
	// Token: 0x02000961 RID: 2401
	[Flags]
	public enum DateTimeStyles
	{
		// Token: 0x0400347A RID: 13434
		None = 0,
		// Token: 0x0400347B RID: 13435
		AllowLeadingWhite = 1,
		// Token: 0x0400347C RID: 13436
		AllowTrailingWhite = 2,
		// Token: 0x0400347D RID: 13437
		AllowInnerWhite = 4,
		// Token: 0x0400347E RID: 13438
		AllowWhiteSpaces = 7,
		// Token: 0x0400347F RID: 13439
		NoCurrentDateDefault = 8,
		// Token: 0x04003480 RID: 13440
		AdjustToUniversal = 16,
		// Token: 0x04003481 RID: 13441
		AssumeLocal = 32,
		// Token: 0x04003482 RID: 13442
		AssumeUniversal = 64,
		// Token: 0x04003483 RID: 13443
		RoundtripKind = 128
	}
}
