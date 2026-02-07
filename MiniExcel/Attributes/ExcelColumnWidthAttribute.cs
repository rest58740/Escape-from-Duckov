using System;

namespace MiniExcelLibs.Attributes
{
	// Token: 0x0200006D RID: 109
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class ExcelColumnWidthAttribute : Attribute
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600039F RID: 927 RVA: 0x000135F1 File Offset: 0x000117F1
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x000135F9 File Offset: 0x000117F9
		public double ExcelColumnWidth { get; set; }

		// Token: 0x060003A1 RID: 929 RVA: 0x00013602 File Offset: 0x00011802
		public ExcelColumnWidthAttribute(double excelColumnWidth)
		{
			this.ExcelColumnWidth = excelColumnWidth;
		}
	}
}
