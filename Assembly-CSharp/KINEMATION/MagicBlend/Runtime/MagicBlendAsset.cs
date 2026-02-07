using System;
using System.Collections.Generic;
using KINEMATION.KAnimationCore.Runtime.Attributes;
using KINEMATION.KAnimationCore.Runtime.Rig;
using UnityEngine;

namespace KINEMATION.MagicBlend.Runtime
{
	// Token: 0x02000054 RID: 84
	public class MagicBlendAsset : ScriptableObject, IRigUser
	{
		// Token: 0x06000318 RID: 792 RVA: 0x0000CD3B File Offset: 0x0000AF3B
		public KRig GetRigAsset()
		{
			return this.rigAsset;
		}

		// Token: 0x040001DF RID: 479
		[Header("Rig")]
		public KRig rigAsset;

		// Token: 0x040001E0 RID: 480
		[Header("Blending")]
		[Min(0f)]
		public float blendTime = 0.15f;

		// Token: 0x040001E1 RID: 481
		public AnimationCurve blendCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x040001E2 RID: 482
		[Header("Poses")]
		public AnimationClip basePose;

		// Token: 0x040001E3 RID: 483
		public AnimationClip overlayPose;

		// Token: 0x040001E4 RID: 484
		[Tooltip("If Overlay is static or not.")]
		public bool isAnimation;

		// Token: 0x040001E5 RID: 485
		[Unfold]
		public List<LayeredBlend> layeredBlends = new List<LayeredBlend>();

		// Token: 0x040001E6 RID: 486
		[Range(0f, 1f)]
		public float globalWeight = 1f;
	}
}
