using System;
using UnityEngine;

namespace LeTai.Asset.TranslucentImage.Demo
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public struct Preset
	{
		// Token: 0x0400001A RID: 26
		public RuntimePlatform platform;

		// Token: 0x0400001B RID: 27
		public float size;

		// Token: 0x0400001C RID: 28
		public int iteration;

		// Token: 0x0400001D RID: 29
		public int downsample;

		// Token: 0x0400001E RID: 30
		public float maxUpdateRate;
	}
}
