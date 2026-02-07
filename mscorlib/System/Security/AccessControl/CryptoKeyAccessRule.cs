using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200051B RID: 1307
	public sealed class CryptoKeyAccessRule : AccessRule
	{
		// Token: 0x060033D4 RID: 13268 RVA: 0x000BE078 File Offset: 0x000BC278
		public CryptoKeyAccessRule(IdentityReference identity, CryptoKeyRights cryptoKeyRights, AccessControlType type) : base(identity, (int)cryptoKeyRights, false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
		{
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x000BE086 File Offset: 0x000BC286
		public CryptoKeyAccessRule(string identity, CryptoKeyRights cryptoKeyRights, AccessControlType type) : this(new NTAccount(identity), cryptoKeyRights, type)
		{
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060033D6 RID: 13270 RVA: 0x000BC72E File Offset: 0x000BA92E
		public CryptoKeyRights CryptoKeyRights
		{
			get
			{
				return (CryptoKeyRights)base.AccessMask;
			}
		}
	}
}
