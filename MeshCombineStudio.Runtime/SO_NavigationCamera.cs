using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000045 RID: 69
	public class SO_NavigationCamera : ScriptableObject
	{
		// Token: 0x040001BB RID: 443
		public float mouseSensitity = 1f;

		// Token: 0x040001BC RID: 444
		public float speedUpLerpMulti = 1f;

		// Token: 0x040001BD RID: 445
		public float speedDownLerpMulti = 15f;

		// Token: 0x040001BE RID: 446
		public float speedSlow = 1f;

		// Token: 0x040001BF RID: 447
		public float speedNormal = 10f;

		// Token: 0x040001C0 RID: 448
		public float speedFast = 25f;

		// Token: 0x040001C1 RID: 449
		public float mouseScrollWheelMulti = 25f;
	}
}
