using System;
using TMPro;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200006E RID: 110
	[Serializable]
	public struct TextStyle
	{
		// Token: 0x04000263 RID: 611
		public static readonly TextStyle defaultTextStyle = new TextStyle
		{
			font = ShapesAssets.Instance.defaultFont,
			size = 1f,
			style = FontStyles.Normal,
			alignment = TextAlign.Center,
			characterSpacing = 0f,
			wordSpacing = 0f,
			lineSpacing = 0f,
			paragraphSpacing = 0f,
			margins = Vector4.zero,
			wrap = TextWrappingModes.Normal,
			overflow = TextOverflowModes.Overflow,
			curvature = 0f,
			curvaturePivot = Vector2.zero
		};

		// Token: 0x04000264 RID: 612
		public TMP_FontAsset font;

		// Token: 0x04000265 RID: 613
		public float size;

		// Token: 0x04000266 RID: 614
		public FontStyles style;

		// Token: 0x04000267 RID: 615
		public TextAlign alignment;

		// Token: 0x04000268 RID: 616
		public float characterSpacing;

		// Token: 0x04000269 RID: 617
		public float wordSpacing;

		// Token: 0x0400026A RID: 618
		public float lineSpacing;

		// Token: 0x0400026B RID: 619
		public float paragraphSpacing;

		// Token: 0x0400026C RID: 620
		public Vector4 margins;

		// Token: 0x0400026D RID: 621
		public TextWrappingModes wrap;

		// Token: 0x0400026E RID: 622
		public TextOverflowModes overflow;

		// Token: 0x0400026F RID: 623
		public float curvature;

		// Token: 0x04000270 RID: 624
		public Vector2 curvaturePivot;
	}
}
