using System;

namespace System.Reflection
{
	// Token: 0x0200088D RID: 2189
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyProductAttribute : Attribute
	{
		// Token: 0x0600486F RID: 18543 RVA: 0x000EE144 File Offset: 0x000EC344
		public AssemblyProductAttribute(string product)
		{
			this.Product = product;
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06004870 RID: 18544 RVA: 0x000EE153 File Offset: 0x000EC353
		public string Product { get; }
	}
}
