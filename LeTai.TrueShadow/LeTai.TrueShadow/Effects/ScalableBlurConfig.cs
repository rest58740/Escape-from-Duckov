using System;
using UnityEngine;

namespace LeTai.Effects
{
	// Token: 0x0200002B RID: 43
	public class ScalableBlurConfig : BlurConfig
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00006A8E File Offset: 0x00004C8E
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00006A96 File Offset: 0x00004C96
		public float Radius
		{
			get
			{
				return this.radius;
			}
			set
			{
				this.radius = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00006AA9 File Offset: 0x00004CA9
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00006AB1 File Offset: 0x00004CB1
		public int Iteration
		{
			get
			{
				return this.iteration;
			}
			set
			{
				this.iteration = Mathf.Max(0, value);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00006AC0 File Offset: 0x00004CC0
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00006AC8 File Offset: 0x00004CC8
		public int MaxDepth
		{
			get
			{
				return this.maxDepth;
			}
			set
			{
				this.maxDepth = Mathf.Max(1, value);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00006AD8 File Offset: 0x00004CD8
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00006B0B File Offset: 0x00004D0B
		public float Strength
		{
			get
			{
				return this.strength = this.radius * (float)(3 * (1 << this.iteration) - 2) / ScalableBlurConfig.UNIT_VARIANCE;
			}
			set
			{
				this.strength = Mathf.Max(0f, value);
				this.SetAdvancedFieldFromSimple();
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00006B24 File Offset: 0x00004D24
		protected virtual void SetAdvancedFieldFromSimple()
		{
			if ((double)this.strength < 0.01)
			{
				this.iteration = 0;
				this.radius = 0f;
				return;
			}
			float num = this.strength * ScalableBlurConfig.UNIT_VARIANCE;
			this.iteration = Mathf.CeilToInt(Mathf.Log(0.16666667f * (Mathf.Sqrt(12f * num + 1f) + 5f)) / Mathf.Log(2f));
			this.radius = num / (float)(3 * (1 << this.iteration) - 2);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00006BB4 File Offset: 0x00004DB4
		private void OnValidate()
		{
			this.SetAdvancedFieldFromSimple();
		}

		// Token: 0x040000B9 RID: 185
		[SerializeField]
		private float radius = 4f;

		// Token: 0x040000BA RID: 186
		[SerializeField]
		private int iteration = 4;

		// Token: 0x040000BB RID: 187
		[SerializeField]
		private int maxDepth = 6;

		// Token: 0x040000BC RID: 188
		[SerializeField]
		[Range(0f, 256f)]
		private float strength;

		// Token: 0x040000BD RID: 189
		private static readonly float UNIT_VARIANCE = 1f + Mathf.Sqrt(2f) / 2f;
	}
}
