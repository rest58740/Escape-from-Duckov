using System;
using UnityEngine;

namespace LeTai.Asset.TranslucentImage
{
	// Token: 0x0200000E RID: 14
	public static class ShaderId
	{
		// Token: 0x04000038 RID: 56
		public static readonly int MAIN_TEX = Shader.PropertyToID("_MainTex");

		// Token: 0x04000039 RID: 57
		public static readonly int RADIUS = Shader.PropertyToID("_Radius");

		// Token: 0x0400003A RID: 58
		public static readonly int COLOR = Shader.PropertyToID("_Color");

		// Token: 0x0400003B RID: 59
		public static readonly int BACKGROUND_COLOR = Shader.PropertyToID("_BackgroundColor");

		// Token: 0x0400003C RID: 60
		public static readonly int CROP_REGION = Shader.PropertyToID("_CropRegion");
	}
}
