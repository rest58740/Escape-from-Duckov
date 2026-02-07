using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x02000402 RID: 1026
	[ComVisible(true)]
	public static class ApplicationSecurityManager
	{
		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x060029ED RID: 10733 RVA: 0x000981B5 File Offset: 0x000963B5
		public static IApplicationTrustManager ApplicationTrustManager
		{
			[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
			get
			{
				if (ApplicationSecurityManager._appTrustManager == null)
				{
					ApplicationSecurityManager._appTrustManager = new MonoTrustManager();
				}
				return ApplicationSecurityManager._appTrustManager;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x060029EE RID: 10734 RVA: 0x000981CD File Offset: 0x000963CD
		public static ApplicationTrustCollection UserApplicationTrusts
		{
			get
			{
				if (ApplicationSecurityManager._userAppTrusts == null)
				{
					ApplicationSecurityManager._userAppTrusts = new ApplicationTrustCollection();
				}
				return ApplicationSecurityManager._userAppTrusts;
			}
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x000981E5 File Offset: 0x000963E5
		[MonoTODO("Missing application manifest support")]
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true, ControlEvidence = true)]
		public static bool DetermineApplicationTrust(ActivationContext activationContext, TrustManagerContext context)
		{
			if (activationContext == null)
			{
				throw new NullReferenceException("activationContext");
			}
			return ApplicationSecurityManager.ApplicationTrustManager.DetermineApplicationTrust(activationContext, context).IsApplicationTrustedToRun;
		}

		// Token: 0x04001F59 RID: 8025
		private static IApplicationTrustManager _appTrustManager;

		// Token: 0x04001F5A RID: 8026
		private static ApplicationTrustCollection _userAppTrusts;
	}
}
