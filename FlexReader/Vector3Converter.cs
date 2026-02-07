using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FlexFramework.Excel
{
	// Token: 0x0200000C RID: 12
	public sealed class Vector3Converter : CustomConverter<Vector3>
	{
		// Token: 0x0600001C RID: 28 RVA: 0x000027D4 File Offset: 0x000009D4
		public override Vector3 Convert(string input)
		{
			if (!Regex.IsMatch(input, "^\\([-+]?[0-9]*\\.?[0-9]+\\b,[-+]?[0-9]*\\.?[0-9]+\\b,[-+]?[0-9]*\\.?[0-9]+\\b\\)$"))
			{
				throw new FormatException("Vector3 value expression invalid: " + input);
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
			return new Vector3(x, y, z);
		}
	}
}
