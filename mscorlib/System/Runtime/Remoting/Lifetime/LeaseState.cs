using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x02000589 RID: 1417
	[ComVisible(true)]
	[Serializable]
	public enum LeaseState
	{
		// Token: 0x04002595 RID: 9621
		Null,
		// Token: 0x04002596 RID: 9622
		Initial,
		// Token: 0x04002597 RID: 9623
		Active,
		// Token: 0x04002598 RID: 9624
		Renewing,
		// Token: 0x04002599 RID: 9625
		Expired
	}
}
