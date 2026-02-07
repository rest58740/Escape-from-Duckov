using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FlexFramework.Excel
{
	// Token: 0x0200000D RID: 13
	public sealed class Vector4Converter : CustomConverter<Vector4>
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002850 File Offset: 0x00000A50
		public override Vector4 Convert(string input)
		{
			if (!Regex.IsMatch(input, "^\\([-+]?[0-9]*\\.?[0-9]+\\b,[-+]?[0-9]*\\.?[0-9]+\\b,[-+]?[0-9]*\\.?[0-9]+\\b\\),[-+]?[0-9]*\\.?[0-9]+\\b$"))
			{
				throw new FormatException("Vector4 value expression invalid: " + input);
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
			float z = ValueConverter.Convert<float>(array[2]);
			float w = ValueConverter.Convert<float>(array[3]);
			return new Vector4(x, y, z, w);
		}
	}
}
