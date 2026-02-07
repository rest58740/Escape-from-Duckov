using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000D2 RID: 210
	[AttributeUsage(32767)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x0600074B RID: 1867 RVA: 0x0001711B File Offset: 0x0001531B
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}

		// Token: 0x04000240 RID: 576
		public readonly string description;
	}
}
