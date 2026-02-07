using System;
using System.Collections.Generic;

namespace MeshCombineStudio
{
	// Token: 0x02000049 RID: 73
	[Serializable]
	public class FoundCombineConditions
	{
		// Token: 0x040001C9 RID: 457
		public HashSet<CombineCondition> combineConditions = new HashSet<CombineCondition>();

		// Token: 0x040001CA RID: 458
		public int combineConditionsCount;

		// Token: 0x040001CB RID: 459
		public int matCount;

		// Token: 0x040001CC RID: 460
		public int lightmapIndexCount;

		// Token: 0x040001CD RID: 461
		public int shadowCastingCount;

		// Token: 0x040001CE RID: 462
		public int receiveShadowsCount;

		// Token: 0x040001CF RID: 463
		public int lightmapScale;

		// Token: 0x040001D0 RID: 464
		public int receiveGICount;

		// Token: 0x040001D1 RID: 465
		public int lightProbeUsageCount;

		// Token: 0x040001D2 RID: 466
		public int reflectionProbeUsageCount;

		// Token: 0x040001D3 RID: 467
		public int probeAnchorCount;

		// Token: 0x040001D4 RID: 468
		public int motionVectorGenerationModeCount;

		// Token: 0x040001D5 RID: 469
		public int layerCount;

		// Token: 0x040001D6 RID: 470
		public int staticEditorFlagsCount;
	}
}
