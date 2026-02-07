using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x02000599 RID: 1433
	[ComVisible(true)]
	public interface IContributeEnvoySink
	{
		// Token: 0x060037DB RID: 14299
		IMessageSink GetEnvoySink(MarshalByRefObject obj, IMessageSink nextSink);
	}
}
