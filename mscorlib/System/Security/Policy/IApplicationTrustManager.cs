using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x02000414 RID: 1044
	[ComVisible(true)]
	public interface IApplicationTrustManager : ISecurityEncodable
	{
		// Token: 0x06002ABE RID: 10942
		ApplicationTrust DetermineApplicationTrust(ActivationContext activationContext, TrustManagerContext context);
	}
}
