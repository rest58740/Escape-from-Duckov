using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003F2 RID: 1010
	public abstract class FadeElement : MonoBehaviour
	{
		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060024D0 RID: 9424 RVA: 0x000810BA File Offset: 0x0007F2BA
		public UniTask ActiveTask
		{
			get
			{
				return this.activeTask;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060024D1 RID: 9425 RVA: 0x000810C2 File Offset: 0x0007F2C2
		protected int ActiveTaskToken
		{
			get
			{
				return this.activeTaskToken;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060024D2 RID: 9426 RVA: 0x000810CA File Offset: 0x0007F2CA
		protected bool ManageGameObjectActive
		{
			get
			{
				return this.manageGameObjectActive;
			}
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x000810D2 File Offset: 0x0007F2D2
		private void CacheNewTaskToken()
		{
			this.activeTaskToken = Random.Range(1, int.MaxValue);
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060024D4 RID: 9428 RVA: 0x000810E5 File Offset: 0x0007F2E5
		// (set) Token: 0x060024D5 RID: 9429 RVA: 0x000810ED File Offset: 0x0007F2ED
		public bool IsFading { get; private set; }

		// Token: 0x060024D6 RID: 9430 RVA: 0x000810F8 File Offset: 0x0007F2F8
		public UniTask Show(float delay = 0f)
		{
			FadeElement.<Show>d__18 <Show>d__;
			<Show>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Show>d__.<>4__this = this;
			<Show>d__.delay = delay;
			<Show>d__.<>1__state = -1;
			<Show>d__.<>t__builder.Start<FadeElement.<Show>d__18>(ref <Show>d__);
			return <Show>d__.<>t__builder.Task;
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x00081144 File Offset: 0x0007F344
		public UniTask Hide()
		{
			FadeElement.<Hide>d__19 <Hide>d__;
			<Hide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Hide>d__.<>4__this = this;
			<Hide>d__.<>1__state = -1;
			<Hide>d__.<>t__builder.Start<FadeElement.<Hide>d__19>(ref <Hide>d__);
			return <Hide>d__.<>t__builder.Task;
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x00081188 File Offset: 0x0007F388
		private UniTask WrapShowTask(int token, float delay = 0f)
		{
			FadeElement.<WrapShowTask>d__20 <WrapShowTask>d__;
			<WrapShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WrapShowTask>d__.<>4__this = this;
			<WrapShowTask>d__.token = token;
			<WrapShowTask>d__.delay = delay;
			<WrapShowTask>d__.<>1__state = -1;
			<WrapShowTask>d__.<>t__builder.Start<FadeElement.<WrapShowTask>d__20>(ref <WrapShowTask>d__);
			return <WrapShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x000811DC File Offset: 0x0007F3DC
		private UniTask WrapHideTask(int token, float delay = 0f)
		{
			FadeElement.<WrapHideTask>d__21 <WrapHideTask>d__;
			<WrapHideTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WrapHideTask>d__.<>4__this = this;
			<WrapHideTask>d__.token = token;
			<WrapHideTask>d__.delay = delay;
			<WrapHideTask>d__.<>1__state = -1;
			<WrapHideTask>d__.<>t__builder.Start<FadeElement.<WrapHideTask>d__21>(ref <WrapHideTask>d__);
			return <WrapHideTask>d__.<>t__builder.Task;
		}

		// Token: 0x060024DA RID: 9434
		protected abstract UniTask ShowTask(int token);

		// Token: 0x060024DB RID: 9435
		protected abstract UniTask HideTask(int token);

		// Token: 0x060024DC RID: 9436
		protected abstract void OnSkipHide();

		// Token: 0x060024DD RID: 9437
		protected abstract void OnSkipShow();

		// Token: 0x060024DE RID: 9438 RVA: 0x0008122F File Offset: 0x0007F42F
		public void SkipHide()
		{
			this.activeTaskToken = 0;
			this.OnSkipHide();
			if (this.ManageGameObjectActive)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x00081252 File Offset: 0x0007F452
		internal void SkipShow()
		{
			this.activeTaskToken = 0;
			this.OnSkipShow();
			if (this.ManageGameObjectActive)
			{
				base.gameObject.SetActive(true);
			}
		}

		// Token: 0x040018FF RID: 6399
		protected UniTask activeTask;

		// Token: 0x04001900 RID: 6400
		private int activeTaskToken;

		// Token: 0x04001901 RID: 6401
		[SerializeField]
		private bool manageGameObjectActive;

		// Token: 0x04001902 RID: 6402
		[SerializeField]
		private float delay;

		// Token: 0x04001903 RID: 6403
		[SerializeField]
		private string sfx_Show;

		// Token: 0x04001904 RID: 6404
		[SerializeField]
		private string sfx_Hide;

		// Token: 0x04001906 RID: 6406
		private bool isShown;
	}
}
