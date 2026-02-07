using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000DB RID: 219
	[AttributeUsage(256)]
	public class CallbackAttribute : DrawerAttribute
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x000171DD File Offset: 0x000153DD
		public override bool isDecorator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x000171E0 File Offset: 0x000153E0
		public override int priority
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x000171E3 File Offset: 0x000153E3
		public CallbackAttribute(string methodName)
		{
			this.methodName = methodName;
		}

		// Token: 0x0400024B RID: 587
		public readonly string methodName;
	}
}
