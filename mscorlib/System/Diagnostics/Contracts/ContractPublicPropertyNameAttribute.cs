using System;

namespace System.Diagnostics.Contracts
{
	// Token: 0x020009CB RID: 2507
	[Conditional("CONTRACTS_FULL")]
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class ContractPublicPropertyNameAttribute : Attribute
	{
		// Token: 0x06005A05 RID: 23045 RVA: 0x00133D5C File Offset: 0x00131F5C
		public ContractPublicPropertyNameAttribute(string name)
		{
			this._publicName = name;
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x06005A06 RID: 23046 RVA: 0x00133D6B File Offset: 0x00131F6B
		public string Name
		{
			get
			{
				return this._publicName;
			}
		}

		// Token: 0x040037A6 RID: 14246
		private string _publicName;
	}
}
