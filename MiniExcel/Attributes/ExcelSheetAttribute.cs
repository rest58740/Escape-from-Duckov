using System;
using MiniExcelLibs.OpenXml;

namespace MiniExcelLibs.Attributes
{
	// Token: 0x02000070 RID: 112
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class ExcelSheetAttribute : Attribute
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x00013651 File Offset: 0x00011851
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x00013659 File Offset: 0x00011859
		public string Name { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003AA RID: 938 RVA: 0x00013662 File Offset: 0x00011862
		// (set) Token: 0x060003AB RID: 939 RVA: 0x0001366A File Offset: 0x0001186A
		public SheetState State { get; set; }
	}
}
