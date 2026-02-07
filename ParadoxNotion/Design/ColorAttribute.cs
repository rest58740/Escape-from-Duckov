using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000D4 RID: 212
	[AttributeUsage(4)]
	public class ColorAttribute : Attribute
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x00017156 File Offset: 0x00015356
		public ColorAttribute(string hexColor)
		{
			this.hexColor = hexColor;
		}

		// Token: 0x04000245 RID: 581
		public readonly string hexColor;
	}
}
