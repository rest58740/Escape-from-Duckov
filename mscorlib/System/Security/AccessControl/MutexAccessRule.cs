using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000531 RID: 1329
	public sealed class MutexAccessRule : AccessRule
	{
		// Token: 0x060034A3 RID: 13475 RVA: 0x000BF950 File Offset: 0x000BDB50
		public MutexAccessRule(IdentityReference identity, MutexRights eventRights, AccessControlType type) : base(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x000BF95E File Offset: 0x000BDB5E
		public MutexAccessRule(string identity, MutexRights eventRights, AccessControlType type) : this(new NTAccount(identity), eventRights, type)
		{
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060034A5 RID: 13477 RVA: 0x000BC72E File Offset: 0x000BA92E
		public MutexRights MutexRights
		{
			get
			{
				return (MutexRights)base.AccessMask;
			}
		}
	}
}
