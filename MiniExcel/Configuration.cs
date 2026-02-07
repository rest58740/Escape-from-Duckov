using System;
using System.Globalization;
using MiniExcelLibs.Attributes;

namespace MiniExcelLibs
{
	// Token: 0x0200000E RID: 14
	public abstract class Configuration : IConfiguration
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002711 File Offset: 0x00000911
		// (set) Token: 0x06000029 RID: 41 RVA: 0x00002719 File Offset: 0x00000919
		public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002722 File Offset: 0x00000922
		// (set) Token: 0x0600002B RID: 43 RVA: 0x0000272A File Offset: 0x0000092A
		public DynamicExcelColumn[] DynamicColumns { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002733 File Offset: 0x00000933
		// (set) Token: 0x0600002D RID: 45 RVA: 0x0000273B File Offset: 0x0000093B
		public int BufferSize { get; set; } = 524288;

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002744 File Offset: 0x00000944
		// (set) Token: 0x0600002F RID: 47 RVA: 0x0000274C File Offset: 0x0000094C
		public bool FastMode { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002755 File Offset: 0x00000955
		// (set) Token: 0x06000031 RID: 49 RVA: 0x0000275D File Offset: 0x0000095D
		public bool DynamicColumnFirst { get; set; }
	}
}
