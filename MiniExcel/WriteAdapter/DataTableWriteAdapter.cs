using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using MiniExcelLibs.Utils;

namespace MiniExcelLibs.WriteAdapter
{
	// Token: 0x02000020 RID: 32
	internal class DataTableWriteAdapter : IMiniExcelWriteAdapter
	{
		// Token: 0x060000DB RID: 219 RVA: 0x000041DB File Offset: 0x000023DB
		public DataTableWriteAdapter(DataTable dataTable, Configuration configuration)
		{
			this._dataTable = dataTable;
			this._configuration = configuration;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000041F1 File Offset: 0x000023F1
		public bool TryGetKnownCount(out int count)
		{
			count = this._dataTable.Rows.Count;
			return true;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004208 File Offset: 0x00002408
		public List<ExcelColumnInfo> GetColumns()
		{
			List<ExcelColumnInfo> list = new List<ExcelColumnInfo>();
			for (int i = 0; i < this._dataTable.Columns.Count; i++)
			{
				ExcelColumnInfo columnInfosFromDynamicConfiguration = CustomPropertyHelper.GetColumnInfosFromDynamicConfiguration(this._dataTable.Columns[i].Caption ?? this._dataTable.Columns[i].ColumnName, this._configuration);
				list.Add(columnInfosFromDynamicConfiguration);
			}
			return list;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000427A File Offset: 0x0000247A
		public IEnumerable<IEnumerable<CellWriteInfo>> GetRows(List<ExcelColumnInfo> props, CancellationToken cancellationToken = default(CancellationToken))
		{
			int num;
			for (int row = 0; row < this._dataTable.Rows.Count; row = num + 1)
			{
				cancellationToken.ThrowIfCancellationRequested();
				yield return this.GetRowValues(row, props);
				num = row;
			}
			yield break;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004298 File Offset: 0x00002498
		private IEnumerable<CellWriteInfo> GetRowValues(int row, List<ExcelColumnInfo> props)
		{
			int i = 0;
			int column = 1;
			while (i < this._dataTable.Columns.Count)
			{
				yield return new CellWriteInfo(this._dataTable.Rows[row][i], column, props[i]);
				int num = i;
				i = num + 1;
				num = column;
				column = num + 1;
			}
			yield break;
		}

		// Token: 0x04000033 RID: 51
		private readonly DataTable _dataTable;

		// Token: 0x04000034 RID: 52
		private readonly Configuration _configuration;
	}
}
