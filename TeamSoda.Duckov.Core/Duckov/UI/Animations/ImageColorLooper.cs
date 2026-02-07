using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI.Animations
{
	// Token: 0x020003F5 RID: 1013
	public class ImageColorLooper : LooperElement
	{
		// Token: 0x060024FE RID: 9470 RVA: 0x0008181C File Offset: 0x0007FA1C
		protected override void OnTick(LooperClock clock, float t)
		{
			Color color = this.colorOverT.Evaluate(t);
			float num = this.alphaOverT.Evaluate(t);
			color.a *= num;
			this.image.color = color;
		}

		// Token: 0x04001918 RID: 6424
		[SerializeField]
		private Image image;

		// Token: 0x04001919 RID: 6425
		[GradientUsage(true)]
		[SerializeField]
		private Gradient colorOverT;

		// Token: 0x0400191A RID: 6426
		[SerializeField]
		private AnimationCurve alphaOverT;
	}
}
