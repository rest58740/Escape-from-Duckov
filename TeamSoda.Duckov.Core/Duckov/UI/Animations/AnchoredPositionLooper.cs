using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003F4 RID: 1012
	public class AnchoredPositionLooper : LooperElement
	{
		// Token: 0x060024FB RID: 9467 RVA: 0x000817B9 File Offset: 0x0007F9B9
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x000817CC File Offset: 0x0007F9CC
		protected override void OnTick(LooperClock clock, float t)
		{
			if (this.rectTransform == null)
			{
				return;
			}
			Vector2 anchoredPosition = Vector2.Lerp(this.anchoredPositionA, this.anchoredPositionB, this.curve.Evaluate(t));
			this.rectTransform.anchoredPosition = anchoredPosition;
		}

		// Token: 0x04001914 RID: 6420
		[SerializeField]
		private Vector2 anchoredPositionA;

		// Token: 0x04001915 RID: 6421
		[SerializeField]
		private Vector2 anchoredPositionB;

		// Token: 0x04001916 RID: 6422
		[SerializeField]
		private AnimationCurve curve;

		// Token: 0x04001917 RID: 6423
		private RectTransform rectTransform;
	}
}
