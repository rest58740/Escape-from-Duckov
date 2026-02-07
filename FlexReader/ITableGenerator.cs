using System;
using System.Collections.Generic;

namespace FlexFramework.Excel
{
	// Token: 0x0200001F RID: 31
	public interface ITableGenerator
	{
		// Token: 0x060000D6 RID: 214
		IEnumerable<object> Instantiate(Table table);
	}
}
