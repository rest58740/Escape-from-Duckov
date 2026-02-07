using System;
using System.ComponentModel;

namespace Shapes
{
	// Token: 0x0200006B RID: 107
	public enum ShapesBlendMode
	{
		// Token: 0x04000232 RID: 562
		[Description("Opaque")]
		Opaque,
		// Token: 0x04000233 RID: 563
		[Description("Transparent_")]
		Transparent,
		// Token: 0x04000234 RID: 564
		[Description("Linear Dodge (Additive)")]
		Additive,
		// Token: 0x04000235 RID: 565
		[Description("Color Dodge")]
		ColorDodge = 9,
		// Token: 0x04000236 RID: 566
		[Description("Screen")]
		Screen = 4,
		// Token: 0x04000237 RID: 567
		[Description("Lighten_")]
		Lighten = 7,
		// Token: 0x04000238 RID: 568
		[Description("Linear Burn")]
		LinearBurn = 6,
		// Token: 0x04000239 RID: 569
		[Description("Color Burn")]
		ColorBurn = 10,
		// Token: 0x0400023A RID: 570
		[Description("Multiply")]
		Multiplicative = 3,
		// Token: 0x0400023B RID: 571
		[Description("Darken_")]
		Darken = 8,
		// Token: 0x0400023C RID: 572
		[Description("Subtract")]
		Subtractive = 5
	}
}
