using System;

namespace FMODUnity
{
	// Token: 0x0200011C RID: 284
	[Flags]
	public enum ThreadAffinity : uint
	{
		// Token: 0x040005EC RID: 1516
		Any = 0U,
		// Token: 0x040005ED RID: 1517
		Core0 = 1U,
		// Token: 0x040005EE RID: 1518
		Core1 = 2U,
		// Token: 0x040005EF RID: 1519
		Core2 = 4U,
		// Token: 0x040005F0 RID: 1520
		Core3 = 8U,
		// Token: 0x040005F1 RID: 1521
		Core4 = 16U,
		// Token: 0x040005F2 RID: 1522
		Core5 = 32U,
		// Token: 0x040005F3 RID: 1523
		Core6 = 64U,
		// Token: 0x040005F4 RID: 1524
		Core7 = 128U,
		// Token: 0x040005F5 RID: 1525
		Core8 = 256U,
		// Token: 0x040005F6 RID: 1526
		Core9 = 512U,
		// Token: 0x040005F7 RID: 1527
		Core10 = 1024U,
		// Token: 0x040005F8 RID: 1528
		Core11 = 2048U,
		// Token: 0x040005F9 RID: 1529
		Core12 = 4096U,
		// Token: 0x040005FA RID: 1530
		Core13 = 8192U,
		// Token: 0x040005FB RID: 1531
		Core14 = 16384U,
		// Token: 0x040005FC RID: 1532
		Core15 = 32768U
	}
}
