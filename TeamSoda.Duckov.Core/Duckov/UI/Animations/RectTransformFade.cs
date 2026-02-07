using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003F0 RID: 1008
	public class RectTransformFade : FadeElement
	{
		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060024B4 RID: 9396 RVA: 0x00080B3C File Offset: 0x0007ED3C
		private Vector2 TargetAnchoredPosition
		{
			get
			{
				return this.cachedAnchordPosition + this.offset;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x060024B5 RID: 9397 RVA: 0x00080B4F File Offset: 0x0007ED4F
		private Vector3 TargetScale
		{
			get
			{
				return this.cachedScale + Vector3.one * this.uniformScale;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x060024B6 RID: 9398 RVA: 0x00080B6C File Offset: 0x0007ED6C
		private Vector3 TargetRotation
		{
			get
			{
				return this.cachedRotation + Vector3.forward * this.rotateZ;
			}
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x00080B89 File Offset: 0x0007ED89
		private void Initialize()
		{
			if (this.initialized)
			{
				Debug.LogError("Object Initialized Twice, aborting");
				return;
			}
			this.CachePose();
			this.initialized = true;
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x00080BAC File Offset: 0x0007EDAC
		private void CachePose()
		{
			if (this.rectTransform == null)
			{
				return;
			}
			this.cachedAnchordPosition = this.rectTransform.anchoredPosition;
			this.cachedScale = this.rectTransform.localScale;
			this.cachedRotation = this.rectTransform.localRotation.eulerAngles;
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x00080C04 File Offset: 0x0007EE04
		private void Awake()
		{
			if (this.rectTransform == null || this.rectTransform.gameObject != base.gameObject)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x00080C51 File Offset: 0x0007EE51
		private void OnValidate()
		{
			if (this.rectTransform == null || this.rectTransform.gameObject != base.gameObject)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x00080C88 File Offset: 0x0007EE88
		protected override UniTask HideTask(int token)
		{
			RectTransformFade.<HideTask>d__22 <HideTask>d__;
			<HideTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<HideTask>d__.<>4__this = this;
			<HideTask>d__.<>1__state = -1;
			<HideTask>d__.<>t__builder.Start<RectTransformFade.<HideTask>d__22>(ref <HideTask>d__);
			return <HideTask>d__.<>t__builder.Task;
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x00080CCC File Offset: 0x0007EECC
		protected override UniTask ShowTask(int token)
		{
			RectTransformFade.<ShowTask>d__23 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<RectTransformFade.<ShowTask>d__23>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x00080D10 File Offset: 0x0007EF10
		protected override void OnSkipHide()
		{
			if (this.debug)
			{
				Debug.Log("OnSkipHide");
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.rectTransform.anchoredPosition = this.TargetAnchoredPosition;
			this.rectTransform.localScale = this.TargetScale;
			this.rectTransform.localRotation = Quaternion.Euler(this.TargetRotation);
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x00080D75 File Offset: 0x0007EF75
		private void OnDestroy()
		{
			RectTransform rectTransform = this.rectTransform;
			if (rectTransform == null)
			{
				return;
			}
			ShortcutExtensions.DOKill(rectTransform, false);
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x00080D8C File Offset: 0x0007EF8C
		protected override void OnSkipShow()
		{
			if (this.debug)
			{
				Debug.Log("OnSkipShow");
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.rectTransform.anchoredPosition = this.cachedAnchordPosition;
			this.rectTransform.localScale = this.cachedScale;
			this.rectTransform.localRotation = Quaternion.Euler(this.cachedRotation);
		}

		// Token: 0x040018EC RID: 6380
		[SerializeField]
		private bool debug;

		// Token: 0x040018ED RID: 6381
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x040018EE RID: 6382
		[SerializeField]
		private float duration = 0.4f;

		// Token: 0x040018EF RID: 6383
		[SerializeField]
		private Vector2 offset = Vector2.left * 10f;

		// Token: 0x040018F0 RID: 6384
		[SerializeField]
		[Range(-1f, 1f)]
		private float uniformScale;

		// Token: 0x040018F1 RID: 6385
		[SerializeField]
		[Range(-180f, 180f)]
		private float rotateZ;

		// Token: 0x040018F2 RID: 6386
		[SerializeField]
		private AnimationCurve showingAnimationCurve;

		// Token: 0x040018F3 RID: 6387
		[SerializeField]
		private AnimationCurve hidingAnimationCurve;

		// Token: 0x040018F4 RID: 6388
		private Vector2 cachedAnchordPosition = Vector2.zero;

		// Token: 0x040018F5 RID: 6389
		private Vector3 cachedScale = Vector3.one;

		// Token: 0x040018F6 RID: 6390
		private Vector3 cachedRotation = Vector3.zero;

		// Token: 0x040018F7 RID: 6391
		private bool initialized;
	}
}
