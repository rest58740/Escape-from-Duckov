using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000504 RID: 1284
	public class AccessRule<T> : AccessRule where T : struct
	{
		// Token: 0x0600333D RID: 13117 RVA: 0x000BC839 File Offset: 0x000BAA39
		public AccessRule(string identity, T rights, AccessControlType type) : this(new NTAccount(identity), rights, type)
		{
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x000BC849 File Offset: 0x000BAA49
		public AccessRule(IdentityReference identity, T rights, AccessControlType type) : this(identity, rights, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x000BC856 File Offset: 0x000BAA56
		public AccessRule(string identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : this(new NTAccount(identity), rights, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x000BC86A File Offset: 0x000BAA6A
		public AccessRule(IdentityReference identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : this(identity, (int)((object)rights), false, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x000BC71D File Offset: 0x000BA91D
		internal AccessRule(IdentityReference identity, int rights, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(identity, rights, isInherited, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06003342 RID: 13122 RVA: 0x000BC884 File Offset: 0x000BAA84
		public T Rights
		{
			get
			{
				return (T)((object)base.AccessMask);
			}
		}
	}
}
