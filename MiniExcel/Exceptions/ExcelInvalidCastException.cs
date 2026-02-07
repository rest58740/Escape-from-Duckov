using System;

namespace MiniExcelLibs.Exceptions
{
	// Token: 0x02000063 RID: 99
	public class ExcelInvalidCastException : InvalidCastException
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00012AF1 File Offset: 0x00010CF1
		// (set) Token: 0x06000345 RID: 837 RVA: 0x00012AF9 File Offset: 0x00010CF9
		public string ColumnName { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000346 RID: 838 RVA: 0x00012B02 File Offset: 0x00010D02
		// (set) Token: 0x06000347 RID: 839 RVA: 0x00012B0A File Offset: 0x00010D0A
		public int Row { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00012B13 File Offset: 0x00010D13
		// (set) Token: 0x06000349 RID: 841 RVA: 0x00012B1B File Offset: 0x00010D1B
		public object Value { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00012B24 File Offset: 0x00010D24
		// (set) Token: 0x0600034B RID: 843 RVA: 0x00012B2C File Offset: 0x00010D2C
		public Type InvalidCastType { get; set; }

		// Token: 0x0600034C RID: 844 RVA: 0x00012B35 File Offset: 0x00010D35
		public ExcelInvalidCastException(string columnName, int row, object value, Type invalidCastType, string message) : base(message)
		{
			this.ColumnName = columnName;
			this.Row = row;
			this.Value = value;
			this.InvalidCastType = invalidCastType;
		}
	}
}
