using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000542 RID: 1346
	public abstract class ObjectSecurity<T> : NativeObjectSecurity where T : struct
	{
		// Token: 0x06003546 RID: 13638 RVA: 0x000C0E49 File Offset: 0x000BF049
		protected ObjectSecurity(bool isContainer, ResourceType resourceType) : base(isContainer, resourceType)
		{
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x000C0E53 File Offset: 0x000BF053
		protected ObjectSecurity(bool isContainer, ResourceType resourceType, SafeHandle safeHandle, AccessControlSections includeSections) : base(isContainer, resourceType, safeHandle, includeSections)
		{
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x000C0E60 File Offset: 0x000BF060
		protected ObjectSecurity(bool isContainer, ResourceType resourceType, string name, AccessControlSections includeSections) : base(isContainer, resourceType, name, includeSections)
		{
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x000C0E6D File Offset: 0x000BF06D
		protected ObjectSecurity(bool isContainer, ResourceType resourceType, SafeHandle safeHandle, AccessControlSections includeSections, NativeObjectSecurity.ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext) : base(isContainer, resourceType, safeHandle, includeSections, exceptionFromErrorCode, exceptionContext)
		{
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x000C0E7E File Offset: 0x000BF07E
		protected ObjectSecurity(bool isContainer, ResourceType resourceType, string name, AccessControlSections includeSections, NativeObjectSecurity.ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext) : base(isContainer, resourceType, name, includeSections, exceptionFromErrorCode, exceptionContext)
		{
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x0600354B RID: 13643 RVA: 0x000C0E8F File Offset: 0x000BF08F
		public override Type AccessRightType
		{
			get
			{
				return typeof(T);
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x0600354C RID: 13644 RVA: 0x000C0E9B File Offset: 0x000BF09B
		public override Type AccessRuleType
		{
			get
			{
				return typeof(AccessRule<T>);
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x0600354D RID: 13645 RVA: 0x000C0EA7 File Offset: 0x000BF0A7
		public override Type AuditRuleType
		{
			get
			{
				return typeof(AuditRule<T>);
			}
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x000C0EB3 File Offset: 0x000BF0B3
		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new AccessRule<T>(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, type);
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x000BC650 File Offset: 0x000BA850
		public virtual void AddAccessRule(AccessRule<T> rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x000BC66B File Offset: 0x000BA86B
		public virtual bool RemoveAccessRule(AccessRule<T> rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x000BC674 File Offset: 0x000BA874
		public virtual void RemoveAccessRuleAll(AccessRule<T> rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x000BC67D File Offset: 0x000BA87D
		public virtual void RemoveAccessRuleSpecific(AccessRule<T> rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x000BC662 File Offset: 0x000BA862
		public virtual void ResetAccessRule(AccessRule<T> rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x000BC659 File Offset: 0x000BA859
		public virtual void SetAccessRule(AccessRule<T> rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x000C0EC3 File Offset: 0x000BF0C3
		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new AuditRule<T>(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x000BC686 File Offset: 0x000BA886
		public virtual void AddAuditRule(AuditRule<T> rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x000BC698 File Offset: 0x000BA898
		public virtual bool RemoveAuditRule(AuditRule<T> rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x000BC6A1 File Offset: 0x000BA8A1
		public virtual void RemoveAuditRuleAll(AuditRule<T> rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x000BC6AA File Offset: 0x000BA8AA
		public virtual void RemoveAuditRuleSpecific(AuditRule<T> rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x000BC68F File Offset: 0x000BA88F
		public virtual void SetAuditRule(AuditRule<T> rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x000C0ED4 File Offset: 0x000BF0D4
		protected void Persist(SafeHandle handle)
		{
			base.WriteLock();
			try
			{
				this.Persist(handle, base.AccessControlSectionsModified);
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x000C0F10 File Offset: 0x000BF110
		protected void Persist(string name)
		{
			base.WriteLock();
			try
			{
				this.Persist(name, base.AccessControlSectionsModified);
			}
			finally
			{
				base.WriteUnlock();
			}
		}
	}
}
