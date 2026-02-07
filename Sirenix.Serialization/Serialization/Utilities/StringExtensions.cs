using System;
using System.Globalization;
using System.Text;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000BE RID: 190
	internal static class StringExtensions
	{
		// Token: 0x0600052D RID: 1325 RVA: 0x0002410C File Offset: 0x0002230C
		public static string ToTitleCase(this string input)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < input.Length; i++)
			{
				char c = input.get_Chars(i);
				if (c == '_' && i + 1 < input.Length)
				{
					char c2 = input.get_Chars(i + 1);
					if (char.IsLower(c2))
					{
						c2 = char.ToUpper(c2, CultureInfo.InvariantCulture);
					}
					stringBuilder.Append(c2);
					i++;
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00024184 File Offset: 0x00022384
		public static bool IsNullOrWhitespace(this string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				for (int i = 0; i < str.Length; i++)
				{
					if (!char.IsWhiteSpace(str.get_Chars(i)))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
