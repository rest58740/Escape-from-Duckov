using System;
using UnityEngine;

namespace Drawing
{
	// Token: 0x02000049 RID: 73
	public class DrawingSettings : ScriptableObject
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0000D2BF File Offset: 0x0000B4BF
		public static DrawingSettings.Settings DefaultSettings
		{
			get
			{
				return new DrawingSettings.Settings();
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000D2C6 File Offset: 0x0000B4C6
		public static DrawingSettings GetSettingsAsset()
		{
			return Resources.Load<DrawingSettings>("ALINE");
		}

		// Token: 0x0400011C RID: 284
		public const string SettingsPathCompatibility = "Assets/Settings/ALINE.asset";

		// Token: 0x0400011D RID: 285
		public const string SettingsName = "ALINE";

		// Token: 0x0400011E RID: 286
		public const string SettingsPath = "Assets/Settings/Resources/ALINE.asset";

		// Token: 0x0400011F RID: 287
		[SerializeField]
		private int version;

		// Token: 0x04000120 RID: 288
		public DrawingSettings.Settings settings;

		// Token: 0x0200004A RID: 74
		[Serializable]
		public class Settings
		{
			// Token: 0x04000121 RID: 289
			public float lineOpacity = 1f;

			// Token: 0x04000122 RID: 290
			public float solidOpacity = 0.55f;

			// Token: 0x04000123 RID: 291
			public float textOpacity = 1f;

			// Token: 0x04000124 RID: 292
			public float lineOpacityBehindObjects = 0.12f;

			// Token: 0x04000125 RID: 293
			public float solidOpacityBehindObjects = 0.45f;

			// Token: 0x04000126 RID: 294
			public float textOpacityBehindObjects = 0.9f;

			// Token: 0x04000127 RID: 295
			public float curveResolution = 1f;
		}
	}
}
