using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200002D RID: 45
	internal class MpbLine3D : MetaMpb, IDashableMpb
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x00016D81 File Offset: 0x00014F81
		List<float> IDashableMpb.dashOffset { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x00016D89 File Offset: 0x00014F89
		List<float> IDashableMpb.dashShapeModifier { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x00016D91 File Offset: 0x00014F91
		List<float> IDashableMpb.dashSize { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x00016D99 File Offset: 0x00014F99
		List<float> IDashableMpb.dashSnap { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x00016DA1 File Offset: 0x00014FA1
		List<float> IDashableMpb.dashSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x00016DA9 File Offset: 0x00014FA9
		List<float> IDashableMpb.dashSpacing { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x00016DB1 File Offset: 0x00014FB1
		List<float> IDashableMpb.dashType { get; } = MetaMpb.InitList<float>();

		// Token: 0x06000B90 RID: 2960 RVA: 0x00016DBC File Offset: 0x00014FBC
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propColorEnd, this.colorEnd);
			base.Transfer(ShapesMaterialUtils.propPointEnd, this.pointEnd);
			base.Transfer(ShapesMaterialUtils.propPointStart, this.pointStart);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
		}

		// Token: 0x04000137 RID: 311
		internal readonly List<Vector4> colorEnd = MetaMpb.InitList<Vector4>();

		// Token: 0x04000138 RID: 312
		internal readonly List<Vector4> pointEnd = MetaMpb.InitList<Vector4>();

		// Token: 0x04000139 RID: 313
		internal readonly List<Vector4> pointStart = MetaMpb.InitList<Vector4>();

		// Token: 0x0400013A RID: 314
		internal readonly List<float> scaleMode = MetaMpb.InitList<float>();

		// Token: 0x0400013B RID: 315
		internal readonly List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x0400013C RID: 316
		internal readonly List<float> thicknessSpace = MetaMpb.InitList<float>();
	}
}
