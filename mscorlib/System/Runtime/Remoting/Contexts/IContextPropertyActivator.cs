using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x02000596 RID: 1430
	[ComVisible(true)]
	public interface IContextPropertyActivator
	{
		// Token: 0x060037D4 RID: 14292
		void CollectFromClientContext(IConstructionCallMessage msg);

		// Token: 0x060037D5 RID: 14293
		void CollectFromServerContext(IConstructionReturnMessage msg);

		// Token: 0x060037D6 RID: 14294
		bool DeliverClientContextToServerContext(IConstructionCallMessage msg);

		// Token: 0x060037D7 RID: 14295
		bool DeliverServerContextToClientContext(IConstructionReturnMessage msg);

		// Token: 0x060037D8 RID: 14296
		bool IsOKToActivate(IConstructionCallMessage msg);
	}
}
