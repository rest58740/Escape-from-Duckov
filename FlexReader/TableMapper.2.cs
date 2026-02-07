using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexFramework.Excel
{
	// Token: 0x02000027 RID: 39
	public sealed class TableMapper : TableMapperBase<TableMapper>, IGenerator, ITableGenerator
	{
		// Token: 0x06000109 RID: 265 RVA: 0x000050C0 File Offset: 0x000032C0
		public TableMapper(Type type) : base(type)
		{
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000050C9 File Offset: 0x000032C9
		object IGenerator.Instantiate(Row row)
		{
			return base.Instantiate(row);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000050D2 File Offset: 0x000032D2
		IEnumerable<object> ITableGenerator.Instantiate(Table table)
		{
			return from row in table.Where((Row r, int i) => !this.excludes.Contains(i))
			select base.Instantiate(row);
		}
	}
}
