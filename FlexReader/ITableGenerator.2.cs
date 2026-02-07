using System;
using System.Collections.Generic;

namespace FlexFramework.Excel
{
	// Token: 0x02000020 RID: 32
	public interface ITableGenerator<T>
	{
		// Token: 0x060000D7 RID: 215
		IEnumerable<T> Instantiate(Table table);
	}
}
