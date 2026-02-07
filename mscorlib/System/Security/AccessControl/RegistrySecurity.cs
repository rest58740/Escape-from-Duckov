using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;

namespace System.Security.AccessControl
{
	// Token: 0x020004FB RID: 1275
	public sealed class RegistrySecurity : NativeObjectSecurity
	{
		// Token: 0x06003315 RID: 13077 RVA: 0x000BC504 File Offset: 0x000BA704
		private static Exception _HandleErrorCodeCore(int errorCode, string name, SafeHandle handle, object context)
		{
			Exception result = null;
			if (errorCode != 2)
			{
				if (errorCode != 6)
				{
					if (errorCode == 123)
					{
						result = new ArgumentException(SR.Format("Registry key name must start with a valid base key name.", "name"));
					}
				}
				else
				{
					result = new ArgumentException("The supplied handle is invalid. This can happen when trying to set an ACL on an anonymous kernel object.");
				}
			}
			else
			{
				result = new IOException(SR.Format("The specified registry key does not exist.", errorCode));
			}
			return result;
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x000BC55D File Offset: 0x000BA75D
		public RegistrySecurity() : base(true, ResourceType.RegistryKey)
		{
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x000BC567 File Offset: 0x000BA767
		internal RegistrySecurity(SafeRegistryHandle hKey, string name, AccessControlSections includeSections) : base(true, ResourceType.RegistryKey, hKey, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(RegistrySecurity._HandleErrorCode), null)
		{
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x000BC580 File Offset: 0x000BA780
		private static Exception _HandleErrorCode(int errorCode, string name, SafeHandle handle, object context)
		{
			return RegistrySecurity._HandleErrorCodeCore(errorCode, name, handle, context);
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x000BC58B File Offset: 0x000BA78B
		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new RegistryAccessRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, type);
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x000BC59B File Offset: 0x000BA79B
		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new RegistryAuditRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x000BC5AC File Offset: 0x000BA7AC
		internal AccessControlSections GetAccessControlSectionsFromChanges()
		{
			AccessControlSections accessControlSections = AccessControlSections.None;
			if (base.AccessRulesModified)
			{
				accessControlSections = AccessControlSections.Access;
			}
			if (base.AuditRulesModified)
			{
				accessControlSections |= AccessControlSections.Audit;
			}
			if (base.OwnerModified)
			{
				accessControlSections |= AccessControlSections.Owner;
			}
			if (base.GroupModified)
			{
				accessControlSections |= AccessControlSections.Group;
			}
			return accessControlSections;
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x000BC5EC File Offset: 0x000BA7EC
		internal void Persist(SafeRegistryHandle hKey, string keyName)
		{
			base.WriteLock();
			try
			{
				AccessControlSections accessControlSectionsFromChanges = this.GetAccessControlSectionsFromChanges();
				if (accessControlSectionsFromChanges != AccessControlSections.None)
				{
					this.Persist(hKey, accessControlSectionsFromChanges);
					base.OwnerModified = (base.GroupModified = (base.AuditRulesModified = (base.AccessRulesModified = false)));
				}
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x000BC650 File Offset: 0x000BA850
		public void AddAccessRule(RegistryAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x000BC659 File Offset: 0x000BA859
		public void SetAccessRule(RegistryAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x000BC662 File Offset: 0x000BA862
		public void ResetAccessRule(RegistryAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x000BC66B File Offset: 0x000BA86B
		public bool RemoveAccessRule(RegistryAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x000BC674 File Offset: 0x000BA874
		public void RemoveAccessRuleAll(RegistryAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x000BC67D File Offset: 0x000BA87D
		public void RemoveAccessRuleSpecific(RegistryAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x000BC686 File Offset: 0x000BA886
		public void AddAuditRule(RegistryAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x000BC68F File Offset: 0x000BA88F
		public void SetAuditRule(RegistryAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x000BC698 File Offset: 0x000BA898
		public bool RemoveAuditRule(RegistryAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x000BC6A1 File Offset: 0x000BA8A1
		public void RemoveAuditRuleAll(RegistryAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x000BC6AA File Offset: 0x000BA8AA
		public void RemoveAuditRuleSpecific(RegistryAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06003328 RID: 13096 RVA: 0x000BC6B3 File Offset: 0x000BA8B3
		public override Type AccessRightType
		{
			get
			{
				return typeof(RegistryRights);
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06003329 RID: 13097 RVA: 0x000BC6BF File Offset: 0x000BA8BF
		public override Type AccessRuleType
		{
			get
			{
				return typeof(RegistryAccessRule);
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x0600332A RID: 13098 RVA: 0x000BC6CB File Offset: 0x000BA8CB
		public override Type AuditRuleType
		{
			get
			{
				return typeof(RegistryAuditRule);
			}
		}
	}
}
