using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000DA RID: 218
	[AttributeUsage(256)]
	public class ShowButtonAttribute : DrawerAttribute
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x000171C1 File Offset: 0x000153C1
		public override bool isDecorator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x000171C4 File Offset: 0x000153C4
		public override int priority
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x000171C7 File Offset: 0x000153C7
		public ShowButtonAttribute(string buttonTitle, string methodnameCallback)
		{
			this.buttonTitle = buttonTitle;
			this.methodName = methodnameCallback;
		}

		// Token: 0x04000249 RID: 585
		public readonly string buttonTitle;

		// Token: 0x0400024A RID: 586
		public readonly string methodName;
	}
}
