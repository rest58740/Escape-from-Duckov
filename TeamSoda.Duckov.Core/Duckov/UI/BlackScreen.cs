using System;
using Cysharp.Threading.Tasks;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000393 RID: 915
	public class BlackScreen : MonoBehaviour
	{
		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06001FCC RID: 8140 RVA: 0x000702C1 File Offset: 0x0006E4C1
		public static BlackScreen Instance
		{
			get
			{
				return GameManager.BlackScreen;
			}
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x000702C8 File Offset: 0x0006E4C8
		private void Awake()
		{
			if (BlackScreen.Instance != this)
			{
				Debug.LogError("检测到应当删除的BlackScreen实例", base.gameObject);
			}
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x000702E7 File Offset: 0x0006E4E7
		private void SetFadeCurve(AnimationCurve curve)
		{
			this.fadeElement.ShowCurve = curve;
			this.fadeElement.HideCurve = curve;
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x00070301 File Offset: 0x0006E501
		private void SetCircleFade(float circleFade)
		{
			this.fadeImage.material.SetFloat("_CircleFade", circleFade);
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x0007031C File Offset: 0x0006E51C
		private UniTask LShowAndReturnTask(AnimationCurve animationCurve = null, float circleFade = 0f, float duration = -1f)
		{
			this.taskCounter++;
			if (this.taskCounter > 1)
			{
				return UniTask.CompletedTask;
			}
			this.fadeElement.Duration = ((duration > 0f) ? duration : this.defaultDuration);
			if (animationCurve == null)
			{
				this.SetFadeCurve(this.defaultShowCurve);
			}
			else
			{
				this.SetFadeCurve(animationCurve);
			}
			this.SetCircleFade(circleFade);
			return this.fadeGroup.ShowAndReturnTask();
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x0007038C File Offset: 0x0006E58C
		private UniTask LHideAndReturnTask(AnimationCurve animationCurve = null, float circleFade = 0f, float duration = -1f)
		{
			int num = this.taskCounter - 1;
			this.taskCounter = num;
			if (num > 0)
			{
				return UniTask.CompletedTask;
			}
			this.fadeElement.Duration = ((duration > 0f) ? duration : this.defaultDuration);
			if (animationCurve == null)
			{
				this.SetFadeCurve(this.defaultHideCurve);
			}
			else
			{
				this.SetFadeCurve(animationCurve);
			}
			this.SetCircleFade(circleFade);
			return this.fadeGroup.HideAndReturnTask();
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x000703F9 File Offset: 0x0006E5F9
		public static UniTask ShowAndReturnTask(AnimationCurve animationCurve = null, float circleFade = 0f, float duration = 0.5f)
		{
			if (BlackScreen.Instance == null)
			{
				return UniTask.CompletedTask;
			}
			return BlackScreen.Instance.LShowAndReturnTask(animationCurve, circleFade, duration);
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0007041B File Offset: 0x0006E61B
		public static UniTask HideAndReturnTask(AnimationCurve animationCurve = null, float circleFade = 0f, float duration = 0.5f)
		{
			if (BlackScreen.Instance == null)
			{
				return UniTask.CompletedTask;
			}
			return BlackScreen.Instance.LHideAndReturnTask(animationCurve, circleFade, duration);
		}

		// Token: 0x040015BF RID: 5567
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040015C0 RID: 5568
		[SerializeField]
		private MaterialPropertyFade fadeElement;

		// Token: 0x040015C1 RID: 5569
		[SerializeField]
		private Image fadeImage;

		// Token: 0x040015C2 RID: 5570
		[SerializeField]
		private float defaultDuration = 0.5f;

		// Token: 0x040015C3 RID: 5571
		[SerializeField]
		private AnimationCurve defaultShowCurve;

		// Token: 0x040015C4 RID: 5572
		[SerializeField]
		private AnimationCurve defaultHideCurve;

		// Token: 0x040015C5 RID: 5573
		private int taskCounter;
	}
}
