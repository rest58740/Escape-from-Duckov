using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003F6 RID: 1014
	public class LocalPositionLooper : LooperElement
	{
		// Token: 0x06002500 RID: 9472 RVA: 0x00081864 File Offset: 0x0007FA64
		protected override void OnTick(LooperClock clock, float t)
		{
			if (base.transform == null)
			{
				return;
			}
			Vector2 vector = Vector2.Lerp(this.localPositionA, this.localPositionB, this.curve.Evaluate(t));
			base.transform.localPosition = vector;
		}

		// Token: 0x0400191B RID: 6427
		[SerializeField]
		private Vector3 localPositionA;

		// Token: 0x0400191C RID: 6428
		[SerializeField]
		private Vector3 localPositionB;

		// Token: 0x0400191D RID: 6429
		[SerializeField]
		private AnimationCurve curve;
	}
}
