using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000516 RID: 1302
	public abstract class CommonObjectSecurity : ObjectSecurity
	{
		// Token: 0x060033A0 RID: 13216 RVA: 0x000BD85E File Offset: 0x000BBA5E
		protected CommonObjectSecurity(bool isContainer) : base(isContainer, false)
		{
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x000BD868 File Offset: 0x000BBA68
		internal CommonObjectSecurity(CommonSecurityDescriptor securityDescriptor) : base(securityDescriptor)
		{
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x000BD871 File Offset: 0x000BBA71
		public AuthorizationRuleCollection GetAccessRules(bool includeExplicit, bool includeInherited, Type targetType)
		{
			return base.InternalGetAccessRules(includeExplicit, includeInherited, targetType);
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x000BD87C File Offset: 0x000BBA7C
		public AuthorizationRuleCollection GetAuditRules(bool includeExplicit, bool includeInherited, Type targetType)
		{
			return base.InternalGetAuditRules(includeExplicit, includeInherited, targetType);
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x000BD888 File Offset: 0x000BBA88
		protected void AddAccessRule(AccessRule rule)
		{
			bool flag;
			this.ModifyAccess(AccessControlModification.Add, rule, out flag);
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x000BD8A0 File Offset: 0x000BBAA0
		protected bool RemoveAccessRule(AccessRule rule)
		{
			bool flag;
			return this.ModifyAccess(AccessControlModification.Remove, rule, out flag);
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x000BD8B8 File Offset: 0x000BBAB8
		protected void RemoveAccessRuleAll(AccessRule rule)
		{
			bool flag;
			this.ModifyAccess(AccessControlModification.RemoveAll, rule, out flag);
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x000BD8D0 File Offset: 0x000BBAD0
		protected void RemoveAccessRuleSpecific(AccessRule rule)
		{
			bool flag;
			this.ModifyAccess(AccessControlModification.RemoveSpecific, rule, out flag);
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x000BD8E8 File Offset: 0x000BBAE8
		protected void ResetAccessRule(AccessRule rule)
		{
			bool flag;
			this.ModifyAccess(AccessControlModification.Reset, rule, out flag);
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x000BD900 File Offset: 0x000BBB00
		protected void SetAccessRule(AccessRule rule)
		{
			bool flag;
			this.ModifyAccess(AccessControlModification.Set, rule, out flag);
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x000BD918 File Offset: 0x000BBB18
		protected override bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			modified = true;
			base.WriteLock();
			try
			{
				switch (modification)
				{
				case AccessControlModification.Add:
					break;
				case AccessControlModification.Set:
					this.descriptor.DiscretionaryAcl.SetAccess(rule.AccessControlType, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
					goto IL_13D;
				case AccessControlModification.Reset:
					this.PurgeAccessRules(rule.IdentityReference);
					break;
				case AccessControlModification.Remove:
					modified = this.descriptor.DiscretionaryAcl.RemoveAccess(rule.AccessControlType, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
					goto IL_13D;
				case AccessControlModification.RemoveAll:
					this.PurgeAccessRules(rule.IdentityReference);
					goto IL_13D;
				case AccessControlModification.RemoveSpecific:
					this.descriptor.DiscretionaryAcl.RemoveAccessSpecific(rule.AccessControlType, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
					goto IL_13D;
				default:
					throw new ArgumentOutOfRangeException("modification");
				}
				this.descriptor.DiscretionaryAcl.AddAccess(rule.AccessControlType, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
				IL_13D:
				if (modified)
				{
					base.AccessRulesModified = true;
				}
			}
			finally
			{
				base.WriteUnlock();
			}
			return modified;
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x000BDA94 File Offset: 0x000BBC94
		protected void AddAuditRule(AuditRule rule)
		{
			bool flag;
			this.ModifyAudit(AccessControlModification.Add, rule, out flag);
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x000BDAAC File Offset: 0x000BBCAC
		protected bool RemoveAuditRule(AuditRule rule)
		{
			bool flag;
			return this.ModifyAudit(AccessControlModification.Remove, rule, out flag);
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x000BDAC4 File Offset: 0x000BBCC4
		protected void RemoveAuditRuleAll(AuditRule rule)
		{
			bool flag;
			this.ModifyAudit(AccessControlModification.RemoveAll, rule, out flag);
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x000BDADC File Offset: 0x000BBCDC
		protected void RemoveAuditRuleSpecific(AuditRule rule)
		{
			bool flag;
			this.ModifyAudit(AccessControlModification.RemoveSpecific, rule, out flag);
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x000BDAF4 File Offset: 0x000BBCF4
		protected void SetAuditRule(AuditRule rule)
		{
			bool flag;
			this.ModifyAudit(AccessControlModification.Set, rule, out flag);
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x000BDB0C File Offset: 0x000BBD0C
		protected override bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			modified = true;
			base.WriteLock();
			try
			{
				switch (modification)
				{
				case AccessControlModification.Add:
					if (this.descriptor.SystemAcl == null)
					{
						this.descriptor.SystemAcl = new SystemAcl(base.IsContainer, base.IsDS, 1);
					}
					this.descriptor.SystemAcl.AddAudit(rule.AuditFlags, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
					break;
				case AccessControlModification.Set:
					if (this.descriptor.SystemAcl == null)
					{
						this.descriptor.SystemAcl = new SystemAcl(base.IsContainer, base.IsDS, 1);
					}
					this.descriptor.SystemAcl.SetAudit(rule.AuditFlags, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
					break;
				case AccessControlModification.Reset:
					break;
				case AccessControlModification.Remove:
					if (this.descriptor.SystemAcl == null)
					{
						modified = false;
					}
					else
					{
						modified = this.descriptor.SystemAcl.RemoveAudit(rule.AuditFlags, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
					}
					break;
				case AccessControlModification.RemoveAll:
					this.PurgeAuditRules(rule.IdentityReference);
					break;
				case AccessControlModification.RemoveSpecific:
					if (this.descriptor.SystemAcl != null)
					{
						this.descriptor.SystemAcl.RemoveAuditSpecific(rule.AuditFlags, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException("modification");
				}
				if (modified)
				{
					base.AuditRulesModified = true;
				}
			}
			finally
			{
				base.WriteUnlock();
			}
			return modified;
		}
	}
}
