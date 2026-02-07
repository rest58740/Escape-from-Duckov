using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005C0 RID: 1472
	[ComVisible(true)]
	public interface IServerChannelSinkProvider
	{
		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06003892 RID: 14482
		// (set) Token: 0x06003893 RID: 14483
		IServerChannelSinkProvider Next { get; set; }

		// Token: 0x06003894 RID: 14484
		IServerChannelSink CreateSink(IChannelReceiver channel);

		// Token: 0x06003895 RID: 14485
		void GetChannelData(IChannelDataStore channelData);
	}
}
