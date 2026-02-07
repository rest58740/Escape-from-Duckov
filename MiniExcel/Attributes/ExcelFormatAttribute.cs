using System;

namespace MiniExcelLibs.Attributes
{
	// Token: 0x0200006E RID: 110
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class ExcelFormatAttribute : Attribute
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00013611 File Offset: 0x00011811
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x00013619 File Offset: 0x00011819
		public string Format { get; set; }

		// Token: 0x060003A4 RID: 932 RVA: 0x00013622 File Offset: 0x00011822
		public ExcelFormatAttribute(string format)
		{
			this.Format = format;
		}
	}
}
