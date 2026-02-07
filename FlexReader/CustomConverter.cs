using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FlexFramework.Excel
{
	// Token: 0x02000003 RID: 3
	public abstract class CustomConverter<T> : IConverter
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		Type IConverter.Type
		{
			get
			{
				return typeof(T);
			}
		}

		// Token: 0x06000004 RID: 4
		public abstract T Convert(string input);

		// Token: 0x06000005 RID: 5 RVA: 0x000020CC File Offset: 0x000002CC
		public bool TryConvert(string input, out T value)
		{
			value = default(T);
			try
			{
				value = this.Convert(input);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002108 File Offset: 0x00000308
		protected string[] Split(string input, params char[] separators)
		{
			string text = string.Empty;
			for (int i = 0; i < separators.Length; i++)
			{
				text = ((i > 0) ? "|" : (string.Empty + text + "(?<!\\\\)" + separators[i].ToString()));
			}
			string[] array = Regex.Split(input, text, RegexOptions.Multiline);
			for (int j = 0; j < array.Length; j++)
			{
				foreach (char c in separators)
				{
					array[j] = array[j].Replace("\\" + c.ToString(), c.ToString());
				}
			}
			return array;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021A9 File Offset: 0x000003A9
		protected IEnumerable<string> SplitGroup(string input, char start, char end)
		{
			string pattern = "\\G\\[(\\\\\\[|\\\\\\]|[^\\[\\]])+\\]".Replace('[', start).Replace(']', end);
			foreach (object obj in Regex.Matches(input, pattern, RegexOptions.Multiline))
			{
				Match match = (Match)obj;
				string text = match.Value;
				if (!match.Success)
				{
					throw new ArgumentException("Group expression invalid", input);
				}
				text = text.TrimStart(start).TrimEnd(end);
				text = text.Replace("\\" + start.ToString(), start.ToString()).Replace("\\" + end.ToString(), end.ToString());
				if (Regex.IsMatch(text, "(?<=\\([^]]*),(?=[^]]*\\))"))
				{
					text = Regex.Replace(text, "(?<=\\([^]]*),(?=[^]]*\\))", "\\,");
				}
				yield return text;
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021C7 File Offset: 0x000003C7
		object IConverter.Convert(string input)
		{
			return this.Convert(input);
		}
	}
}
