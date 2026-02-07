using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000522 RID: 1314
	public sealed class DiscretionaryAcl : CommonAcl
	{
		// Token: 0x0600340B RID: 13323 RVA: 0x000BE770 File Offset: 0x000BC970
		public DiscretionaryAcl(bool isContainer, bool isDS, int capacity) : base(isContainer, isDS, capacity)
		{
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x000BE77B File Offset: 0x000BC97B
		public DiscretionaryAcl(bool isContainer, bool isDS, RawAcl rawAcl) : base(isContainer, isDS, rawAcl)
		{
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x000BE786 File Offset: 0x000BC986
		public DiscretionaryAcl(bool isContainer, bool isDS, byte revision, int capacity) : base(isContainer, isDS, revision, capacity)
		{
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x000BE793 File Offset: 0x000BC993
		public void AddAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			base.AddAce(DiscretionaryAcl.GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None);
		}

		// Token: 0x0600340F RID: 13327 RVA: 0x000BE7A8 File Offset: 0x000BC9A8
		public void AddAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			base.AddAce(DiscretionaryAcl.GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None, objectFlags, objectType, inheritedObjectType);
		}

		// Token: 0x06003410 RID: 13328 RVA: 0x000BE7D0 File Offset: 0x000BC9D0
		public void AddAccess(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
		{
			this.AddAccess(accessType, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public bool RemoveAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public bool RemoveAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x000BE80C File Offset: 0x000BCA0C
		public bool RemoveAccess(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
		{
			return this.RemoveAccess(accessType, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x000BE845 File Offset: 0x000BCA45
		public void RemoveAccessSpecific(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			base.RemoveAceSpecific(DiscretionaryAcl.GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None);
		}

		// Token: 0x06003415 RID: 13333 RVA: 0x000BE85C File Offset: 0x000BCA5C
		public void RemoveAccessSpecific(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			base.RemoveAceSpecific(DiscretionaryAcl.GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None, objectFlags, objectType, inheritedObjectType);
		}

		// Token: 0x06003416 RID: 13334 RVA: 0x000BE884 File Offset: 0x000BCA84
		public void RemoveAccessSpecific(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
		{
			this.RemoveAccessSpecific(accessType, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x000BE8BD File Offset: 0x000BCABD
		public void SetAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			base.SetAce(DiscretionaryAcl.GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None);
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x000BE8D4 File Offset: 0x000BCAD4
		public void SetAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			base.SetAce(DiscretionaryAcl.GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None, objectFlags, objectType, inheritedObjectType);
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x000BE8FC File Offset: 0x000BCAFC
		public void SetAccess(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
		{
			this.SetAccess(accessType, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x000BE938 File Offset: 0x000BCB38
		internal override void ApplyCanonicalSortToExplicitAces()
		{
			int canonicalExplicitAceCount = base.GetCanonicalExplicitAceCount();
			int canonicalExplicitDenyAceCount = base.GetCanonicalExplicitDenyAceCount();
			base.ApplyCanonicalSortToExplicitAces(0, canonicalExplicitDenyAceCount);
			base.ApplyCanonicalSortToExplicitAces(canonicalExplicitDenyAceCount, canonicalExplicitAceCount - canonicalExplicitDenyAceCount);
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x000BE965 File Offset: 0x000BCB65
		internal override int GetAceInsertPosition(AceQualifier aceQualifier)
		{
			if (aceQualifier == AceQualifier.AccessAllowed)
			{
				return base.GetCanonicalExplicitDenyAceCount();
			}
			return 0;
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x000BE972 File Offset: 0x000BCB72
		private static AceQualifier GetAceQualifier(AccessControlType accessType)
		{
			if (accessType == AccessControlType.Allow)
			{
				return AceQualifier.AccessAllowed;
			}
			if (AccessControlType.Deny == accessType)
			{
				return AceQualifier.AccessDenied;
			}
			throw new ArgumentOutOfRangeException("accessType");
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x000BE98C File Offset: 0x000BCB8C
		internal override bool IsAceMeaningless(GenericAce ace)
		{
			if (base.IsAceMeaningless(ace))
			{
				return true;
			}
			if (ace.AuditFlags != AuditFlags.None)
			{
				return true;
			}
			QualifiedAce qualifiedAce = ace as QualifiedAce;
			return null != qualifiedAce && qualifiedAce.AceQualifier != AceQualifier.AccessAllowed && AceQualifier.AccessDenied != qualifiedAce.AceQualifier;
		}
	}
}
