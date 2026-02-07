using System;

namespace System.Reflection
{
	// Token: 0x020008B9 RID: 2233
	[Flags]
	public enum PortableExecutableKinds
	{
		// Token: 0x04002F10 RID: 12048
		NotAPortableExecutableImage = 0,
		// Token: 0x04002F11 RID: 12049
		ILOnly = 1,
		// Token: 0x04002F12 RID: 12050
		Required32Bit = 2,
		// Token: 0x04002F13 RID: 12051
		PE32Plus = 4,
		// Token: 0x04002F14 RID: 12052
		Unmanaged32Bit = 8,
		// Token: 0x04002F15 RID: 12053
		Preferred32Bit = 16
	}
}
