using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020009DF RID: 2527
	[ComVisible(true)]
	[Serializable]
	public enum SymAddressKind
	{
		// Token: 0x040037BF RID: 14271
		ILOffset = 1,
		// Token: 0x040037C0 RID: 14272
		NativeRVA,
		// Token: 0x040037C1 RID: 14273
		NativeRegister,
		// Token: 0x040037C2 RID: 14274
		NativeRegisterRelative,
		// Token: 0x040037C3 RID: 14275
		NativeOffset,
		// Token: 0x040037C4 RID: 14276
		NativeRegisterRegister,
		// Token: 0x040037C5 RID: 14277
		NativeRegisterStack,
		// Token: 0x040037C6 RID: 14278
		NativeStackRegister,
		// Token: 0x040037C7 RID: 14279
		BitField,
		// Token: 0x040037C8 RID: 14280
		NativeSectionOffset
	}
}
