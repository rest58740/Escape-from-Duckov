using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x02000598 RID: 1432
	[ComVisible(true)]
	public interface IContributeDynamicSink
	{
		// Token: 0x060037DA RID: 14298
		IDynamicMessageSink GetDynamicSink();
	}
}
