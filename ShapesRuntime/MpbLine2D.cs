using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200002C RID: 44
	internal class MpbLine2D : MetaMpb, IDashableMpb
	{
		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x00016C18 File Offset: 0x00014E18
		List<float> IDashableMpb.dashOffset { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x00016C20 File Offset: 0x00014E20
		List<float> IDashableMpb.dashShapeModifier { get; } = MetaMpb.InitList<float>();

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x00016C28 File Offset: 0x00014E28
		List<float> IDashableMpb.dashSize { get; } = MetaMpb.InitList<float>();

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x00016C30 File Offset: 0x00014E30
		List<float> IDashableMpb.dashSnap { get; } = MetaMpb.InitList<float>();

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x00016C38 File Offset: 0x00014E38
		List<float> IDashableMpb.dashSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x00016C40 File Offset: 0x00014E40
		List<float> IDashableMpb.dashSpacing { get; } = MetaMpb.InitList<float>();

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x00016C48 File Offset: 0x00014E48
		List<float> IDashableMpb.dashType { get; } = MetaMpb.InitList<float>();

		// Token: 0x06000B87 RID: 2951 RVA: 0x00016C50 File Offset: 0x00014E50
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propAlignment, this.alignment);
			base.Transfer(ShapesMaterialUtils.propColorEnd, this.colorEnd);
			base.Transfer(ShapesMaterialUtils.propPointEnd, this.pointEnd);
			base.Transfer(ShapesMaterialUtils.propPointStart, this.pointStart);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
		}

		// Token: 0x04000129 RID: 297
		internal readonly List<float> alignment = MetaMpb.InitList<float>();

		// Token: 0x0400012A RID: 298
		internal readonly List<Vector4> colorEnd = MetaMpb.InitList<Vector4>();

		// Token: 0x0400012B RID: 299
		internal readonly List<Vector4> pointEnd = MetaMpb.InitList<Vector4>();

		// Token: 0x0400012C RID: 300
		internal readonly List<Vector4> pointStart = MetaMpb.InitList<Vector4>();

		// Token: 0x0400012D RID: 301
		internal readonly List<float> scaleMode = MetaMpb.InitList<float>();

		// Token: 0x0400012E RID: 302
		internal readonly List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x0400012F RID: 303
		internal readonly List<float> thicknessSpace = MetaMpb.InitList<float>();
	}
}
