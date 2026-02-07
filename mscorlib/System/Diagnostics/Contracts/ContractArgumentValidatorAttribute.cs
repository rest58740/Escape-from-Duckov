using System;

namespace System.Diagnostics.Contracts
{
	// Token: 0x020009CC RID: 2508
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	[Conditional("CONTRACTS_FULL")]
	public sealed class ContractArgumentValidatorAttribute : Attribute
	{
	}
}
