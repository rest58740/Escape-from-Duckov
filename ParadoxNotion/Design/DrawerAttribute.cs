using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000D5 RID: 213
	[AttributeUsage(256)]
	public abstract class DrawerAttribute : Attribute
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x00017165 File Offset: 0x00015365
		public virtual int priority
		{
			get
			{
				return int.MaxValue;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x0001716C File Offset: 0x0001536C
		public virtual bool isDecorator
		{
			get
			{
				return false;
			}
		}
	}
}
