using System;

namespace System.Diagnostics.Contracts
{
	// Token: 0x020009C5 RID: 2501
	[Conditional("CONTRACTS_FULL")]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	[Conditional("DEBUG")]
	public sealed class ContractClassAttribute : Attribute
	{
		// Token: 0x060059FC RID: 23036 RVA: 0x00133D17 File Offset: 0x00131F17
		public ContractClassAttribute(Type typeContainingContracts)
		{
			this._typeWithContracts = typeContainingContracts;
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x060059FD RID: 23037 RVA: 0x00133D26 File Offset: 0x00131F26
		public Type TypeContainingContracts
		{
			get
			{
				return this._typeWithContracts;
			}
		}

		// Token: 0x040037A3 RID: 14243
		private Type _typeWithContracts;
	}
}
