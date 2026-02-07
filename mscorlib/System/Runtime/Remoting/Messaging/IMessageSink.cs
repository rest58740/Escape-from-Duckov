using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200061D RID: 1565
	[ComVisible(true)]
	public interface IMessageSink
	{
		// Token: 0x06003AFA RID: 15098
		IMessage SyncProcessMessage(IMessage msg);

		// Token: 0x06003AFB RID: 15099
		IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink);

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06003AFC RID: 15100
		IMessageSink NextSink { get; }
	}
}
