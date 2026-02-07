using System;

namespace System.Reflection
{
	// Token: 0x020008AE RID: 2222
	public enum MethodImplAttributes
	{
		// Token: 0x04002EDE RID: 11998
		CodeTypeMask = 3,
		// Token: 0x04002EDF RID: 11999
		IL = 0,
		// Token: 0x04002EE0 RID: 12000
		Native,
		// Token: 0x04002EE1 RID: 12001
		OPTIL,
		// Token: 0x04002EE2 RID: 12002
		Runtime,
		// Token: 0x04002EE3 RID: 12003
		ManagedMask,
		// Token: 0x04002EE4 RID: 12004
		Unmanaged = 4,
		// Token: 0x04002EE5 RID: 12005
		Managed = 0,
		// Token: 0x04002EE6 RID: 12006
		ForwardRef = 16,
		// Token: 0x04002EE7 RID: 12007
		PreserveSig = 128,
		// Token: 0x04002EE8 RID: 12008
		InternalCall = 4096,
		// Token: 0x04002EE9 RID: 12009
		Synchronized = 32,
		// Token: 0x04002EEA RID: 12010
		NoInlining = 8,
		// Token: 0x04002EEB RID: 12011
		AggressiveInlining = 256,
		// Token: 0x04002EEC RID: 12012
		NoOptimization = 64,
		// Token: 0x04002EED RID: 12013
		MaxMethodImplVal = 65535,
		// Token: 0x04002EEE RID: 12014
		SecurityMitigations = 1024
	}
}
