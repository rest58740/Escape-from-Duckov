using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000239 RID: 569
	public struct Progress
	{
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x0004605D File Offset: 0x0004425D
		public float progress
		{
			get
			{
				if (this.total > 0f)
				{
					return Mathf.Clamp01(this.current / this.total);
				}
				return 1f;
			}
		}

		// Token: 0x04000DF0 RID: 3568
		public bool inProgress;

		// Token: 0x04000DF1 RID: 3569
		public float total;

		// Token: 0x04000DF2 RID: 3570
		public float current;

		// Token: 0x04000DF3 RID: 3571
		public string progressName;
	}
}
