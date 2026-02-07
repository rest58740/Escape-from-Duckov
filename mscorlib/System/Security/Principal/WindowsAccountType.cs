using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004EA RID: 1258
	[ComVisible(true)]
	[Serializable]
	public enum WindowsAccountType
	{
		// Token: 0x0400232E RID: 9006
		Normal,
		// Token: 0x0400232F RID: 9007
		Guest,
		// Token: 0x04002330 RID: 9008
		System,
		// Token: 0x04002331 RID: 9009
		Anonymous
	}
}
