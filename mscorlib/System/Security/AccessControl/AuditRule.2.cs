using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200050B RID: 1291
	public class AuditRule<T> : AuditRule where T : struct
	{
		// Token: 0x0600334B RID: 13131 RVA: 0x000BC93A File Offset: 0x000BAB3A
		public AuditRule(string identity, T rights, AuditFlags flags) : this(new NTAccount(identity), rights, flags)
		{
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x000BC94A File Offset: 0x000BAB4A
		public AuditRule(IdentityReference identity, T rights, AuditFlags flags) : this(identity, rights, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x000BC957 File Offset: 0x000BAB57
		public AuditRule(string identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : this(new NTAccount(identity), rights, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x000BC96B File Offset: 0x000BAB6B
		public AuditRule(IdentityReference identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : this(identity, (int)((object)rights), false, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x000BC75B File Offset: 0x000BA95B
		internal AuditRule(IdentityReference identity, int rights, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(identity, rights, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06003350 RID: 13136 RVA: 0x000BC884 File Offset: 0x000BAA84
		public T Rights
		{
			get
			{
				return (T)((object)base.AccessMask);
			}
		}
	}
}
