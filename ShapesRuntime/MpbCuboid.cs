using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200002A RID: 42
	internal class MpbCuboid : MetaMpb
	{
		// Token: 0x06000B75 RID: 2933 RVA: 0x000169DC File Offset: 0x00014BDC
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propSize, this.size);
			base.Transfer(ShapesMaterialUtils.propSizeSpace, this.sizeSpace);
		}

		// Token: 0x04000114 RID: 276
		internal readonly List<Vector4> size = MetaMpb.InitList<Vector4>();

		// Token: 0x04000115 RID: 277
		internal readonly List<float> sizeSpace = MetaMpb.InitList<float>();
	}
}
