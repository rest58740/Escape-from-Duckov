using System;

namespace Microsoft.Win32
{
	// Token: 0x020000A7 RID: 167
	public enum RegistryValueKind
	{
		// Token: 0x04000F72 RID: 3954
		String = 1,
		// Token: 0x04000F73 RID: 3955
		ExpandString,
		// Token: 0x04000F74 RID: 3956
		Binary,
		// Token: 0x04000F75 RID: 3957
		DWord,
		// Token: 0x04000F76 RID: 3958
		MultiString = 7,
		// Token: 0x04000F77 RID: 3959
		QWord = 11,
		// Token: 0x04000F78 RID: 3960
		Unknown = 0,
		// Token: 0x04000F79 RID: 3961
		None = -1
	}
}
