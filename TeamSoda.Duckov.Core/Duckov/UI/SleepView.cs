using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A3 RID: 931
	public class SleepView : View
	{
		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x0600206E RID: 8302 RVA: 0x000720A1 File Offset: 0x000702A1
		public static SleepView Instance
		{
			get
			{
				return View.GetViewInstance<SleepView>();
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x0600206F RID: 8303 RVA: 0x000720A8 File Offset: 0x000702A8
		private TimeSpan SleepTimeSpan
		{
			get
			{
				return TimeSpan.FromMinutes((double)this.sleepForMinuts);
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06002070 RID: 8304 RVA: 0x000720B6 File Offset: 0x000702B6
		private TimeSpan WillWakeUpAt
		{
			get
			{
				return GameClock.TimeOfDay + this.SleepTimeSpan;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06002071 RID: 8305 RVA: 0x000720C8 File Offset: 0x000702C8
		private bool WillWakeUpNextDay
		{
			get
			{
				return this.WillWakeUpAt.Days > 0;
			}
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x000720E6 File Offset: 0x000702E6
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x000720F9 File Offset: 0x000702F9
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x0007210C File Offset: 0x0007030C
		protected override void Awake()
		{
			base.Awake();
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x0007214C File Offset: 0x0007034C
		private void OnConfirmButtonClicked()
		{
			this.Sleep((float)this.sleepForMinuts).Forget();
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x00072160 File Offset: 0x00070360
		private UniTask Sleep(float minuts)
		{
			SleepView.<Sleep>d__21 <Sleep>d__;
			<Sleep>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Sleep>d__.<>4__this = this;
			<Sleep>d__.minuts = minuts;
			<Sleep>d__.<>1__state = -1;
			<Sleep>d__.<>t__builder.Start<SleepView.<Sleep>d__21>(ref <Sleep>d__);
			return <Sleep>d__.<>t__builder.Task;
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000721AB File Offset: 0x000703AB
		private void OnGameClockStep()
		{
			this.Refresh();
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x000721B3 File Offset: 0x000703B3
		private void OnEnable()
		{
			this.InitializeUI();
			GameClock.OnGameClockStep += this.OnGameClockStep;
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x000721CC File Offset: 0x000703CC
		private void OnDisable()
		{
			GameClock.OnGameClockStep -= this.OnGameClockStep;
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x000721DF File Offset: 0x000703DF
		private void OnSliderValueChanged(float newValue)
		{
			this.sleepForMinuts = Mathf.RoundToInt(newValue);
			this.Refresh();
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x000721F3 File Offset: 0x000703F3
		private void InitializeUI()
		{
			this.slider.SetValueWithoutNotify((float)this.sleepForMinuts);
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x00072207 File Offset: 0x00070407
		private void Update()
		{
			this.Refresh();
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x00072210 File Offset: 0x00070410
		private void Refresh()
		{
			TimeSpan willWakeUpAt = this.WillWakeUpAt;
			this.willWakeUpAtText.text = string.Format("{0:00}:{1:00}", willWakeUpAt.Hours, willWakeUpAt.Minutes);
			TimeSpan sleepTimeSpan = this.SleepTimeSpan;
			this.sleepTimeSpanText.text = string.Format("{0:00} h {1:00} min", (int)sleepTimeSpan.TotalHours, sleepTimeSpan.Minutes);
			this.nextDayIndicator.gameObject.SetActive(willWakeUpAt.Days > 0);
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x000722A0 File Offset: 0x000704A0
		public static void Show()
		{
			if (SleepView.Instance == null)
			{
				return;
			}
			SleepView.Instance.Open(null);
		}

		// Token: 0x04001626 RID: 5670
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001627 RID: 5671
		[SerializeField]
		private Slider slider;

		// Token: 0x04001628 RID: 5672
		[SerializeField]
		private TextMeshProUGUI willWakeUpAtText;

		// Token: 0x04001629 RID: 5673
		[SerializeField]
		private TextMeshProUGUI sleepTimeSpanText;

		// Token: 0x0400162A RID: 5674
		[SerializeField]
		private GameObject nextDayIndicator;

		// Token: 0x0400162B RID: 5675
		[SerializeField]
		private Button confirmButton;

		// Token: 0x0400162C RID: 5676
		private int sleepForMinuts;

		// Token: 0x0400162D RID: 5677
		public static Action OnAfterSleep;

		// Token: 0x0400162E RID: 5678
		private bool sleeping;
	}
}
