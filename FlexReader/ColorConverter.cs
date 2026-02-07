using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FlexFramework.Excel
{
	// Token: 0x02000006 RID: 6
	public sealed class ColorConverter : CustomConverter<Color>
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002330 File Offset: 0x00000530
		public override Color Convert(string input)
		{
			if (Regex.IsMatch(input, "^[#]?([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3}|[A-Fa-f0-9]{6}[Ff]{2})$"))
			{
				return ColorConverter.HexToColor(input.TrimStart('#'));
			}
			if (Regex.IsMatch(input, "^\\((0(\\.\\d+)?|1(\\.0+)?),(0(\\.\\d+)?|1(\\.0+)?),(0(\\.\\d+)?|1(\\.0+)?),(0(\\.\\d+)?|1(\\.0+)?)\\)$"))
			{
				string[] array = base.Split(input.Trim(new char[]
				{
					'(',
					')'
				}), new char[]
				{
					','
				});
				float r = ValueConverter.Convert<float>(array[0]);
				float g = ValueConverter.Convert<float>(array[1]);
				float b = ValueConverter.Convert<float>(array[2]);
				float a = ValueConverter.Convert<float>(array[3]);
				return new Color(r, g, b, a);
			}
			if (Regex.IsMatch(input, "^\\((0(\\.\\d+)?|1(\\.0+)?),(0(\\.\\d+)?|1(\\.0+)?),(0(\\.\\d+)?|1(\\.0+)?)\\)$"))
			{
				string[] array2 = base.Split(input.Trim(new char[]
				{
					'(',
					')'
				}), new char[]
				{
					','
				});
				float r2 = ValueConverter.Convert<float>(array2[0]);
				float g2 = ValueConverter.Convert<float>(array2[1]);
				float b2 = ValueConverter.Convert<float>(array2[2]);
				return new Color(r2, g2, b2);
			}
			throw new FormatException("Color value expression invalid: " + input);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002426 File Offset: 0x00000626
		public static string ColorToHex(Color color)
		{
			return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002460 File Offset: 0x00000660
		public static Color HexToColor(string hex)
		{
			byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			return new Color32(r, g, b, byte.MaxValue);
		}
	}
}
