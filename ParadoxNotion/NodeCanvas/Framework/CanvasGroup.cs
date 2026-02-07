using System;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class CanvasGroup
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000021F5 File Offset: 0x000003F5
		public CanvasGroup()
		{
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021FD File Offset: 0x000003FD
		public CanvasGroup(Rect rect, string name)
		{
			this.rect = rect;
			this.name = name;
		}

		// Token: 0x04000011 RID: 17
		public string name;

		// Token: 0x04000012 RID: 18
		public Rect rect;

		// Token: 0x04000013 RID: 19
		public Color color;

		// Token: 0x04000014 RID: 20
		public bool autoGroup;

		// Token: 0x04000015 RID: 21
		public string notes;
	}
}
