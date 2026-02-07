using System;

namespace FlexFramework.Excel
{
	// Token: 0x02000021 RID: 33
	public sealed class Mapper<T> : MapperBase<Mapper<T>>, IGenerator<T> where T : new()
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x000048D3 File Offset: 0x00002AD3
		public Mapper() : base(typeof(T))
		{
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000048E5 File Offset: 0x00002AE5
		protected override object CreateInstance()
		{
			return Activator.CreateInstance<T>();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000048F1 File Offset: 0x00002AF1
		T IGenerator<!0>.Instantiate(Row row)
		{
			return (T)((object)base.Instantiate(row));
		}
	}
}
