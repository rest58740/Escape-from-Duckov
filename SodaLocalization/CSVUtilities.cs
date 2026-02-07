using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace SodaCraft.Localizations
{
	// Token: 0x02000006 RID: 6
	public static class CSVUtilities
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002178 File Offset: 0x00000378
		public static void SaveAsCSV(List<List<string>> table, string path)
		{
			string text = table.ToCSVString();
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (FileStream fileStream = File.Open(path, 2, 3, 3))
			{
				using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
				{
					streamWriter.Write(text);
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002200 File Offset: 0x00000400
		public static List<List<string>> ReadCSV(string csvContent)
		{
			List<List<string>> list = new List<List<string>>();
			csvContent = csvContent.Replace("\r", "\n");
			string[] array = csvContent.Split('\n', 0);
			CSVUtilities.<>c__DisplayClass1_0 CS$<>8__locals1;
			CS$<>8__locals1.tokenBuffer = new List<char>();
			bool flag = false;
			bool flag2 = false;
			foreach (string text in array)
			{
				List<string> list2 = new List<string>();
				CS$<>8__locals1.tokenBuffer.Clear();
				for (int j = 0; j < text.Length; j++)
				{
					char c = text.get_Chars(j);
					if (flag2)
					{
						flag2 = false;
						if (c == 'n')
						{
							CS$<>8__locals1.tokenBuffer.Add('\n');
						}
						else
						{
							CS$<>8__locals1.tokenBuffer.Add(c);
						}
					}
					else if (flag)
					{
						if (c != '"')
						{
							if (c == '\\')
							{
								flag2 = true;
							}
							else
							{
								CS$<>8__locals1.tokenBuffer.Add(c);
							}
						}
						else
						{
							flag = false;
						}
					}
					else if (c != '"')
					{
						if (c != ',')
						{
							if (c == '\\')
							{
								flag2 = true;
							}
							else if (!char.IsWhiteSpace(c) || CS$<>8__locals1.tokenBuffer.Count != 0)
							{
								CS$<>8__locals1.tokenBuffer.Add(c);
							}
						}
						else
						{
							string text2 = CSVUtilities.<ReadCSV>g__FlushBuffer|1_0(ref CS$<>8__locals1);
							text2 = text2.Trim();
							list2.Add(text2);
						}
					}
					else if (CS$<>8__locals1.tokenBuffer.Count == 0)
					{
						flag = true;
					}
				}
				if (CS$<>8__locals1.tokenBuffer.Count > 0)
				{
					string text3 = CSVUtilities.<ReadCSV>g__FlushBuffer|1_0(ref CS$<>8__locals1);
					text3 = text3.Trim();
					list2.Add(text3);
				}
				if (list2.Count > 0)
				{
					list.Add(list2);
				}
			}
			return list;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002394 File Offset: 0x00000594
		public static string ToCSVString(this List<List<string>> table)
		{
			char c = ',';
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < table.Count; i++)
			{
				List<string> list = table[i];
				if (list.Count >= 1)
				{
					for (int j = 0; j < list.Count; j++)
					{
						string text = list[j];
						if (text.Contains(','))
						{
							text = "\"" + text + "\"";
						}
						if (j > 0)
						{
							stringBuilder.Append(c);
						}
						if (text.Contains('\n'))
						{
							Debug.Log("Replacing \\n");
							text.Replace("\n", "\\n");
						}
						stringBuilder.Append(text);
					}
					stringBuilder.Append("\n");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002461 File Offset: 0x00000661
		[CompilerGenerated]
		internal static string <ReadCSV>g__FlushBuffer|1_0(ref CSVUtilities.<>c__DisplayClass1_0 A_0)
		{
			string result = new string(A_0.tokenBuffer.ToArray());
			A_0.tokenBuffer.Clear();
			return result;
		}
	}
}
