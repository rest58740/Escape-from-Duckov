using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000D1 RID: 209
	[AttributeUsage(32767)]
	public class CategoryAttribute : Attribute
	{
		// Token: 0x0600074A RID: 1866 RVA: 0x0001710C File Offset: 0x0001530C
		public CategoryAttribute(string category)
		{
			this.category = category;
		}

		// Token: 0x0400023F RID: 575
		public readonly string category;
	}
}
