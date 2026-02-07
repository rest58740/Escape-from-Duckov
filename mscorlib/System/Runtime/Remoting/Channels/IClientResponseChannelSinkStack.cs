using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005BD RID: 1469
	[ComVisible(true)]
	public interface IClientResponseChannelSinkStack
	{
		// Token: 0x06003889 RID: 14473
		void AsyncProcessResponse(ITransportHeaders headers, Stream stream);

		// Token: 0x0600388A RID: 14474
		void DispatchException(Exception e);

		// Token: 0x0600388B RID: 14475
		void DispatchReplyMessage(IMessage msg);
	}
}
