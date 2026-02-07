using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005B6 RID: 1462
	[ComVisible(true)]
	public interface IChannelSender : IChannel
	{
		// Token: 0x0600387D RID: 14461
		IMessageSink CreateMessageSink(string url, object remoteChannelData, out string objectURI);
	}
}
