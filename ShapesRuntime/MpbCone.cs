using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x02000029 RID: 41
	internal class MpbCone : MetaMpb
	{
		// Token: 0x06000B73 RID: 2931 RVA: 0x0001697E File Offset: 0x00014B7E
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propLength, this.length);
			base.Transfer(ShapesMaterialUtils.propRadius, this.radius);
			base.Transfer(ShapesMaterialUtils.propSizeSpace, this.sizeSpace);
		}

		// Token: 0x04000111 RID: 273
		internal readonly List<float> length = MetaMpb.InitList<float>();

		// Token: 0x04000112 RID: 274
		internal readonly List<float> radius = MetaMpb.InitList<float>();

		// Token: 0x04000113 RID: 275
		internal readonly List<float> sizeSpace = MetaMpb.InitList<float>();
	}
}
