using System;

namespace System.Security.AccessControl
{
	// Token: 0x0200054A RID: 1354
	[Flags]
	public enum SecurityInfos
	{
		// Token: 0x040024FF RID: 9471
		Owner = 1,
		// Token: 0x04002500 RID: 9472
		Group = 2,
		// Token: 0x04002501 RID: 9473
		DiscretionaryAcl = 4,
		// Token: 0x04002502 RID: 9474
		SystemAcl = 8
	}
}
