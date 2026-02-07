using System;

namespace System.Diagnostics.Contracts
{
	// Token: 0x020009C6 RID: 2502
	[Conditional("CONTRACTS_FULL")]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ContractClassForAttribute : Attribute
	{
		// Token: 0x060059FE RID: 23038 RVA: 0x00133D2E File Offset: 0x00131F2E
		public ContractClassForAttribute(Type typeContractsAreFor)
		{
			this._typeIAmAContractFor = typeContractsAreFor;
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x060059FF RID: 23039 RVA: 0x00133D3D File Offset: 0x00131F3D
		public Type TypeContractsAreFor
		{
			get
			{
				return this._typeIAmAContractFor;
			}
		}

		// Token: 0x040037A4 RID: 14244
		private Type _typeIAmAContractFor;
	}
}
