using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000031 RID: 49
	internal class MpbRect : MetaMpb, IFillableMpb, IDashableMpb
	{
		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000B9D RID: 2973 RVA: 0x000170A8 File Offset: 0x000152A8
		List<Vector4> IFillableMpb.fillColorEnd { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x000170B0 File Offset: 0x000152B0
		List<Vector4> IFillableMpb.fillEnd { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x000170B8 File Offset: 0x000152B8
		List<float> IFillableMpb.fillSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x000170C0 File Offset: 0x000152C0
		List<Vector4> IFillableMpb.fillStart { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x000170C8 File Offset: 0x000152C8
		List<float> IFillableMpb.fillType { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x000170D0 File Offset: 0x000152D0
		List<float> IDashableMpb.dashOffset { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x000170D8 File Offset: 0x000152D8
		List<float> IDashableMpb.dashShapeModifier { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x000170E0 File Offset: 0x000152E0
		List<float> IDashableMpb.dashSize { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x000170E8 File Offset: 0x000152E8
		List<float> IDashableMpb.dashSnap { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x000170F0 File Offset: 0x000152F0
		List<float> IDashableMpb.dashSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x000170F8 File Offset: 0x000152F8
		List<float> IDashableMpb.dashSpacing { get; } = MetaMpb.InitList<float>();

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x00017100 File Offset: 0x00015300
		List<float> IDashableMpb.dashType { get; } = MetaMpb.InitList<float>();

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00017108 File Offset: 0x00015308
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propCornerRadii, this.cornerRadii);
			base.Transfer(ShapesMaterialUtils.propRect, this.rect);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
		}

		// Token: 0x04000154 RID: 340
		internal readonly List<Vector4> cornerRadii = MetaMpb.InitList<Vector4>();

		// Token: 0x04000155 RID: 341
		internal readonly List<Vector4> rect = MetaMpb.InitList<Vector4>();

		// Token: 0x04000156 RID: 342
		internal readonly List<float> scaleMode = MetaMpb.InitList<float>();

		// Token: 0x04000157 RID: 343
		internal readonly List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x04000158 RID: 344
		internal readonly List<float> thicknessSpace = MetaMpb.InitList<float>();
	}
}
