using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000DC RID: 220
	[AttributeUsage(256)]
	public class MinValueAttribute : DrawerAttribute
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x000171F2 File Offset: 0x000153F2
		public override int priority
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x000171F5 File Offset: 0x000153F5
		public MinValueAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00017204 File Offset: 0x00015404
		public MinValueAttribute(int min)
		{
			this.min = (float)min;
		}

		// Token: 0x0400024C RID: 588
		public readonly float min;
	}
}
