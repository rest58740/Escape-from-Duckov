using System;
using Duckov.Economy;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000160 RID: 352
public class CostDisplay : MonoBehaviour
{
	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x000302C4 File Offset: 0x0002E4C4
	private PrefabPool<ItemAmountDisplay> ItemPool
	{
		get
		{
			if (this._itemPool == null)
			{
				this._itemPool = new PrefabPool<ItemAmountDisplay>(this.itemAmountTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._itemPool;
		}
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x000302FD File Offset: 0x0002E4FD
	private void OnEnable()
	{
		EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
		ItemUtilities.OnPlayerItemOperation += this.OnItemOperation;
		LevelManager.OnLevelInitialized += this.OnLevelInitialized;
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x00030332 File Offset: 0x0002E532
	private void OnDisable()
	{
		EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
		ItemUtilities.OnPlayerItemOperation -= this.OnItemOperation;
		LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x00030367 File Offset: 0x0002E567
	private void OnLevelInitialized()
	{
		this.RefreshBackground();
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0003036F File Offset: 0x0002E56F
	private void OnItemOperation()
	{
		this.RefreshBackground();
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x00030377 File Offset: 0x0002E577
	private void RefreshBackground()
	{
		if (this.background == null)
		{
			return;
		}
		this.background.color = (this.cost.Enough ? this.enoughColor : this.normalColor);
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x000303AE File Offset: 0x0002E5AE
	private void OnMoneyChanged(long arg1, long arg2)
	{
		this.RefreshMoneyBackground();
		this.RefreshBackground();
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x000303BC File Offset: 0x0002E5BC
	public void Setup(Cost cost, int multiplier = 1)
	{
		this.cost = cost;
		this.moneyContainer.SetActive(cost.money > 0L);
		this.money.text = (cost.money * (long)multiplier).ToString("n0");
		this.itemsContainer.SetActive(cost.items != null && cost.items.Length != 0);
		this.ItemPool.ReleaseAll();
		if (cost.items != null)
		{
			foreach (Cost.ItemEntry itemEntry in cost.items)
			{
				ItemAmountDisplay itemAmountDisplay = this.ItemPool.Get(null);
				itemAmountDisplay.Setup(itemEntry.id, itemEntry.amount * (long)multiplier);
				itemAmountDisplay.transform.SetAsLastSibling();
			}
		}
		this.RefreshMoneyBackground();
		this.RefreshBackground();
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x00030490 File Offset: 0x0002E690
	private void RefreshMoneyBackground()
	{
		bool flag = this.cost.money <= EconomyManager.Money;
		this.moneyBackground.color = (flag ? this.money_enoughColor : this.money_normalColor);
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x000304CF File Offset: 0x0002E6CF
	internal void Clear()
	{
		this.cost = default(Cost);
		this.moneyContainer.SetActive(false);
		this.ItemPool.ReleaseAll();
	}

	// Token: 0x0400099F RID: 2463
	[SerializeField]
	private GameObject moneyContainer;

	// Token: 0x040009A0 RID: 2464
	[SerializeField]
	private GameObject itemsContainer;

	// Token: 0x040009A1 RID: 2465
	[SerializeField]
	private Image background;

	// Token: 0x040009A2 RID: 2466
	[SerializeField]
	private Image moneyBackground;

	// Token: 0x040009A3 RID: 2467
	[SerializeField]
	private TextMeshProUGUI money;

	// Token: 0x040009A4 RID: 2468
	[SerializeField]
	private ItemAmountDisplay itemAmountTemplate;

	// Token: 0x040009A5 RID: 2469
	[SerializeField]
	private Color normalColor;

	// Token: 0x040009A6 RID: 2470
	[SerializeField]
	private Color enoughColor;

	// Token: 0x040009A7 RID: 2471
	[SerializeField]
	private Color money_normalColor;

	// Token: 0x040009A8 RID: 2472
	[SerializeField]
	private Color money_enoughColor;

	// Token: 0x040009A9 RID: 2473
	private PrefabPool<ItemAmountDisplay> _itemPool;

	// Token: 0x040009AA RID: 2474
	private Cost cost;
}
