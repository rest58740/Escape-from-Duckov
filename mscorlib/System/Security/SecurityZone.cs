using System;

namespace System.Security
{
	// Token: 0x020003CB RID: 971
	public enum SecurityZone
	{
		// Token: 0x04001E94 RID: 7828
		Internet = 3,
		// Token: 0x04001E95 RID: 7829
		Intranet = 1,
		// Token: 0x04001E96 RID: 7830
		MyComputer = 0,
		// Token: 0x04001E97 RID: 7831
		NoZone = -1,
		// Token: 0x04001E98 RID: 7832
		Trusted = 2,
		// Token: 0x04001E99 RID: 7833
		Untrusted = 4
	}
}
