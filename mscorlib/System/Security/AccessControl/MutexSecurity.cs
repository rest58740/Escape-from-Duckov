using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace System.Security.AccessControl
{
	// Token: 0x02000534 RID: 1332
	public sealed class MutexSecurity : NativeObjectSecurity
	{
		// Token: 0x060034A8 RID: 13480 RVA: 0x000BEA07 File Offset: 0x000BCC07
		public MutexSecurity() : base(false, ResourceType.KernelObject)
		{
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x000BF96E File Offset: 0x000BDB6E
		public MutexSecurity(string name, AccessControlSections includeSections) : base(false, ResourceType.KernelObject, name, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(MutexSecurity.MutexExceptionFromErrorCode), null)
		{
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x000BF987 File Offset: 0x000BDB87
		internal MutexSecurity(SafeHandle handle, AccessControlSections includeSections) : base(false, ResourceType.KernelObject, handle, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(MutexSecurity.MutexExceptionFromErrorCode), null)
		{
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x060034AB RID: 13483 RVA: 0x000BF9A0 File Offset: 0x000BDBA0
		public override Type AccessRightType
		{
			get
			{
				return typeof(MutexRights);
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060034AC RID: 13484 RVA: 0x000BF9AC File Offset: 0x000BDBAC
		public override Type AccessRuleType
		{
			get
			{
				return typeof(MutexAccessRule);
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060034AD RID: 13485 RVA: 0x000BF9B8 File Offset: 0x000BDBB8
		public override Type AuditRuleType
		{
			get
			{
				return typeof(MutexAuditRule);
			}
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x000BF9C4 File Offset: 0x000BDBC4
		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new MutexAccessRule(identityReference, (MutexRights)accessMask, type);
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x000BC650 File Offset: 0x000BA850
		public void AddAccessRule(MutexAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000BC66B File Offset: 0x000BA86B
		public bool RemoveAccessRule(MutexAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000BC674 File Offset: 0x000BA874
		public void RemoveAccessRuleAll(MutexAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x000BC67D File Offset: 0x000BA87D
		public void RemoveAccessRuleSpecific(MutexAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x000BC662 File Offset: 0x000BA862
		public void ResetAccessRule(MutexAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x000BC659 File Offset: 0x000BA859
		public void SetAccessRule(MutexAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x000BF9CF File Offset: 0x000BDBCF
		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new MutexAuditRule(identityReference, (MutexRights)accessMask, flags);
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x000BC686 File Offset: 0x000BA886
		public void AddAuditRule(MutexAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x000BC698 File Offset: 0x000BA898
		public bool RemoveAuditRule(MutexAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x000BC6A1 File Offset: 0x000BA8A1
		public void RemoveAuditRuleAll(MutexAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x000BC6AA File Offset: 0x000BA8AA
		public void RemoveAuditRuleSpecific(MutexAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x000BC68F File Offset: 0x000BA88F
		public void SetAuditRule(MutexAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x000BF9DA File Offset: 0x000BDBDA
		private static Exception MutexExceptionFromErrorCode(int errorCode, string name, SafeHandle handle, object context)
		{
			if (errorCode == 2)
			{
				return new WaitHandleCannotBeOpenedException();
			}
			return NativeObjectSecurity.DefaultExceptionFromErrorCode(errorCode, name, handle, context);
		}
	}
}
