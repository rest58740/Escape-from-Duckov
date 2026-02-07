using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FlexFramework.Excel
{
	// Token: 0x0200000B RID: 11
	public sealed class Vector2Converter : CustomConverter<Vector2>
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002760 File Offset: 0x00000960
		public override Vector2 Convert(string input)
		{
			if (!Regex.IsMatch(input, "^\\([-+]?[0-9]*\\.?[0-9]+\\b,[-+]?[0-9]*\\.?[0-9]+\\b\\)$"))
			{
				throw new FormatException("Vector2 value expression invalid: " + input);
			}
			string[] array = base.Split(input.Trim(new char[]
			{
				'(',
				')'
			}), new char[]
			{
				','
			});
			float x = ValueConverter.Convert<float>(array[0]);
			float y = ValueConverter.Convert<float>(array[1]);
			return new Vector2(x, y);
		}
	}
}
