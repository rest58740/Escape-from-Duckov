using System;
using UnityEngine;

namespace LeTai.TrueShadow
{
	// Token: 0x0200000D RID: 13
	[Serializable]
	public struct QuickPreset
	{
		// Token: 0x06000060 RID: 96 RVA: 0x0000330B File Offset: 0x0000150B
		public QuickPreset(string name, float size, float spread, float distance, float alpha)
		{
			this.name = name;
			this.size = size;
			this.spread = spread;
			this.distance = distance;
			this.alpha = alpha;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003334 File Offset: 0x00001534
		public void Apply(TrueShadow target)
		{
			target.Size = this.size;
			target.Spread = this.spread;
			target.OffsetDistance = this.distance;
			Color color = target.Color;
			color.a = this.alpha;
			target.Color = color;
		}

		// Token: 0x04000045 RID: 69
		public string name;

		// Token: 0x04000046 RID: 70
		[Min(0f)]
		public float size;

		// Token: 0x04000047 RID: 71
		[SpreadSlider]
		public float spread;

		// Token: 0x04000048 RID: 72
		[Min(0f)]
		public float distance;

		// Token: 0x04000049 RID: 73
		[Range(0f, 1f)]
		public float alpha;
	}
}
