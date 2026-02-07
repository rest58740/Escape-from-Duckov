using System;

namespace System.IO
{
	// Token: 0x02000B0A RID: 2826
	[Flags]
	public enum FileShare
	{
		// Token: 0x04003B45 RID: 15173
		None = 0,
		// Token: 0x04003B46 RID: 15174
		Read = 1,
		// Token: 0x04003B47 RID: 15175
		Write = 2,
		// Token: 0x04003B48 RID: 15176
		ReadWrite = 3,
		// Token: 0x04003B49 RID: 15177
		Delete = 4,
		// Token: 0x04003B4A RID: 15178
		Inheritable = 16
	}
}
