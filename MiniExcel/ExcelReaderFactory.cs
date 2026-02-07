using System;
using System.IO;
using MiniExcelLibs.Csv;
using MiniExcelLibs.OpenXml;

namespace MiniExcelLibs
{
	// Token: 0x02000009 RID: 9
	internal class ExcelReaderFactory
	{
		// Token: 0x06000022 RID: 34 RVA: 0x0000264E File Offset: 0x0000084E
		internal static IExcelReader GetProvider(Stream stream, ExcelType excelType, IConfiguration configuration)
		{
			if (excelType == ExcelType.XLSX)
			{
				return new ExcelOpenXmlSheetReader(stream, configuration);
			}
			if (excelType == ExcelType.CSV)
			{
				return new CsvReader(stream, configuration);
			}
			throw new NotSupportedException("Please Issue for me");
		}
	}
}
