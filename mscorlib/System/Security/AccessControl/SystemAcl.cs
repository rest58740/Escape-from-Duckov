using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200054B RID: 1355
	public sealed class SystemAcl : CommonAcl
	{
		// Token: 0x06003594 RID: 13716 RVA: 0x000BE770 File Offset: 0x000BC970
		public SystemAcl(bool isContainer, bool isDS, int capacity) : base(isContainer, isDS, capacity)
		{
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000BE77B File Offset: 0x000BC97B
		public SystemAcl(bool isContainer, bool isDS, RawAcl rawAcl) : base(isContainer, isDS, rawAcl)
		{
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x000BE786 File Offset: 0x000BC986
		public SystemAcl(bool isContainer, bool isDS, byte revision, int capacity) : base(isContainer, isDS, revision, capacity)
		{
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x000C1D6E File Offset: 0x000BFF6E
		public void AddAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			base.AddAce(AceQualifier.SystemAudit, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags);
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x000C1D80 File Offset: 0x000BFF80
		public void AddAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			base.AddAce(AceQualifier.SystemAudit, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags, objectFlags, objectType, inheritedObjectType);
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x000C1DA4 File Offset: 0x000BFFA4
		public void AddAudit(SecurityIdentifier sid, ObjectAuditRule rule)
		{
			this.AddAudit(rule.AuditFlags, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public bool RemoveAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public bool RemoveAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x000C1DE4 File Offset: 0x000BFFE4
		public bool RemoveAudit(SecurityIdentifier sid, ObjectAuditRule rule)
		{
			return this.RemoveAudit(rule.AuditFlags, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x000C1E22 File Offset: 0x000C0022
		public void RemoveAuditSpecific(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			base.RemoveAceSpecific(AceQualifier.SystemAudit, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags);
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x000C1E34 File Offset: 0x000C0034
		public void RemoveAuditSpecific(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			base.RemoveAceSpecific(AceQualifier.SystemAudit, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags, objectFlags, objectType, inheritedObjectType);
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x000C1E58 File Offset: 0x000C0058
		public void RemoveAuditSpecific(SecurityIdentifier sid, ObjectAuditRule rule)
		{
			this.RemoveAuditSpecific(rule.AuditFlags, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x000C1E96 File Offset: 0x000C0096
		public void SetAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			base.SetAce(AceQualifier.SystemAudit, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags);
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x000C1EA8 File Offset: 0x000C00A8
		public void SetAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			base.SetAce(AceQualifier.SystemAudit, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags, objectFlags, objectType, inheritedObjectType);
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x000C1ECC File Offset: 0x000C00CC
		public void SetAudit(SecurityIdentifier sid, ObjectAuditRule rule)
		{
			this.SetAudit(rule.AuditFlags, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x000C1F0C File Offset: 0x000C010C
		internal override void ApplyCanonicalSortToExplicitAces()
		{
			int canonicalExplicitAceCount = base.GetCanonicalExplicitAceCount();
			base.ApplyCanonicalSortToExplicitAces(0, canonicalExplicitAceCount);
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal override int GetAceInsertPosition(AceQualifier aceQualifier)
		{
			return 0;
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x000C1F28 File Offset: 0x000C0128
		internal override bool IsAceMeaningless(GenericAce ace)
		{
			if (base.IsAceMeaningless(ace))
			{
				return true;
			}
			if (!SystemAcl.IsValidAuditFlags(ace.AuditFlags))
			{
				return true;
			}
			QualifiedAce qualifiedAce = ace as QualifiedAce;
			return null != qualifiedAce && AceQualifier.SystemAudit != qualifiedAce.AceQualifier && AceQualifier.SystemAlarm != qualifiedAce.AceQualifier;
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x000C1F74 File Offset: 0x000C0174
		private static bool IsValidAuditFlags(AuditFlags auditFlags)
		{
			return auditFlags != AuditFlags.None && auditFlags == ((AuditFlags.Success | AuditFlags.Failure) & auditFlags);
		}
	}
}
