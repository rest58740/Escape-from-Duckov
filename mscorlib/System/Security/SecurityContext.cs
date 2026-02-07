using System;
using System.Security.Permissions;
using System.Threading;

namespace System.Security
{
	// Token: 0x020003DC RID: 988
	public sealed class SecurityContext : IDisposable
	{
		// Token: 0x06002881 RID: 10369 RVA: 0x0000259F File Offset: 0x0000079F
		private SecurityContext()
		{
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x0000270D File Offset: 0x0000090D
		public SecurityContext CreateCopy()
		{
			return this;
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x00093074 File Offset: 0x00091274
		public static SecurityContext Capture()
		{
			return new SecurityContext();
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void Dispose()
		{
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public static bool IsFlowSuppressed()
		{
			return false;
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public static bool IsWindowsIdentityFlowSuppressed()
		{
			return false;
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public static void RestoreFlow()
		{
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x0009307B File Offset: 0x0009127B
		[SecurityPermission(SecurityAction.Assert, ControlPrincipal = true)]
		[SecurityPermission(SecurityAction.LinkDemand, Infrastructure = true)]
		public static void Run(SecurityContext securityContext, ContextCallback callback, object state)
		{
			callback(state);
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x000472CC File Offset: 0x000454CC
		[SecurityPermission(SecurityAction.LinkDemand, Infrastructure = true)]
		public static AsyncFlowControl SuppressFlow()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x000472CC File Offset: 0x000454CC
		public static AsyncFlowControl SuppressFlowWindowsIdentity()
		{
			throw new NotSupportedException();
		}
	}
}
