using System;
using UnityEngine;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x02000009 RID: 9
	public class MinMaxAttribute : PropertyAttribute
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002123 File Offset: 0x00000323
		public MinMaxAttribute(float minValue, float maxValue, bool markNegativeArea = false)
		{
			this.MinValue = minValue;
			this.MaxValue = maxValue;
			this.MarkNegativeArea = markNegativeArea;
		}

		// Token: 0x0400000C RID: 12
		public bool MarkNegativeArea;

		// Token: 0x0400000D RID: 13
		public float MinValue;

		// Token: 0x0400000E RID: 14
		public float MaxValue;
	}
}
