using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200050C RID: 1292
	public abstract class AuthorizationRule
	{
		// Token: 0x06003351 RID: 13137 RVA: 0x0000259F File Offset: 0x0000079F
		internal AuthorizationRule()
		{
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x000BC988 File Offset: 0x000BAB88
		protected internal AuthorizationRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			if (null == identity)
			{
				throw new ArgumentNullException("identity");
			}
			if (!(identity is SecurityIdentifier) && !(identity is NTAccount))
			{
				throw new ArgumentException("identity");
			}
			if (accessMask == 0)
			{
				throw new ArgumentException("accessMask");
			}
			if ((inheritanceFlags & ~(InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit)) != InheritanceFlags.None)
			{
				throw new ArgumentOutOfRangeException();
			}
			if ((propagationFlags & ~(PropagationFlags.NoPropagateInherit | PropagationFlags.InheritOnly)) != PropagationFlags.None)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.identity = identity;
			this.accessMask = accessMask;
			this.isInherited = isInherited;
			this.inheritanceFlags = inheritanceFlags;
			this.propagationFlags = propagationFlags;
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06003353 RID: 13139 RVA: 0x000BCA17 File Offset: 0x000BAC17
		public IdentityReference IdentityReference
		{
			get
			{
				return this.identity;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06003354 RID: 13140 RVA: 0x000BCA1F File Offset: 0x000BAC1F
		public InheritanceFlags InheritanceFlags
		{
			get
			{
				return this.inheritanceFlags;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06003355 RID: 13141 RVA: 0x000BCA27 File Offset: 0x000BAC27
		public bool IsInherited
		{
			get
			{
				return this.isInherited;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06003356 RID: 13142 RVA: 0x000BCA2F File Offset: 0x000BAC2F
		public PropagationFlags PropagationFlags
		{
			get
			{
				return this.propagationFlags;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06003357 RID: 13143 RVA: 0x000BCA37 File Offset: 0x000BAC37
		protected internal int AccessMask
		{
			get
			{
				return this.accessMask;
			}
		}

		// Token: 0x04002439 RID: 9273
		private IdentityReference identity;

		// Token: 0x0400243A RID: 9274
		private int accessMask;

		// Token: 0x0400243B RID: 9275
		private bool isInherited;

		// Token: 0x0400243C RID: 9276
		private InheritanceFlags inheritanceFlags;

		// Token: 0x0400243D RID: 9277
		private PropagationFlags propagationFlags;
	}
}
