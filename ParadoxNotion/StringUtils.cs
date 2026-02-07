using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x0200007D RID: 125
	public static class StringUtils
	{
		// Token: 0x060004B4 RID: 1204 RVA: 0x0000D658 File Offset: 0x0000B858
		public static string SplitCamelCase(this string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			string text;
			if (StringUtils.splitCaseCache.TryGetValue(s, ref text))
			{
				return text;
			}
			text = s;
			int num = text.IndexOf('_');
			if (num <= 1)
			{
				text = text.Substring(num + 1);
			}
			text = Regex.Replace(text, "(?<=[a-z])([A-Z])", " $1").CapitalizeFirst().Trim();
			return StringUtils.splitCaseCache[s] = text;
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000D6C4 File Offset: 0x0000B8C4
		public static string CapitalizeFirst(this string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			return s.First<char>().ToString().ToUpper() + s.Substring(1);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000D6FA File Offset: 0x0000B8FA
		public static string CapLength(this string s, int max)
		{
			if (string.IsNullOrEmpty(s) || s.Length <= max || max <= 3)
			{
				return s;
			}
			return s.Substring(0, Mathf.Min(s.Length, max) - 3) + "...";
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0000D734 File Offset: 0x0000B934
		public static string GetCapitals(this string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			string text = "";
			for (int i = 0; i < s.Length; i++)
			{
				char c = s.get_Chars(i);
				if (char.IsUpper(c))
				{
					text += c.ToString();
				}
			}
			return text.Trim();
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0000D78E File Offset: 0x0000B98E
		public static string FormatError(this string input)
		{
			return string.Format("<color=#ff6457>* {0} *</color>", input);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0000D79C File Offset: 0x0000B99C
		public static string GetAlphabetLetter(int index)
		{
			if (index < 0)
			{
				return null;
			}
			if (index >= "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length)
			{
				return index.ToString();
			}
			return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".get_Chars(index).ToString();
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0000D7D8 File Offset: 0x0000B9D8
		public static string GetStringWithinOuter(this string input, char from, char to)
		{
			int num = input.IndexOf(from) + 1;
			int num2 = input.LastIndexOf(to);
			if (num < 0 || num2 < num)
			{
				return null;
			}
			return input.Substring(num, num2 - num);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0000D80C File Offset: 0x0000BA0C
		public static string GetStringWithinInner(this string input, char from, char to)
		{
			int num = input.IndexOf(to);
			int num2 = int.MinValue;
			int num3 = 0;
			while (num3 < input.Length && num3 <= num)
			{
				if (input.get_Chars(num3) == from)
				{
					num2 = num3;
				}
				num3++;
			}
			num2++;
			if (num2 < 0 || num < num2)
			{
				return null;
			}
			return input.Substring(num2, num - num2);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000D860 File Offset: 0x0000BA60
		public static string ReplaceWithin(this string text, char startChar, char endChar, Func<string, string> Process)
		{
			string text2 = text;
			int num = 0;
			while ((num = text2.IndexOf(startChar, num)) != -1)
			{
				int num2 = text2.Substring(num + 1).IndexOf(endChar);
				string text3 = text2.Substring(num + 1, num2);
				string text4 = text2.Substring(num, num2 + 2);
				string text5 = Process.Invoke(text3);
				text2 = text2.Replace(text4, text5);
				num++;
			}
			return text2;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0000D8C0 File Offset: 0x0000BAC0
		public static float ScoreSearchMatch(string input, string leafName, string categoryName = "")
		{
			if (input == null || leafName == null)
			{
				return float.PositiveInfinity;
			}
			if (categoryName == null)
			{
				categoryName = string.Empty;
			}
			input = input.ToUpper();
			string[] array = input.Replace('.', ' ').Split(StringUtils.CHAR_SPACE_ARRAY, 1);
			if (array.Length == 0)
			{
				return 1f;
			}
			leafName = leafName.ToUpper();
			string text = leafName.Split(StringUtils.CHAR_SPACE_ARRAY, 1)[0];
			leafName = leafName.Replace(" ", string.Empty);
			if (input.LastOrDefault<char>() == '.')
			{
				leafName = categoryName.ToUpper().Replace(" ", string.Empty);
			}
			float num = 1f;
			if (categoryName.Contains(array[0]))
			{
				num *= 0.9f;
			}
			if (text == array[array.Length - 1])
			{
				num *= 0.5f;
			}
			if (leafName.StartsWith(array[0]))
			{
				num *= 0.5f;
			}
			if (leafName.StartsWith(array[array.Length - 1]))
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000D9B0 File Offset: 0x0000BBB0
		public static bool SearchMatch(string input, string leafName, string categoryName = "")
		{
			if (input == null || leafName == null)
			{
				return false;
			}
			if (categoryName == null)
			{
				categoryName = string.Empty;
			}
			if (leafName.Length <= 1 && input.Length <= 2)
			{
				string text = null;
				if (ReflectionTools.op_CSharpAliases.TryGetValue(input, ref text))
				{
					return text == leafName;
				}
			}
			if (input.Length <= 1)
			{
				return input == leafName;
			}
			input = input.ToUpper();
			leafName = leafName.ToUpper().Replace(" ", string.Empty);
			categoryName = categoryName.ToUpper().Replace(" ", string.Empty);
			string text2 = categoryName + "/" + leafName;
			string[] array = input.Replace('.', ' ').Split(StringUtils.CHAR_SPACE_ARRAY, 1);
			if (array.Length == 0)
			{
				return false;
			}
			if (input.LastOrDefault<char>() == '.')
			{
				return categoryName.Contains(array[0]);
			}
			string text3 = text2;
			foreach (string text4 in array)
			{
				if (!text3.Contains(text4))
				{
					return false;
				}
				text3 = text3.Substring(text3.IndexOf(text4) + text4.Length);
			}
			string text5 = array[array.Length - 1];
			return leafName.Contains(text5);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000DACC File Offset: 0x0000BCCC
		public static string ToStringAdvanced(this object o)
		{
			if (o == null || o.Equals(null))
			{
				return "NULL";
			}
			if (o is string)
			{
				return string.Format("\"{0}\"", (string)o);
			}
			if (o is Object)
			{
				return (o as Object).name;
			}
			Type type = o.GetType();
			if (type.RTIsSubclassOf(typeof(Enum)) && type.RTIsDefined(true))
			{
				if (o.ToString() == "0")
				{
					return "Nothing";
				}
				if (o.ToString() == "-1")
				{
					return "Everything";
				}
				if (o.ToString().Contains(','))
				{
					return "Mixed...";
				}
			}
			return o.ToString();
		}

		// Token: 0x0400017A RID: 378
		public const string SPACE = " ";

		// Token: 0x0400017B RID: 379
		public const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		// Token: 0x0400017C RID: 380
		public static readonly char[] CHAR_SPACE_ARRAY = new char[]
		{
			' '
		};

		// Token: 0x0400017D RID: 381
		private static Dictionary<string, string> splitCaseCache = new Dictionary<string, string>(StringComparer.Ordinal);
	}
}
