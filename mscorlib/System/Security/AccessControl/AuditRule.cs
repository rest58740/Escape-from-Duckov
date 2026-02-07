using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200050A RID: 1290
	public abstract class AuditRule : AuthorizationRule
	{
		// Token: 0x06003349 RID: 13129 RVA: 0x000BC903 File Offset: 0x000BAB03
		protected AuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags)
		{
			if (auditFlags != ((AuditFlags.Success | AuditFlags.Failure) & auditFlags))
			{
				throw new ArgumentException("Invalid audit flags.", "auditFlags");
			}
			this.auditFlags = auditFlags;
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x0600334A RID: 13130 RVA: 0x000BC932 File Offset: 0x000BAB32
		public AuditFlags AuditFlags
		{
			get
			{
				return this.auditFlags;
			}
		}

		// Token: 0x04002438 RID: 9272
		private AuditFlags auditFlags;
	}
}
