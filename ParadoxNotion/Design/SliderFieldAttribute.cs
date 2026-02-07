using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000E2 RID: 226
	[AttributeUsage(256)]
	public class SliderFieldAttribute : DrawerAttribute
	{
		// Token: 0x0600076B RID: 1899 RVA: 0x0001724A File Offset: 0x0001544A
		public SliderFieldAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00017260 File Offset: 0x00015460
		public SliderFieldAttribute(int min, int max)
		{
			this.min = (float)min;
			this.max = (float)max;
		}

		// Token: 0x0400024F RID: 591
		public readonly float min;

		// Token: 0x04000250 RID: 592
		public readonly float max;
	}
}
