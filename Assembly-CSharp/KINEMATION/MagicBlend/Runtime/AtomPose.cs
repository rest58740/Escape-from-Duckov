using System;
using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;

namespace KINEMATION.MagicBlend.Runtime
{
	// Token: 0x02000059 RID: 89
	public struct AtomPose
	{
		// Token: 0x0600032E RID: 814 RVA: 0x0000DC64 File Offset: 0x0000BE64
		public static AtomPose Lerp(AtomPose a, AtomPose b, float alpha)
		{
			return new AtomPose
			{
				basePose = KTransform.Lerp(a.basePose, b.basePose, alpha),
				overlayPose = KTransform.Lerp(a.overlayPose, b.overlayPose, alpha),
				localOverlayRotation = Quaternion.Slerp(a.localOverlayRotation, b.localOverlayRotation, alpha),
				additiveWeight = Mathf.Lerp(a.additiveWeight, b.additiveWeight, alpha),
				baseWeight = Mathf.Lerp(a.baseWeight, b.baseWeight, alpha),
				localWeight = Mathf.Lerp(a.localWeight, b.localWeight, alpha)
			};
		}

		// Token: 0x0400020A RID: 522
		public KTransform basePose;

		// Token: 0x0400020B RID: 523
		public KTransform overlayPose;

		// Token: 0x0400020C RID: 524
		public Quaternion localOverlayRotation;

		// Token: 0x0400020D RID: 525
		public float baseWeight;

		// Token: 0x0400020E RID: 526
		public float additiveWeight;

		// Token: 0x0400020F RID: 527
		public float localWeight;
	}
}
