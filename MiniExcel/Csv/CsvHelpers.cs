using System;

namespace MiniExcelLibs.Csv
{
	// Token: 0x02000065 RID: 101
	internal static class CsvHelpers
	{
		// Token: 0x0600035F RID: 863 RVA: 0x00012C74 File Offset: 0x00010E74
		public static string ConvertToCsvValue(string value, bool alwaysQuote, char separator)
		{
			if (value == null)
			{
				return string.Empty;
			}
			if (value.Contains("\""))
			{
				value = value.Replace("\"", "\"\"");
				return "\"" + value + "\"";
			}
			if (value.Contains(separator.ToString()) || value.Contains(" ") || value.Contains("\n") || value.Contains("\r"))
			{
				return "\"" + value + "\"";
			}
			if (alwaysQuote)
			{
				return "\"" + value + "\"";
			}
			return value;
		}
	}
}
