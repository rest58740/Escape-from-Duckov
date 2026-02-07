using System;
using UnityEngine;

namespace Pathfinding.Drawing
{
	// Token: 0x0200004B RID: 75
	public class DrawingSettings : ScriptableObject
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000B6BB File Offset: 0x000098BB
		public static DrawingSettings.Settings DefaultSettings
		{
			get
			{
				return new DrawingSettings.Settings();
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000B6C2 File Offset: 0x000098C2
		public static DrawingSettings GetSettingsAsset()
		{
			return Resources.Load<DrawingSettings>("AstarGizmos");
		}

		// Token: 0x04000127 RID: 295
		public const string SettingsPathCompatibility = "Assets/Settings/ALINE.asset";

		// Token: 0x04000128 RID: 296
		public const string SettingsName = "AstarGizmos";

		// Token: 0x04000129 RID: 297
		public const string SettingsPath = "Assets/Settings/Resources/AstarGizmos.asset";

		// Token: 0x0400012A RID: 298
		[SerializeField]
		private int version;

		// Token: 0x0400012B RID: 299
		public DrawingSettings.Settings settings;

		// Token: 0x0200004C RID: 76
		[Serializable]
		public class Settings
		{
			// Token: 0x0400012C RID: 300
			public float lineOpacity = 1f;

			// Token: 0x0400012D RID: 301
			public float solidOpacity = 0.55f;

			// Token: 0x0400012E RID: 302
			public float textOpacity = 1f;

			// Token: 0x0400012F RID: 303
			public float lineOpacityBehindObjects = 0.12f;

			// Token: 0x04000130 RID: 304
			public float solidOpacityBehindObjects = 0.45f;

			// Token: 0x04000131 RID: 305
			public float textOpacityBehindObjects = 0.9f;

			// Token: 0x04000132 RID: 306
			public float curveResolution = 1f;
		}
	}
}
