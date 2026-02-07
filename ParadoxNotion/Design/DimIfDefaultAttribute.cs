using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000D7 RID: 215
	[AttributeUsage(256)]
	public class DimIfDefaultAttribute : DrawerAttribute
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x00017189 File Offset: 0x00015389
		public override bool isDecorator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x0001718C File Offset: 0x0001538C
		public override int priority
		{
			get
			{
				return 0;
			}
		}
	}
}
