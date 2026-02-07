using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020009DD RID: 2525
	[ComVisible(true)]
	public interface ISymbolVariable
	{
		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x06005A6F RID: 23151
		int AddressField1 { get; }

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x06005A70 RID: 23152
		int AddressField2 { get; }

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06005A71 RID: 23153
		int AddressField3 { get; }

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06005A72 RID: 23154
		SymAddressKind AddressKind { get; }

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x06005A73 RID: 23155
		object Attributes { get; }

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x06005A74 RID: 23156
		int EndOffset { get; }

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x06005A75 RID: 23157
		string Name { get; }

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x06005A76 RID: 23158
		int StartOffset { get; }

		// Token: 0x06005A77 RID: 23159
		byte[] GetSignature();
	}
}
