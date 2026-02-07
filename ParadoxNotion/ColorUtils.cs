using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x02000077 RID: 119
	public static class ColorUtils
	{
		// Token: 0x06000489 RID: 1161 RVA: 0x0000CB06 File Offset: 0x0000AD06
		public static Color WithAlpha(this Color color, float alpha)
		{
			color.a = alpha;
			return color;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0000CB11 File Offset: 0x0000AD11
		public static Color Grey(float value)
		{
			return new Color(value, value, value, 1f);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000CB20 File Offset: 0x0000AD20
		public static string ColorToHex(Color32 color)
		{
			string text;
			if (ColorUtils.colorHexCache.TryGetValue(color, ref text))
			{
				return text;
			}
			text = ("#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2")).ToUpper();
			return ColorUtils.colorHexCache[color] = text;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000CB90 File Offset: 0x0000AD90
		public static Color HexToColor(string hex)
		{
			Color color;
			if (ColorUtils.hexColorCache.TryGetValue(hex, ref color))
			{
				return color;
			}
			if (hex.Length != 6)
			{
				throw new Exception("Invalid length for hex color provided");
			}
			byte r = byte.Parse(hex.Substring(0, 2), 515);
			byte g = byte.Parse(hex.Substring(2, 2), 515);
			byte b = byte.Parse(hex.Substring(4, 2), 515);
			color = new Color32(r, g, b, byte.MaxValue);
			return ColorUtils.hexColorCache[hex] = color;
		}

		// Token: 0x04000176 RID: 374
		private static Dictionary<Color32, string> colorHexCache = new Dictionary<Color32, string>();

		// Token: 0x04000177 RID: 375
		private static Dictionary<string, Color> hexColorCache = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);
	}
}
