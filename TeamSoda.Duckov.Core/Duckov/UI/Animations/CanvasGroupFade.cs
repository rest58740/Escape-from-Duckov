using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003EE RID: 1006
	[RequireComponent(typeof(CanvasGroup))]
	public class CanvasGroupFade : FadeElement
	{
		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002499 RID: 9369 RVA: 0x00080674 File Offset: 0x0007E874
		private float ShowingDuration
		{
			get
			{
				return this.fadeDuration;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x0600249A RID: 9370 RVA: 0x0008067C File Offset: 0x0007E87C
		private float HidingDuration
		{
			get
			{
				return this.fadeDuration;
			}
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x00080684 File Offset: 0x0007E884
		private void Awake()
		{
			if (this.canvasGroup == null || this.canvasGroup.gameObject != base.gameObject)
			{
				this.canvasGroup = base.GetComponent<CanvasGroup>();
			}
			this.awaked = true;
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x000806BF File Offset: 0x0007E8BF
		private void OnValidate()
		{
			if (this.canvasGroup == null || this.canvasGroup.gameObject != base.gameObject)
			{
				this.canvasGroup = base.GetComponent<CanvasGroup>();
			}
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x000806F4 File Offset: 0x0007E8F4
		protected override UniTask ShowTask(int taskToken)
		{
			if (this.canvasGroup == null)
			{
				return default(UniTask);
			}
			if (!this.awaked)
			{
				this.canvasGroup.alpha = 0f;
			}
			if (this.manageBlockRaycast)
			{
				this.canvasGroup.blocksRaycasts = true;
			}
			return this.FadeTask(taskToken, base.IsFading ? this.canvasGroup.alpha : 0f, 1f, this.showingCurve, this.ShowingDuration);
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x00080778 File Offset: 0x0007E978
		protected override UniTask HideTask(int taskToken)
		{
			if (this.canvasGroup == null)
			{
				return default(UniTask);
			}
			if (this.manageBlockRaycast)
			{
				this.canvasGroup.blocksRaycasts = false;
			}
			return this.FadeTask(taskToken, base.IsFading ? this.canvasGroup.alpha : 1f, 0f, this.hidingCurve, this.HidingDuration);
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x000807E4 File Offset: 0x0007E9E4
		private UniTask FadeTask(int token, float beginAlpha, float targetAlpha, AnimationCurve animationCurve, float duration)
		{
			CanvasGroupFade.<FadeTask>d__14 <FadeTask>d__;
			<FadeTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<FadeTask>d__.<>4__this = this;
			<FadeTask>d__.token = token;
			<FadeTask>d__.beginAlpha = beginAlpha;
			<FadeTask>d__.targetAlpha = targetAlpha;
			<FadeTask>d__.animationCurve = animationCurve;
			<FadeTask>d__.duration = duration;
			<FadeTask>d__.<>1__state = -1;
			<FadeTask>d__.<>t__builder.Start<CanvasGroupFade.<FadeTask>d__14>(ref <FadeTask>d__);
			return <FadeTask>d__.<>t__builder.Task;
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x00080851 File Offset: 0x0007EA51
		protected override void OnSkipHide()
		{
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = 0f;
			}
			if (this.manageBlockRaycast)
			{
				this.canvasGroup.blocksRaycasts = false;
			}
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x00080885 File Offset: 0x0007EA85
		protected override void OnSkipShow()
		{
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = 1f;
			}
			if (this.manageBlockRaycast)
			{
				this.canvasGroup.blocksRaycasts = true;
			}
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000808CC File Offset: 0x0007EACC
		[CompilerGenerated]
		private bool <FadeTask>g__CheckTaskValid|14_0(ref CanvasGroupFade.<>c__DisplayClass14_0 A_1)
		{
			return this.canvasGroup != null && A_1.token == base.ActiveTaskToken;
		}

		// Token: 0x040018DF RID: 6367
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x040018E0 RID: 6368
		[SerializeField]
		private AnimationCurve showingCurve;

		// Token: 0x040018E1 RID: 6369
		[SerializeField]
		private AnimationCurve hidingCurve;

		// Token: 0x040018E2 RID: 6370
		[SerializeField]
		private float fadeDuration = 0.2f;

		// Token: 0x040018E3 RID: 6371
		[SerializeField]
		private bool manageBlockRaycast;

		// Token: 0x040018E4 RID: 6372
		private bool awaked;
	}
}
