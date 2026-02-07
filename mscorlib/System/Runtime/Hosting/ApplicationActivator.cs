using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Policy;

namespace System.Runtime.Hosting
{
	// Token: 0x02000556 RID: 1366
	[ComVisible(true)]
	[MonoTODO("missing manifest support")]
	public class ApplicationActivator
	{
		// Token: 0x060035C9 RID: 13769 RVA: 0x000C212C File Offset: 0x000C032C
		public virtual ObjectHandle CreateInstance(ActivationContext activationContext)
		{
			return this.CreateInstance(activationContext, null);
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x000C2136 File Offset: 0x000C0336
		public virtual ObjectHandle CreateInstance(ActivationContext activationContext, string[] activationCustomData)
		{
			if (activationContext == null)
			{
				throw new ArgumentNullException("activationContext");
			}
			return ApplicationActivator.CreateInstanceHelper(new AppDomainSetup(activationContext));
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x000C2154 File Offset: 0x000C0354
		protected static ObjectHandle CreateInstanceHelper(AppDomainSetup adSetup)
		{
			if (adSetup == null)
			{
				throw new ArgumentNullException("adSetup");
			}
			if (adSetup.ActivationArguments == null)
			{
				throw new ArgumentException(string.Format(Locale.GetText("{0} is missing it's {1} property"), "AppDomainSetup", "ActivationArguments"), "adSetup");
			}
			HostSecurityManager hostSecurityManager;
			if (AppDomain.CurrentDomain.DomainManager != null)
			{
				hostSecurityManager = AppDomain.CurrentDomain.DomainManager.HostSecurityManager;
			}
			else
			{
				hostSecurityManager = new HostSecurityManager();
			}
			Evidence evidence = new Evidence();
			evidence.AddHost(adSetup.ActivationArguments);
			TrustManagerContext context = new TrustManagerContext();
			if (!hostSecurityManager.DetermineApplicationTrust(evidence, null, context).IsApplicationTrustedToRun)
			{
				throw new PolicyException(Locale.GetText("Current policy doesn't allow execution of addin."));
			}
			return AppDomain.CreateDomain("friendlyName", null, adSetup).CreateInstance("assemblyName", "typeName", null);
		}
	}
}
