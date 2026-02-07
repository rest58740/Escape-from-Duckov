using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000529 RID: 1321
	public sealed class FileSystemAuditRule : AuditRule
	{
		// Token: 0x0600343F RID: 13375 RVA: 0x000BEAC2 File Offset: 0x000BCCC2
		public FileSystemAuditRule(IdentityReference identity, FileSystemRights fileSystemRights, AuditFlags flags) : this(identity, fileSystemRights, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x000BEACF File Offset: 0x000BCCCF
		public FileSystemAuditRule(string identity, FileSystemRights fileSystemRights, AuditFlags flags) : this(new NTAccount(identity), fileSystemRights, flags)
		{
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x000BEADF File Offset: 0x000BCCDF
		public FileSystemAuditRule(IdentityReference identity, FileSystemRights fileSystemRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : this(identity, fileSystemRights, false, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x000BC75B File Offset: 0x000BA95B
		internal FileSystemAuditRule(IdentityReference identity, FileSystemRights fileSystemRights, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(identity, (int)fileSystemRights, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x000BEAEF File Offset: 0x000BCCEF
		public FileSystemAuditRule(string identity, FileSystemRights fileSystemRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : this(new NTAccount(identity), fileSystemRights, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06003444 RID: 13380 RVA: 0x000BC72E File Offset: 0x000BA92E
		public FileSystemRights FileSystemRights
		{
			get
			{
				return (FileSystemRights)base.AccessMask;
			}
		}
	}
}
