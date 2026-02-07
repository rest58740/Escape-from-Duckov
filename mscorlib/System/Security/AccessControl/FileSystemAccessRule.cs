using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000528 RID: 1320
	public sealed class FileSystemAccessRule : AccessRule
	{
		// Token: 0x06003439 RID: 13369 RVA: 0x000BEA7F File Offset: 0x000BCC7F
		public FileSystemAccessRule(IdentityReference identity, FileSystemRights fileSystemRights, AccessControlType type) : this(identity, fileSystemRights, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x000BEA8C File Offset: 0x000BCC8C
		public FileSystemAccessRule(string identity, FileSystemRights fileSystemRights, AccessControlType type) : this(new NTAccount(identity), fileSystemRights, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x000BEA9E File Offset: 0x000BCC9E
		public FileSystemAccessRule(IdentityReference identity, FileSystemRights fileSystemRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : this(identity, fileSystemRights, false, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x000BC71D File Offset: 0x000BA91D
		internal FileSystemAccessRule(IdentityReference identity, FileSystemRights fileSystemRights, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : base(identity, (int)fileSystemRights, isInherited, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x000BEAAE File Offset: 0x000BCCAE
		public FileSystemAccessRule(string identity, FileSystemRights fileSystemRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type) : this(new NTAccount(identity), fileSystemRights, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x0600343E RID: 13374 RVA: 0x000BC72E File Offset: 0x000BA92E
		public FileSystemRights FileSystemRights
		{
			get
			{
				return (FileSystemRights)base.AccessMask;
			}
		}
	}
}
