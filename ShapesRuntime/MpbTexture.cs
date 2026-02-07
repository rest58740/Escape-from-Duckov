using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000037 RID: 55
	internal class MpbTexture : MetaMpb
	{
		// Token: 0x06000BC8 RID: 3016 RVA: 0x00017748 File Offset: 0x00015948
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propRect, this.rect);
			base.Transfer(ShapesMaterialUtils.propUvs, this.uvs);
			base.Transfer(ShapesMaterialUtils.propMainTex, ref this.texture);
		}

		// Token: 0x04000195 RID: 405
		internal Texture texture;

		// Token: 0x04000196 RID: 406
		internal readonly List<Vector4> rect = MetaMpb.InitList<Vector4>();

		// Token: 0x04000197 RID: 407
		internal readonly List<Vector4> uvs = MetaMpb.InitList<Vector4>();
	}
}
