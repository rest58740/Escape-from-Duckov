using System;
using DG.Tweening;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003A8 RID: 936
	public class PunchReceiver : MonoBehaviour
	{
		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x060020B1 RID: 8369 RVA: 0x00072A83 File Offset: 0x00070C83
		private float PunchAnchorPositionDuration
		{
			get
			{
				return this.duration;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060020B2 RID: 8370 RVA: 0x00072A8B File Offset: 0x00070C8B
		private float PunchScaleDuration
		{
			get
			{
				return this.duration;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060020B3 RID: 8371 RVA: 0x00072A93 File Offset: 0x00070C93
		private float PunchRotationDuration
		{
			get
			{
				return this.duration;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060020B4 RID: 8372 RVA: 0x00072A9B File Offset: 0x00070C9B
		private bool ShouldPunchPosition
		{
			get
			{
				return this.randomAnchorPosition.magnitude > 0.001f && this.punchAnchorPosition.magnitude > 0.001f;
			}
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x00072AC3 File Offset: 0x00070CC3
		private void Awake()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			this.CachePose();
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x00072AE5 File Offset: 0x00070CE5
		private void Start()
		{
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00072AE8 File Offset: 0x00070CE8
		[ContextMenu("Punch")]
		public void Punch()
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.rectTransform == null)
			{
				return;
			}
			if (this.particle != null)
			{
				this.particle.Play();
			}
			ShortcutExtensions.DOKill(this.rectTransform, false);
			if (this.cacheWhenPunched)
			{
				this.CachePose();
			}
			Vector2 vector = this.punchAnchorPosition + new Vector2(Random.Range(-this.randomAnchorPosition.x, this.randomAnchorPosition.x), Random.Range(-this.randomAnchorPosition.y, this.randomAnchorPosition.y));
			float num = this.punchScaleUniform;
			float num2 = this.punchRotationZ + Random.Range(-this.randomRotationZ, this.randomRotationZ);
			if (this.ShouldPunchPosition)
			{
				TweenSettingsExtensions.OnKill<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOPunchAnchorPos(this.rectTransform, vector, this.PunchAnchorPositionDuration, this.vibrato, this.elasticity, false), this.animationCurve), new TweenCallback(this.RestorePose));
			}
			TweenSettingsExtensions.OnKill<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOPunchScale(this.rectTransform, Vector3.one * num, this.PunchScaleDuration, this.vibrato, this.elasticity), this.animationCurve), new TweenCallback(this.RestorePose));
			TweenSettingsExtensions.OnKill<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOPunchRotation(this.rectTransform, Vector3.forward * num2, this.PunchRotationDuration, this.vibrato, this.elasticity), this.animationCurve), new TweenCallback(this.RestorePose));
			if (!string.IsNullOrWhiteSpace(this.sfx))
			{
				AudioManager.Post(this.sfx);
			}
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x00072C94 File Offset: 0x00070E94
		private void CachePose()
		{
			if (this.rectTransform == null)
			{
				return;
			}
			this.cachedAnchorPosition = this.rectTransform.anchoredPosition;
			this.cachedScale = this.rectTransform.localScale;
			this.cachedRotation = this.rectTransform.localRotation.eulerAngles;
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x00072CF8 File Offset: 0x00070EF8
		private void RestorePose()
		{
			if (this.rectTransform == null)
			{
				return;
			}
			if (this.ShouldPunchPosition)
			{
				this.rectTransform.anchoredPosition = this.cachedAnchorPosition;
			}
			this.rectTransform.localScale = this.cachedScale;
			this.rectTransform.localRotation = Quaternion.Euler(this.cachedRotation);
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x00072D5E File Offset: 0x00070F5E
		private void OnDestroy()
		{
			RectTransform rectTransform = this.rectTransform;
			if (rectTransform == null)
			{
				return;
			}
			ShortcutExtensions.DOKill(rectTransform, false);
		}

		// Token: 0x0400164D RID: 5709
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x0400164E RID: 5710
		[SerializeField]
		private ParticleSystem particle;

		// Token: 0x0400164F RID: 5711
		[Min(0.0001f)]
		[SerializeField]
		private float duration = 0.01f;

		// Token: 0x04001650 RID: 5712
		public int vibrato = 10;

		// Token: 0x04001651 RID: 5713
		public float elasticity = 1f;

		// Token: 0x04001652 RID: 5714
		[SerializeField]
		private Vector2 punchAnchorPosition;

		// Token: 0x04001653 RID: 5715
		[SerializeField]
		[Range(-1f, 1f)]
		private float punchScaleUniform;

		// Token: 0x04001654 RID: 5716
		[SerializeField]
		[Range(-180f, 180f)]
		private float punchRotationZ;

		// Token: 0x04001655 RID: 5717
		[SerializeField]
		private Vector2 randomAnchorPosition;

		// Token: 0x04001656 RID: 5718
		[SerializeField]
		[Range(0f, 180f)]
		private float randomRotationZ;

		// Token: 0x04001657 RID: 5719
		[SerializeField]
		private AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04001658 RID: 5720
		[SerializeField]
		private bool cacheWhenPunched;

		// Token: 0x04001659 RID: 5721
		[SerializeField]
		private string sfx;

		// Token: 0x0400165A RID: 5722
		private Vector2 cachedAnchorPosition;

		// Token: 0x0400165B RID: 5723
		private Vector2 cachedScale;

		// Token: 0x0400165C RID: 5724
		private Vector2 cachedRotation;
	}
}
