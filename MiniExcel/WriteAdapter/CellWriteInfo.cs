using System;
using MiniExcelLibs.Utils;

namespace MiniExcelLibs.WriteAdapter
{
	// Token: 0x02000023 RID: 35
	internal readonly struct CellWriteInfo
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x000043A2 File Offset: 0x000025A2
		public CellWriteInfo(object value, int cellIndex, ExcelColumnInfo prop)
		{
			this.Value = value;
			this.CellIndex = cellIndex;
			this.Prop = prop;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000043B9 File Offset: 0x000025B9
		public object Value { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000043C1 File Offset: 0x000025C1
		public int CellIndex { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000043C9 File Offset: 0x000025C9
		public ExcelColumnInfo Prop { get; }
	}
}
