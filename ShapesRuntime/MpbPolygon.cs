using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200002E RID: 46
	internal class MpbPolygon : MetaMpb, IFillableMpb
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x00016ED2 File Offset: 0x000150D2
		List<Vector4> IFillableMpb.fillColorEnd { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x00016EDA File Offset: 0x000150DA
		List<Vector4> IFillableMpb.fillEnd { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x00016EE2 File Offset: 0x000150E2
		List<float> IFillableMpb.fillSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x00016EEA File Offset: 0x000150EA
		List<Vector4> IFillableMpb.fillStart { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x00016EF2 File Offset: 0x000150F2
		List<float> IFillableMpb.fillType { get; } = MetaMpb.InitList<float>();

		// Token: 0x06000B97 RID: 2967 RVA: 0x00016EFA File Offset: 0x000150FA
		protected override void TransferShapeProperties()
		{
		}
	}
}
