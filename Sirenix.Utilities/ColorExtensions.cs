using System;
using System.Globalization;
using UnityEngine;

namespace Sirenix.Utilities
{
	// Token: 0x02000002 RID: 2
	public static class ColorExtensions
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static Color Lerp(this Color[] colors, float t)
		{
			t = Mathf.Clamp(t, 0f, 1f) * (float)(colors.Length - 1);
			int num = (int)t;
			int num2 = Mathf.Min((int)t + 1, colors.Length - 1);
			return Color.Lerp(colors[num], colors[num2], t - (float)((int)t));
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020A0 File Offset: 0x000002A0
		public static Color MoveTowards(this Color from, Color to, float maxDelta)
		{
			Color color = new Color
			{
				r = Mathf.MoveTowards(from.r, to.r, maxDelta),
				g = Mathf.MoveTowards(from.g, to.g, maxDelta),
				b = Mathf.MoveTowards(from.b, to.b, maxDelta),
				a = Mathf.MoveTowards(from.a, to.a, maxDelta)
			};
			from.r = color.r;
			from.g = color.g;
			from.b = color.b;
			from.a = color.a;
			return color;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002150 File Offset: 0x00000350
		public static bool TryParseString(string colorStr, out Color color)
		{
			color = default(Color);
			if (colorStr == null || colorStr.Length < 2 || colorStr.Length > 100)
			{
				return false;
			}
			if (colorStr.StartsWith("new Color", 2))
			{
				colorStr = colorStr.Substring("new Color".Length, colorStr.Length - "new Color".Length).Replace("f", "");
			}
			bool flag = colorStr.get_Chars(0) == '#' || char.IsLetter(colorStr.get_Chars(0)) || char.IsNumber(colorStr.get_Chars(0));
			bool flag2 = colorStr.get_Chars(0) == 'R' || colorStr.get_Chars(0) == '(' || char.IsNumber(colorStr.get_Chars(0));
			if (!flag && !flag2)
			{
				return false;
			}
			bool result = false;
			if (!flag2 && (!flag || (result = ColorUtility.TryParseHtmlString(colorStr, out color)) || !flag2))
			{
				return result;
			}
			colorStr = colorStr.TrimStart(ColorExtensions.trimRGBStart).TrimEnd(new char[]
			{
				')'
			});
			string[] array = colorStr.Split(new char[]
			{
				','
			});
			if (array.Length < 2 || array.Length > 4)
			{
				return false;
			}
			Color color2 = new Color(0f, 0f, 0f, 1f);
			for (int i = 0; i < array.Length; i++)
			{
				float num;
				if (!float.TryParse(array[i], ref num))
				{
					return false;
				}
				if (i == 0)
				{
					color2.r = num;
				}
				if (i == 1)
				{
					color2.g = num;
				}
				if (i == 2)
				{
					color2.b = num;
				}
				if (i == 3)
				{
					color2.a = num;
				}
			}
			color = color2;
			return true;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000022F4 File Offset: 0x000004F4
		public static string ToCSharpColor(this Color color)
		{
			return string.Concat(new string[]
			{
				"new Color(",
				ColorExtensions.TrimFloat(color.r),
				"f, ",
				ColorExtensions.TrimFloat(color.g),
				"f, ",
				ColorExtensions.TrimFloat(color.b),
				"f, ",
				ColorExtensions.TrimFloat(color.a),
				"f)"
			});
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002370 File Offset: 0x00000570
		public static Color Pow(this Color color, float factor)
		{
			color.r = Mathf.Pow(color.r, factor);
			color.g = Mathf.Pow(color.g, factor);
			color.b = Mathf.Pow(color.b, factor);
			color.a = Mathf.Pow(color.a, factor);
			return color;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000023CC File Offset: 0x000005CC
		public static Color NormalizeRGB(this Color color)
		{
			Vector3 normalized = new Vector3(color.r, color.g, color.b).normalized;
			color.r = normalized.x;
			color.g = normalized.y;
			color.b = normalized.z;
			return color;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002424 File Offset: 0x00000624
		public static float PerceivedLuminosity(this Color color, bool includeAlpha = true)
		{
			if (includeAlpha && color.a <= 0f)
			{
				return 0f;
			}
			float num = 0.3f * color.r + 0.59f * color.g + 0.11f * color.b;
			if (includeAlpha && color.a < 1f)
			{
				num *= color.a;
			}
			return num;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002488 File Offset: 0x00000688
		private static string TrimFloat(float value)
		{
			string text = value.ToString("F3", CultureInfo.InvariantCulture).TrimEnd(new char[]
			{
				'0'
			});
			char c = text.get_Chars(text.Length - 1);
			if (c == '.' || c == ',')
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}

		// Token: 0x04000001 RID: 1
		private static readonly char[] trimRGBStart = new char[]
		{
			'R',
			'r',
			'G',
			'g',
			'B',
			'b',
			'A',
			'a',
			'('
		};
	}
}
