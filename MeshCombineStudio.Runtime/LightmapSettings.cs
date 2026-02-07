using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000034 RID: 52
	[ExecuteInEditMode]
	public class LightmapSettings : MonoBehaviour
	{
		// Token: 0x06000123 RID: 291 RVA: 0x0000B10D File Offset: 0x0000930D
		private void OnEnable()
		{
			if (this.mr)
			{
				this.mr.lightmapIndex = this.lightmapIndex;
				if (this.setLightmapScaleOffset)
				{
					this.mr.lightmapScaleOffset = this.lightmapScaleOffset;
				}
			}
		}

		// Token: 0x0400013B RID: 315
		public MeshRenderer mr;

		// Token: 0x0400013C RID: 316
		public int lightmapIndex;

		// Token: 0x0400013D RID: 317
		public bool setLightmapScaleOffset;

		// Token: 0x0400013E RID: 318
		public Vector4 lightmapScaleOffset;
	}
}
