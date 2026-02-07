using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FlexFramework.Excel
{
	// Token: 0x02000005 RID: 5
	public class Color32Converter : CustomConverter<Color32>
	{
		// Token: 0x0600000C RID: 12 RVA: 0x0000223C File Offset: 0x0000043C
		public override Color32 Convert(string input)
		{
			if (Regex.IsMatch(input, "^\\(([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])\\)$"))
			{
				string[] array = base.Split(input.Trim(new char[]
				{
					'(',
					')'
				}), new char[]
				{
					','
				});
				int num = ValueConverter.Convert<int>(array[0]);
				int num2 = ValueConverter.Convert<int>(array[1]);
				int num3 = ValueConverter.Convert<int>(array[2]);
				int num4 = ValueConverter.Convert<int>(array[3]);
				return new Color((float)num, (float)num2, (float)num3, (float)num4);
			}
			if (Regex.IsMatch(input, "^\\(([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])\\)$"))
			{
				string[] array2 = base.Split(input.Trim(new char[]
				{
					'(',
					')'
				}), new char[]
				{
					','
				});
				int num5 = ValueConverter.Convert<int>(array2[0]);
				int num6 = ValueConverter.Convert<int>(array2[1]);
				int num7 = ValueConverter.Convert<int>(array2[2]);
				return new Color((float)num5, (float)num6, (float)num7);
			}
			throw new FormatException("Color32 value expression invalid: " + input);
		}
	}
}
