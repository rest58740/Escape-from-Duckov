using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005B8 RID: 1464
	[ComVisible(true)]
	public interface IClientChannelSink : IChannelSinkBase
	{
		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x0600387F RID: 14463
		IClientChannelSink NextChannelSink { get; }

		// Token: 0x06003880 RID: 14464
		void AsyncProcessRequest(IClientChannelSinkStack sinkStack, IMessage msg, ITransportHeaders headers, Stream stream);

		// Token: 0x06003881 RID: 14465
		void AsyncProcessResponse(IClientResponseChannelSinkStack sinkStack, object state, ITransportHeaders headers, Stream stream);

		// Token: 0x06003882 RID: 14466
		Stream GetRequestStream(IMessage msg, ITransportHeaders headers);

		// Token: 0x06003883 RID: 14467
		void ProcessMessage(IMessage msg, ITransportHeaders requestHeaders, Stream requestStream, out ITransportHeaders responseHeaders, out Stream responseStream);
	}
}
