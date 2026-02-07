using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000532 RID: 1330
	public sealed class MutexAuditRule : AuditRule
	{
		// Token: 0x060034A6 RID: 13478 RVA: 0x000BE096 File Offset: 0x000BC296
		public MutexAuditRule(IdentityReference identity, MutexRights eventRights, AuditFlags flags) : base(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060034A7 RID: 13479 RVA: 0x000BC72E File Offset: 0x000BA92E
		public MutexRights MutexRights
		{
			get
			{
				return (MutexRights)base.AccessMask;
			}
		}
	}
}
