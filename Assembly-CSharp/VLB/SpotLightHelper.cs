using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000040 RID: 64
	public static class SpotLightHelper
	{
		// Token: 0x0600025A RID: 602 RVA: 0x00009C1A File Offset: 0x00007E1A
		public static float GetIntensity(Light light)
		{
			if (!(light != null))
			{
				return 0f;
			}
			return light.intensity;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00009C31 File Offset: 0x00007E31
		public static float GetSpotAngle(Light light)
		{
			if (!(light != null))
			{
				return 0f;
			}
			return light.spotAngle;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00009C48 File Offset: 0x00007E48
		public static float GetFallOffEnd(Light light)
		{
			if (!(light != null))
			{
				return 0f;
			}
			return light.range;
		}
	}
}
