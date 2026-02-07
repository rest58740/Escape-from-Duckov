using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000704 RID: 1796
	[Flags]
	public enum DllImportSearchPath
	{
		// Token: 0x04002AC9 RID: 10953
		UseDllDirectoryForDependencies = 256,
		// Token: 0x04002ACA RID: 10954
		ApplicationDirectory = 512,
		// Token: 0x04002ACB RID: 10955
		UserDirectories = 1024,
		// Token: 0x04002ACC RID: 10956
		System32 = 2048,
		// Token: 0x04002ACD RID: 10957
		SafeDirectories = 4096,
		// Token: 0x04002ACE RID: 10958
		AssemblyDirectory = 2,
		// Token: 0x04002ACF RID: 10959
		LegacyBehavior = 0
	}
}
