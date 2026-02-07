using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Security;

namespace System.Runtime.Remoting.Proxies
{
	// Token: 0x0200057E RID: 1406
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class)]
	public class ProxyAttribute : Attribute, IContextAttribute
	{
		// Token: 0x06003717 RID: 14103 RVA: 0x000C6D68 File Offset: 0x000C4F68
		public virtual MarshalByRefObject CreateInstance(Type serverType)
		{
			return (MarshalByRefObject)new RemotingProxy(serverType, ChannelServices.CrossContextUrl, null).GetTransparentProxy();
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x000C6D80 File Offset: 0x000C4F80
		public virtual RealProxy CreateProxy(ObjRef objRef, Type serverType, object serverObject, Context serverContext)
		{
			return RemotingServices.GetRealProxy(RemotingServices.GetProxyForRemoteObject(objRef, serverType));
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[SecurityCritical]
		[ComVisible(true)]
		public void GetPropertiesForNewContext(IConstructionCallMessage msg)
		{
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x000040F7 File Offset: 0x000022F7
		[ComVisible(true)]
		[SecurityCritical]
		public bool IsContextOK(Context ctx, IConstructionCallMessage msg)
		{
			return true;
		}
	}
}
