using System;

namespace System.Reflection
{
	// Token: 0x0200087E RID: 2174
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyCompanyAttribute : Attribute
	{
		// Token: 0x06004851 RID: 18513 RVA: 0x000EDFFC File Offset: 0x000EC1FC
		public AssemblyCompanyAttribute(string company)
		{
			this.Company = company;
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06004852 RID: 18514 RVA: 0x000EE00B File Offset: 0x000EC20B
		public string Company { get; }
	}
}
