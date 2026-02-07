using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200002B RID: 43
	internal class MpbDisc : MetaMpb, IDashableMpb
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x00016A1E File Offset: 0x00014C1E
		List<float> IDashableMpb.dashOffset { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x00016A26 File Offset: 0x00014C26
		List<float> IDashableMpb.dashShapeModifier { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x00016A2E File Offset: 0x00014C2E
		List<float> IDashableMpb.dashSize { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x00016A36 File Offset: 0x00014C36
		List<float> IDashableMpb.dashSnap { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x00016A3E File Offset: 0x00014C3E
		List<float> IDashableMpb.dashSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x00016A46 File Offset: 0x00014C46
		List<float> IDashableMpb.dashSpacing { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x00016A4E File Offset: 0x00014C4E
		List<float> IDashableMpb.dashType { get; } = MetaMpb.InitList<float>();

		// Token: 0x06000B7E RID: 2942 RVA: 0x00016A58 File Offset: 0x00014C58
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propAlignment, this.alignment);
			base.Transfer(ShapesMaterialUtils.propAngEnd, this.angleEnd);
			base.Transfer(ShapesMaterialUtils.propAngStart, this.angleStart);
			base.Transfer(ShapesMaterialUtils.propColorInnerEnd, this.colorInnerEnd);
			base.Transfer(ShapesMaterialUtils.propColorOuterEnd, this.colorOuterEnd);
			base.Transfer(ShapesMaterialUtils.propColorOuterStart, this.colorOuterStart);
			base.Transfer(ShapesMaterialUtils.propRadius, this.radius);
			base.Transfer(ShapesMaterialUtils.propRadiusSpace, this.radiusSpace);
			base.Transfer(ShapesMaterialUtils.propRoundCaps, this.roundCaps);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
		}

		// Token: 0x04000116 RID: 278
		internal readonly List<float> alignment = MetaMpb.InitList<float>();

		// Token: 0x04000117 RID: 279
		internal readonly List<float> angleEnd = MetaMpb.InitList<float>();

		// Token: 0x04000118 RID: 280
		internal readonly List<float> angleStart = MetaMpb.InitList<float>();

		// Token: 0x04000119 RID: 281
		internal readonly List<Vector4> colorInnerEnd = MetaMpb.InitList<Vector4>();

		// Token: 0x0400011A RID: 282
		internal readonly List<Vector4> colorOuterEnd = MetaMpb.InitList<Vector4>();

		// Token: 0x0400011B RID: 283
		internal readonly List<Vector4> colorOuterStart = MetaMpb.InitList<Vector4>();

		// Token: 0x0400011C RID: 284
		internal readonly List<float> radius = MetaMpb.InitList<float>();

		// Token: 0x0400011D RID: 285
		internal readonly List<float> radiusSpace = MetaMpb.InitList<float>();

		// Token: 0x0400011E RID: 286
		internal readonly List<float> roundCaps = MetaMpb.InitList<float>();

		// Token: 0x0400011F RID: 287
		internal readonly List<float> scaleMode = MetaMpb.InitList<float>();

		// Token: 0x04000120 RID: 288
		internal readonly List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x04000121 RID: 289
		internal readonly List<float> thicknessSpace = MetaMpb.InitList<float>();
	}
}
