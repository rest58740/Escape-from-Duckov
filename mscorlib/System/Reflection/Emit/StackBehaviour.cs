using System;

namespace System.Reflection.Emit
{
	// Token: 0x0200090A RID: 2314
	public enum StackBehaviour
	{
		// Token: 0x040030A8 RID: 12456
		Pop0,
		// Token: 0x040030A9 RID: 12457
		Pop1,
		// Token: 0x040030AA RID: 12458
		Pop1_pop1,
		// Token: 0x040030AB RID: 12459
		Popi,
		// Token: 0x040030AC RID: 12460
		Popi_pop1,
		// Token: 0x040030AD RID: 12461
		Popi_popi,
		// Token: 0x040030AE RID: 12462
		Popi_popi8,
		// Token: 0x040030AF RID: 12463
		Popi_popi_popi,
		// Token: 0x040030B0 RID: 12464
		Popi_popr4,
		// Token: 0x040030B1 RID: 12465
		Popi_popr8,
		// Token: 0x040030B2 RID: 12466
		Popref,
		// Token: 0x040030B3 RID: 12467
		Popref_pop1,
		// Token: 0x040030B4 RID: 12468
		Popref_popi,
		// Token: 0x040030B5 RID: 12469
		Popref_popi_popi,
		// Token: 0x040030B6 RID: 12470
		Popref_popi_popi8,
		// Token: 0x040030B7 RID: 12471
		Popref_popi_popr4,
		// Token: 0x040030B8 RID: 12472
		Popref_popi_popr8,
		// Token: 0x040030B9 RID: 12473
		Popref_popi_popref,
		// Token: 0x040030BA RID: 12474
		Push0,
		// Token: 0x040030BB RID: 12475
		Push1,
		// Token: 0x040030BC RID: 12476
		Push1_push1,
		// Token: 0x040030BD RID: 12477
		Pushi,
		// Token: 0x040030BE RID: 12478
		Pushi8,
		// Token: 0x040030BF RID: 12479
		Pushr4,
		// Token: 0x040030C0 RID: 12480
		Pushr8,
		// Token: 0x040030C1 RID: 12481
		Pushref,
		// Token: 0x040030C2 RID: 12482
		Varpop,
		// Token: 0x040030C3 RID: 12483
		Varpush,
		// Token: 0x040030C4 RID: 12484
		Popref_popi_pop1
	}
}
