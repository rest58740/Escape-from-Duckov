using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000027 RID: 39
	internal static class ColumnHelper
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x000045A8 File Offset: 0x000027A8
		static ColumnHelper()
		{
			ColumnHelper._IntMappingAlphabetCount = ColumnHelper._IntMappingAlphabet.Count;
			ColumnHelper.CheckAndSetMaxColumnIndex(255);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000045E0 File Offset: 0x000027E0
		public static string GetAlphabetColumnName(int columnIndex)
		{
			ColumnHelper.CheckAndSetMaxColumnIndex(columnIndex);
			string result;
			if (ColumnHelper._IntMappingAlphabet.TryGetValue(columnIndex, out result))
			{
				return result;
			}
			throw new KeyNotFoundException();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000460C File Offset: 0x0000280C
		public static int GetColumnIndex(string columnName)
		{
			int num;
			if (ColumnHelper._AlphabetMappingInt.TryGetValue(columnName, out num))
			{
				ColumnHelper.CheckAndSetMaxColumnIndex(num);
			}
			return num;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004630 File Offset: 0x00002830
		private static void CheckAndSetMaxColumnIndex(int columnIndex)
		{
			if (columnIndex >= ColumnHelper._IntMappingAlphabetCount)
			{
				if (columnIndex > 16383)
				{
					throw new InvalidDataException(string.Format("ColumnIndex {0} over excel vaild max index.", columnIndex));
				}
				int j;
				int i;
				for (i = ColumnHelper._IntMappingAlphabet.Count; i <= columnIndex; i = j + 1)
				{
					ColumnHelper._IntMappingAlphabet.AddOrUpdate(i, ColumnHelper.IntToLetters(i), (int a, string b) => ColumnHelper.IntToLetters(i));
					ColumnHelper._AlphabetMappingInt.AddOrUpdate(ColumnHelper.IntToLetters(i), i, (string a, int b) => i);
					j = i;
				}
				ColumnHelper._IntMappingAlphabetCount = ColumnHelper._IntMappingAlphabet.Count;
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000046F8 File Offset: 0x000028F8
		internal static string IntToLetters(int value)
		{
			value++;
			string text = string.Empty;
			while (--value >= 0)
			{
				text = ((char)(65 + value % 26)).ToString() + text;
				value /= 26;
			}
			return text;
		}

		// Token: 0x0400003D RID: 61
		private const int GENERAL_COLUMN_INDEX = 255;

		// Token: 0x0400003E RID: 62
		private const int MAX_COLUMN_INDEX = 16383;

		// Token: 0x0400003F RID: 63
		private static int _IntMappingAlphabetCount = 0;

		// Token: 0x04000040 RID: 64
		private static readonly ConcurrentDictionary<int, string> _IntMappingAlphabet = new ConcurrentDictionary<int, string>();

		// Token: 0x04000041 RID: 65
		private static readonly ConcurrentDictionary<string, int> _AlphabetMappingInt = new ConcurrentDictionary<string, int>();
	}
}
