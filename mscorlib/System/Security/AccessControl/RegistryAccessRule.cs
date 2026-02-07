using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020004FC RID: 1276
	public sealed class RegistryAccessRule : AccessRule
	{
		// Token: 0x0600332B RID: 13099 RVA: 0x000BC6D7 File Offset: 0x000BA8D7
		public RegistryAccessRule(IdentityReference identity, RegistryRights registryRights, AccessControlType type) : this(identity, (int)registryRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x000BC6E5 File Offset: 0x000BA8E5
		public RegistryAccessRule(string identity, RegistryRights registryRights, AccessControlType type) : this(new NTAccount(identity), (int)registryRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x000BC6F8 File Offset: 0x000BA8F8
		public RegistryAccessRule(IdentityReference identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : this(identity, (int)registryRights, false, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x000BC708 File Offset: 0x000BA908
		public RegistryAccessRule(string identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : this(new NTAccount(identity), (int)registryRights, false, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x000BC71D File Offset: 0x000BA91D
		internal RegistryAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06003330 RID: 13104 RVA: 0x000BC72E File Offset: 0x000BA92E
		public RegistryRights RegistryRights
		{
			get
			{
				return (RegistryRights)base.AccessMask;
			}
		}
	}
}
