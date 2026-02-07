using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x0200024D RID: 589
	public class StrongNotificationContent
	{
		// Token: 0x0600129D RID: 4765 RVA: 0x00047AEC File Offset: 0x00045CEC
		public StrongNotificationContent(string mainText, string subText = "", Sprite image = null)
		{
			this.mainText = mainText;
			this.subText = subText;
			this.image = image;
		}

		// Token: 0x04000E49 RID: 3657
		public string mainText;

		// Token: 0x04000E4A RID: 3658
		public string subText;

		// Token: 0x04000E4B RID: 3659
		public Sprite image;
	}
}
