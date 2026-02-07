using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020004FD RID: 1277
	public sealed class RegistryAuditRule : AuditRule
	{
		// Token: 0x06003331 RID: 13105 RVA: 0x000BC736 File Offset: 0x000BA936
		public RegistryAuditRule(IdentityReference identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : this(identity, (int)registryRights, false, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x000BC746 File Offset: 0x000BA946
		public RegistryAuditRule(string identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : this(new NTAccount(identity), (int)registryRights, false, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x000BC75B File Offset: 0x000BA95B
		internal RegistryAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06003334 RID: 13108 RVA: 0x000BC72E File Offset: 0x000BA92E
		public RegistryRights RegistryRights
		{
			get
			{
				return (RegistryRights)base.AccessMask;
			}
		}
	}
}
