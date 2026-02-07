using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C7 RID: 199
public class EvacuationCountdownUI : MonoBehaviour
{
	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000666 RID: 1638 RVA: 0x0001D01A File Offset: 0x0001B21A
	public static EvacuationCountdownUI Instance
	{
		get
		{
			return EvacuationCountdownUI._instance;
		}
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x0001D021 File Offset: 0x0001B221
	private void Awake()
	{
		if (EvacuationCountdownUI._instance == null)
		{
			EvacuationCountdownUI._instance = this;
		}
		if (EvacuationCountdownUI._instance != this)
		{
			Debug.LogWarning("Multiple Evacuation Countdown UI detected");
		}
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x0001D050 File Offset: 0x0001B250
	private string ToDigitString(float number)
	{
		int num = (int)number;
		int num2 = Mathf.Min(999, Mathf.RoundToInt((number - (float)num) * 1000f));
		int num3 = num / 60;
		num -= num3 * 60;
		return string.Format(this.digitFormat, num3, num, num2);
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x0001D0A3 File Offset: 0x0001B2A3
	private void Update()
	{
		if (this.target == null && this.fadeGroup.IsShown)
		{
			this.Hide().Forget();
		}
		this.Refresh();
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x0001D0D4 File Offset: 0x0001B2D4
	private void Refresh()
	{
		if (this.target == null)
		{
			return;
		}
		this.progressFill.fillAmount = this.target.Progress;
		this.countdownDigit.text = this.ToDigitString(this.target.RemainingTime);
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0001D124 File Offset: 0x0001B324
	private UniTask Hide()
	{
		EvacuationCountdownUI.<Hide>d__12 <Hide>d__;
		<Hide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Hide>d__.<>4__this = this;
		<Hide>d__.<>1__state = -1;
		<Hide>d__.<>t__builder.Start<EvacuationCountdownUI.<Hide>d__12>(ref <Hide>d__);
		return <Hide>d__.<>t__builder.Task;
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x0001D168 File Offset: 0x0001B368
	private UniTask Show(CountDownArea target)
	{
		EvacuationCountdownUI.<Show>d__13 <Show>d__;
		<Show>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Show>d__.<>4__this = this;
		<Show>d__.target = target;
		<Show>d__.<>1__state = -1;
		<Show>d__.<>t__builder.Start<EvacuationCountdownUI.<Show>d__13>(ref <Show>d__);
		return <Show>d__.<>t__builder.Task;
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x0001D1B3 File Offset: 0x0001B3B3
	public static void Request(CountDownArea target)
	{
		if (EvacuationCountdownUI.Instance == null)
		{
			return;
		}
		EvacuationCountdownUI.Instance.Show(target).Forget();
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x0001D1D3 File Offset: 0x0001B3D3
	public static void Release(CountDownArea target)
	{
		if (EvacuationCountdownUI.Instance == null)
		{
			return;
		}
		if (EvacuationCountdownUI.Instance.target == target)
		{
			EvacuationCountdownUI.Instance.Hide().Forget();
		}
	}

	// Token: 0x0400062F RID: 1583
	private static EvacuationCountdownUI _instance;

	// Token: 0x04000630 RID: 1584
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000631 RID: 1585
	[SerializeField]
	private Image progressFill;

	// Token: 0x04000632 RID: 1586
	[SerializeField]
	private TextMeshProUGUI countdownDigit;

	// Token: 0x04000633 RID: 1587
	[SerializeField]
	private string digitFormat = "{0:00}:{1:00}<sub>.{2:000}</sub>";

	// Token: 0x04000634 RID: 1588
	private CountDownArea target;
}
