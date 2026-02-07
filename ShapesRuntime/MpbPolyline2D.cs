using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x0200002F RID: 47
	internal class MpbPolyline2D : MetaMpb
	{
		// Token: 0x06000B99 RID: 2969 RVA: 0x00016F3C File Offset: 0x0001513C
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propAlignment, this.alignment);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
		}

		// Token: 0x04000149 RID: 329
		internal readonly List<float> alignment = MetaMpb.InitList<float>();

		// Token: 0x0400014A RID: 330
		internal readonly List<float> scaleMode = MetaMpb.InitList<float>();

		// Token: 0x0400014B RID: 331
		internal readonly List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x0400014C RID: 332
		internal readonly List<float> thicknessSpace = MetaMpb.InitList<float>();
	}
}
