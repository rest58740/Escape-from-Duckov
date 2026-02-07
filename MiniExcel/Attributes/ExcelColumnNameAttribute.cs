using System;

namespace MiniExcelLibs.Attributes
{
	// Token: 0x0200006C RID: 108
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class ExcelColumnNameAttribute : Attribute
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600039A RID: 922 RVA: 0x000135B9 File Offset: 0x000117B9
		// (set) Token: 0x0600039B RID: 923 RVA: 0x000135C1 File Offset: 0x000117C1
		public string ExcelColumnName { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600039C RID: 924 RVA: 0x000135CA File Offset: 0x000117CA
		// (set) Token: 0x0600039D RID: 925 RVA: 0x000135D2 File Offset: 0x000117D2
		public string[] Aliases { get; set; }

		// Token: 0x0600039E RID: 926 RVA: 0x000135DB File Offset: 0x000117DB
		public ExcelColumnNameAttribute(string excelColumnName, string[] aliases = null)
		{
			this.ExcelColumnName = excelColumnName;
			this.Aliases = aliases;
		}
	}
}
