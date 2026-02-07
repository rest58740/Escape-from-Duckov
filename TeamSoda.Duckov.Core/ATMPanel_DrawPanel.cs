using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001A2 RID: 418
public class ATMPanel_DrawPanel : MonoBehaviour
{
	// Token: 0x1400006A RID: 106
	// (add) Token: 0x06000C83 RID: 3203 RVA: 0x00035918 File Offset: 0x00033B18
	// (remove) Token: 0x06000C84 RID: 3204 RVA: 0x00035950 File Offset: 0x00033B50
	public event Action<ATMPanel_DrawPanel> onQuit;

	// Token: 0x06000C85 RID: 3205 RVA: 0x00035985 File Offset: 0x00033B85
	private void OnEnable()
	{
		EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
		this.Refresh();
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x0003599E File Offset: 0x00033B9E
	private void OnDisable()
	{
		EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x000359B4 File Offset: 0x00033BB4
	private void Awake()
	{
		this.inputPanel.onInputFieldValueChanged += this.OnInputValueChanged;
		this.inputPanel.maxFunction = delegate()
		{
			long num = EconomyManager.Money;
			if (num > 10000000L)
			{
				num = 10000000L;
			}
			return num;
		};
		this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		this.quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonClicked));
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x00035A3A File Offset: 0x00033C3A
	private void OnQuitButtonClicked()
	{
		Action<ATMPanel_DrawPanel> action = this.onQuit;
		if (action == null)
		{
			return;
		}
		action(this);
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x00035A4D File Offset: 0x00033C4D
	private void OnMoneyChanged(long arg1, long arg2)
	{
		this.Refresh();
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x00035A58 File Offset: 0x00033C58
	private void OnConfirmButtonClicked()
	{
		if (this.inputPanel.Value <= 0L)
		{
			this.inputPanel.Clear();
			return;
		}
		long num = EconomyManager.Money;
		if (num > 10000000L)
		{
			num = 10000000L;
		}
		if (this.inputPanel.Value > num)
		{
			return;
		}
		this.DrawTask(this.inputPanel.Value).Forget();
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x00035ABC File Offset: 0x00033CBC
	private UniTask DrawTask(long value)
	{
		ATMPanel_DrawPanel.<DrawTask>d__14 <DrawTask>d__;
		<DrawTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DrawTask>d__.<>4__this = this;
		<DrawTask>d__.value = value;
		<DrawTask>d__.<>1__state = -1;
		<DrawTask>d__.<>t__builder.Start<ATMPanel_DrawPanel.<DrawTask>d__14>(ref <DrawTask>d__);
		return <DrawTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x00035B07 File Offset: 0x00033D07
	private void OnInputValueChanged(string v)
	{
		this.Refresh();
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x00035B10 File Offset: 0x00033D10
	private void Refresh()
	{
		bool flag = EconomyManager.Money >= this.inputPanel.Value;
		flag &= (this.inputPanel.Value <= 10000000L);
		flag &= (this.inputPanel.Value >= 0L);
		this.insufficientIndicator.SetActive(!flag);
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x00035B70 File Offset: 0x00033D70
	internal void Show()
	{
		this.fadeGroup.Show();
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x00035B7D File Offset: 0x00033D7D
	internal void Hide(bool skip)
	{
		if (skip)
		{
			this.fadeGroup.SkipHide();
			return;
		}
		this.fadeGroup.Hide();
	}

	// Token: 0x04000AF8 RID: 2808
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000AF9 RID: 2809
	[SerializeField]
	private DigitInputPanel inputPanel;

	// Token: 0x04000AFA RID: 2810
	[SerializeField]
	private Button confirmButton;

	// Token: 0x04000AFB RID: 2811
	[SerializeField]
	private GameObject insufficientIndicator;

	// Token: 0x04000AFC RID: 2812
	[SerializeField]
	private Button quitButton;
}
