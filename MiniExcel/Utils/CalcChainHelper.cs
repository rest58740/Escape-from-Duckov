using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000026 RID: 38
	internal static class CalcChainHelper
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x000044F0 File Offset: 0x000026F0
		public static string GetCalcChainContent(List<string> cellRefs, int sheetIndex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string arg in cellRefs)
			{
				stringBuilder.Append(string.Format("<c r=\"{0}\" i=\"{1}\"/>", arg, sheetIndex));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000455C File Offset: 0x0000275C
		public static void GenerateCalcChainSheet(Stream calcChainStream, string calcChainContent)
		{
			using (StreamWriter streamWriter = new StreamWriter(calcChainStream, Encoding.UTF8))
			{
				streamWriter.Write("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><calcChain xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">" + calcChainContent + "</calcChain>");
			}
		}
	}
}
