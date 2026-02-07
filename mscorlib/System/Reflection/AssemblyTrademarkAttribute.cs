using System;

namespace System.Reflection
{
	// Token: 0x02000890 RID: 2192
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyTrademarkAttribute : Attribute
	{
		// Token: 0x06004876 RID: 18550 RVA: 0x000EE198 File Offset: 0x000EC398
		public AssemblyTrademarkAttribute(string trademark)
		{
			this.Trademark = trademark;
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06004877 RID: 18551 RVA: 0x000EE1A7 File Offset: 0x000EC3A7
		public string Trademark { get; }
	}
}
