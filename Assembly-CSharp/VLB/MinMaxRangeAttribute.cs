using System;

namespace VLB
{
	// Token: 0x02000033 RID: 51
	public class MinMaxRangeAttribute : Attribute
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00006D16 File Offset: 0x00004F16
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00006D1E File Offset: 0x00004F1E
		public float minValue { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00006D27 File Offset: 0x00004F27
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00006D2F File Offset: 0x00004F2F
		public float maxValue { get; private set; }

		// Token: 0x06000178 RID: 376 RVA: 0x00006D38 File Offset: 0x00004F38
		public MinMaxRangeAttribute(float min, float max)
		{
			this.minValue = min;
			this.maxValue = max;
		}
	}
}
