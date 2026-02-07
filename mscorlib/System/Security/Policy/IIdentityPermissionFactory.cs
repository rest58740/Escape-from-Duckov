using System;

namespace System.Security.Policy
{
	// Token: 0x020003F9 RID: 1017
	public interface IIdentityPermissionFactory
	{
		// Token: 0x060029AE RID: 10670
		IPermission CreateIdentityPermission(Evidence evidence);
	}
}
