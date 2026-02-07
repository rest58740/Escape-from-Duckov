using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006E6 RID: 1766
	[ComVisible(true)]
	[Serializable]
	public enum ComInterfaceType
	{
		// Token: 0x04002A2F RID: 10799
		InterfaceIsDual,
		// Token: 0x04002A30 RID: 10800
		InterfaceIsIUnknown,
		// Token: 0x04002A31 RID: 10801
		InterfaceIsIDispatch,
		// Token: 0x04002A32 RID: 10802
		[ComVisible(false)]
		InterfaceIsIInspectable
	}
}
