using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000517 RID: 1303
	public sealed class CommonSecurityDescriptor : GenericSecurityDescriptor
	{
		// Token: 0x060033B1 RID: 13233 RVA: 0x000BDCF0 File Offset: 0x000BBEF0
		public CommonSecurityDescriptor(bool isContainer, bool isDS, RawSecurityDescriptor rawSecurityDescriptor)
		{
			this.Init(isContainer, isDS, rawSecurityDescriptor);
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x000BDD01 File Offset: 0x000BBF01
		public CommonSecurityDescriptor(bool isContainer, bool isDS, string sddlForm)
		{
			this.Init(isContainer, isDS, new RawSecurityDescriptor(sddlForm));
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x000BDD17 File Offset: 0x000BBF17
		public CommonSecurityDescriptor(bool isContainer, bool isDS, byte[] binaryForm, int offset)
		{
			this.Init(isContainer, isDS, new RawSecurityDescriptor(binaryForm, offset));
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x000BDD2F File Offset: 0x000BBF2F
		public CommonSecurityDescriptor(bool isContainer, bool isDS, ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, SystemAcl systemAcl, DiscretionaryAcl discretionaryAcl)
		{
			this.Init(isContainer, isDS, flags, owner, group, systemAcl, discretionaryAcl);
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x000BDD48 File Offset: 0x000BBF48
		private void Init(bool isContainer, bool isDS, RawSecurityDescriptor rawSecurityDescriptor)
		{
			if (rawSecurityDescriptor == null)
			{
				throw new ArgumentNullException("rawSecurityDescriptor");
			}
			SystemAcl systemAcl = null;
			if (rawSecurityDescriptor.SystemAcl != null)
			{
				systemAcl = new SystemAcl(isContainer, isDS, rawSecurityDescriptor.SystemAcl);
			}
			DiscretionaryAcl discretionaryAcl = null;
			if (rawSecurityDescriptor.DiscretionaryAcl != null)
			{
				discretionaryAcl = new DiscretionaryAcl(isContainer, isDS, rawSecurityDescriptor.DiscretionaryAcl);
			}
			this.Init(isContainer, isDS, rawSecurityDescriptor.ControlFlags, rawSecurityDescriptor.Owner, rawSecurityDescriptor.Group, systemAcl, discretionaryAcl);
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x000BDDAF File Offset: 0x000BBFAF
		private void Init(bool isContainer, bool isDS, ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, SystemAcl systemAcl, DiscretionaryAcl discretionaryAcl)
		{
			this.flags = (flags & ~ControlFlags.SystemAclPresent);
			this.is_container = isContainer;
			this.is_ds = isDS;
			this.Owner = owner;
			this.Group = group;
			this.SystemAcl = systemAcl;
			this.DiscretionaryAcl = discretionaryAcl;
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060033B7 RID: 13239 RVA: 0x000BDDEC File Offset: 0x000BBFEC
		public override ControlFlags ControlFlags
		{
			get
			{
				ControlFlags controlFlags = this.flags;
				controlFlags |= ControlFlags.DiscretionaryAclPresent;
				controlFlags |= ControlFlags.SelfRelative;
				if (this.SystemAcl != null)
				{
					controlFlags |= ControlFlags.SystemAclPresent;
				}
				return controlFlags;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060033B8 RID: 13240 RVA: 0x000BDE1A File Offset: 0x000BC01A
		// (set) Token: 0x060033B9 RID: 13241 RVA: 0x000BDE24 File Offset: 0x000BC024
		public DiscretionaryAcl DiscretionaryAcl
		{
			get
			{
				return this.discretionary_acl;
			}
			set
			{
				if (value == null)
				{
					value = new DiscretionaryAcl(this.IsContainer, this.IsDS, 1);
					value.AddAccess(AccessControlType.Allow, new SecurityIdentifier("WD"), -1, this.IsContainer ? (InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit) : InheritanceFlags.None, PropagationFlags.None);
					value.IsAefa = true;
				}
				this.CheckAclConsistency(value);
				this.discretionary_acl = value;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060033BA RID: 13242 RVA: 0x000BDE7C File Offset: 0x000BC07C
		internal override GenericAcl InternalDacl
		{
			get
			{
				return this.DiscretionaryAcl;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060033BB RID: 13243 RVA: 0x000BDE84 File Offset: 0x000BC084
		// (set) Token: 0x060033BC RID: 13244 RVA: 0x000BDE8C File Offset: 0x000BC08C
		public override SecurityIdentifier Group
		{
			get
			{
				return this.group;
			}
			set
			{
				this.group = value;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060033BD RID: 13245 RVA: 0x000BDE95 File Offset: 0x000BC095
		public bool IsContainer
		{
			get
			{
				return this.is_container;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060033BE RID: 13246 RVA: 0x000BDE9D File Offset: 0x000BC09D
		public bool IsDiscretionaryAclCanonical
		{
			get
			{
				return this.DiscretionaryAcl.IsCanonical;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060033BF RID: 13247 RVA: 0x000BDEAA File Offset: 0x000BC0AA
		public bool IsDS
		{
			get
			{
				return this.is_ds;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060033C0 RID: 13248 RVA: 0x000BDEB2 File Offset: 0x000BC0B2
		public bool IsSystemAclCanonical
		{
			get
			{
				return this.SystemAcl == null || this.SystemAcl.IsCanonical;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060033C1 RID: 13249 RVA: 0x000BDEC9 File Offset: 0x000BC0C9
		// (set) Token: 0x060033C2 RID: 13250 RVA: 0x000BDED1 File Offset: 0x000BC0D1
		public override SecurityIdentifier Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				this.owner = value;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x060033C3 RID: 13251 RVA: 0x000BDEDA File Offset: 0x000BC0DA
		// (set) Token: 0x060033C4 RID: 13252 RVA: 0x000BDEE2 File Offset: 0x000BC0E2
		public SystemAcl SystemAcl
		{
			get
			{
				return this.system_acl;
			}
			set
			{
				if (value != null)
				{
					this.CheckAclConsistency(value);
				}
				this.system_acl = value;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060033C5 RID: 13253 RVA: 0x000BDEF5 File Offset: 0x000BC0F5
		internal override GenericAcl InternalSacl
		{
			get
			{
				return this.SystemAcl;
			}
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x000BDEFD File Offset: 0x000BC0FD
		public void PurgeAccessControl(SecurityIdentifier sid)
		{
			this.DiscretionaryAcl.Purge(sid);
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x000BDF0B File Offset: 0x000BC10B
		public void PurgeAudit(SecurityIdentifier sid)
		{
			if (this.SystemAcl != null)
			{
				this.SystemAcl.Purge(sid);
			}
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x000BDF24 File Offset: 0x000BC124
		public void SetDiscretionaryAclProtection(bool isProtected, bool preserveInheritance)
		{
			this.DiscretionaryAcl.IsAefa = false;
			if (!isProtected)
			{
				this.flags &= ~ControlFlags.DiscretionaryAclProtected;
				return;
			}
			this.flags |= ControlFlags.DiscretionaryAclProtected;
			if (!preserveInheritance)
			{
				this.DiscretionaryAcl.RemoveInheritedAces();
			}
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x000BDF73 File Offset: 0x000BC173
		public void SetSystemAclProtection(bool isProtected, bool preserveInheritance)
		{
			if (!isProtected)
			{
				this.flags &= ~ControlFlags.SystemAclProtected;
				return;
			}
			this.flags |= ControlFlags.SystemAclProtected;
			if (!preserveInheritance && this.SystemAcl != null)
			{
				this.SystemAcl.RemoveInheritedAces();
			}
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x000BDFB3 File Offset: 0x000BC1B3
		public void AddDiscretionaryAcl(byte revision, int trusted)
		{
			this.DiscretionaryAcl = new DiscretionaryAcl(this.IsContainer, this.IsDS, revision, trusted);
			this.flags |= ControlFlags.DiscretionaryAclPresent;
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x000BDFDC File Offset: 0x000BC1DC
		public void AddSystemAcl(byte revision, int trusted)
		{
			this.SystemAcl = new SystemAcl(this.IsContainer, this.IsDS, revision, trusted);
			this.flags |= ControlFlags.SystemAclPresent;
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x000BE006 File Offset: 0x000BC206
		private void CheckAclConsistency(CommonAcl acl)
		{
			if (this.IsContainer != acl.IsContainer)
			{
				throw new ArgumentException("IsContainer must match between descriptor and ACL.");
			}
			if (this.IsDS != acl.IsDS)
			{
				throw new ArgumentException("IsDS must match between descriptor and ACL.");
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x060033CD RID: 13261 RVA: 0x000BE03A File Offset: 0x000BC23A
		internal override bool DaclIsUnmodifiedAefa
		{
			get
			{
				return this.DiscretionaryAcl.IsAefa;
			}
		}

		// Token: 0x04002456 RID: 9302
		private bool is_container;

		// Token: 0x04002457 RID: 9303
		private bool is_ds;

		// Token: 0x04002458 RID: 9304
		private ControlFlags flags;

		// Token: 0x04002459 RID: 9305
		private SecurityIdentifier owner;

		// Token: 0x0400245A RID: 9306
		private SecurityIdentifier group;

		// Token: 0x0400245B RID: 9307
		private SystemAcl system_acl;

		// Token: 0x0400245C RID: 9308
		private DiscretionaryAcl discretionary_acl;
	}
}
