using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x0200059B RID: 1435
	[ComVisible(true)]
	public interface IContributeServerContextSink
	{
		// Token: 0x060037DD RID: 14301
		IMessageSink GetServerContextSink(IMessageSink nextSink);
	}
}
