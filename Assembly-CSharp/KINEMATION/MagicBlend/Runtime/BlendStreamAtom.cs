using System;
using KINEMATION.KAnimationCore.Runtime.Core;
using Unity.Collections;
using UnityEngine.Animations;

namespace KINEMATION.MagicBlend.Runtime
{
	// Token: 0x0200005A RID: 90
	public struct BlendStreamAtom
	{
		// Token: 0x0600032F RID: 815 RVA: 0x0000DD10 File Offset: 0x0000BF10
		public AtomPose GetBlendedAtomPose(float blendWeight)
		{
			return AtomPose.Lerp(this.cachedPose, this.activePose, blendWeight);
		}

		// Token: 0x04000210 RID: 528
		[ReadOnly]
		public TransformStreamHandle handle;

		// Token: 0x04000211 RID: 529
		[ReadOnly]
		public float baseWeight;

		// Token: 0x04000212 RID: 530
		[ReadOnly]
		public float additiveWeight;

		// Token: 0x04000213 RID: 531
		[ReadOnly]
		public float localWeight;

		// Token: 0x04000214 RID: 532
		public KTransform meshStreamPose;

		// Token: 0x04000215 RID: 533
		public AtomPose activePose;

		// Token: 0x04000216 RID: 534
		public AtomPose cachedPose;
	}
}
