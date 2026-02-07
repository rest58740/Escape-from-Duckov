using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x02000033 RID: 51
	internal class MpbSphere : MetaMpb
	{
		// Token: 0x06000BB9 RID: 3001 RVA: 0x00017459 File Offset: 0x00015659
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propRadius, this.radius);
			base.Transfer(ShapesMaterialUtils.propRadiusSpace, this.radiusSpace);
		}

		// Token: 0x0400017B RID: 379
		internal readonly List<float> radius = MetaMpb.InitList<float>();

		// Token: 0x0400017C RID: 380
		internal readonly List<float> radiusSpace = MetaMpb.InitList<float>();
	}
}
