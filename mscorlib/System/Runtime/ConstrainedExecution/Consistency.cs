using System;

namespace System.Runtime.ConstrainedExecution
{
	// Token: 0x020007D4 RID: 2004
	public enum Consistency
	{
		// Token: 0x04002D1B RID: 11547
		MayCorruptProcess,
		// Token: 0x04002D1C RID: 11548
		MayCorruptAppDomain,
		// Token: 0x04002D1D RID: 11549
		MayCorruptInstance,
		// Token: 0x04002D1E RID: 11550
		WillNotCorruptState
	}
}
