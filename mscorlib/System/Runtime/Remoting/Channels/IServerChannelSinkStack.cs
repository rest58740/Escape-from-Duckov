using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005C1 RID: 1473
	[ComVisible(true)]
	public interface IServerChannelSinkStack : IServerResponseChannelSinkStack
	{
		// Token: 0x06003896 RID: 14486
		object Pop(IServerChannelSink sink);

		// Token: 0x06003897 RID: 14487
		void Push(IServerChannelSink sink, object state);

		// Token: 0x06003898 RID: 14488
		void ServerCallback(IAsyncResult ar);

		// Token: 0x06003899 RID: 14489
		void Store(IServerChannelSink sink, object state);

		// Token: 0x0600389A RID: 14490
		void StoreAndDispatch(IServerChannelSink sink, object state);
	}
}
