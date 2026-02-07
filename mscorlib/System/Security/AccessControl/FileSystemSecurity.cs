using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Unity;

namespace System.Security.AccessControl
{
	// Token: 0x0200052B RID: 1323
	public abstract class FileSystemSecurity : NativeObjectSecurity
	{
		// Token: 0x06003445 RID: 13381 RVA: 0x000BEB03 File Offset: 0x000BCD03
		internal FileSystemSecurity(bool isContainer) : base(isContainer, ResourceType.FileObject)
		{
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x000BEB0D File Offset: 0x000BCD0D
		internal FileSystemSecurity(bool isContainer, string name, AccessControlSections includeSections) : base(isContainer, ResourceType.FileObject, name, includeSections)
		{
		}

		// Token: 0x06003447 RID: 13383 RVA: 0x000BEB19 File Offset: 0x000BCD19
		internal FileSystemSecurity(bool isContainer, SafeHandle handle, AccessControlSections includeSections) : base(isContainer, ResourceType.FileObject, handle, includeSections)
		{
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06003448 RID: 13384 RVA: 0x000BEB25 File Offset: 0x000BCD25
		public override Type AccessRightType
		{
			get
			{
				return typeof(FileSystemRights);
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06003449 RID: 13385 RVA: 0x000BEB31 File Offset: 0x000BCD31
		public override Type AccessRuleType
		{
			get
			{
				return typeof(FileSystemAccessRule);
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x0600344A RID: 13386 RVA: 0x000BEB3D File Offset: 0x000BCD3D
		public override Type AuditRuleType
		{
			get
			{
				return typeof(FileSystemAuditRule);
			}
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x000BEB49 File Offset: 0x000BCD49
		public sealed override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new FileSystemAccessRule(identityReference, (FileSystemRights)accessMask, isInherited, inheritanceFlags, propagationFlags, type);
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x000BC650 File Offset: 0x000BA850
		public void AddAccessRule(FileSystemAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x000BC66B File Offset: 0x000BA86B
		public bool RemoveAccessRule(FileSystemAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x000BC674 File Offset: 0x000BA874
		public void RemoveAccessRuleAll(FileSystemAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x000BC67D File Offset: 0x000BA87D
		public void RemoveAccessRuleSpecific(FileSystemAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x000BC662 File Offset: 0x000BA862
		public void ResetAccessRule(FileSystemAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x000BC659 File Offset: 0x000BA859
		public void SetAccessRule(FileSystemAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x000BEB59 File Offset: 0x000BCD59
		public sealed override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new FileSystemAuditRule(identityReference, (FileSystemRights)accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x000BC686 File Offset: 0x000BA886
		public void AddAuditRule(FileSystemAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x06003454 RID: 13396 RVA: 0x000BC698 File Offset: 0x000BA898
		public bool RemoveAuditRule(FileSystemAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x000BC6A1 File Offset: 0x000BA8A1
		public void RemoveAuditRuleAll(FileSystemAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x06003456 RID: 13398 RVA: 0x000BC6AA File Offset: 0x000BA8AA
		public void RemoveAuditRuleSpecific(FileSystemAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x06003457 RID: 13399 RVA: 0x000BC68F File Offset: 0x000BA88F
		public void SetAuditRule(FileSystemAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x06003458 RID: 13400 RVA: 0x000173AD File Offset: 0x000155AD
		internal FileSystemSecurity()
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}
}
