using System;

namespace System.Reflection
{
	// Token: 0x0200088C RID: 2188
	[Flags]
	public enum AssemblyNameFlags
	{
		// Token: 0x04002E5C RID: 11868
		None = 0,
		// Token: 0x04002E5D RID: 11869
		PublicKey = 1,
		// Token: 0x04002E5E RID: 11870
		EnableJITcompileOptimizer = 16384,
		// Token: 0x04002E5F RID: 11871
		EnableJITcompileTracking = 32768,
		// Token: 0x04002E60 RID: 11872
		Retargetable = 256
	}
}
