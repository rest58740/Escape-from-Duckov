using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000032 RID: 50
	internal class MpbRegularPolygon : MetaMpb, IFillableMpb, IDashableMpb
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x0001723A File Offset: 0x0001543A
		List<Vector4> IFillableMpb.fillColorEnd { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x00017242 File Offset: 0x00015442
		List<Vector4> IFillableMpb.fillEnd { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x0001724A File Offset: 0x0001544A
		List<float> IFillableMpb.fillSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x00017252 File Offset: 0x00015452
		List<Vector4> IFillableMpb.fillStart { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x0001725A File Offset: 0x0001545A
		List<float> IFillableMpb.fillType { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x00017262 File Offset: 0x00015462
		List<float> IDashableMpb.dashOffset { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x0001726A File Offset: 0x0001546A
		List<float> IDashableMpb.dashShapeModifier { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x00017272 File Offset: 0x00015472
		List<float> IDashableMpb.dashSize { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x0001727A File Offset: 0x0001547A
		List<float> IDashableMpb.dashSnap { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x00017282 File Offset: 0x00015482
		List<float> IDashableMpb.dashSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x0001728A File Offset: 0x0001548A
		List<float> IDashableMpb.dashSpacing { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x00017292 File Offset: 0x00015492
		List<float> IDashableMpb.dashType { get; } = MetaMpb.InitList<float>();

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0001729C File Offset: 0x0001549C
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propAlignment, this.alignment);
			base.Transfer(ShapesMaterialUtils.propAng, this.angle);
			base.Transfer(ShapesMaterialUtils.propBorder, this.hollow);
			base.Transfer(ShapesMaterialUtils.propRadius, this.radius);
			base.Transfer(ShapesMaterialUtils.propRadiusSpace, this.radiusSpace);
			base.Transfer(ShapesMaterialUtils.propRoundness, this.roundness);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
			base.Transfer(ShapesMaterialUtils.propSides, this.sides);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
		}

		// Token: 0x04000165 RID: 357
		internal readonly List<float> alignment = MetaMpb.InitList<float>();

		// Token: 0x04000166 RID: 358
		internal readonly List<float> angle = MetaMpb.InitList<float>();

		// Token: 0x04000167 RID: 359
		internal readonly List<float> hollow = MetaMpb.InitList<float>();

		// Token: 0x04000168 RID: 360
		internal readonly List<float> radius = MetaMpb.InitList<float>();

		// Token: 0x04000169 RID: 361
		internal readonly List<float> radiusSpace = MetaMpb.InitList<float>();

		// Token: 0x0400016A RID: 362
		internal readonly List<float> roundness = MetaMpb.InitList<float>();

		// Token: 0x0400016B RID: 363
		internal readonly List<float> scaleMode = MetaMpb.InitList<float>();

		// Token: 0x0400016C RID: 364
		internal readonly List<float> sides = MetaMpb.InitList<float>();

		// Token: 0x0400016D RID: 365
		internal readonly List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x0400016E RID: 366
		internal readonly List<float> thicknessSpace = MetaMpb.InitList<float>();
	}
}
