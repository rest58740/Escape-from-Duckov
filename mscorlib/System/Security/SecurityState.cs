using System;

namespace System.Security
{
	// Token: 0x020003EC RID: 1004
	public abstract class SecurityState
	{
		// Token: 0x0600297C RID: 10620
		public abstract void EnsureState();

		// Token: 0x0600297D RID: 10621 RVA: 0x0009664C File Offset: 0x0009484C
		public bool IsStateAvailable()
		{
			AppDomainManager domainManager = AppDomain.CurrentDomain.DomainManager;
			return domainManager != null && domainManager.CheckSecuritySettings(this);
		}
	}
}
