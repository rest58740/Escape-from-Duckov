using System;
using TMPro;

namespace Shapes
{
	// Token: 0x0200006D RID: 109
	public static class TextAlignExtensions
	{
		// Token: 0x06000CAD RID: 3245 RVA: 0x00019904 File Offset: 0x00017B04
		public static TextAlignmentOptions GetTMPAlignment(this TextAlign align)
		{
			switch (align)
			{
			case TextAlign.TopLeft:
				return TextAlignmentOptions.TopLeft;
			case TextAlign.Top:
				return TextAlignmentOptions.Top;
			case TextAlign.TopRight:
				return TextAlignmentOptions.TopRight;
			case TextAlign.Left:
				return TextAlignmentOptions.Left;
			case TextAlign.Center:
				return TextAlignmentOptions.Center;
			case TextAlign.Right:
				return TextAlignmentOptions.Right;
			case TextAlign.BottomLeft:
				return TextAlignmentOptions.BottomLeft;
			case TextAlign.Bottom:
				return TextAlignmentOptions.Bottom;
			case TextAlign.BottomRight:
				return TextAlignmentOptions.BottomRight;
			case TextAlign.TopJustified:
				return TextAlignmentOptions.TopJustified;
			case TextAlign.TopFlush:
				return TextAlignmentOptions.TopFlush;
			case TextAlign.TopGeoAligned:
				return TextAlignmentOptions.TopGeoAligned;
			case TextAlign.Justified:
				return TextAlignmentOptions.Justified;
			case TextAlign.Flush:
				return TextAlignmentOptions.Flush;
			case TextAlign.CenterGeoAligned:
				return TextAlignmentOptions.CenterGeoAligned;
			case TextAlign.BottomJustified:
				return TextAlignmentOptions.BottomJustified;
			case TextAlign.BottomFlush:
				return TextAlignmentOptions.BottomFlush;
			case TextAlign.BottomGeoAligned:
				return TextAlignmentOptions.BottomGeoAligned;
			case TextAlign.BaselineLeft:
				return TextAlignmentOptions.BaselineLeft;
			case TextAlign.Baseline:
				return TextAlignmentOptions.Baseline;
			case TextAlign.BaselineRight:
				return TextAlignmentOptions.BaselineRight;
			case TextAlign.BaselineJustified:
				return TextAlignmentOptions.BaselineJustified;
			case TextAlign.BaselineFlush:
				return TextAlignmentOptions.BaselineFlush;
			case TextAlign.BaselineGeoAligned:
				return TextAlignmentOptions.BaselineGeoAligned;
			case TextAlign.MidlineLeft:
				return TextAlignmentOptions.MidlineLeft;
			case TextAlign.Midline:
				return TextAlignmentOptions.Midline;
			case TextAlign.MidlineRight:
				return TextAlignmentOptions.MidlineRight;
			case TextAlign.MidlineJustified:
				return TextAlignmentOptions.MidlineJustified;
			case TextAlign.MidlineFlush:
				return TextAlignmentOptions.MidlineFlush;
			case TextAlign.MidlineGeoAligned:
				return TextAlignmentOptions.MidlineGeoAligned;
			case TextAlign.CaplineLeft:
				return TextAlignmentOptions.CaplineLeft;
			case TextAlign.Capline:
				return TextAlignmentOptions.Capline;
			case TextAlign.CaplineRight:
				return TextAlignmentOptions.CaplineRight;
			case TextAlign.CaplineJustified:
				return TextAlignmentOptions.CaplineJustified;
			case TextAlign.CaplineFlush:
				return TextAlignmentOptions.CaplineFlush;
			case TextAlign.CaplineGeoAligned:
				return TextAlignmentOptions.CaplineGeoAligned;
			case TextAlign.Converted:
				return TextAlignmentOptions.Converted;
			default:
				return (TextAlignmentOptions)0;
			}
		}
	}
}
