using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000D9 RID: 217
	[AttributeUsage(256)]
	public class RequiredFieldAttribute : DrawerAttribute
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x000171B3 File Offset: 0x000153B3
		public override bool isDecorator
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x000171B6 File Offset: 0x000153B6
		public override int priority
		{
			get
			{
				return 2;
			}
		}
	}
}
