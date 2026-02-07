using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000503 RID: 1283
	public abstract class AccessRule : AuthorizationRule
	{
		// Token: 0x0600333B RID: 13115 RVA: 0x000BC800 File Offset: 0x000BAA00
		protected AccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags)
		{
			if (type < AccessControlType.Allow || type > AccessControlType.Deny)
			{
				throw new ArgumentException("Invalid access control type.", "type");
			}
			this.type = type;
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x0600333C RID: 13116 RVA: 0x000BC831 File Offset: 0x000BAA31
		public AccessControlType AccessControlType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x0400240E RID: 9230
		private AccessControlType type;
	}
}
