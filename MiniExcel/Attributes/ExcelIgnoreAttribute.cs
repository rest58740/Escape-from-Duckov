using System;

namespace MiniExcelLibs.Attributes
{
	// Token: 0x0200006F RID: 111
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class ExcelIgnoreAttribute : Attribute
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00013631 File Offset: 0x00011831
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00013639 File Offset: 0x00011839
		public bool ExcelIgnore { get; set; }

		// Token: 0x060003A7 RID: 935 RVA: 0x00013642 File Offset: 0x00011842
		public ExcelIgnoreAttribute(bool excelIgnore = true)
		{
			this.ExcelIgnore = excelIgnore;
		}
	}
}
