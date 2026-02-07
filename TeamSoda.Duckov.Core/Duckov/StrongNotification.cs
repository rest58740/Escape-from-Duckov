using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov
{
	// Token: 0x0200024C RID: 588
	public class StrongNotification : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700033E RID: 830
		// (get) Token: 0x0600128B RID: 4747 RVA: 0x00047908 File Offset: 0x00045B08
		// (set) Token: 0x0600128C RID: 4748 RVA: 0x0004790F File Offset: 0x00045B0F
		public static StrongNotification Instance { get; private set; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x00047917 File Offset: 0x00045B17
		private bool showing
		{
			get
			{
				return this.showingTask.Status == UniTaskStatus.Pending;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x00047927 File Offset: 0x00045B27
		public static bool Showing
		{
			get
			{
				return !(StrongNotification.Instance == null) && StrongNotification.Instance.showing;
			}
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00047944 File Offset: 0x00045B44
		private void Awake()
		{
			if (StrongNotification.Instance == null)
			{
				StrongNotification.Instance = this;
			}
			UIInputManager.OnConfirm += this.OnConfirm;
			UIInputManager.OnCancel += this.OnCancel;
			View.OnActiveViewChanged += this.View_OnActiveViewChanged;
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00047997 File Offset: 0x00045B97
		private void OnDestroy()
		{
			UIInputManager.OnConfirm -= this.OnConfirm;
			UIInputManager.OnCancel -= this.OnCancel;
			View.OnActiveViewChanged -= this.View_OnActiveViewChanged;
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x000479CC File Offset: 0x00045BCC
		private void View_OnActiveViewChanged()
		{
			this.confirmed = true;
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x000479D5 File Offset: 0x00045BD5
		private void OnCancel(UIInputEventData data)
		{
			this.confirmed = true;
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x000479DE File Offset: 0x00045BDE
		private void OnConfirm(UIInputEventData data)
		{
			this.confirmed = true;
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x000479E7 File Offset: 0x00045BE7
		private void Update()
		{
			if (!this.showing && StrongNotification.pending.Count > 0)
			{
				this.BeginShow();
			}
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x00047A04 File Offset: 0x00045C04
		private void BeginShow()
		{
			this.showingTask = this.ShowTask();
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x00047A14 File Offset: 0x00045C14
		private UniTask ShowTask()
		{
			StrongNotification.<ShowTask>d__23 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<StrongNotification.<ShowTask>d__23>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00047A58 File Offset: 0x00045C58
		private UniTask DisplayContent(StrongNotificationContent cur)
		{
			StrongNotification.<DisplayContent>d__24 <DisplayContent>d__;
			<DisplayContent>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DisplayContent>d__.<>4__this = this;
			<DisplayContent>d__.cur = cur;
			<DisplayContent>d__.<>1__state = -1;
			<DisplayContent>d__.<>t__builder.Start<StrongNotification.<DisplayContent>d__24>(ref <DisplayContent>d__);
			return <DisplayContent>d__.<>t__builder.Task;
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x00047AA3 File Offset: 0x00045CA3
		public void OnPointerClick(PointerEventData eventData)
		{
			this.confirmed = true;
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x00047AAC File Offset: 0x00045CAC
		public static void Push(StrongNotificationContent content)
		{
			StrongNotification.pending.Add(content);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00047AB9 File Offset: 0x00045CB9
		public static void Push(string mainText, string subText = "")
		{
			StrongNotification.pending.Add(new StrongNotificationContent(mainText, subText, null));
		}

		// Token: 0x04000E3F RID: 3647
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04000E40 RID: 3648
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x04000E41 RID: 3649
		[SerializeField]
		private TextMeshProUGUI textMain;

		// Token: 0x04000E42 RID: 3650
		[SerializeField]
		private TextMeshProUGUI textSub;

		// Token: 0x04000E43 RID: 3651
		[SerializeField]
		private Image image;

		// Token: 0x04000E44 RID: 3652
		[SerializeField]
		private float contentDelay = 0.5f;

		// Token: 0x04000E45 RID: 3653
		private static List<StrongNotificationContent> pending = new List<StrongNotificationContent>();

		// Token: 0x04000E47 RID: 3655
		private UniTask showingTask;

		// Token: 0x04000E48 RID: 3656
		private bool confirmed;
	}
}
