using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000714 RID: 1812
	[ComVisible(true)]
	[Serializable]
	public enum CallingConvention
	{
		// Token: 0x04002AF3 RID: 10995
		Winapi = 1,
		// Token: 0x04002AF4 RID: 10996
		Cdecl,
		// Token: 0x04002AF5 RID: 10997
		StdCall,
		// Token: 0x04002AF6 RID: 10998
		ThisCall,
		// Token: 0x04002AF7 RID: 10999
		FastCall
	}
}
