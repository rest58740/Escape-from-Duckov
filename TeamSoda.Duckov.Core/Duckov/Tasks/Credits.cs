using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.Tasks
{
	// Token: 0x02000389 RID: 905
	public class Credits : MonoBehaviour, ITaskBehaviour
	{
		// Token: 0x06001F79 RID: 8057 RVA: 0x0006F712 File Offset: 0x0006D912
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x0006F725 File Offset: 0x0006D925
		public void Begin()
		{
			if (this.task.Status == UniTaskStatus.Pending)
			{
				return;
			}
			this.skip = false;
			this.fadeGroup.SkipHide();
			this.fadeGroup.gameObject.SetActive(true);
			this.task = this.Task();
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x0006F764 File Offset: 0x0006D964
		public bool IsPending()
		{
			return this.task.Status == UniTaskStatus.Pending;
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x0006F774 File Offset: 0x0006D974
		public bool IsComplete()
		{
			return !this.IsPending();
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x0006F780 File Offset: 0x0006D980
		private UniTask Task()
		{
			Credits.<Task>d__13 <Task>d__;
			<Task>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Task>d__.<>4__this = this;
			<Task>d__.<>1__state = -1;
			<Task>d__.<>t__builder.Start<Credits.<Task>d__13>(ref <Task>d__);
			return <Task>d__.<>t__builder.Task;
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x0006F7C3 File Offset: 0x0006D9C3
		public void Skip()
		{
			this.skip = true;
			if (this.fadeOut && this.fadeGroup.IsFading)
			{
				this.fadeGroup.SkipHide();
			}
			if (!this.mute)
			{
				AudioManager.StopBGM();
			}
		}

		// Token: 0x0400157C RID: 5500
		private RectTransform rectTransform;

		// Token: 0x0400157D RID: 5501
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400157E RID: 5502
		[SerializeField]
		private RectTransform content;

		// Token: 0x0400157F RID: 5503
		[SerializeField]
		private float scrollSpeed;

		// Token: 0x04001580 RID: 5504
		[SerializeField]
		private float holdForSeconds;

		// Token: 0x04001581 RID: 5505
		[SerializeField]
		private bool fadeOut;

		// Token: 0x04001582 RID: 5506
		[SerializeField]
		private bool mute;

		// Token: 0x04001583 RID: 5507
		private UniTask task;

		// Token: 0x04001584 RID: 5508
		private bool skip;
	}
}
