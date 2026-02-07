using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005BF RID: 1471
	[ComVisible(true)]
	public interface IServerChannelSink : IChannelSinkBase
	{
		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x0600388E RID: 14478
		IServerChannelSink NextChannelSink { get; }

		// Token: 0x0600388F RID: 14479
		void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream);

		// Token: 0x06003890 RID: 14480
		Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers);

		// Token: 0x06003891 RID: 14481
		ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream);
	}
}
