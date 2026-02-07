using System;

namespace MiniExcelLibs.OpenXml.Constants
{
	// Token: 0x0200005F RID: 95
	internal static class ExcelFileNames
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0001250E File Offset: 0x0001070E
		internal static string SheetRels(int sheetId)
		{
			return string.Format("xl/worksheets/_rels/sheet{0}.xml.rels", sheetId);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00012520 File Offset: 0x00010720
		internal static string Drawing(int sheetIndex)
		{
			return string.Format("xl/drawings/drawing{0}.xml", sheetIndex + 1);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00012534 File Offset: 0x00010734
		internal static string DrawingRels(int sheetIndex)
		{
			return string.Format("xl/drawings/_rels/drawing{0}.xml.rels", sheetIndex + 1);
		}

		// Token: 0x04000129 RID: 297
		internal const string Rels = "_rels/.rels";

		// Token: 0x0400012A RID: 298
		internal const string SharedStrings = "xl/sharedStrings.xml";

		// Token: 0x0400012B RID: 299
		internal const string ContentTypes = "[Content_Types].xml";

		// Token: 0x0400012C RID: 300
		internal const string Styles = "xl/styles.xml";

		// Token: 0x0400012D RID: 301
		internal const string Workbook = "xl/workbook.xml";

		// Token: 0x0400012E RID: 302
		internal const string WorkbookRels = "xl/_rels/workbook.xml.rels";
	}
}
