using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000027 RID: 39
	internal interface IFillableMpb
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000B67 RID: 2919
		List<float> fillType { get; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000B68 RID: 2920
		List<float> fillSpace { get; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000B69 RID: 2921
		List<Vector4> fillStart { get; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000B6A RID: 2922
		List<Vector4> fillEnd { get; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000B6B RID: 2923
		List<Vector4> fillColorEnd { get; }
	}
}
