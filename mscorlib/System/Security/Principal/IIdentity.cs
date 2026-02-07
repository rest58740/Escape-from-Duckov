using System;

namespace System.Security.Principal
{
	// Token: 0x020004DE RID: 1246
	public interface IIdentity
	{
		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060031E4 RID: 12772
		string Name { get; }

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060031E5 RID: 12773
		string AuthenticationType { get; }

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060031E6 RID: 12774
		bool IsAuthenticated { get; }
	}
}
