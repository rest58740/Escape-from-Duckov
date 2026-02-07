using System;

namespace System.Reflection
{
	// Token: 0x02000891 RID: 2193
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyVersionAttribute : Attribute
	{
		// Token: 0x06004878 RID: 18552 RVA: 0x000EE1AF File Offset: 0x000EC3AF
		public AssemblyVersionAttribute(string version)
		{
			this.Version = version;
		}

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06004879 RID: 18553 RVA: 0x000EE1BE File Offset: 0x000EC3BE
		public string Version { get; }
	}
}
