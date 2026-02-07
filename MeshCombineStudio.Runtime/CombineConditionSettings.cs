using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200004B RID: 75
	[Serializable]
	public class CombineConditionSettings
	{
		// Token: 0x040001E3 RID: 483
		public bool foldout = true;

		// Token: 0x040001E4 RID: 484
		public bool sameMaterial = true;

		// Token: 0x040001E5 RID: 485
		public bool sameShadowCastingMode;

		// Token: 0x040001E6 RID: 486
		public bool sameReceiveShadows;

		// Token: 0x040001E7 RID: 487
		public bool sameReceiveGI;

		// Token: 0x040001E8 RID: 488
		public bool sameLightmapScale;

		// Token: 0x040001E9 RID: 489
		public bool sameLightProbeUsage;

		// Token: 0x040001EA RID: 490
		public bool sameReflectionProbeUsage;

		// Token: 0x040001EB RID: 491
		public bool sameProbeAnchor;

		// Token: 0x040001EC RID: 492
		public bool sameMotionVectorGenerationMode;

		// Token: 0x040001ED RID: 493
		public bool sameStaticEditorFlags;

		// Token: 0x040001EE RID: 494
		public bool sameLayer;

		// Token: 0x040001EF RID: 495
		public Material material;

		// Token: 0x040001F0 RID: 496
		public CombineCondition combineCondition = CombineCondition.Default;
	}
}
