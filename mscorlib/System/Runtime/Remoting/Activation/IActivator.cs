using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020005D0 RID: 1488
	[ComVisible(true)]
	public interface IActivator
	{
		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060038D7 RID: 14551
		ActivatorLevel Level { get; }

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x060038D8 RID: 14552
		// (set) Token: 0x060038D9 RID: 14553
		IActivator NextActivator { get; set; }

		// Token: 0x060038DA RID: 14554
		IConstructionReturnMessage Activate(IConstructionCallMessage msg);
	}
}
