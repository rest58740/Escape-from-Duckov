using System;
using MiniExcelLibs.OpenXml;
using MiniExcelLibs.OpenXml.Models;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000029 RID: 41
	internal class ExcellSheetInfo
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000116 RID: 278 RVA: 0x0000482F File Offset: 0x00002A2F
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00004837 File Offset: 0x00002A37
		public object Key { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00004840 File Offset: 0x00002A40
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00004848 File Offset: 0x00002A48
		public string ExcelSheetName { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00004851 File Offset: 0x00002A51
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00004859 File Offset: 0x00002A59
		public SheetState ExcelSheetState { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00004864 File Offset: 0x00002A64
		private string ExcelSheetStateAsString
		{
			get
			{
				return this.ExcelSheetState.ToString().ToLower();
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000488A File Offset: 0x00002A8A
		public SheetDto ToDto(int sheetIndex)
		{
			return new SheetDto
			{
				Name = this.ExcelSheetName,
				SheetIdx = sheetIndex,
				State = this.ExcelSheetStateAsString
			};
		}
	}
}
