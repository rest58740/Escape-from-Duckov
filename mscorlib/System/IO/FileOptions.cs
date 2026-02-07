using System;

namespace System.IO
{
	// Token: 0x02000B09 RID: 2825
	[Flags]
	public enum FileOptions
	{
		// Token: 0x04003B3D RID: 15165
		None = 0,
		// Token: 0x04003B3E RID: 15166
		WriteThrough = -2147483648,
		// Token: 0x04003B3F RID: 15167
		Asynchronous = 1073741824,
		// Token: 0x04003B40 RID: 15168
		RandomAccess = 268435456,
		// Token: 0x04003B41 RID: 15169
		DeleteOnClose = 67108864,
		// Token: 0x04003B42 RID: 15170
		SequentialScan = 134217728,
		// Token: 0x04003B43 RID: 15171
		Encrypted = 16384
	}
}
