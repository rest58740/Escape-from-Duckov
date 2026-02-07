using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005B5 RID: 1461
	[ComVisible(true)]
	public interface IChannelReceiverHook
	{
		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06003879 RID: 14457
		string ChannelScheme { get; }

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x0600387A RID: 14458
		IServerChannelSink ChannelSinkChain { get; }

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x0600387B RID: 14459
		bool WantsToListen { get; }

		// Token: 0x0600387C RID: 14460
		void AddHookChannelUri(string channelUri);
	}
}
