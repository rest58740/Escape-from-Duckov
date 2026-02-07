using System;

namespace MiniExcelLibs.Attributes
{
	// Token: 0x0200006A RID: 106
	public class DynamicExcelColumn : ExcelColumnAttribute
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600038E RID: 910 RVA: 0x000134E6 File Offset: 0x000116E6
		// (set) Token: 0x0600038F RID: 911 RVA: 0x000134EE File Offset: 0x000116EE
		public string Key { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000390 RID: 912 RVA: 0x000134F7 File Offset: 0x000116F7
		// (set) Token: 0x06000391 RID: 913 RVA: 0x000134FF File Offset: 0x000116FF
		public Func<object, string> CustomFormatter { get; set; }

		// Token: 0x06000392 RID: 914 RVA: 0x00013508 File Offset: 0x00011708
		public DynamicExcelColumn(string key)
		{
			this.Key = key;
		}
	}
}
