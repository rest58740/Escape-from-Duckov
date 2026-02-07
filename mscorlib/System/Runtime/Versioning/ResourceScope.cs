using System;

namespace System.Runtime.Versioning
{
	// Token: 0x02000641 RID: 1601
	[Flags]
	public enum ResourceScope
	{
		// Token: 0x040026FA RID: 9978
		None = 0,
		// Token: 0x040026FB RID: 9979
		Machine = 1,
		// Token: 0x040026FC RID: 9980
		Process = 2,
		// Token: 0x040026FD RID: 9981
		AppDomain = 4,
		// Token: 0x040026FE RID: 9982
		Library = 8,
		// Token: 0x040026FF RID: 9983
		Private = 16,
		// Token: 0x04002700 RID: 9984
		Assembly = 32
	}
}
