using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;

// Token: 0x020000D2 RID: 210
public class TimeOfDayAlert : MonoBehaviour
{
	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06000694 RID: 1684 RVA: 0x0001DF70 File Offset: 0x0001C170
	// (remove) Token: 0x06000695 RID: 1685 RVA: 0x0001DFA4 File Offset: 0x0001C1A4
	public static event Action OnAlertTriggeredEvent;

	// Token: 0x06000696 RID: 1686 RVA: 0x0001DFD7 File Offset: 0x0001C1D7
	private void Awake()
	{
		this.canvasGroup.alpha = 0f;
		TimeOfDayAlert.OnAlertTriggeredEvent += this.OnAlertTriggered;
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0001DFFA File Offset: 0x0001C1FA
	private void OnDestroy()
	{
		TimeOfDayAlert.OnAlertTriggeredEvent -= this.OnAlertTriggered;
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0001E010 File Offset: 0x0001C210
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!LevelManager.Instance.IsBaseLevel)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
		}
		if (this.timer <= 0f && this.canvasGroup.alpha > 0f)
		{
			this.canvasGroup.alpha = Mathf.MoveTowards(this.canvasGroup.alpha, 0f, 0.4f * Time.unscaledDeltaTime);
		}
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x0001E0A8 File Offset: 0x0001C2A8
	private void OnAlertTriggered()
	{
		bool flag = false;
		float time = TimeOfDayController.Instance.Time;
		if (TimeOfDayController.Instance.AtNight)
		{
			flag = true;
			Debug.Log(string.Format("At Night,time:{0}", time));
			this.text.text = LocalizationManager.ToPlainText(this.inNightKey);
		}
		else if (TimeOfDayController.Instance.nightStart - time < 4f)
		{
			flag = true;
			Debug.Log(string.Format("Near Night,time:{0},night start:{1}", time, TimeOfDayController.Instance.nightStart));
			this.text.text = LocalizationManager.ToPlainText(this.nearNightKey);
		}
		if (!flag)
		{
			return;
		}
		this.canvasGroup.alpha = 1f;
		this.timer = this.stayTime;
		this.blinkPunch.Punch();
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0001E177 File Offset: 0x0001C377
	public static void EnterAlertTrigger()
	{
		Action onAlertTriggeredEvent = TimeOfDayAlert.OnAlertTriggeredEvent;
		if (onAlertTriggeredEvent == null)
		{
			return;
		}
		onAlertTriggeredEvent();
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0001E188 File Offset: 0x0001C388
	public static void LeaveAlertTrigger()
	{
	}

	// Token: 0x04000668 RID: 1640
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x04000669 RID: 1641
	[SerializeField]
	public TextMeshProUGUI text;

	// Token: 0x0400066A RID: 1642
	[SerializeField]
	private ColorPunch blinkPunch;

	// Token: 0x0400066C RID: 1644
	[LocalizationKey("Default")]
	public string nearNightKey = "TODAlert_NearNight";

	// Token: 0x0400066D RID: 1645
	[LocalizationKey("Default")]
	public string inNightKey = "TODAlert_InNight";

	// Token: 0x0400066E RID: 1646
	private float stayTime = 5f;

	// Token: 0x0400066F RID: 1647
	private float timer;
}
