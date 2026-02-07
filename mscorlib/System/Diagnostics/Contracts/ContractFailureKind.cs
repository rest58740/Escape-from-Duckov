using System;

namespace System.Diagnostics.Contracts
{
	// Token: 0x020009D0 RID: 2512
	public enum ContractFailureKind
	{
		// Token: 0x040037AD RID: 14253
		Precondition,
		// Token: 0x040037AE RID: 14254
		Postcondition,
		// Token: 0x040037AF RID: 14255
		PostconditionOnException,
		// Token: 0x040037B0 RID: 14256
		Invariant,
		// Token: 0x040037B1 RID: 14257
		Assert,
		// Token: 0x040037B2 RID: 14258
		Assume
	}
}
