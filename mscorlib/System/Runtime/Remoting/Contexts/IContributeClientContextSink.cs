using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x02000597 RID: 1431
	[ComVisible(true)]
	public interface IContributeClientContextSink
	{
		// Token: 0x060037D9 RID: 14297
		IMessageSink GetClientContextSink(IMessageSink nextSink);
	}
}
