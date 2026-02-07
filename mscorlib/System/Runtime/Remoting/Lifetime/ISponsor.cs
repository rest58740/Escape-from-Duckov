using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x02000584 RID: 1412
	[ComVisible(true)]
	public interface ISponsor
	{
		// Token: 0x0600375C RID: 14172
		TimeSpan Renewal(ILease lease);
	}
}
