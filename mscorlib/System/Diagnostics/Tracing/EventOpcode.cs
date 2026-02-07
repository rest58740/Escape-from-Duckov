using System;

namespace System.Diagnostics.Tracing
{
	// Token: 0x020009E9 RID: 2537
	public enum EventOpcode
	{
		// Token: 0x040037F2 RID: 14322
		Info,
		// Token: 0x040037F3 RID: 14323
		Start,
		// Token: 0x040037F4 RID: 14324
		Stop,
		// Token: 0x040037F5 RID: 14325
		DataCollectionStart,
		// Token: 0x040037F6 RID: 14326
		DataCollectionStop,
		// Token: 0x040037F7 RID: 14327
		Extension,
		// Token: 0x040037F8 RID: 14328
		Reply,
		// Token: 0x040037F9 RID: 14329
		Resume,
		// Token: 0x040037FA RID: 14330
		Suspend,
		// Token: 0x040037FB RID: 14331
		Send,
		// Token: 0x040037FC RID: 14332
		Receive = 240
	}
}
