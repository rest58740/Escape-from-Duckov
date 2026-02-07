using System;
using UnityEngine;

namespace LeTai.Asset.TranslucentImage
{
	// Token: 0x02000009 RID: 9
	[CreateAssetMenu(fileName = "New Scalable Blur Config", menuName = "Translucent Image/ Scalable Blur Config", order = 100)]
	public class ScalableBlurConfig : BlurConfig
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000023FD File Offset: 0x000005FD
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002405 File Offset: 0x00000605
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

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002418 File Offset: 0x00000618
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002420 File Offset: 0x00000620
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

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002430 File Offset: 0x00000630
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002460 File Offset: 0x00000660
		public float Strength
		{
			get
			{
				return this.strength = this.Radius * Mathf.Pow(2f, (float)this.Iteration);
			}
			set
			{
				this.strength = Mathf.Clamp(value, 0f, 268435460f);
				this.radius = Mathf.Sqrt(this.strength);
				this.iteration = 0;
				while ((float)(1 << this.iteration) < this.radius)
				{
					this.iteration++;
				}
				this.radius = this.strength / (float)(1 << this.iteration);
			}
		}

		// Token: 0x0400000E RID: 14
		[SerializeField]
		[Tooltip("Blurriness. Does NOT affect performance")]
		private float radius = 4f;

		// Token: 0x0400000F RID: 15
		[SerializeField]
		[Tooltip("The number of times to run the algorithm to increase the smoothness of the effect. Can affect performance when increase")]
		[Range(0f, 8f)]
		private int iteration = 4;

		// Token: 0x04000010 RID: 16
		[SerializeField]
		[Tooltip("How strong the blur is")]
		private float strength;
	}
}
