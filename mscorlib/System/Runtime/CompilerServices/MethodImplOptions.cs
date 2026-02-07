using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000843 RID: 2115
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum MethodImplOptions
	{
		// Token: 0x04002D7F RID: 11647
		Unmanaged = 4,
		// Token: 0x04002D80 RID: 11648
		ForwardRef = 16,
		// Token: 0x04002D81 RID: 11649
		PreserveSig = 128,
		// Token: 0x04002D82 RID: 11650
		InternalCall = 4096,
		// Token: 0x04002D83 RID: 11651
		Synchronized = 32,
		// Token: 0x04002D84 RID: 11652
		NoInlining = 8,
		// Token: 0x04002D85 RID: 11653
		[ComVisible(false)]
		AggressiveInlining = 256,
		// Token: 0x04002D86 RID: 11654
		NoOptimization = 64,
		// Token: 0x04002D87 RID: 11655
		SecurityMitigations = 1024
	}
}
