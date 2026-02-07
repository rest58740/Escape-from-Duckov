using System;

namespace System.Reflection
{
	// Token: 0x02000888 RID: 2184
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyInformationalVersionAttribute : Attribute
	{
		// Token: 0x06004866 RID: 18534 RVA: 0x000EE0D9 File Offset: 0x000EC2D9
		public AssemblyInformationalVersionAttribute(string informationalVersion)
		{
			this.InformationalVersion = informationalVersion;
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06004867 RID: 18535 RVA: 0x000EE0E8 File Offset: 0x000EC2E8
		public string InformationalVersion { get; }
	}
}
