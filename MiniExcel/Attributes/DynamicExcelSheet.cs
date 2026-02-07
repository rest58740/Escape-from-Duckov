using System;

namespace MiniExcelLibs.Attributes
{
	// Token: 0x02000071 RID: 113
	public class DynamicExcelSheet : ExcelSheetAttribute
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003AD RID: 941 RVA: 0x0001367B File Offset: 0x0001187B
		// (set) Token: 0x060003AE RID: 942 RVA: 0x00013683 File Offset: 0x00011883
		public string Key { get; set; }

		// Token: 0x060003AF RID: 943 RVA: 0x0001368C File Offset: 0x0001188C
		public DynamicExcelSheet(string key)
		{
			this.Key = key;
		}
	}
}
