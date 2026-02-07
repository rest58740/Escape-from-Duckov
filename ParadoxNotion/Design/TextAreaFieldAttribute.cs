using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000E0 RID: 224
	[AttributeUsage(256)]
	public class TextAreaFieldAttribute : DrawerAttribute
	{
		// Token: 0x06000769 RID: 1897 RVA: 0x0001722C File Offset: 0x0001542C
		public TextAreaFieldAttribute(int numberOfLines)
		{
			this.numberOfLines = numberOfLines;
		}

		// Token: 0x0400024D RID: 589
		public readonly int numberOfLines;
	}
}
