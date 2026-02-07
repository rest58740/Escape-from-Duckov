using System;
using System.Reflection;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Security.Policy;
using Unity;

namespace System.Security
{
	// Token: 0x020003DF RID: 991
	[ComVisible(true)]
	[Serializable]
	public class HostSecurityManager
	{
		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x0000AF5E File Offset: 0x0000915E
		public virtual PolicyLevel DomainPolicy
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060028AD RID: 10413 RVA: 0x0009330F File Offset: 0x0009150F
		public virtual HostSecurityManagerOptions Flags
		{
			get
			{
				return HostSecurityManagerOptions.AllFlags;
			}
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x00093314 File Offset: 0x00091514
		public virtual ApplicationTrust DetermineApplicationTrust(Evidence applicationEvidence, Evidence activatorEvidence, TrustManagerContext context)
		{
			if (applicationEvidence == null)
			{
				throw new ArgumentNullException("applicationEvidence");
			}
			ActivationArguments activationArguments = null;
			foreach (object obj in applicationEvidence)
			{
				activationArguments = (obj as ActivationArguments);
				if (activationArguments != null)
				{
					break;
				}
			}
			if (activationArguments == null)
			{
				throw new ArgumentException(string.Format(Locale.GetText("No {0} found in {1}."), "ActivationArguments", "Evidence"), "applicationEvidence");
			}
			if (activationArguments.ActivationContext == null)
			{
				throw new ArgumentException(string.Format(Locale.GetText("No {0} found in {1}."), "ActivationContext", "ActivationArguments"), "applicationEvidence");
			}
			if (!ApplicationSecurityManager.DetermineApplicationTrust(activationArguments.ActivationContext, context))
			{
				return null;
			}
			if (activationArguments.ApplicationIdentity == null)
			{
				return new ApplicationTrust();
			}
			return new ApplicationTrust(activationArguments.ApplicationIdentity);
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x00002731 File Offset: 0x00000931
		public virtual Evidence ProvideAppDomainEvidence(Evidence inputEvidence)
		{
			return inputEvidence;
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x0008869B File Offset: 0x0008689B
		public virtual Evidence ProvideAssemblyEvidence(Assembly loadedAssembly, Evidence inputEvidence)
		{
			return inputEvidence;
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000933F4 File Offset: 0x000915F4
		public virtual PermissionSet ResolvePolicy(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new NullReferenceException("evidence");
			}
			return SecurityManager.ResolvePolicy(evidence);
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x00052959 File Offset: 0x00050B59
		public virtual EvidenceBase GenerateAppDomainEvidence(Type evidenceType)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x00052959 File Offset: 0x00050B59
		public virtual EvidenceBase GenerateAssemblyEvidence(Type evidenceType, Assembly assembly)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x00052959 File Offset: 0x00050B59
		public virtual Type[] GetHostSuppliedAppDomainEvidenceTypes()
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x00052959 File Offset: 0x00050B59
		public virtual Type[] GetHostSuppliedAssemblyEvidenceTypes(Assembly assembly)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}
}
