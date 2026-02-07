using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003F1 RID: 1009
	public class ScaleFade : FadeElement
	{
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060024C7 RID: 9415 RVA: 0x00080EEB File Offset: 0x0007F0EB
		private Vector3 HiddenScale
		{
			get
			{
				return Vector3.one + Vector3.one * this.uniformScale + this.scale;
			}
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x00080F12 File Offset: 0x0007F112
		private void CachePose()
		{
			this.cachedScale = base.transform.localScale;
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x00080F25 File Offset: 0x0007F125
		private void RestorePose()
		{
			base.transform.localScale = this.cachedScale;
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x00080F38 File Offset: 0x0007F138
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.initialized = true;
			this.CachePose();
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x00080F50 File Offset: 0x0007F150
		protected override UniTask HideTask(int token)
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			if (!base.transform)
			{
				return UniTask.CompletedTask;
			}
			return TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(base.transform, this.HiddenScale, this.duration), this.hideCurve).ToUniTask(TweenCancelBehaviour.Kill, default(CancellationToken));
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x00080FAF File Offset: 0x0007F1AF
		protected override void OnSkipHide()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			base.transform.localScale = this.HiddenScale;
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x00080FD0 File Offset: 0x0007F1D0
		protected override void OnSkipShow()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.RestorePose();
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00080FE8 File Offset: 0x0007F1E8
		protected override UniTask ShowTask(int token)
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			return TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(base.transform, this.cachedScale, this.duration), this.showCurve), new TweenCallback(this.RestorePose)).ToUniTask(TweenCancelBehaviour.Kill, default(CancellationToken));
		}

		// Token: 0x040018F8 RID: 6392
		[SerializeField]
		private float duration = 0.1f;

		// Token: 0x040018F9 RID: 6393
		[SerializeField]
		private Vector3 scale = Vector3.zero;

		// Token: 0x040018FA RID: 6394
		[SerializeField]
		[Range(-1f, 1f)]
		private float uniformScale;

		// Token: 0x040018FB RID: 6395
		[SerializeField]
		private AnimationCurve showCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040018FC RID: 6396
		[SerializeField]
		private AnimationCurve hideCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x040018FD RID: 6397
		private Vector3 cachedScale = Vector3.one;

		// Token: 0x040018FE RID: 6398
		private bool initialized;
	}
}
