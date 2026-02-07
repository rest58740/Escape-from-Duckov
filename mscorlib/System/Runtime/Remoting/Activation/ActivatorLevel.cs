using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020005CC RID: 1484
	[ComVisible(true)]
	[Serializable]
	public enum ActivatorLevel
	{
		// Token: 0x040025F4 RID: 9716
		Construction = 4,
		// Token: 0x040025F5 RID: 9717
		Context = 8,
		// Token: 0x040025F6 RID: 9718
		AppDomain = 12,
		// Token: 0x040025F7 RID: 9719
		Process = 16,
		// Token: 0x040025F8 RID: 9720
		Machine = 20
	}
}
