using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000D6 RID: 214
	[AttributeUsage(256)]
	public class HeaderAttribute : DrawerAttribute
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x00017177 File Offset: 0x00015377
		public override bool isDecorator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001717A File Offset: 0x0001537A
		public HeaderAttribute(string title)
		{
			this.title = title;
		}

		// Token: 0x04000246 RID: 582
		public readonly string title;
	}
}
