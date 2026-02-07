using System;
using System.Globalization;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000039 RID: 57
	internal static class ReferenceHelper
	{
		// Token: 0x06000176 RID: 374 RVA: 0x00006454 File Offset: 0x00004654
		public static string GetCellNumber(string cell)
		{
			string text = string.Empty;
			for (int i = 0; i < cell.Length; i++)
			{
				if (char.IsDigit(cell[i]))
				{
					text += cell[i].ToString();
				}
			}
			return text;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000064A0 File Offset: 0x000046A0
		public static string GetCellLetter(string cell)
		{
			string text = string.Empty;
			for (int i = 0; i < cell.Length; i++)
			{
				if (char.IsLetter(cell[i]))
				{
					text += cell[i].ToString();
				}
			}
			return text;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000064EC File Offset: 0x000046EC
		public static Tuple<int, int> ConvertCellToXY(string cell)
		{
			int num = 0;
			string cellLetter = ReferenceHelper.GetCellLetter(cell);
			for (int i = 0; i < cellLetter.Length; i++)
			{
				num = num * 26 + " ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(cellLetter[i]);
			}
			string cellNumber = ReferenceHelper.GetCellNumber(cell);
			return Tuple.Create<int, int>(num, int.Parse(cellNumber));
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006540 File Offset: 0x00004740
		public static string ConvertXyToCell(int x, int y)
		{
			int i = x;
			string text = string.Empty;
			while (i > 0)
			{
				int num = (i - 1) % 26;
				text = Convert.ToChar(65 + num).ToString() + text;
				i = (i - num) / 26;
			}
			return string.Format("{0}{1}", text, y);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00006594 File Offset: 0x00004794
		public static bool ParseReference(string value, out int column, out int row)
		{
			column = 0;
			int i = 0;
			if (value != null)
			{
				while (i < value.Length)
				{
					char c = value[i];
					if (c >= 'A' && c <= 'Z')
					{
						i++;
						column *= 26;
						column += (int)(c - '@');
					}
					else
					{
						if (!char.IsDigit(c))
						{
							i = 0;
							break;
						}
						break;
					}
				}
			}
			if (i == 0)
			{
				column = 0;
				row = 0;
				return false;
			}
			return int.TryParse(value.Substring(i), NumberStyles.None, CultureInfo.InvariantCulture, out row);
		}
	}
}
