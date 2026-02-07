using System;
using System.Globalization;
using UnityEngine;

namespace DG.DemiLib
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	public class DeColorPalette
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static Color HexToColor(string hex)
		{
			if (hex[0] == '#')
			{
				hex = hex.Substring(1);
			}
			int length = hex.Length;
			if (length < 6)
			{
				float r = ((float)DeColorPalette.HexToInt(hex[0]) + (float)DeColorPalette.HexToInt(hex[0]) * 16f) / 255f;
				float g = ((float)DeColorPalette.HexToInt(hex[1]) + (float)DeColorPalette.HexToInt(hex[1]) * 16f) / 255f;
				float b = ((float)DeColorPalette.HexToInt(hex[2]) + (float)DeColorPalette.HexToInt(hex[2]) * 16f) / 255f;
				float a = (length == 4) ? (((float)DeColorPalette.HexToInt(hex[3]) + (float)DeColorPalette.HexToInt(hex[3]) * 16f) / 255f) : 1f;
				return new Color(r, g, b, a);
			}
			float r2 = ((float)DeColorPalette.HexToInt(hex[1]) + (float)DeColorPalette.HexToInt(hex[0]) * 16f) / 255f;
			float g2 = ((float)DeColorPalette.HexToInt(hex[3]) + (float)DeColorPalette.HexToInt(hex[2]) * 16f) / 255f;
			float b2 = ((float)DeColorPalette.HexToInt(hex[5]) + (float)DeColorPalette.HexToInt(hex[4]) * 16f) / 255f;
			float a2 = (length == 8) ? (((float)DeColorPalette.HexToInt(hex[7]) + (float)DeColorPalette.HexToInt(hex[6]) * 16f) / 255f) : 1f;
			return new Color(r2, g2, b2, a2);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000021EC File Offset: 0x000003EC
		private static int HexToInt(char hexVal)
		{
			return int.Parse(hexVal.ToString(), NumberStyles.HexNumber);
		}

		// Token: 0x0400000A RID: 10
		public DeColorGlobal global = new DeColorGlobal();

		// Token: 0x0400000B RID: 11
		public DeColorBG bg = new DeColorBG();

		// Token: 0x0400000C RID: 12
		public DeColorContent content = new DeColorContent();

		// Token: 0x0400000D RID: 13
		public DeToggleColors toggle = new DeToggleColors();
	}
}
