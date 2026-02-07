using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000D3 RID: 211
	[AttributeUsage(4)]
	public class IconAttribute : Attribute
	{
		// Token: 0x0600074C RID: 1868 RVA: 0x0001712A File Offset: 0x0001532A
		public IconAttribute(string iconName = "", bool fixedColor = false, string runtimeIconTypeCallback = "")
		{
			this.iconName = iconName;
			this.fixedColor = fixedColor;
			this.runtimeIconTypeCallback = runtimeIconTypeCallback;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00017147 File Offset: 0x00015347
		public IconAttribute(Type fromType)
		{
			this.fromType = fromType;
		}

		// Token: 0x04000241 RID: 577
		public readonly string iconName;

		// Token: 0x04000242 RID: 578
		public readonly bool fixedColor;

		// Token: 0x04000243 RID: 579
		public readonly string runtimeIconTypeCallback;

		// Token: 0x04000244 RID: 580
		public readonly Type fromType;
	}
}
