using System;
using UnityEngine;

namespace Duckov.Weathers
{
	// Token: 0x02000252 RID: 594
	[Serializable]
	public class Precipitation
	{
		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060012BA RID: 4794 RVA: 0x00047EB2 File Offset: 0x000460B2
		public float CloudyThreshold
		{
			get
			{
				return this.cloudyThreshold;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060012BB RID: 4795 RVA: 0x00047EBA File Offset: 0x000460BA
		public float RainyThreshold
		{
			get
			{
				return this.rainyThreshold;
			}
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00047EC2 File Offset: 0x000460C2
		public bool IsRainy(TimeSpan dayAndTime)
		{
			return this.Get(dayAndTime) > this.rainyThreshold;
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00047ED3 File Offset: 0x000460D3
		public bool IsCloudy(TimeSpan dayAndTime)
		{
			return this.Get(dayAndTime) > this.cloudyThreshold;
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00047EE4 File Offset: 0x000460E4
		public float Get(TimeSpan dayAndTime)
		{
			Vector2 perlinNoiseCoord = this.GetPerlinNoiseCoord(dayAndTime);
			return Mathf.Clamp01(((Mathf.PerlinNoise(perlinNoiseCoord.x, perlinNoiseCoord.y) + Mathf.PerlinNoise(perlinNoiseCoord.x + 0.5f + 123.4f, perlinNoiseCoord.y - 567.8f)) / 2f - 0.5f) * this.contrast + 0.5f + this.offset);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00047F54 File Offset: 0x00046154
		public Vector2 GetPerlinNoiseCoord(TimeSpan dayAndTime)
		{
			float num = (float)(dayAndTime.Days % 3650) * 24f + (float)dayAndTime.Hours + (float)dayAndTime.Minutes / 60f;
			int num2 = dayAndTime.Days / 3650;
			return new Vector2(num * this.frequency, (float)(this.seed + num2));
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00047FB0 File Offset: 0x000461B0
		internal void SetSeed(int seed)
		{
			this.seed = seed;
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00047FBC File Offset: 0x000461BC
		public float Get()
		{
			TimeSpan now = GameClock.Now;
			return this.Get(now);
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00047FD8 File Offset: 0x000461D8
		public bool IsRainy()
		{
			TimeSpan now = GameClock.Now;
			return this.IsRainy(now);
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00047FF4 File Offset: 0x000461F4
		public bool IsCloudy()
		{
			TimeSpan now = GameClock.Now;
			return this.IsCloudy(now);
		}

		// Token: 0x04000E66 RID: 3686
		[SerializeField]
		private int seed;

		// Token: 0x04000E67 RID: 3687
		[SerializeField]
		[Range(0f, 1f)]
		private float cloudyThreshold;

		// Token: 0x04000E68 RID: 3688
		[SerializeField]
		[Range(0f, 1f)]
		private float rainyThreshold;

		// Token: 0x04000E69 RID: 3689
		[SerializeField]
		private float frequency = 1f;

		// Token: 0x04000E6A RID: 3690
		[SerializeField]
		private float offset;

		// Token: 0x04000E6B RID: 3691
		[SerializeField]
		private float contrast = 1f;
	}
}
