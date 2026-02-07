using System;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x0200067B RID: 1659
	public interface IFieldInfo
	{
		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06003DC6 RID: 15814
		// (set) Token: 0x06003DC7 RID: 15815
		string[] FieldNames { get; set; }

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06003DC8 RID: 15816
		// (set) Token: 0x06003DC9 RID: 15817
		Type[] FieldTypes { get; set; }
	}
}
