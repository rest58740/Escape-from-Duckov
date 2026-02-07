using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000030 RID: 48
	internal class MpbQuad : MetaMpb
	{
		// Token: 0x06000B9B RID: 2971 RVA: 0x00016FC4 File Offset: 0x000151C4
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propA, this.a);
			base.Transfer(ShapesMaterialUtils.propB, this.b);
			base.Transfer(ShapesMaterialUtils.propC, this.c);
			base.Transfer(ShapesMaterialUtils.propColorB, this.colorB);
			base.Transfer(ShapesMaterialUtils.propColorC, this.colorC);
			base.Transfer(ShapesMaterialUtils.propColorD, this.colorD);
			base.Transfer(ShapesMaterialUtils.propD, this.d);
		}

		// Token: 0x0400014D RID: 333
		internal readonly List<Vector4> a = MetaMpb.InitList<Vector4>();

		// Token: 0x0400014E RID: 334
		internal readonly List<Vector4> b = MetaMpb.InitList<Vector4>();

		// Token: 0x0400014F RID: 335
		internal readonly List<Vector4> c = MetaMpb.InitList<Vector4>();

		// Token: 0x04000150 RID: 336
		internal readonly List<Vector4> colorB = MetaMpb.InitList<Vector4>();

		// Token: 0x04000151 RID: 337
		internal readonly List<Vector4> colorC = MetaMpb.InitList<Vector4>();

		// Token: 0x04000152 RID: 338
		internal readonly List<Vector4> colorD = MetaMpb.InitList<Vector4>();

		// Token: 0x04000153 RID: 339
		internal readonly List<Vector4> d = MetaMpb.InitList<Vector4>();
	}
}
