using System;
using KINEMATION.KAnimationCore.Runtime.Rig;
using UnityEngine;

namespace KINEMATION.MagicBlend.Runtime
{
	// Token: 0x0200005B RID: 91
	[Serializable]
	public struct LayeredBlend
	{
		// Token: 0x04000217 RID: 535
		public KRigElementChain layer;

		// Token: 0x04000218 RID: 536
		[Range(0f, 1f)]
		public float baseWeight;

		// Token: 0x04000219 RID: 537
		[Range(0f, 1f)]
		public float additiveWeight;

		// Token: 0x0400021A RID: 538
		[Range(0f, 1f)]
		public float localWeight;
	}
}
