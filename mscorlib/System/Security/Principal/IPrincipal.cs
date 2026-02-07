using System;

namespace System.Security.Principal
{
	// Token: 0x020004DF RID: 1247
	public interface IPrincipal
	{
		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060031E7 RID: 12775
		IIdentity Identity { get; }

		// Token: 0x060031E8 RID: 12776
		bool IsInRole(string role);
	}
}
