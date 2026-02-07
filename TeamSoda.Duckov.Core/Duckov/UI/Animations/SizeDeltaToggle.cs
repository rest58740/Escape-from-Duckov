using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003FF RID: 1023
	public class SizeDeltaToggle : ToggleAnimation
	{
		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06002522 RID: 9506 RVA: 0x00081E9E File Offset: 0x0008009E
		private RectTransform RectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = base.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x00081EC0 File Offset: 0x000800C0
		private void CachePose()
		{
			this.cachedSizeDelta = this.RectTransform.sizeDelta;
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x00081ED3 File Offset: 0x000800D3
		private void Awake()
		{
			this.CachePose();
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x00081EDC File Offset: 0x000800DC
		protected override void OnSetToggle(bool status)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			Vector2 vector = status ? this.activeSizeDelta : this.idleSizeDelta;
			ShortcutExtensions.DOKill(this.RectTransform, false);
			TweenSettingsExtensions.SetLink<TweenerCore<Vector2, Vector2, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(DOTweenModuleUI.DOSizeDelta(this.RectTransform, vector, this.duration, false), this.animationCurve), base.gameObject);
		}

		// Token: 0x04001939 RID: 6457
		public Vector2 idleSizeDelta = Vector2.zero;

		// Token: 0x0400193A RID: 6458
		public Vector2 activeSizeDelta = Vector2.one * 12f;

		// Token: 0x0400193B RID: 6459
		public float duration = 0.1f;

		// Token: 0x0400193C RID: 6460
		public AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x0400193D RID: 6461
		private Vector2 cachedSizeDelta = Vector3.one;

		// Token: 0x0400193E RID: 6462
		private RectTransform _rectTransform;
	}
}
