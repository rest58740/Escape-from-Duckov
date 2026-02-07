using System;

namespace System.Reflection.Emit
{
	// Token: 0x02000908 RID: 2312
	public enum OperandType
	{
		// Token: 0x0400308B RID: 12427
		InlineBrTarget,
		// Token: 0x0400308C RID: 12428
		InlineField,
		// Token: 0x0400308D RID: 12429
		InlineI,
		// Token: 0x0400308E RID: 12430
		InlineI8,
		// Token: 0x0400308F RID: 12431
		InlineMethod,
		// Token: 0x04003090 RID: 12432
		InlineNone,
		// Token: 0x04003091 RID: 12433
		[Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		InlinePhi,
		// Token: 0x04003092 RID: 12434
		InlineR,
		// Token: 0x04003093 RID: 12435
		InlineSig = 9,
		// Token: 0x04003094 RID: 12436
		InlineString,
		// Token: 0x04003095 RID: 12437
		InlineSwitch,
		// Token: 0x04003096 RID: 12438
		InlineTok,
		// Token: 0x04003097 RID: 12439
		InlineType,
		// Token: 0x04003098 RID: 12440
		InlineVar,
		// Token: 0x04003099 RID: 12441
		ShortInlineBrTarget,
		// Token: 0x0400309A RID: 12442
		ShortInlineI,
		// Token: 0x0400309B RID: 12443
		ShortInlineR,
		// Token: 0x0400309C RID: 12444
		ShortInlineVar
	}
}
