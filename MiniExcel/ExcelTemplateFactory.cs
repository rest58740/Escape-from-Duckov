using System;
using System.IO;
using MiniExcelLibs.OpenXml.SaveByTemplate;

namespace MiniExcelLibs
{
	// Token: 0x0200000B RID: 11
	internal class ExcelTemplateFactory
	{
		// Token: 0x06000026 RID: 38 RVA: 0x000026E0 File Offset: 0x000008E0
		internal static IExcelTemplateAsync GetProvider(Stream stream, IConfiguration configuration, ExcelType excelType = ExcelType.XLSX)
		{
			if (excelType == ExcelType.XLSX)
			{
				InputValueExtractor inputValueExtractor = new InputValueExtractor();
				return new ExcelOpenXmlTemplate(stream, configuration, inputValueExtractor);
			}
			throw new NotSupportedException("Please Issue for me");
		}
	}
}
