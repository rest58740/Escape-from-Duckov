using System;
using System.Collections;
using System.Data;

namespace MiniExcelLibs.WriteAdapter
{
	// Token: 0x02000024 RID: 36
	internal static class MiniExcelWriteAdapterFactory
	{
		// Token: 0x060000EC RID: 236 RVA: 0x000043D4 File Offset: 0x000025D4
		public static IMiniExcelWriteAdapter GetWriteAdapter(object values, Configuration configuration)
		{
			IDataReader dataReader = values as IDataReader;
			if (dataReader != null)
			{
				return new DataReaderWriteAdapter(dataReader, configuration);
			}
			IEnumerable enumerable = values as IEnumerable;
			if (enumerable != null)
			{
				return new EnumerableWriteAdapter(enumerable, configuration);
			}
			DataTable dataTable = values as DataTable;
			if (dataTable == null)
			{
				throw new NotImplementedException();
			}
			return new DataTableWriteAdapter(dataTable, configuration);
		}
	}
}
