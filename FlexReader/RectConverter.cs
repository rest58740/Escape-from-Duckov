using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FlexFramework.Excel
{
	// Token: 0x0200000A RID: 10
	public sealed class RectConverter : CustomConverter<Rect>
	{
		// Token: 0x06000018 RID: 24 RVA: 0x000026D8 File Offset: 0x000008D8
		public override Rect Convert(string input)
		{
			if (!Regex.IsMatch(input, "^\\([-+]?[0-9]*\\.?[0-9]+\\b,[-+]?[0-9]*\\.?[0-9]+\\b,[-+]?[0-9]*\\.?[0-9]+\\b,[-+]?[0-9]*\\.?[0-9]+\\b\\)$"))
			{
				throw new FormatException("Rect value expression invalid: " + input);
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
			float width = ValueConverter.Convert<float>(array[2]);
			float height = ValueConverter.Convert<float>(array[3]);
			return new Rect(x, y, width, height);
		}
	}
}
