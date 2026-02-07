using System;
using System.Reflection;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;

namespace System
{
	// Token: 0x02000226 RID: 550
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Infrastructure = true)]
	[SecurityPermission(SecurityAction.LinkDemand, Infrastructure = true)]
	public class AppDomainManager : MarshalByRefObject
	{
		// Token: 0x06001881 RID: 6273 RVA: 0x0005D91C File Offset: 0x0005BB1C
		public AppDomainManager()
		{
			this._flags = AppDomainManagerInitializationOptions.None;
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06001882 RID: 6274 RVA: 0x0005D92B File Offset: 0x0005BB2B
		public virtual ApplicationActivator ApplicationActivator
		{
			get
			{
				if (this._activator == null)
				{
					this._activator = new ApplicationActivator();
				}
				return this._activator;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06001883 RID: 6275 RVA: 0x0005D946 File Offset: 0x0005BB46
		public virtual Assembly EntryAssembly
		{
			get
			{
				return Assembly.GetEntryAssembly();
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06001884 RID: 6276 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public virtual HostExecutionContextManager HostExecutionContextManager
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06001885 RID: 6277 RVA: 0x0000AF5E File Offset: 0x0000915E
		public virtual HostSecurityManager HostSecurityManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06001886 RID: 6278 RVA: 0x0005D94D File Offset: 0x0005BB4D
		// (set) Token: 0x06001887 RID: 6279 RVA: 0x0005D955 File Offset: 0x0005BB55
		public AppDomainManagerInitializationOptions InitializationFlags
		{
			get
			{
				return this._flags;
			}
			set
			{
				this._flags = value;
			}
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x0005D960 File Offset: 0x0005BB60
		public virtual AppDomain CreateDomain(string friendlyName, Evidence securityInfo, AppDomainSetup appDomainInfo)
		{
			this.InitializeNewDomain(appDomainInfo);
			AppDomain appDomain = AppDomainManager.CreateDomainHelper(friendlyName, securityInfo, appDomainInfo);
			if ((this.HostSecurityManager.Flags & HostSecurityManagerOptions.HostPolicyLevel) == HostSecurityManagerOptions.HostPolicyLevel)
			{
				PolicyLevel domainPolicy = this.HostSecurityManager.DomainPolicy;
				if (domainPolicy != null)
				{
					appDomain.SetAppDomainPolicy(domainPolicy);
				}
			}
			return appDomain;
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public virtual void InitializeNewDomain(AppDomainSetup appDomainInfo)
		{
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool CheckSecuritySettings(SecurityState state)
		{
			return false;
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x0005D9A4 File Offset: 0x0005BBA4
		protected static AppDomain CreateDomainHelper(string friendlyName, Evidence securityInfo, AppDomainSetup appDomainInfo)
		{
			return AppDomain.CreateDomain(friendlyName, securityInfo, appDomainInfo);
		}

		// Token: 0x040016B9 RID: 5817
		private ApplicationActivator _activator;

		// Token: 0x040016BA RID: 5818
		private AppDomainManagerInitializationOptions _flags;
	}
}
