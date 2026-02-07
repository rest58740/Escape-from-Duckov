using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x02000028 RID: 40
	internal interface IDashableMpb
	{
		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000B6C RID: 2924
		List<float> dashSize { get; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000B6D RID: 2925
		List<float> dashType { get; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000B6E RID: 2926
		List<float> dashShapeModifier { get; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000B6F RID: 2927
		List<float> dashSpace { get; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000B70 RID: 2928
		List<float> dashSnap { get; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000B71 RID: 2929
		List<float> dashOffset { get; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000B72 RID: 2930
		List<float> dashSpacing { get; }
	}
}
