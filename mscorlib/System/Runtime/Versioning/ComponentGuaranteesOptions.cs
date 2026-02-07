using System;

namespace System.Runtime.Versioning
{
	// Token: 0x0200063E RID: 1598
	[Flags]
	public enum ComponentGuaranteesOptions
	{
		// Token: 0x040026F2 RID: 9970
		None = 0,
		// Token: 0x040026F3 RID: 9971
		Exchange = 1,
		// Token: 0x040026F4 RID: 9972
		Stable = 2,
		// Token: 0x040026F5 RID: 9973
		SideBySide = 4
	}
}
