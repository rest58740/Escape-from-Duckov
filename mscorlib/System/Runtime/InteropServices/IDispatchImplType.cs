using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006F2 RID: 1778
	[ComVisible(true)]
	[Obsolete("The IDispatchImplAttribute is deprecated.", false)]
	[Serializable]
	public enum IDispatchImplType
	{
		// Token: 0x04002A40 RID: 10816
		SystemDefinedImpl,
		// Token: 0x04002A41 RID: 10817
		InternalImpl,
		// Token: 0x04002A42 RID: 10818
		CompatibleImpl
	}
}
