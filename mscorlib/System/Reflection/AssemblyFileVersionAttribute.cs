using System;

namespace System.Reflection
{
	// Token: 0x02000886 RID: 2182
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyFileVersionAttribute : Attribute
	{
		// Token: 0x0600485F RID: 18527 RVA: 0x000EE09D File Offset: 0x000EC29D
		public AssemblyFileVersionAttribute(string version)
		{
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this.Version = version;
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06004860 RID: 18528 RVA: 0x000EE0BA File Offset: 0x000EC2BA
		public string Version { get; }
	}
}
