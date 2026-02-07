using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000035 RID: 53
	internal class MpbTriangle : MetaMpb, IDashableMpb
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00017580 File Offset: 0x00015780
		List<float> IDashableMpb.dashOffset { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x00017588 File Offset: 0x00015788
		List<float> IDashableMpb.dashShapeModifier { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x00017590 File Offset: 0x00015790
		List<float> IDashableMpb.dashSize { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x00017598 File Offset: 0x00015798
		List<float> IDashableMpb.dashSnap { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x000175A0 File Offset: 0x000157A0
		List<float> IDashableMpb.dashSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x000175A8 File Offset: 0x000157A8
		List<float> IDashableMpb.dashSpacing { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x000175B0 File Offset: 0x000157B0
		List<float> IDashableMpb.dashType { get; } = MetaMpb.InitList<float>();

		// Token: 0x06000BC4 RID: 3012 RVA: 0x000175B8 File Offset: 0x000157B8
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propA, this.a);
			base.Transfer(ShapesMaterialUtils.propB, this.b);
			base.Transfer(ShapesMaterialUtils.propC, this.c);
			base.Transfer(ShapesMaterialUtils.propColorB, this.colorB);
			base.Transfer(ShapesMaterialUtils.propColorC, this.colorC);
			base.Transfer(ShapesMaterialUtils.propBorder, this.hollow);
			base.Transfer(ShapesMaterialUtils.propRoundness, this.roundness);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
		}

		// Token: 0x04000184 RID: 388
		internal readonly List<Vector4> a = MetaMpb.InitList<Vector4>();

		// Token: 0x04000185 RID: 389
		internal readonly List<Vector4> b = MetaMpb.InitList<Vector4>();

		// Token: 0x04000186 RID: 390
		internal readonly List<Vector4> c = MetaMpb.InitList<Vector4>();

		// Token: 0x04000187 RID: 391
		internal readonly List<Vector4> colorB = MetaMpb.InitList<Vector4>();

		// Token: 0x04000188 RID: 392
		internal readonly List<Vector4> colorC = MetaMpb.InitList<Vector4>();

		// Token: 0x04000189 RID: 393
		internal readonly List<float> hollow = MetaMpb.InitList<float>();

		// Token: 0x0400018A RID: 394
		internal readonly List<float> roundness = MetaMpb.InitList<float>();

		// Token: 0x0400018B RID: 395
		internal readonly List<float> scaleMode = MetaMpb.InitList<float>();

		// Token: 0x0400018C RID: 396
		internal readonly List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x0400018D RID: 397
		internal readonly List<float> thicknessSpace = MetaMpb.InitList<float>();
	}
}
