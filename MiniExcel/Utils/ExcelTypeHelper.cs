using System;
using System.IO;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000035 RID: 53
	public static class ExcelTypeHelper
	{
		// Token: 0x06000170 RID: 368 RVA: 0x00006240 File Offset: 0x00004440
		internal static ExcelType GetExcelType(string filePath, ExcelType excelType)
		{
			if (excelType != ExcelType.UNKNOWN)
			{
				return excelType;
			}
			string text = Path.GetExtension(filePath).ToLowerInvariant();
			if (text == ".csv")
			{
				return ExcelType.CSV;
			}
			if (!(text == ".xlsx") && !(text == ".xlsm"))
			{
				throw new NotSupportedException("Extension : " + text + " not suppprt, or you can specify exceltype.");
			}
			return ExcelType.XLSX;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000062A4 File Offset: 0x000044A4
		internal static ExcelType GetExcelType(Stream stream, ExcelType excelType)
		{
			if (excelType != ExcelType.UNKNOWN)
			{
				return excelType;
			}
			byte[] array = new byte[8];
			stream.Seek(0L, SeekOrigin.Begin);
			stream.Read(array, 0, array.Length);
			stream.Seek(0L, SeekOrigin.Begin);
			if (array[0] == 80 && array[1] == 75)
			{
				return ExcelType.XLSX;
			}
			throw new NotSupportedException("Stream cannot know the file type, please specify ExcelType manually");
		}
	}
}
