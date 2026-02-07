using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200003B RID: 59
	internal struct ShapeDrawState
	{
		// Token: 0x06000C18 RID: 3096 RVA: 0x00018422 File Offset: 0x00016622
		internal bool CompatibleWith(ShapeDrawState other)
		{
			return this.mesh == other.mesh && this.submesh == other.submesh && this.mat == other.mat;
		}

		// Token: 0x0400019D RID: 413
		public Mesh mesh;

		// Token: 0x0400019E RID: 414
		public Material mat;

		// Token: 0x0400019F RID: 415
		public int submesh;
	}
}
