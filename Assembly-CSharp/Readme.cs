using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class Readme : ScriptableObject
{
	// Token: 0x0400000E RID: 14
	public Texture2D icon;

	// Token: 0x0400000F RID: 15
	public string title;

	// Token: 0x04000010 RID: 16
	public Readme.Section[] sections;

	// Token: 0x04000011 RID: 17
	public bool loadedLayout;

	// Token: 0x02000095 RID: 149
	[Serializable]
	public class Section
	{
		// Token: 0x040002E3 RID: 739
		public string heading;

		// Token: 0x040002E4 RID: 740
		public string text;

		// Token: 0x040002E5 RID: 741
		public string linkText;

		// Token: 0x040002E6 RID: 742
		public string url;
	}
}
