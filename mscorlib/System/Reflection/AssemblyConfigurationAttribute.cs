using System;

namespace System.Reflection
{
	// Token: 0x0200087F RID: 2175
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyConfigurationAttribute : Attribute
	{
		// Token: 0x06004853 RID: 18515 RVA: 0x000EE013 File Offset: 0x000EC213
		public AssemblyConfigurationAttribute(string configuration)
		{
			this.Configuration = configuration;
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06004854 RID: 18516 RVA: 0x000EE022 File Offset: 0x000EC222
		public string Configuration { get; }
	}
}
