using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000E1 RID: 225
	[AttributeUsage(256)]
	public class PopupFieldAttribute : DrawerAttribute
	{
		// Token: 0x0600076A RID: 1898 RVA: 0x0001723B File Offset: 0x0001543B
		public PopupFieldAttribute(params object[] options)
		{
			this.options = options;
		}

		// Token: 0x0400024E RID: 590
		public readonly object[] options;
	}
}
