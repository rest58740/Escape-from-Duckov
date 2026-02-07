using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace System.Runtime.Remoting.Services
{
	// Token: 0x0200057B RID: 1403
	[ComVisible(true)]
	public sealed class EnterpriseServicesHelper
	{
		// Token: 0x06003708 RID: 14088 RVA: 0x000C6A79 File Offset: 0x000C4C79
		[ComVisible(true)]
		public static IConstructionReturnMessage CreateConstructionReturnMessage(IConstructionCallMessage ctorMsg, MarshalByRefObject retObj)
		{
			return new ConstructionResponse(retObj, null, ctorMsg);
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x000472CC File Offset: 0x000454CC
		[MonoTODO]
		public static void SwitchWrappers(RealProxy oldcp, RealProxy newcp)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x000472CC File Offset: 0x000454CC
		[MonoTODO]
		public static object WrapIUnknownWithComObject(IntPtr punk)
		{
			throw new NotSupportedException();
		}
	}
}
