using System;

namespace FlexFramework.Excel
{
	// Token: 0x0200001E RID: 30
	public interface IGenerator<T>
	{
		// Token: 0x060000D5 RID: 213
		T Instantiate(Row row);
	}
}
