using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200051C RID: 1308
	public sealed class CryptoKeyAuditRule : AuditRule
	{
		// Token: 0x060033D7 RID: 13271 RVA: 0x000BE096 File Offset: 0x000BC296
		public CryptoKeyAuditRule(IdentityReference identity, CryptoKeyRights cryptoKeyRights, AuditFlags flags) : base(identity, (int)cryptoKeyRights, false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x000BE0A4 File Offset: 0x000BC2A4
		public CryptoKeyAuditRule(string identity, CryptoKeyRights cryptoKeyRights, AuditFlags flags) : this(new NTAccount(identity), cryptoKeyRights, flags)
		{
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060033D9 RID: 13273 RVA: 0x000BC72E File Offset: 0x000BA92E
		public CryptoKeyRights CryptoKeyRights
		{
			get
			{
				return (CryptoKeyRights)base.AccessMask;
			}
		}
	}
}
