using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001A1 RID: 417
public class ATMPanel : MonoBehaviour
{
	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06000C72 RID: 3186 RVA: 0x000356AB File Offset: 0x000338AB
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

	// Token: 0x06000C73 RID: 3187 RVA: 0x000356CC File Offset: 0x000338CC
	private void Awake()
	{
		this.btnSelectSave.onClick.AddListener(new UnityAction(this.ShowSavePanel));
		this.btnSelectDraw.onClick.AddListener(new UnityAction(this.ShowDrawPanel));
		this.savePanel.onQuit += this.SavePanel_onQuit;
		this.drawPanel.onQuit += this.DrawPanel_onQuit;
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x0003573F File Offset: 0x0003393F
	private void DrawPanel_onQuit(ATMPanel_DrawPanel panel)
	{
		this.ShowSelectPanel(false);
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x00035748 File Offset: 0x00033948
	private void SavePanel_onQuit(ATMPanel_SavePanel obj)
	{
		this.ShowSelectPanel(false);
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x00035751 File Offset: 0x00033951
	private void HideAllPanels(bool skip = false)
	{
		if (skip)
		{
			this.selectPanel.SkipHide();
		}
		else
		{
			this.selectPanel.Hide();
		}
		this.savePanel.Hide(skip);
		this.drawPanel.Hide(skip);
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x00035786 File Offset: 0x00033986
	public void ShowSelectPanel(bool skipHideOthers = false)
	{
		this.HideAllPanels(skipHideOthers);
		this.selectPanel.Show();
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x0003579A File Offset: 0x0003399A
	public void ShowDrawPanel()
	{
		this.HideAllPanels(false);
		this.drawPanel.Show();
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x000357AE File Offset: 0x000339AE
	public void ShowSavePanel()
	{
		this.HideAllPanels(false);
		this.savePanel.Show();
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x000357C2 File Offset: 0x000339C2
	private void OnEnable()
	{
		EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
		ItemUtilities.OnPlayerItemOperation += this.OnPlayerItemOperation;
		this.RefreshCash();
		this.RefreshBalance();
		this.ShowSelectPanel(false);
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x000357F9 File Offset: 0x000339F9
	private void OnDisable()
	{
		EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
		ItemUtilities.OnPlayerItemOperation -= this.OnPlayerItemOperation;
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x0003581D File Offset: 0x00033A1D
	private void OnPlayerItemOperation()
	{
		this.RefreshCash();
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x00035825 File Offset: 0x00033A25
	private void OnMoneyChanged(long oldMoney, long changedMoney)
	{
		this.RefreshBalance();
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x0003582D File Offset: 0x00033A2D
	private void RefreshCash()
	{
		this._cachedCashAmount = ItemUtilities.GetItemCount(451);
		this.cashAmountText.text = string.Format("{0:n0}", this.CashAmount);
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x0003585F File Offset: 0x00033A5F
	private void RefreshBalance()
	{
		this.balanceAmountText.text = string.Format("{0:n0}", EconomyManager.Money);
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x00035880 File Offset: 0x00033A80
	public static UniTask<bool> Draw(long amount)
	{
		ATMPanel.<Draw>d__26 <Draw>d__;
		<Draw>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<Draw>d__.amount = amount;
		<Draw>d__.<>1__state = -1;
		<Draw>d__.<>t__builder.Start<ATMPanel.<Draw>d__26>(ref <Draw>d__);
		return <Draw>d__.<>t__builder.Task;
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x000358C4 File Offset: 0x00033AC4
	public static bool Save(long amount)
	{
		Cost cost = new Cost(0L, new ValueTuple<int, long>[]
		{
			new ValueTuple<int, long>(451, amount)
		});
		if (!cost.Pay(false, true))
		{
			return false;
		}
		EconomyManager.Add(amount);
		return true;
	}

	// Token: 0x04000AED RID: 2797
	private const int CashItemTypeID = 451;

	// Token: 0x04000AEE RID: 2798
	[SerializeField]
	private TextMeshProUGUI balanceAmountText;

	// Token: 0x04000AEF RID: 2799
	[SerializeField]
	private TextMeshProUGUI cashAmountText;

	// Token: 0x04000AF0 RID: 2800
	[SerializeField]
	private Button btnSelectSave;

	// Token: 0x04000AF1 RID: 2801
	[SerializeField]
	private Button btnSelectDraw;

	// Token: 0x04000AF2 RID: 2802
	[SerializeField]
	private FadeGroup selectPanel;

	// Token: 0x04000AF3 RID: 2803
	[SerializeField]
	private ATMPanel_SavePanel savePanel;

	// Token: 0x04000AF4 RID: 2804
	[SerializeField]
	private ATMPanel_DrawPanel drawPanel;

	// Token: 0x04000AF5 RID: 2805
	private int _cachedCashAmount = -1;

	// Token: 0x04000AF6 RID: 2806
	private static bool drawingMoney;

	// Token: 0x04000AF7 RID: 2807
	public const long MaxDrawAmount = 10000000L;
}
