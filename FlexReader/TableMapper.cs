using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexFramework.Excel
{
	// Token: 0x02000026 RID: 38
	public sealed class TableMapper<T> : TableMapperBase<TableMapper<T>>, IGenerator<T>, ITableGenerator<T> where T : new()
	{
		// Token: 0x06000103 RID: 259 RVA: 0x00005050 File Offset: 0x00003250
		public TableMapper() : base(typeof(T))
		{
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005062 File Offset: 0x00003262
		protected override object CreateInstance()
		{
			return Activator.CreateInstance<T>();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000506E File Offset: 0x0000326E
		T IGenerator<!0>.Instantiate(Row row)
		{
			return (T)((object)base.Instantiate(row));
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000507C File Offset: 0x0000327C
		IEnumerable<T> ITableGenerator<!0>.Instantiate(Table table)
		{
			return from row in table.Where((Row r, int i) => !this.excludes.Contains(i))
			select (T)((object)base.Instantiate(row));
		}
	}
}
