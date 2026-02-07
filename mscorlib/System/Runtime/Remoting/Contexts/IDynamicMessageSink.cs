using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x0200059C RID: 1436
	[ComVisible(true)]
	public interface IDynamicMessageSink
	{
		// Token: 0x060037DE RID: 14302
		void ProcessMessageFinish(IMessage replyMsg, bool bCliSide, bool bAsync);

		// Token: 0x060037DF RID: 14303
		void ProcessMessageStart(IMessage reqMsg, bool bCliSide, bool bAsync);
	}
}
