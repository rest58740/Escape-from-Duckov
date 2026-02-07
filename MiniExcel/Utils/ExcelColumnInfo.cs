using System;
using MiniExcelLibs.Attributes;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000028 RID: 40
	internal class ExcelColumnInfo
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004739 File Offset: 0x00002939
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00004741 File Offset: 0x00002941
		public object Key { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000FB RID: 251 RVA: 0x0000474A File Offset: 0x0000294A
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00004752 File Offset: 0x00002952
		public int? ExcelColumnIndex { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000FD RID: 253 RVA: 0x0000475B File Offset: 0x0000295B
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00004763 File Offset: 0x00002963
		public string ExcelColumnName { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000FF RID: 255 RVA: 0x0000476C File Offset: 0x0000296C
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00004774 File Offset: 0x00002974
		public string[] ExcelColumnAliases { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000101 RID: 257 RVA: 0x0000477D File Offset: 0x0000297D
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00004785 File Offset: 0x00002985
		public Property Property { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000103 RID: 259 RVA: 0x0000478E File Offset: 0x0000298E
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00004796 File Offset: 0x00002996
		public Type ExcludeNullableType { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000105 RID: 261 RVA: 0x0000479F File Offset: 0x0000299F
		// (set) Token: 0x06000106 RID: 262 RVA: 0x000047A7 File Offset: 0x000029A7
		public bool Nullable { get; internal set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000047B0 File Offset: 0x000029B0
		// (set) Token: 0x06000108 RID: 264 RVA: 0x000047B8 File Offset: 0x000029B8
		public string ExcelFormat { get; internal set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000109 RID: 265 RVA: 0x000047C1 File Offset: 0x000029C1
		// (set) Token: 0x0600010A RID: 266 RVA: 0x000047C9 File Offset: 0x000029C9
		public double? ExcelColumnWidth { get; internal set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600010B RID: 267 RVA: 0x000047D2 File Offset: 0x000029D2
		// (set) Token: 0x0600010C RID: 268 RVA: 0x000047DA File Offset: 0x000029DA
		public string ExcelIndexName { get; internal set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000047E3 File Offset: 0x000029E3
		// (set) Token: 0x0600010E RID: 270 RVA: 0x000047EB File Offset: 0x000029EB
		public bool ExcelIgnore { get; internal set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000047F4 File Offset: 0x000029F4
		// (set) Token: 0x06000110 RID: 272 RVA: 0x000047FC File Offset: 0x000029FC
		public int ExcelFormatId { get; internal set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00004805 File Offset: 0x00002A05
		// (set) Token: 0x06000112 RID: 274 RVA: 0x0000480D File Offset: 0x00002A0D
		public ColumnType ExcelColumnType { get; internal set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00004816 File Offset: 0x00002A16
		// (set) Token: 0x06000114 RID: 276 RVA: 0x0000481E File Offset: 0x00002A1E
		public Func<object, string> CustomFormatter { get; set; }
	}
}
