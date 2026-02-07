using System;

namespace System.IO.IsolatedStorage
{
	// Token: 0x02000B70 RID: 2928
	[Flags]
	public enum IsolatedStorageScope
	{
		// Token: 0x04003D8E RID: 15758
		None = 0,
		// Token: 0x04003D8F RID: 15759
		User = 1,
		// Token: 0x04003D90 RID: 15760
		Domain = 2,
		// Token: 0x04003D91 RID: 15761
		Assembly = 4,
		// Token: 0x04003D92 RID: 15762
		Roaming = 8,
		// Token: 0x04003D93 RID: 15763
		Machine = 16,
		// Token: 0x04003D94 RID: 15764
		Application = 32
	}
}
