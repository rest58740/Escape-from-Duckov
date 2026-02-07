using System;
using System.Globalization;
using System.Text;

namespace Sirenix.Utilities
{
	// Token: 0x0200000F RID: 15
	public static class StringExtensions
	{
		// Token: 0x0600009E RID: 158 RVA: 0x00004188 File Offset: 0x00002388
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

		// Token: 0x0600009F RID: 159 RVA: 0x000041FE File Offset: 0x000023FE
		public static bool Contains(this string source, string toCheck, StringComparison comparisonType)
		{
			return source.IndexOf(toCheck, comparisonType) >= 0;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004210 File Offset: 0x00002410
		public static string SplitPascalCase(this string input)
		{
			if (input == null || input.Length == 0)
			{
				return input;
			}
			StringBuilder stringBuilder = new StringBuilder(input.Length);
			if (char.IsLetter(input.get_Chars(0)))
			{
				stringBuilder.Append(char.ToUpper(input.get_Chars(0)));
			}
			else
			{
				stringBuilder.Append(input.get_Chars(0));
			}
			for (int i = 1; i < input.Length; i++)
			{
				char c = input.get_Chars(i);
				if (char.IsUpper(c) && !char.IsUpper(input.get_Chars(i - 1)))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000042B0 File Offset: 0x000024B0
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

		// Token: 0x060000A2 RID: 162 RVA: 0x000042E8 File Offset: 0x000024E8
		public static int CalculateLevenshteinDistance(string source1, string source2)
		{
			int length = source1.Length;
			int length2 = source2.Length;
			int[,] array = new int[length + 1, length2 + 1];
			if (length == 0)
			{
				return length2;
			}
			if (length2 == 0)
			{
				return length;
			}
			int i = 0;
			while (i <= length)
			{
				array[i, 0] = i++;
			}
			int j = 0;
			while (j <= length2)
			{
				array[0, j] = j++;
			}
			for (int k = 1; k <= length; k++)
			{
				for (int l = 1; l <= length2; l++)
				{
					int num = (source2.get_Chars(l - 1) != source1.get_Chars(k - 1)) ? 1 : 0;
					array[k, l] = Math.Min(Math.Min(array[k - 1, l] + 1, array[k, l - 1] + 1), array[k - 1, l - 1] + num);
				}
			}
			return array[length, length2];
		}
	}
}
