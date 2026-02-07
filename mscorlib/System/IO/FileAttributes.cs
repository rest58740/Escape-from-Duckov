using System;

namespace System.IO
{
	// Token: 0x02000B49 RID: 2889
	[Flags]
	public enum FileAttributes
	{
		// Token: 0x04003CC1 RID: 15553
		ReadOnly = 1,
		// Token: 0x04003CC2 RID: 15554
		Hidden = 2,
		// Token: 0x04003CC3 RID: 15555
		System = 4,
		// Token: 0x04003CC4 RID: 15556
		Directory = 16,
		// Token: 0x04003CC5 RID: 15557
		Archive = 32,
		// Token: 0x04003CC6 RID: 15558
		Device = 64,
		// Token: 0x04003CC7 RID: 15559
		Normal = 128,
		// Token: 0x04003CC8 RID: 15560
		Temporary = 256,
		// Token: 0x04003CC9 RID: 15561
		SparseFile = 512,
		// Token: 0x04003CCA RID: 15562
		ReparsePoint = 1024,
		// Token: 0x04003CCB RID: 15563
		Compressed = 2048,
		// Token: 0x04003CCC RID: 15564
		Offline = 4096,
		// Token: 0x04003CCD RID: 15565
		NotContentIndexed = 8192,
		// Token: 0x04003CCE RID: 15566
		Encrypted = 16384,
		// Token: 0x04003CCF RID: 15567
		IntegrityStream = 32768,
		// Token: 0x04003CD0 RID: 15568
		NoScrubData = 131072
	}
}
