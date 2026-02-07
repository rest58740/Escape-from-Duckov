using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x0200059A RID: 1434
	[ComVisible(true)]
	public interface IContributeObjectSink
	{
		// Token: 0x060037DC RID: 14300
		IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink);
	}
}
