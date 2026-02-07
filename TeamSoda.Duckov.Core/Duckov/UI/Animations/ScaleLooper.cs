using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003FA RID: 1018
	public class ScaleLooper : LooperElement
	{
		// Token: 0x0600250E RID: 9486 RVA: 0x00081A6C File Offset: 0x0007FC6C
		protected override void OnTick(LooperClock clock, float t)
		{
			float num = this.xOverT.Evaluate(t);
			float num2 = this.yOverT.Evaluate(t);
			float num3 = this.zOverT.Evaluate(t);
			float num4 = this.uniformScaleOverT.Evaluate(t);
			num *= num4;
			num2 *= num4;
			num3 *= num4;
			base.transform.localScale = new Vector3(num, num2, num3);
		}

		// Token: 0x04001925 RID: 6437
		[SerializeField]
		private AnimationCurve uniformScaleOverT = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x04001926 RID: 6438
		[SerializeField]
		private AnimationCurve xOverT = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x04001927 RID: 6439
		[SerializeField]
		private AnimationCurve yOverT = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x04001928 RID: 6440
		[SerializeField]
		private AnimationCurve zOverT = AnimationCurve.Linear(0f, 1f, 1f, 1f);
	}
}
