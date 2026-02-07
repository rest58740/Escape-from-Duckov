using System;

namespace System.Globalization
{
	// Token: 0x02000956 RID: 2390
	[Flags]
	public enum CompareOptions
	{
		// Token: 0x040033C9 RID: 13257
		None = 0,
		// Token: 0x040033CA RID: 13258
		IgnoreCase = 1,
		// Token: 0x040033CB RID: 13259
		IgnoreNonSpace = 2,
		// Token: 0x040033CC RID: 13260
		IgnoreSymbols = 4,
		// Token: 0x040033CD RID: 13261
		IgnoreKanaType = 8,
		// Token: 0x040033CE RID: 13262
		IgnoreWidth = 16,
		// Token: 0x040033CF RID: 13263
		OrdinalIgnoreCase = 268435456,
		// Token: 0x040033D0 RID: 13264
		StringSort = 536870912,
		// Token: 0x040033D1 RID: 13265
		Ordinal = 1073741824
	}
}
