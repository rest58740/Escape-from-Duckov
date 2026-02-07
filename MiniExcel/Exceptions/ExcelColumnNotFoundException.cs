using System;
using System.Collections.Generic;

namespace MiniExcelLibs.Exceptions
{
	// Token: 0x02000062 RID: 98
	public class ExcelColumnNotFoundException : KeyNotFoundException
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00012A66 File Offset: 0x00010C66
		// (set) Token: 0x0600033A RID: 826 RVA: 0x00012A6E File Offset: 0x00010C6E
		public string ColumnName { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00012A77 File Offset: 0x00010C77
		public string[] ColumnAliases { get; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600033C RID: 828 RVA: 0x00012A7F File Offset: 0x00010C7F
		// (set) Token: 0x0600033D RID: 829 RVA: 0x00012A87 File Offset: 0x00010C87
		public string ColumnIndex { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00012A90 File Offset: 0x00010C90
		// (set) Token: 0x0600033F RID: 831 RVA: 0x00012A98 File Offset: 0x00010C98
		public int RowIndex { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00012AA1 File Offset: 0x00010CA1
		public IDictionary<string, int> Headers { get; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000341 RID: 833 RVA: 0x00012AA9 File Offset: 0x00010CA9
		// (set) Token: 0x06000342 RID: 834 RVA: 0x00012AB1 File Offset: 0x00010CB1
		public object RowValues { get; set; }

		// Token: 0x06000343 RID: 835 RVA: 0x00012ABA File Offset: 0x00010CBA
		public ExcelColumnNotFoundException(string columnIndex, string columnName, string[] columnAliases, int rowIndex, IDictionary<string, int> headers, object value, string message) : base(message)
		{
			this.ColumnIndex = columnIndex;
			this.ColumnName = columnName;
			this.ColumnAliases = columnAliases;
			this.RowIndex = rowIndex;
			this.Headers = headers;
			this.RowValues = value;
		}
	}
}
