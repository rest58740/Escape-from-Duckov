using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Serialization;
using System.Security;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000615 RID: 1557
	[ComVisible(true)]
	[CLSCompliant(false)]
	[Serializable]
	public class ConstructionResponse : MethodResponse, IConstructionReturnMessage, IMethodReturnMessage, IMethodMessage, IMessage
	{
		// Token: 0x06003AD0 RID: 15056 RVA: 0x000CE2A3 File Offset: 0x000CC4A3
		public ConstructionResponse(Header[] h, IMethodCallMessage mcm) : base(h, mcm)
		{
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x000CE2AD File Offset: 0x000CC4AD
		internal ConstructionResponse(object resultObject, LogicalCallContext callCtx, IMethodCallMessage msg) : base(resultObject, null, callCtx, msg)
		{
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x000CE2B9 File Offset: 0x000CC4B9
		internal ConstructionResponse(Exception e, IMethodCallMessage msg) : base(e, msg)
		{
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x000CE2C3 File Offset: 0x000CC4C3
		internal ConstructionResponse(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06003AD4 RID: 15060 RVA: 0x000CE2CD File Offset: 0x000CC4CD
		public override IDictionary Properties
		{
			[SecurityCritical]
			get
			{
				return base.Properties;
			}
		}
	}
}
