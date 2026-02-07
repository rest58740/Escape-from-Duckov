using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003F3 RID: 1011
	public class FadeGroup : MonoBehaviour
	{
		// Token: 0x140000F9 RID: 249
		// (add) Token: 0x060024E1 RID: 9441 RVA: 0x00081280 File Offset: 0x0007F480
		// (remove) Token: 0x060024E2 RID: 9442 RVA: 0x000812B8 File Offset: 0x0007F4B8
		public event Action<FadeGroup> OnFadeComplete;

		// Token: 0x140000FA RID: 250
		// (add) Token: 0x060024E3 RID: 9443 RVA: 0x000812F0 File Offset: 0x0007F4F0
		// (remove) Token: 0x060024E4 RID: 9444 RVA: 0x00081328 File Offset: 0x0007F528
		public event Action<FadeGroup> OnShowComplete;

		// Token: 0x140000FB RID: 251
		// (add) Token: 0x060024E5 RID: 9445 RVA: 0x00081360 File Offset: 0x0007F560
		// (remove) Token: 0x060024E6 RID: 9446 RVA: 0x00081398 File Offset: 0x0007F598
		public event Action<FadeGroup> OnHideComplete;

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060024E7 RID: 9447 RVA: 0x000813CD File Offset: 0x0007F5CD
		public bool IsHidingInProgress
		{
			get
			{
				return this.isHidingInProgress;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060024E8 RID: 9448 RVA: 0x000813D5 File Offset: 0x0007F5D5
		public bool IsShowingInProgress
		{
			get
			{
				return this.isShowingInProgress;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060024E9 RID: 9449 RVA: 0x000813DD File Offset: 0x0007F5DD
		public bool IsShown
		{
			get
			{
				return this.isShown;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060024EA RID: 9450 RVA: 0x000813E5 File Offset: 0x0007F5E5
		public bool IsHidden
		{
			get
			{
				return !this.isShown;
			}
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000813F0 File Offset: 0x0007F5F0
		private void Start()
		{
			if (this.skipHideOnStart)
			{
				this.SkipHide();
			}
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x00081400 File Offset: 0x0007F600
		private void OnEnable()
		{
			if (this.showOnEnable)
			{
				this.Show();
			}
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x00081410 File Offset: 0x0007F610
		[ContextMenu("Show")]
		public void Show()
		{
			if (this == null || base.gameObject == null)
			{
				return;
			}
			if (this.debug)
			{
				Debug.Log("Fadegroup SHOW " + base.name);
			}
			this.skipHideOnStart = false;
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(true);
			}
			this.ShowTask().Forget();
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x00081478 File Offset: 0x0007F678
		[ContextMenu("Hide")]
		public void Hide()
		{
			if (this == null || base.gameObject == null)
			{
				return;
			}
			if (this.debug)
			{
				Debug.Log("Fadegroup HIDE " + base.name, base.gameObject);
			}
			this.HideTask().Forget();
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x000814CB File Offset: 0x0007F6CB
		public void Toggle()
		{
			if (this.IsShown)
			{
				this.Hide();
				return;
			}
			if (this.IsHidden)
			{
				this.Show();
			}
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x000814EA File Offset: 0x0007F6EA
		public UniTask ShowAndReturnTask()
		{
			if (this.skipHideBeforeShow)
			{
				this.SkipHide();
			}
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(true);
			}
			return this.ShowTask();
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x00081514 File Offset: 0x0007F714
		public UniTask HideAndReturnTask()
		{
			return this.HideTask();
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x0008151C File Offset: 0x0007F71C
		private int CacheNewTaskToken()
		{
			this.activeTaskToken = Random.Range(0, int.MaxValue);
			return this.activeTaskToken;
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x00081538 File Offset: 0x0007F738
		public UniTask ShowTask()
		{
			FadeGroup.<ShowTask>d__35 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<FadeGroup.<ShowTask>d__35>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x0008157C File Offset: 0x0007F77C
		public UniTask HideTask()
		{
			FadeGroup.<HideTask>d__36 <HideTask>d__;
			<HideTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<HideTask>d__.<>4__this = this;
			<HideTask>d__.<>1__state = -1;
			<HideTask>d__.<>t__builder.Start<FadeGroup.<HideTask>d__36>(ref <HideTask>d__);
			return <HideTask>d__.<>t__builder.Task;
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000815BF File Offset: 0x0007F7BF
		private void ShowComplete()
		{
			this.isShowingInProgress = false;
			Action<FadeGroup> onFadeComplete = this.OnFadeComplete;
			if (onFadeComplete != null)
			{
				onFadeComplete(this);
			}
			Action<FadeGroup> onShowComplete = this.OnShowComplete;
			if (onShowComplete == null)
			{
				return;
			}
			onShowComplete(this);
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x000815EC File Offset: 0x0007F7EC
		private void HideComplete()
		{
			this.isHidingInProgress = false;
			Action<FadeGroup> onFadeComplete = this.OnFadeComplete;
			if (onFadeComplete != null)
			{
				onFadeComplete(this);
			}
			Action<FadeGroup> onHideComplete = this.OnHideComplete;
			if (onHideComplete != null)
			{
				onHideComplete(this);
			}
			if (this == null)
			{
				return;
			}
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x00081644 File Offset: 0x0007F844
		public void SkipHide()
		{
			foreach (FadeElement fadeElement in this.fadeElements)
			{
				if (fadeElement == null)
				{
					Debug.LogWarning("Element in fade group " + base.name + " is null");
				}
				else
				{
					fadeElement.SkipHide();
				}
			}
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(false);
			}
			this.isShown = false;
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060024F8 RID: 9464 RVA: 0x000816D8 File Offset: 0x0007F8D8
		public bool IsFading
		{
			get
			{
				return this.fadeElements.Any((FadeElement e) => e != null && e.IsFading);
			}
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x00081704 File Offset: 0x0007F904
		internal void SkipShow()
		{
			foreach (FadeElement fadeElement in this.fadeElements)
			{
				if (fadeElement == null)
				{
					Debug.LogWarning("Element in fade group " + base.name + " is null");
				}
				else
				{
					fadeElement.SkipShow();
				}
			}
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(true);
			}
			this.isShown = true;
		}

		// Token: 0x04001907 RID: 6407
		[SerializeField]
		private List<FadeElement> fadeElements = new List<FadeElement>();

		// Token: 0x04001908 RID: 6408
		[SerializeField]
		private bool skipHideOnStart = true;

		// Token: 0x04001909 RID: 6409
		[SerializeField]
		private bool showOnEnable;

		// Token: 0x0400190A RID: 6410
		[SerializeField]
		private bool skipHideBeforeShow = true;

		// Token: 0x0400190E RID: 6414
		public bool manageGameObjectActive;

		// Token: 0x0400190F RID: 6415
		private bool isHidingInProgress;

		// Token: 0x04001910 RID: 6416
		private bool isShowingInProgress;

		// Token: 0x04001911 RID: 6417
		private bool isShown;

		// Token: 0x04001912 RID: 6418
		public bool debug;

		// Token: 0x04001913 RID: 6419
		private int activeTaskToken;
	}
}
