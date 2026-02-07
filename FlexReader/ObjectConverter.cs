using System;
using System.Text.RegularExpressions;

namespace FlexFramework.Excel
{
	// Token: 0x02000009 RID: 9
	public class ObjectConverter : CustomConverter<object>
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000025E4 File Offset: 0x000007E4
		public override object Convert(string input)
		{
			if (Regex.IsMatch(input, "^[+-]?[\\d]+[\\d,]*\\.{0,1}[\\d]*$", RegexOptions.IgnoreCase))
			{
				input = Regex.Replace(input, ",|\\+", string.Empty);
				if (Regex.IsMatch(input, "\\."))
				{
					float num;
					if (float.TryParse(input, out num))
					{
						return num;
					}
					double num2;
					if (double.TryParse(input, out num2))
					{
						return num2;
					}
				}
				ushort num3;
				if (ushort.TryParse(input, out num3))
				{
					return num3;
				}
				short num4;
				if (short.TryParse(input, out num4))
				{
					return num4;
				}
				uint num5;
				if (uint.TryParse(input, out num5))
				{
					return num5;
				}
				int num6;
				if (int.TryParse(input, out num6))
				{
					return num6;
				}
				ulong num7;
				if (ulong.TryParse(input, out num7))
				{
					return num7;
				}
				long num8;
				if (long.TryParse(input, out num8))
				{
					return num8;
				}
			}
			bool flag;
			if (Regex.IsMatch(input, "^(true|false)$", RegexOptions.IgnoreCase) && bool.TryParse(input, out flag))
			{
				return flag;
			}
			return input;
		}
	}
}
