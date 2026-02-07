using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005B2 RID: 1458
	[ComVisible(true)]
	public interface IChannel
	{
		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x0600386F RID: 14447
		string ChannelName { get; }

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06003870 RID: 14448
		int ChannelPriority { get; }

		// Token: 0x06003871 RID: 14449
		string Parse(string url, out string objectURI);
	}
}
