using System;

namespace System.Reflection
{
	// Token: 0x02000883 RID: 2179
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyDefaultAliasAttribute : Attribute
	{
		// Token: 0x06004859 RID: 18521 RVA: 0x000EE058 File Offset: 0x000EC258
		public AssemblyDefaultAliasAttribute(string defaultAlias)
		{
			this.DefaultAlias = defaultAlias;
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x0600485A RID: 18522 RVA: 0x000EE067 File Offset: 0x000EC267
		public string DefaultAlias { get; }
	}
}
