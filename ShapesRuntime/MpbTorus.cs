using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x02000034 RID: 52
	internal class MpbTorus : MetaMpb
	{
		// Token: 0x06000BBB RID: 3003 RVA: 0x0001749C File Offset: 0x0001569C
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propAngEnd, this.angleEnd);
			base.Transfer(ShapesMaterialUtils.propAngStart, this.angleStart);
			base.Transfer(ShapesMaterialUtils.propRadius, this.radius);
			base.Transfer(ShapesMaterialUtils.propRadiusSpace, this.radiusSpace);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
		}

		// Token: 0x0400017D RID: 381
		internal readonly List<float> angleEnd = MetaMpb.InitList<float>();

		// Token: 0x0400017E RID: 382
		internal readonly List<float> angleStart = MetaMpb.InitList<float>();

		// Token: 0x0400017F RID: 383
		internal readonly List<float> radius = MetaMpb.InitList<float>();

		// Token: 0x04000180 RID: 384
		internal readonly List<float> radiusSpace = MetaMpb.InitList<float>();

		// Token: 0x04000181 RID: 385
		internal readonly List<float> scaleMode = MetaMpb.InitList<float>();

		// Token: 0x04000182 RID: 386
		internal readonly List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x04000183 RID: 387
		internal readonly List<float> thicknessSpace = MetaMpb.InitList<float>();
	}
}
