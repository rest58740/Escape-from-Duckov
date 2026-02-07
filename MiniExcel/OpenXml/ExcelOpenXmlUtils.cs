using System;
using MiniExcelLibs.Utils;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x02000044 RID: 68
	internal static class ExcelOpenXmlUtils
	{
		// Token: 0x060001FC RID: 508 RVA: 0x0000A186 File Offset: 0x00008386
		public static string MinifyXml(string xml)
		{
			return xml.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000A1BC File Offset: 0x000083BC
		public static string EncodeXML(string value)
		{
			if (value != null)
			{
				return XmlEncoder.EncodeString(value).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;").ToString();
			}
			return string.Empty;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000A228 File Offset: 0x00008428
		public static string ConvertXyToCell(Tuple<int, int> xy)
		{
			return ExcelOpenXmlUtils.ConvertXyToCell(xy.Item1, xy.Item2);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A23C File Offset: 0x0000843C
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

		// Token: 0x06000200 RID: 512 RVA: 0x0000A28E File Offset: 0x0000848E
		public static Tuple<int, int> ConvertCellToXY(string cell)
		{
			return Tuple.Create<int, int>(ExcelOpenXmlUtils.GetCellColumnIndex(cell), ExcelOpenXmlUtils.GetCellRowNumber(cell));
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A2A4 File Offset: 0x000084A4
		public static int GetColumnNumber(string name)
		{
			int num = -1;
			int num2 = 1;
			for (int i = name.Length - 1; i >= 0; i--)
			{
				num += (int)(name[i] - 'A' + '\u0001') * num2;
				num2 *= 26;
			}
			return num;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A2E0 File Offset: 0x000084E0
		public static int GetCellColumnIndex(string cell)
		{
			int num = 0;
			string cellColumnLetter = ExcelOpenXmlUtils.GetCellColumnLetter(cell);
			for (int i = 0; i < cellColumnLetter.Length; i++)
			{
				num = num * 26 + " ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(cellColumnLetter[i]);
			}
			return num;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000A320 File Offset: 0x00008520
		public static int GetCellRowNumber(string cell)
		{
			if (string.IsNullOrEmpty(cell))
			{
				throw new Exception("cell is null or empty");
			}
			string text = string.Empty;
			for (int i = 0; i < cell.Length; i++)
			{
				if (char.IsDigit(cell[i]))
				{
					text += cell[i].ToString();
				}
			}
			return int.Parse(text);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000A384 File Offset: 0x00008584
		public static string GetCellColumnLetter(string cell)
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

		// Token: 0x06000205 RID: 517 RVA: 0x0000A3D0 File Offset: 0x000085D0
		public static string ConvertColumnName(int x)
		{
			int i = x;
			string text = string.Empty;
			while (i > 0)
			{
				int num = (i - 1) % 26;
				text = Convert.ToChar(65 + num).ToString() + text;
				i = (i - num) / 26;
			}
			return text;
		}
	}
}
