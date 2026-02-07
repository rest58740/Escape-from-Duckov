using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x02000053 RID: 83
	public class NameFilter : IScanFilter
	{
		// Token: 0x060003C8 RID: 968 RVA: 0x00016220 File Offset: 0x00014420
		public NameFilter(string filter)
		{
			this.filter_ = filter;
			this.inclusions_ = new ArrayList();
			this.exclusions_ = new ArrayList();
			this.Compile();
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001624C File Offset: 0x0001444C
		public static bool IsValidExpression(string expression)
		{
			bool result = true;
			try
			{
				Regex regex = new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			}
			catch (ArgumentException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00016290 File Offset: 0x00014490
		public static bool IsValidFilterExpression(string toTest)
		{
			bool result = true;
			try
			{
				if (toTest != null)
				{
					string[] array = NameFilter.SplitQuoted(toTest);
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != null && array[i].Length > 0)
						{
							string pattern;
							if (array[i][0] == '+')
							{
								pattern = array[i].Substring(1, array[i].Length - 1);
							}
							else if (array[i][0] == '-')
							{
								pattern = array[i].Substring(1, array[i].Length - 1);
							}
							else
							{
								pattern = array[i];
							}
							Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
						}
					}
				}
			}
			catch (ArgumentException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00016360 File Offset: 0x00014560
		public static string[] SplitQuoted(string original)
		{
			char c = '\\';
			char[] array = new char[]
			{
				';'
			};
			ArrayList arrayList = new ArrayList();
			if (original != null && original.Length > 0)
			{
				int i = -1;
				StringBuilder stringBuilder = new StringBuilder();
				while (i < original.Length)
				{
					i++;
					if (i >= original.Length)
					{
						arrayList.Add(stringBuilder.ToString());
					}
					else if (original[i] == c)
					{
						i++;
						if (i >= original.Length)
						{
							throw new ArgumentException("Missing terminating escape character", "original");
						}
						if (Array.IndexOf<char>(array, original[i]) < 0)
						{
							stringBuilder.Append(c);
						}
						stringBuilder.Append(original[i]);
					}
					else if (Array.IndexOf<char>(array, original[i]) >= 0)
					{
						arrayList.Add(stringBuilder.ToString());
						stringBuilder.Length = 0;
					}
					else
					{
						stringBuilder.Append(original[i]);
					}
				}
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00016480 File Offset: 0x00014680
		public override string ToString()
		{
			return this.filter_;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00016488 File Offset: 0x00014688
		public bool IsIncluded(string name)
		{
			bool result = false;
			if (this.inclusions_.Count == 0)
			{
				result = true;
			}
			else
			{
				foreach (object obj in this.inclusions_)
				{
					Regex regex = (Regex)obj;
					if (regex.IsMatch(name))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00016520 File Offset: 0x00014720
		public bool IsExcluded(string name)
		{
			bool result = false;
			foreach (object obj in this.exclusions_)
			{
				Regex regex = (Regex)obj;
				if (regex.IsMatch(name))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000165A0 File Offset: 0x000147A0
		public bool IsMatch(string name)
		{
			return this.IsIncluded(name) && !this.IsExcluded(name);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000165BC File Offset: 0x000147BC
		private void Compile()
		{
			if (this.filter_ == null)
			{
				return;
			}
			string[] array = NameFilter.SplitQuoted(this.filter_);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].Length > 0)
				{
					bool flag = array[i][0] != '-';
					string pattern;
					if (array[i][0] == '+')
					{
						pattern = array[i].Substring(1, array[i].Length - 1);
					}
					else if (array[i][0] == '-')
					{
						pattern = array[i].Substring(1, array[i].Length - 1);
					}
					else
					{
						pattern = array[i];
					}
					if (flag)
					{
						this.inclusions_.Add(new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline));
					}
					else
					{
						this.exclusions_.Add(new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline));
					}
				}
			}
		}

		// Token: 0x040002B2 RID: 690
		private string filter_;

		// Token: 0x040002B3 RID: 691
		private ArrayList inclusions_;

		// Token: 0x040002B4 RID: 692
		private ArrayList exclusions_;
	}
}
