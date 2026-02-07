using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000526 RID: 1318
	public sealed class EventWaitHandleSecurity : NativeObjectSecurity
	{
		// Token: 0x06003423 RID: 13347 RVA: 0x000BEA07 File Offset: 0x000BCC07
		public EventWaitHandleSecurity() : base(false, ResourceType.KernelObject)
		{
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x000BEA11 File Offset: 0x000BCC11
		internal EventWaitHandleSecurity(SafeHandle handle, AccessControlSections includeSections) : base(false, ResourceType.KernelObject, handle, includeSections)
		{
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06003425 RID: 13349 RVA: 0x000BEA1D File Offset: 0x000BCC1D
		public override Type AccessRightType
		{
			get
			{
				return typeof(EventWaitHandleRights);
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06003426 RID: 13350 RVA: 0x000BEA29 File Offset: 0x000BCC29
		public override Type AccessRuleType
		{
			get
			{
				return typeof(EventWaitHandleAccessRule);
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06003427 RID: 13351 RVA: 0x000BEA35 File Offset: 0x000BCC35
		public override Type AuditRuleType
		{
			get
			{
				return typeof(EventWaitHandleAuditRule);
			}
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x000BEA41 File Offset: 0x000BCC41
		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new EventWaitHandleAccessRule(identityReference, (EventWaitHandleRights)accessMask, type);
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x000BC650 File Offset: 0x000BA850
		public void AddAccessRule(EventWaitHandleAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x000BC66B File Offset: 0x000BA86B
		public bool RemoveAccessRule(EventWaitHandleAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x000BC674 File Offset: 0x000BA874
		public void RemoveAccessRuleAll(EventWaitHandleAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x000BC67D File Offset: 0x000BA87D
		public void RemoveAccessRuleSpecific(EventWaitHandleAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x000BC662 File Offset: 0x000BA862
		public void ResetAccessRule(EventWaitHandleAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x000BC659 File Offset: 0x000BA859
		public void SetAccessRule(EventWaitHandleAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x000BEA4C File Offset: 0x000BCC4C
		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new EventWaitHandleAuditRule(identityReference, (EventWaitHandleRights)accessMask, flags);
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x000BC686 File Offset: 0x000BA886
		public void AddAuditRule(EventWaitHandleAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x000BC698 File Offset: 0x000BA898
		public bool RemoveAuditRule(EventWaitHandleAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x000BC6A1 File Offset: 0x000BA8A1
		public void RemoveAuditRuleAll(EventWaitHandleAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x000BC6AA File Offset: 0x000BA8AA
		public void RemoveAuditRuleSpecific(EventWaitHandleAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x000BC68F File Offset: 0x000BA88F
		public void SetAuditRule(EventWaitHandleAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x000BEA57 File Offset: 0x000BCC57
		internal void Persist(SafeHandle handle)
		{
			base.PersistModifications(handle);
		}
	}
}
