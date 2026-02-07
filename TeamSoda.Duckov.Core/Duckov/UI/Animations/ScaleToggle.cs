using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003FE RID: 1022
	public class ScaleToggle : ToggleAnimation
	{
		// Token: 0x0600251E RID: 9502 RVA: 0x00081DB0 File Offset: 0x0007FFB0
		private void CachePose()
		{
			this.cachedScale = this.rectTransform.localScale;
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x00081DC3 File Offset: 0x0007FFC3
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.CachePose();
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x00081DDC File Offset: 0x0007FFDC
		protected override void OnSetToggle(bool status)
		{
			float num = status ? this.activeScale : this.idleScale;
			num * this.cachedScale;
			ShortcutExtensions.DOKill(this.rectTransform, false);
			TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(this.rectTransform, this.cachedScale * num, this.duration), this.animationCurve);
		}

		// Token: 0x04001933 RID: 6451
		public float idleScale = 1f;

		// Token: 0x04001934 RID: 6452
		public float activeScale = 0.9f;

		// Token: 0x04001935 RID: 6453
		public float duration = 0.1f;

		// Token: 0x04001936 RID: 6454
		public AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001937 RID: 6455
		private Vector3 cachedScale = Vector3.one;

		// Token: 0x04001938 RID: 6456
		private RectTransform rectTransform;
	}
}
