using System;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x02000020 RID: 32
	public struct KTwoBoneIkData
	{
		// Token: 0x04000049 RID: 73
		public KTransform root;

		// Token: 0x0400004A RID: 74
		public KTransform mid;

		// Token: 0x0400004B RID: 75
		public KTransform tip;

		// Token: 0x0400004C RID: 76
		public KTransform target;

		// Token: 0x0400004D RID: 77
		public KTransform hint;

		// Token: 0x0400004E RID: 78
		public float posWeight;

		// Token: 0x0400004F RID: 79
		public float rotWeight;

		// Token: 0x04000050 RID: 80
		public float hintWeight;

		// Token: 0x04000051 RID: 81
		public bool hasValidHint;
	}
}
