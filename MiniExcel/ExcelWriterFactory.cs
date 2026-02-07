using System;
using System.IO;
using MiniExcelLibs.Csv;
using MiniExcelLibs.OpenXml;

namespace MiniExcelLibs
{
	// Token: 0x0200000A RID: 10
	internal class ExcelWriterFactory
	{
		// Token: 0x06000024 RID: 36 RVA: 0x0000267C File Offset: 0x0000087C
		internal static IExcelWriter GetProvider(Stream stream, object value, string sheetName, ExcelType excelType, IConfiguration configuration, bool printHeader)
		{
			if (string.IsNullOrEmpty(sheetName))
			{
				throw new InvalidDataException("Sheet name can not be empty or null");
			}
			if (excelType == ExcelType.UNKNOWN)
			{
				throw new InvalidDataException("Please specify excelType");
			}
			if (excelType == ExcelType.XLSX)
			{
				return new ExcelOpenXmlSheetWriter(stream, value, sheetName, configuration, printHeader);
			}
			if (excelType == ExcelType.CSV)
			{
				return new CsvWriter(stream, value, configuration, printHeader);
			}
			throw new NotSupportedException("Please Issue for me");
		}
	}
}
