using System;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003E4 RID: 996
	public class KontextMenuDataEntry
	{
		// Token: 0x06002461 RID: 9313 RVA: 0x0007FDF1 File Offset: 0x0007DFF1
		public void Invoke()
		{
			Action action = this.action;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x040018BC RID: 6332
		public Sprite icon;

		// Token: 0x040018BD RID: 6333
		public string text;

		// Token: 0x040018BE RID: 6334
		public Action action;
	}
}
