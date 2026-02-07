using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005B9 RID: 1465
	[ComVisible(true)]
	public interface IClientChannelSinkProvider
	{
		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06003884 RID: 14468
		// (set) Token: 0x06003885 RID: 14469
		IClientChannelSinkProvider Next { get; set; }

		// Token: 0x06003886 RID: 14470
		IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData);
	}
}
