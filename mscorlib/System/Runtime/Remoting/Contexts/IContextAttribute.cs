using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x02000594 RID: 1428
	[ComVisible(true)]
	public interface IContextAttribute
	{
		// Token: 0x060037CF RID: 14287
		void GetPropertiesForNewContext(IConstructionCallMessage msg);

		// Token: 0x060037D0 RID: 14288
		bool IsContextOK(Context ctx, IConstructionCallMessage msg);
	}
}
