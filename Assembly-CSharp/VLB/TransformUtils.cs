using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000042 RID: 66
	public static class TransformUtils
	{
		// Token: 0x06000263 RID: 611 RVA: 0x00009D14 File Offset: 0x00007F14
		public static TransformUtils.Packed GetWorldPacked(this Transform self)
		{
			return new TransformUtils.Packed
			{
				position = self.position,
				rotation = self.rotation,
				lossyScale = self.lossyScale
			};
		}

		// Token: 0x020000C5 RID: 197
		public struct Packed
		{
			// Token: 0x060004F6 RID: 1270 RVA: 0x0001407C File Offset: 0x0001227C
			public bool IsSame(Transform transf)
			{
				return transf.position == this.position && transf.rotation == this.rotation && transf.lossyScale == this.lossyScale;
			}

			// Token: 0x04000409 RID: 1033
			public Vector3 position;

			// Token: 0x0400040A RID: 1034
			public Quaternion rotation;

			// Token: 0x0400040B RID: 1035
			public Vector3 lossyScale;
		}
	}
}
