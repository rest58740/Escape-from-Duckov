using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005C3 RID: 1475
	[ComVisible(true)]
	public interface IServerResponseChannelSinkStack
	{
		// Token: 0x0600389B RID: 14491
		void AsyncProcessResponse(IMessage msg, ITransportHeaders headers, Stream stream);

		// Token: 0x0600389C RID: 14492
		Stream GetResponseStream(IMessage msg, ITransportHeaders headers);
	}
}
