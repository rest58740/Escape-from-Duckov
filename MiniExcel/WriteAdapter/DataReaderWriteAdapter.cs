using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.Utils;

namespace MiniExcelLibs.WriteAdapter
{
	// Token: 0x0200001F RID: 31
	internal class DataReaderWriteAdapter : IMiniExcelWriteAdapter
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x000040E2 File Offset: 0x000022E2
		public DataReaderWriteAdapter(IDataReader reader, Configuration configuration)
		{
			this._reader = reader;
			this._configuration = configuration;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000040F8 File Offset: 0x000022F8
		public bool TryGetKnownCount(out int count)
		{
			count = 0;
			return false;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004100 File Offset: 0x00002300
		public List<ExcelColumnInfo> GetColumns()
		{
			List<ExcelColumnInfo> list = new List<ExcelColumnInfo>();
			for (int i = 0; i < this._reader.FieldCount; i++)
			{
				string columnName = this._reader.GetName(i);
				if (!this._configuration.DynamicColumnFirst)
				{
					ExcelColumnInfo columnInfosFromDynamicConfiguration = CustomPropertyHelper.GetColumnInfosFromDynamicConfiguration(columnName, this._configuration);
					list.Add(columnInfosFromDynamicConfiguration);
				}
				else if (this._configuration.DynamicColumns.Any((DynamicExcelColumn a) => string.Equals(a.Key, columnName, StringComparison.OrdinalIgnoreCase)))
				{
					ExcelColumnInfo columnInfosFromDynamicConfiguration2 = CustomPropertyHelper.GetColumnInfosFromDynamicConfiguration(columnName, this._configuration);
					list.Add(columnInfosFromDynamicConfiguration2);
				}
			}
			return list;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000041A6 File Offset: 0x000023A6
		public IEnumerable<IEnumerable<CellWriteInfo>> GetRows(List<ExcelColumnInfo> props, CancellationToken cancellationToken = default(CancellationToken))
		{
			while (this._reader.Read())
			{
				cancellationToken.ThrowIfCancellationRequested();
				yield return this.GetRowValues(props);
			}
			yield break;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000041C4 File Offset: 0x000023C4
		private IEnumerable<CellWriteInfo> GetRowValues(List<ExcelColumnInfo> props)
		{
			int i = 0;
			int column = 1;
			while (i < this._reader.FieldCount)
			{
				if (this._configuration.DynamicColumnFirst)
				{
					int ordinal = this._reader.GetOrdinal(props[i].Key.ToString());
					yield return new CellWriteInfo(this._reader.GetValue(ordinal), column, props[i]);
				}
				else
				{
					yield return new CellWriteInfo(this._reader.GetValue(i), column, props[i]);
				}
				int num = i;
				i = num + 1;
				num = column;
				column = num + 1;
			}
			yield break;
		}

		// Token: 0x04000031 RID: 49
		private readonly IDataReader _reader;

		// Token: 0x04000032 RID: 50
		private readonly Configuration _configuration;
	}
}
