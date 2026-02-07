using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200051E RID: 1310
	public sealed class CryptoKeySecurity : NativeObjectSecurity
	{
		// Token: 0x060033DA RID: 13274 RVA: 0x000BE0B4 File Offset: 0x000BC2B4
		public CryptoKeySecurity() : base(false, ResourceType.Unknown)
		{
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x000BE0BE File Offset: 0x000BC2BE
		public CryptoKeySecurity(CommonSecurityDescriptor securityDescriptor) : base(securityDescriptor, ResourceType.Unknown)
		{
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060033DC RID: 13276 RVA: 0x000BE0C8 File Offset: 0x000BC2C8
		public override Type AccessRightType
		{
			get
			{
				return typeof(CryptoKeyRights);
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060033DD RID: 13277 RVA: 0x000BE0D4 File Offset: 0x000BC2D4
		public override Type AccessRuleType
		{
			get
			{
				return typeof(CryptoKeyAccessRule);
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060033DE RID: 13278 RVA: 0x000BE0E0 File Offset: 0x000BC2E0
		public override Type AuditRuleType
		{
			get
			{
				return typeof(CryptoKeyAuditRule);
			}
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x000BE0EC File Offset: 0x000BC2EC
		public sealed override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new CryptoKeyAccessRule(identityReference, (CryptoKeyRights)accessMask, type);
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x000BC650 File Offset: 0x000BA850
		public void AddAccessRule(CryptoKeyAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x000BC66B File Offset: 0x000BA86B
		public bool RemoveAccessRule(CryptoKeyAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x000BC674 File Offset: 0x000BA874
		public void RemoveAccessRuleAll(CryptoKeyAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x000BC67D File Offset: 0x000BA87D
		public void RemoveAccessRuleSpecific(CryptoKeyAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x000BC662 File Offset: 0x000BA862
		public void ResetAccessRule(CryptoKeyAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x060033E5 RID: 13285 RVA: 0x000BC659 File Offset: 0x000BA859
		public void SetAccessRule(CryptoKeyAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x000BE0F7 File Offset: 0x000BC2F7
		public sealed override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new CryptoKeyAuditRule(identityReference, (CryptoKeyRights)accessMask, flags);
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x000BC686 File Offset: 0x000BA886
		public void AddAuditRule(CryptoKeyAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x000BC698 File Offset: 0x000BA898
		public bool RemoveAuditRule(CryptoKeyAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x000BC6A1 File Offset: 0x000BA8A1
		public void RemoveAuditRuleAll(CryptoKeyAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x000BC6AA File Offset: 0x000BA8AA
		public void RemoveAuditRuleSpecific(CryptoKeyAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x000BC68F File Offset: 0x000BA88F
		public void SetAuditRule(CryptoKeyAuditRule rule)
		{
			base.SetAuditRule(rule);
		}
	}
}
