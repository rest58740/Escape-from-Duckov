using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace System.Security.AccessControl
{
	// Token: 0x02000541 RID: 1345
	public abstract class ObjectSecurity
	{
		// Token: 0x06003509 RID: 13577 RVA: 0x0000259F File Offset: 0x0000079F
		protected ObjectSecurity()
		{
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000C059C File Offset: 0x000BE79C
		protected ObjectSecurity(CommonSecurityDescriptor securityDescriptor)
		{
			if (securityDescriptor == null)
			{
				throw new ArgumentNullException("securityDescriptor");
			}
			this.descriptor = securityDescriptor;
			this.rw_lock = new ReaderWriterLock();
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x000C05C4 File Offset: 0x000BE7C4
		protected ObjectSecurity(bool isContainer, bool isDS) : this(new CommonSecurityDescriptor(isContainer, isDS, ControlFlags.None, null, null, null, new DiscretionaryAcl(isContainer, isDS, 0)))
		{
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x0600350C RID: 13580
		public abstract Type AccessRightType { get; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x0600350D RID: 13581
		public abstract Type AccessRuleType { get; }

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x0600350E RID: 13582
		public abstract Type AuditRuleType { get; }

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x0600350F RID: 13583 RVA: 0x000C05EC File Offset: 0x000BE7EC
		public bool AreAccessRulesCanonical
		{
			get
			{
				this.ReadLock();
				bool isDiscretionaryAclCanonical;
				try
				{
					isDiscretionaryAclCanonical = this.descriptor.IsDiscretionaryAclCanonical;
				}
				finally
				{
					this.ReadUnlock();
				}
				return isDiscretionaryAclCanonical;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06003510 RID: 13584 RVA: 0x000C0628 File Offset: 0x000BE828
		public bool AreAccessRulesProtected
		{
			get
			{
				this.ReadLock();
				bool result;
				try
				{
					result = ((this.descriptor.ControlFlags & ControlFlags.DiscretionaryAclProtected) > ControlFlags.None);
				}
				finally
				{
					this.ReadUnlock();
				}
				return result;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06003511 RID: 13585 RVA: 0x000C066C File Offset: 0x000BE86C
		public bool AreAuditRulesCanonical
		{
			get
			{
				this.ReadLock();
				bool isSystemAclCanonical;
				try
				{
					isSystemAclCanonical = this.descriptor.IsSystemAclCanonical;
				}
				finally
				{
					this.ReadUnlock();
				}
				return isSystemAclCanonical;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06003512 RID: 13586 RVA: 0x000C06A8 File Offset: 0x000BE8A8
		public bool AreAuditRulesProtected
		{
			get
			{
				this.ReadLock();
				bool result;
				try
				{
					result = ((this.descriptor.ControlFlags & ControlFlags.SystemAclProtected) > ControlFlags.None);
				}
				finally
				{
					this.ReadUnlock();
				}
				return result;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06003513 RID: 13587 RVA: 0x000C06EC File Offset: 0x000BE8EC
		// (set) Token: 0x06003514 RID: 13588 RVA: 0x000C06FA File Offset: 0x000BE8FA
		internal AccessControlSections AccessControlSectionsModified
		{
			get
			{
				this.Reading();
				return this.sections_modified;
			}
			set
			{
				this.Writing();
				this.sections_modified = value;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06003515 RID: 13589 RVA: 0x000C0709 File Offset: 0x000BE909
		// (set) Token: 0x06003516 RID: 13590 RVA: 0x000C0712 File Offset: 0x000BE912
		protected bool AccessRulesModified
		{
			get
			{
				return this.AreAccessControlSectionsModified(AccessControlSections.Access);
			}
			set
			{
				this.SetAccessControlSectionsModified(AccessControlSections.Access, value);
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06003517 RID: 13591 RVA: 0x000C071C File Offset: 0x000BE91C
		// (set) Token: 0x06003518 RID: 13592 RVA: 0x000C0725 File Offset: 0x000BE925
		protected bool AuditRulesModified
		{
			get
			{
				return this.AreAccessControlSectionsModified(AccessControlSections.Audit);
			}
			set
			{
				this.SetAccessControlSectionsModified(AccessControlSections.Audit, value);
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06003519 RID: 13593 RVA: 0x000C072F File Offset: 0x000BE92F
		// (set) Token: 0x0600351A RID: 13594 RVA: 0x000C0738 File Offset: 0x000BE938
		protected bool GroupModified
		{
			get
			{
				return this.AreAccessControlSectionsModified(AccessControlSections.Group);
			}
			set
			{
				this.SetAccessControlSectionsModified(AccessControlSections.Group, value);
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x0600351B RID: 13595 RVA: 0x000C0742 File Offset: 0x000BE942
		protected bool IsContainer
		{
			get
			{
				return this.descriptor.IsContainer;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x0600351C RID: 13596 RVA: 0x000C074F File Offset: 0x000BE94F
		protected bool IsDS
		{
			get
			{
				return this.descriptor.IsDS;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x0600351D RID: 13597 RVA: 0x000C075C File Offset: 0x000BE95C
		// (set) Token: 0x0600351E RID: 13598 RVA: 0x000C0765 File Offset: 0x000BE965
		protected bool OwnerModified
		{
			get
			{
				return this.AreAccessControlSectionsModified(AccessControlSections.Owner);
			}
			set
			{
				this.SetAccessControlSectionsModified(AccessControlSections.Owner, value);
			}
		}

		// Token: 0x0600351F RID: 13599
		public abstract AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type);

		// Token: 0x06003520 RID: 13600
		public abstract AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags);

		// Token: 0x06003521 RID: 13601 RVA: 0x000C0770 File Offset: 0x000BE970
		public IdentityReference GetGroup(Type targetType)
		{
			this.ReadLock();
			IdentityReference result;
			try
			{
				if (this.descriptor.Group == null)
				{
					result = null;
				}
				else
				{
					result = this.descriptor.Group.Translate(targetType);
				}
			}
			finally
			{
				this.ReadUnlock();
			}
			return result;
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x000C07C8 File Offset: 0x000BE9C8
		public IdentityReference GetOwner(Type targetType)
		{
			this.ReadLock();
			IdentityReference result;
			try
			{
				if (this.descriptor.Owner == null)
				{
					result = null;
				}
				else
				{
					result = this.descriptor.Owner.Translate(targetType);
				}
			}
			finally
			{
				this.ReadUnlock();
			}
			return result;
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x000C0820 File Offset: 0x000BEA20
		public byte[] GetSecurityDescriptorBinaryForm()
		{
			this.ReadLock();
			byte[] result;
			try
			{
				byte[] array = new byte[this.descriptor.BinaryLength];
				this.descriptor.GetBinaryForm(array, 0);
				result = array;
			}
			finally
			{
				this.ReadUnlock();
			}
			return result;
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x000C0870 File Offset: 0x000BEA70
		public string GetSecurityDescriptorSddlForm(AccessControlSections includeSections)
		{
			this.ReadLock();
			string sddlForm;
			try
			{
				sddlForm = this.descriptor.GetSddlForm(includeSections);
			}
			finally
			{
				this.ReadUnlock();
			}
			return sddlForm;
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x000C08AC File Offset: 0x000BEAAC
		public static bool IsSddlConversionSupported()
		{
			return GenericSecurityDescriptor.IsSddlConversionSupported();
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x000C08B3 File Offset: 0x000BEAB3
		public virtual bool ModifyAccessRule(AccessControlModification modification, AccessRule rule, out bool modified)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			if (!this.AccessRuleType.IsAssignableFrom(rule.GetType()))
			{
				throw new ArgumentException("rule");
			}
			return this.ModifyAccess(modification, rule, out modified);
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x000C08EA File Offset: 0x000BEAEA
		public virtual bool ModifyAuditRule(AccessControlModification modification, AuditRule rule, out bool modified)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			if (!this.AuditRuleType.IsAssignableFrom(rule.GetType()))
			{
				throw new ArgumentException("rule");
			}
			return this.ModifyAudit(modification, rule, out modified);
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x000C0924 File Offset: 0x000BEB24
		public virtual void PurgeAccessRules(IdentityReference identity)
		{
			if (null == identity)
			{
				throw new ArgumentNullException("identity");
			}
			this.WriteLock();
			try
			{
				this.descriptor.PurgeAccessControl(ObjectSecurity.SidFromIR(identity));
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x000C0978 File Offset: 0x000BEB78
		public virtual void PurgeAuditRules(IdentityReference identity)
		{
			if (null == identity)
			{
				throw new ArgumentNullException("identity");
			}
			this.WriteLock();
			try
			{
				this.descriptor.PurgeAudit(ObjectSecurity.SidFromIR(identity));
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x0600352A RID: 13610 RVA: 0x000C09CC File Offset: 0x000BEBCC
		public void SetAccessRuleProtection(bool isProtected, bool preserveInheritance)
		{
			this.WriteLock();
			try
			{
				this.descriptor.SetDiscretionaryAclProtection(isProtected, preserveInheritance);
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x000C0A08 File Offset: 0x000BEC08
		public void SetAuditRuleProtection(bool isProtected, bool preserveInheritance)
		{
			this.WriteLock();
			try
			{
				this.descriptor.SetSystemAclProtection(isProtected, preserveInheritance);
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x000C0A44 File Offset: 0x000BEC44
		public void SetGroup(IdentityReference identity)
		{
			this.WriteLock();
			try
			{
				this.descriptor.Group = ObjectSecurity.SidFromIR(identity);
				this.GroupModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x000C0A88 File Offset: 0x000BEC88
		public void SetOwner(IdentityReference identity)
		{
			this.WriteLock();
			try
			{
				this.descriptor.Owner = ObjectSecurity.SidFromIR(identity);
				this.OwnerModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x000C0ACC File Offset: 0x000BECCC
		public void SetSecurityDescriptorBinaryForm(byte[] binaryForm)
		{
			this.SetSecurityDescriptorBinaryForm(binaryForm, AccessControlSections.All);
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x000C0AD7 File Offset: 0x000BECD7
		public void SetSecurityDescriptorBinaryForm(byte[] binaryForm, AccessControlSections includeSections)
		{
			this.CopySddlForm(new CommonSecurityDescriptor(this.IsContainer, this.IsDS, binaryForm, 0), includeSections);
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x000C0AF3 File Offset: 0x000BECF3
		public void SetSecurityDescriptorSddlForm(string sddlForm)
		{
			this.SetSecurityDescriptorSddlForm(sddlForm, AccessControlSections.All);
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x000C0AFE File Offset: 0x000BECFE
		public void SetSecurityDescriptorSddlForm(string sddlForm, AccessControlSections includeSections)
		{
			this.CopySddlForm(new CommonSecurityDescriptor(this.IsContainer, this.IsDS, sddlForm), includeSections);
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x000C0B1C File Offset: 0x000BED1C
		private void CopySddlForm(CommonSecurityDescriptor sourceDescriptor, AccessControlSections includeSections)
		{
			this.WriteLock();
			try
			{
				this.AccessControlSectionsModified |= includeSections;
				if ((includeSections & AccessControlSections.Audit) != AccessControlSections.None)
				{
					this.descriptor.SystemAcl = sourceDescriptor.SystemAcl;
				}
				if ((includeSections & AccessControlSections.Access) != AccessControlSections.None)
				{
					this.descriptor.DiscretionaryAcl = sourceDescriptor.DiscretionaryAcl;
				}
				if ((includeSections & AccessControlSections.Owner) != AccessControlSections.None)
				{
					this.descriptor.Owner = sourceDescriptor.Owner;
				}
				if ((includeSections & AccessControlSections.Group) != AccessControlSections.None)
				{
					this.descriptor.Group = sourceDescriptor.Group;
				}
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x06003533 RID: 13619
		protected abstract bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified);

		// Token: 0x06003534 RID: 13620
		protected abstract bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified);

		// Token: 0x06003535 RID: 13621 RVA: 0x0004D855 File Offset: 0x0004BA55
		private Exception GetNotImplementedException()
		{
			return new NotImplementedException();
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x000C0BB0 File Offset: 0x000BEDB0
		protected virtual void Persist(SafeHandle handle, AccessControlSections includeSections)
		{
			throw this.GetNotImplementedException();
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x000C0BB0 File Offset: 0x000BEDB0
		protected virtual void Persist(string name, AccessControlSections includeSections)
		{
			throw this.GetNotImplementedException();
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		[HandleProcessCorruptedStateExceptions]
		protected virtual void Persist(bool enableOwnershipPrivilege, string name, AccessControlSections includeSections)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x000C0BB8 File Offset: 0x000BEDB8
		private void Reading()
		{
			if (!this.rw_lock.IsReaderLockHeld && !this.rw_lock.IsWriterLockHeld)
			{
				throw new InvalidOperationException("Either a read or a write lock must be held.");
			}
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x000C0BDF File Offset: 0x000BEDDF
		protected void ReadLock()
		{
			this.rw_lock.AcquireReaderLock(-1);
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x000C0BED File Offset: 0x000BEDED
		protected void ReadUnlock()
		{
			this.rw_lock.ReleaseReaderLock();
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x000C0BFA File Offset: 0x000BEDFA
		private void Writing()
		{
			if (!this.rw_lock.IsWriterLockHeld)
			{
				throw new InvalidOperationException("Write lock must be held.");
			}
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x000C0C14 File Offset: 0x000BEE14
		protected void WriteLock()
		{
			this.rw_lock.AcquireWriterLock(-1);
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x000C0C22 File Offset: 0x000BEE22
		protected void WriteUnlock()
		{
			this.rw_lock.ReleaseWriterLock();
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x000C0C30 File Offset: 0x000BEE30
		internal AuthorizationRuleCollection InternalGetAccessRules(bool includeExplicit, bool includeInherited, Type targetType)
		{
			List<AuthorizationRule> list = new List<AuthorizationRule>();
			this.ReadLock();
			try
			{
				foreach (GenericAce genericAce in this.descriptor.DiscretionaryAcl)
				{
					QualifiedAce qualifiedAce = genericAce as QualifiedAce;
					if (!(null == qualifiedAce) && (!qualifiedAce.IsInherited || includeInherited) && (qualifiedAce.IsInherited || includeExplicit))
					{
						AccessControlType type;
						if (qualifiedAce.AceQualifier == AceQualifier.AccessAllowed)
						{
							type = AccessControlType.Allow;
						}
						else
						{
							if (AceQualifier.AccessDenied != qualifiedAce.AceQualifier)
							{
								continue;
							}
							type = AccessControlType.Deny;
						}
						AccessRule item = this.InternalAccessRuleFactory(qualifiedAce, targetType, type);
						list.Add(item);
					}
				}
			}
			finally
			{
				this.ReadUnlock();
			}
			return new AuthorizationRuleCollection(list.ToArray());
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x000C0CE0 File Offset: 0x000BEEE0
		internal virtual AccessRule InternalAccessRuleFactory(QualifiedAce ace, Type targetType, AccessControlType type)
		{
			return this.AccessRuleFactory(ace.SecurityIdentifier.Translate(targetType), ace.AccessMask, ace.IsInherited, ace.InheritanceFlags, ace.PropagationFlags, type);
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x000C0D10 File Offset: 0x000BEF10
		internal AuthorizationRuleCollection InternalGetAuditRules(bool includeExplicit, bool includeInherited, Type targetType)
		{
			List<AuthorizationRule> list = new List<AuthorizationRule>();
			this.ReadLock();
			try
			{
				if (this.descriptor.SystemAcl != null)
				{
					foreach (GenericAce genericAce in this.descriptor.SystemAcl)
					{
						QualifiedAce qualifiedAce = genericAce as QualifiedAce;
						if (!(null == qualifiedAce) && (!qualifiedAce.IsInherited || includeInherited) && (qualifiedAce.IsInherited || includeExplicit) && AceQualifier.SystemAudit == qualifiedAce.AceQualifier)
						{
							AuditRule item = this.InternalAuditRuleFactory(qualifiedAce, targetType);
							list.Add(item);
						}
					}
				}
			}
			finally
			{
				this.ReadUnlock();
			}
			return new AuthorizationRuleCollection(list.ToArray());
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x000C0DBC File Offset: 0x000BEFBC
		internal virtual AuditRule InternalAuditRuleFactory(QualifiedAce ace, Type targetType)
		{
			return this.AuditRuleFactory(ace.SecurityIdentifier.Translate(targetType), ace.AccessMask, ace.IsInherited, ace.InheritanceFlags, ace.PropagationFlags, ace.AuditFlags);
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x000C0DEE File Offset: 0x000BEFEE
		internal static SecurityIdentifier SidFromIR(IdentityReference identity)
		{
			if (null == identity)
			{
				throw new ArgumentNullException("identity");
			}
			return (SecurityIdentifier)identity.Translate(typeof(SecurityIdentifier));
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x000C0E19 File Offset: 0x000BF019
		private bool AreAccessControlSectionsModified(AccessControlSections mask)
		{
			return (this.AccessControlSectionsModified & mask) > AccessControlSections.None;
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x000C0E26 File Offset: 0x000BF026
		private void SetAccessControlSectionsModified(AccessControlSections mask, bool modified)
		{
			if (modified)
			{
				this.AccessControlSectionsModified |= mask;
				return;
			}
			this.AccessControlSectionsModified &= ~mask;
		}

		// Token: 0x040024CD RID: 9421
		internal CommonSecurityDescriptor descriptor;

		// Token: 0x040024CE RID: 9422
		private AccessControlSections sections_modified;

		// Token: 0x040024CF RID: 9423
		private ReaderWriterLock rw_lock;
	}
}
