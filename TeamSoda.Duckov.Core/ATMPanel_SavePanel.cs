using System;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001A3 RID: 419
public class ATMPanel_SavePanel : MonoBehaviour
{
	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06000C91 RID: 3217 RVA: 0x00035BA1 File Offset: 0x00033DA1
	private int CashAmount
	{
		get
		{
			if (this._cachedCashAmount < 0)
			{
				this._cachedCashAmount = ItemUtilities.GetItemCount(451);
			}
			return this._cachedCashAmount;
		}
	}

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x06000C92 RID: 3218 RVA: 0x00035BC4 File Offset: 0x00033DC4
	// (remove) Token: 0x06000C93 RID: 3219 RVA: 0x00035BFC File Offset: 0x00033DFC
	public event Action<ATMPanel_SavePanel> onQuit;

	// Token: 0x06000C94 RID: 3220 RVA: 0x00035C31 File Offset: 0x00033E31
	private void OnEnable()
	{
		ItemUtilities.OnPlayerItemOperation += this.OnPlayerItemOperation;
		this.RefreshCash();
		this.Refresh();
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x00035C50 File Offset: 0x00033E50
	private void OnDisable()
	{
		ItemUtilities.OnPlayerItemOperation -= this.OnPlayerItemOperation;
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x00035C63 File Offset: 0x00033E63
	private void OnPlayerItemOperation()
	{
		this.RefreshCash();
		this.Refresh();
	}

	// Token: 0x06000C97 RID: 3223 RVA: 0x00035C71 File Offset: 0x00033E71
	private void RefreshCash()
	{
		this._cachedCashAmount = ItemUtilities.GetItemCount(451);
	}

	// Token: 0x06000C98 RID: 3224 RVA: 0x00035C84 File Offset: 0x00033E84
	private void Awake()
	{
		this.inputPanel.onInputFieldValueChanged += this.OnInputValueChanged;
		this.inputPanel.maxFunction = (() => (long)this.CashAmount);
		this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		this.quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonClicked));
	}

	// Token: 0x06000C99 RID: 3225 RVA: 0x00035CF7 File Offset: 0x00033EF7
	private void OnQuitButtonClicked()
	{
		Action<ATMPanel_SavePanel> action = this.onQuit;
		if (action == null)
		{
			return;
		}
		action(this);
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x00035D0C File Offset: 0x00033F0C
	private void OnConfirmButtonClicked()
	{
		if (this.inputPanel.Value <= 0L)
		{
			this.inputPanel.Clear();
			return;
		}
		if (this.inputPanel.Value > (long)this.CashAmount)
		{
			return;
		}
		if (ATMPanel.Save(this.inputPanel.Value))
		{
			this.inputPanel.Clear();
		}
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x00035D66 File Offset: 0x00033F66
	private void OnInputValueChanged(string v)
	{
		this.Refresh();
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x00035D70 File Offset: 0x00033F70
	private void Refresh()
	{
		bool flag = (long)this.CashAmount >= this.inputPanel.Value;
		flag &= (this.inputPanel.Value >= 0L);
		this.insufficientIndicator.SetActive(!flag);
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x00035DB9 File Offset: 0x00033FB9
	internal void Hide(bool skip = false)
	{
		if (skip)
		{
			this.fadeGroup.SkipHide();
			return;
		}
		this.fadeGroup.Hide();
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x00035DD5 File Offset: 0x00033FD5
	internal void Show()
	{
		this.fadeGroup.Show();
	}

	// Token: 0x04000AFE RID: 2814
	private const int CashItemTypeID = 451;

	// Token: 0x04000AFF RID: 2815
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000B00 RID: 2816
	[SerializeField]
	private DigitInputPanel inputPanel;

	// Token: 0x04000B01 RID: 2817
	[SerializeField]
	private Button confirmButton;

	// Token: 0x04000B02 RID: 2818
	[SerializeField]
	private GameObject insufficientIndicator;

	// Token: 0x04000B03 RID: 2819
	[SerializeField]
	private Button quitButton;

	// Token: 0x04000B04 RID: 2820
	private int _cachedCashAmount = -1;
}
