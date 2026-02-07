using System;
using UnityEngine;

namespace DG.DemiLib
{
	// Token: 0x02000005 RID: 5
	[Serializable]
	public class DeColorBG
	{
		// Token: 0x04000016 RID: 22
		public DeSkinColor editor = new DeSkinColor(new Color32(194, 194, 194, byte.MaxValue), new Color32(56, 56, 56, byte.MaxValue));

		// Token: 0x04000017 RID: 23
		public DeSkinColor def = Color.white;

		// Token: 0x04000018 RID: 24
		public DeSkinColor critical = new DeSkinColor(new Color(0.9411765f, 0.2388736f, 0.006920422f, 1f), new Color(1f, 0.2482758f, 0f, 1f));

		// Token: 0x04000019 RID: 25
		public DeSkinColor divider = new DeSkinColor(0.6f, 0.3f);

		// Token: 0x0400001A RID: 26
		public DeSkinColor toggleOn = new DeSkinColor(new Color(0.3158468f, 0.875f, 0.1351103f, 1f), new Color(0.2183823f, 0.7279412f, 0.09099264f, 1f));

		// Token: 0x0400001B RID: 27
		public DeSkinColor toggleOff = new DeSkinColor(0.75f, 0.4f);

		// Token: 0x0400001C RID: 28
		public DeSkinColor toggleCritical = new DeSkinColor(new Color(0.9411765f, 0.2388736f, 0.006920422f, 1f), new Color(1f, 0.2482758f, 0f, 1f));
	}
}
