using System;
using MiniExcelLibs.Attributes;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x0200004A RID: 74
	public class OpenXmlConfiguration : Configuration
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000A7E5 File Offset: 0x000089E5
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000A7ED File Offset: 0x000089ED
		public bool FillMergedCells { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000A7F6 File Offset: 0x000089F6
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0000A7FE File Offset: 0x000089FE
		public TableStyles TableStyles { get; set; } = TableStyles.Default;

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000A807 File Offset: 0x00008A07
		// (set) Token: 0x0600022A RID: 554 RVA: 0x0000A80F File Offset: 0x00008A0F
		public bool AutoFilter { get; set; } = true;

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000A818 File Offset: 0x00008A18
		// (set) Token: 0x0600022C RID: 556 RVA: 0x0000A820 File Offset: 0x00008A20
		public int FreezeRowCount { get; set; } = 1;

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000A829 File Offset: 0x00008A29
		// (set) Token: 0x0600022E RID: 558 RVA: 0x0000A831 File Offset: 0x00008A31
		public int FreezeColumnCount { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000A83A File Offset: 0x00008A3A
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000A842 File Offset: 0x00008A42
		public bool EnableConvertByteArray { get; set; } = true;

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000A84B File Offset: 0x00008A4B
		// (set) Token: 0x06000232 RID: 562 RVA: 0x0000A853 File Offset: 0x00008A53
		public bool IgnoreTemplateParameterMissing { get; set; } = true;

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000A85C File Offset: 0x00008A5C
		// (set) Token: 0x06000234 RID: 564 RVA: 0x0000A864 File Offset: 0x00008A64
		public bool EnableWriteNullValueCell { get; set; } = true;

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000A86D File Offset: 0x00008A6D
		// (set) Token: 0x06000236 RID: 566 RVA: 0x0000A875 File Offset: 0x00008A75
		public bool EnableSharedStringCache { get; set; } = true;

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000A87E File Offset: 0x00008A7E
		// (set) Token: 0x06000238 RID: 568 RVA: 0x0000A886 File Offset: 0x00008A86
		public long SharedStringCacheSize { get; set; } = 5242880L;

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000A88F File Offset: 0x00008A8F
		// (set) Token: 0x0600023A RID: 570 RVA: 0x0000A897 File Offset: 0x00008A97
		public DynamicExcelSheet[] DynamicSheets { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000A8A0 File Offset: 0x00008AA0
		// (set) Token: 0x0600023C RID: 572 RVA: 0x0000A8A8 File Offset: 0x00008AA8
		public bool EnableAutoWidth { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000A8B1 File Offset: 0x00008AB1
		// (set) Token: 0x0600023E RID: 574 RVA: 0x0000A8B9 File Offset: 0x00008AB9
		public double MinWidth { get; set; } = 9.28515625;

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000A8C2 File Offset: 0x00008AC2
		// (set) Token: 0x06000240 RID: 576 RVA: 0x0000A8CA File Offset: 0x00008ACA
		public double MaxWidth { get; set; } = 200.0;

		// Token: 0x040000C2 RID: 194
		internal static readonly OpenXmlConfiguration DefaultConfig = new OpenXmlConfiguration();
	}
}
