using System;
using System.Globalization;
using MiniExcelLibs.Attributes;

namespace MiniExcelLibs.OpenXml.Constants
{
	// Token: 0x02000061 RID: 97
	internal class WorksheetXml
	{
		// Token: 0x0600032C RID: 812 RVA: 0x00012775 File Offset: 0x00010975
		internal static string Dimension(string dimensionRef)
		{
			return "<x:dimension ref=\"" + dimensionRef + "\" />";
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00012787 File Offset: 0x00010987
		internal static string StartSheetView(int tabSelected = 0, int workbookViewId = 0)
		{
			return string.Format("<x:sheetView tabSelected=\"{0}\" workbookViewId=\"{1}\">", tabSelected, workbookViewId);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000127A0 File Offset: 0x000109A0
		internal static string StartPane(int? xSplit, int? ySplit, string topLeftCell, string activePane, string state)
		{
			return string.Concat(new string[]
			{
				"<x:pane",
				(xSplit != null) ? string.Format(" xSplit=\"{0}\"", xSplit.Value) : string.Empty,
				(ySplit != null) ? string.Format(" ySplit=\"{0}\"", ySplit.Value) : string.Empty,
				" topLeftCell=\"" + topLeftCell + "\"",
				" activePane=\"" + activePane + "\"",
				" state=\"" + state + "\"",
				"/>"
			});
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00012854 File Offset: 0x00010A54
		internal static string PaneSelection(string pane, string activeCell, string sqref)
		{
			return string.Concat(new string[]
			{
				"<x:selection",
				" pane=\"" + pane + "\"",
				string.IsNullOrWhiteSpace(activeCell) ? string.Empty : (" activeCell=\"" + activeCell + "\""),
				string.IsNullOrWhiteSpace(sqref) ? string.Empty : (" sqref=\"" + sqref + "\""),
				"/>"
			});
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000128D3 File Offset: 0x00010AD3
		internal static string StartRow(int rowIndex)
		{
			return string.Format("<x:row r=\"{0}\">", rowIndex);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000128E5 File Offset: 0x00010AE5
		internal static string Column(int colIndex, double columnWidth)
		{
			return string.Format("<x:col min=\"{0}\" max=\"{1}\" width=\"{2}\" customWidth=\"1\" />", colIndex, colIndex, columnWidth.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00012909 File Offset: 0x00010B09
		public static int GetColumnPlaceholderLength(int columnCount)
		{
			return "<x:cols>".Length + WorksheetXml._maxColumnLength * columnCount + "</x:cols>".Length;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00012928 File Offset: 0x00010B28
		internal static string EmptyCell(string cellReference, string styleIndex)
		{
			return string.Concat(new string[]
			{
				"<x:c r=\"",
				cellReference,
				"\" s=\"",
				styleIndex,
				"\"></x:c>"
			});
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00012958 File Offset: 0x00010B58
		internal static string Cell(string cellReference, string cellType, string styleIndex, string cellValue, bool preserveSpace = false, ColumnType columnType = ColumnType.Value)
		{
			return string.Concat(new string[]
			{
				"<x:c r=\"",
				cellReference,
				"\"",
				(cellType == null) ? string.Empty : (" t=\"" + cellType + "\""),
				" s=\"",
				styleIndex,
				"\"",
				preserveSpace ? " xml:space=\"preserve\"" : string.Empty,
				"><x:",
				(columnType == ColumnType.Formula) ? "f" : "v",
				">",
				cellValue,
				"</x:",
				(columnType == ColumnType.Formula) ? "f" : "v",
				"></x:c>"
			});
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00012A1B File Offset: 0x00010C1B
		internal static string Autofilter(string dimensionRef)
		{
			return "<x:autoFilter ref=\"" + dimensionRef + "\" />";
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00012A2D File Offset: 0x00010C2D
		internal static string Drawing(int sheetIndex)
		{
			return string.Format("<x:drawing r:id=\"drawing{0}\" />", sheetIndex);
		}

		// Token: 0x04000139 RID: 313
		internal const string StartWorksheet = "<?xml version=\"1.0\" encoding=\"utf-8\"?><x:worksheet xmlns:x=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">";

		// Token: 0x0400013A RID: 314
		internal const string StartWorksheetWithRelationship = "<?xml version=\"1.0\" encoding=\"utf-8\"?><x:worksheet xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" xmlns:x=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" >";

		// Token: 0x0400013B RID: 315
		internal const string EndWorksheet = "</x:worksheet>";

		// Token: 0x0400013C RID: 316
		internal const string StartDimension = "<x:dimension ref=\"";

		// Token: 0x0400013D RID: 317
		internal const string DimensionPlaceholder = "                              />";

		// Token: 0x0400013E RID: 318
		internal const string StartSheetViews = "<x:sheetViews>";

		// Token: 0x0400013F RID: 319
		internal const string EndSheetViews = "</x:sheetViews>";

		// Token: 0x04000140 RID: 320
		internal const string EndSheetView = "</x:sheetView>";

		// Token: 0x04000141 RID: 321
		internal const string StartSheetData = "<x:sheetData>";

		// Token: 0x04000142 RID: 322
		internal const string EndSheetData = "</x:sheetData>";

		// Token: 0x04000143 RID: 323
		internal const string EndRow = "</x:row>";

		// Token: 0x04000144 RID: 324
		internal const string StartCols = "<x:cols>";

		// Token: 0x04000145 RID: 325
		private static readonly int _maxColumnLength = WorksheetXml.Column(int.MaxValue, double.MaxValue).Length;

		// Token: 0x04000146 RID: 326
		internal const string EndCols = "</x:cols>";
	}
}
