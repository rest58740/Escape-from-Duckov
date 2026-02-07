using System;

namespace System.Diagnostics.Contracts
{
	// Token: 0x020009CA RID: 2506
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
	[Conditional("CONTRACTS_FULL")]
	public sealed class ContractVerificationAttribute : Attribute
	{
		// Token: 0x06005A03 RID: 23043 RVA: 0x00133D45 File Offset: 0x00131F45
		public ContractVerificationAttribute(bool value)
		{
			this._value = value;
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x06005A04 RID: 23044 RVA: 0x00133D54 File Offset: 0x00131F54
		public bool Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x040037A5 RID: 14245
		private bool _value;
	}
}
