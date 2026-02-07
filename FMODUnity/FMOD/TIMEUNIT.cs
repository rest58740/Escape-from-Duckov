using System;

namespace FMOD
{
	// Token: 0x0200003B RID: 59
	[Flags]
	public enum TIMEUNIT : uint
	{
		// Token: 0x040001B4 RID: 436
		MS = 1U,
		// Token: 0x040001B5 RID: 437
		PCM = 2U,
		// Token: 0x040001B6 RID: 438
		PCMBYTES = 4U,
		// Token: 0x040001B7 RID: 439
		RAWBYTES = 8U,
		// Token: 0x040001B8 RID: 440
		PCMFRACTION = 16U,
		// Token: 0x040001B9 RID: 441
		MODORDER = 256U,
		// Token: 0x040001BA RID: 442
		MODROW = 512U,
		// Token: 0x040001BB RID: 443
		MODPATTERN = 1024U
	}
}
