using System;

namespace FlexFramework.Excel
{
	// Token: 0x02000022 RID: 34
	public sealed class Mapper : MapperBase<Mapper>, IGenerator
	{
		// Token: 0x060000DB RID: 219 RVA: 0x000048FF File Offset: 0x00002AFF
		public Mapper(Type type) : base(type)
		{
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004908 File Offset: 0x00002B08
		object IGenerator.Instantiate(Row row)
		{
			return base.Instantiate(row);
		}
	}
}
