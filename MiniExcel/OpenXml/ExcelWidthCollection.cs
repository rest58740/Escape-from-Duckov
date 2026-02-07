using System;
using System.Collections.Generic;
using System.Linq;
using MiniExcelLibs.Utils;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x02000046 RID: 70
	public sealed class ExcelWidthCollection
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000A453 File Offset: 0x00008653
		public IEnumerable<ExcelColumnWidth> Columns
		{
			get
			{
				return this._columnWidths.Values;
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000A460 File Offset: 0x00008660
		internal ExcelWidthCollection(double minWidth, double maxWidth, IEnumerable<ExcelColumnInfo> props)
		{
			this._maxWidth = maxWidth;
			this._columnWidths = ExcelColumnWidth.FromProps(props, new double?(minWidth)).ToDictionary((ExcelColumnWidth x) => x.Index);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000A4B0 File Offset: 0x000086B0
		public void AdjustWidth(int columnIndex, string columnValue)
		{
			ExcelColumnWidth excelColumnWidth;
			if (string.IsNullOrEmpty(columnValue) || !this._columnWidths.TryGetValue(columnIndex, out excelColumnWidth))
			{
				return;
			}
			double val = Math.Max(excelColumnWidth.Width, ExcelWidthCollection.GetApproximateRequiredCalibriWidth(columnValue.Length));
			excelColumnWidth.Width = Math.Min(this._maxWidth, val);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000A500 File Offset: 0x00008700
		public static double GetApproximateRequiredCalibriWidth(int textLength)
		{
			double num = 1.2;
			double num2 = 2.0;
			return Math.Round((double)textLength * num + num2, 2);
		}

		// Token: 0x040000B5 RID: 181
		private readonly Dictionary<int, ExcelColumnWidth> _columnWidths;

		// Token: 0x040000B6 RID: 182
		private readonly double _maxWidth;
	}
}
