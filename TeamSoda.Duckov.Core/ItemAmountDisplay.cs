using System;
using ItemStatsSystem;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000161 RID: 353
public class ItemAmountDisplay : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IItemMetaDataProvider
{
	// Token: 0x14000050 RID: 80
	// (add) Token: 0x06000AFF RID: 2815 RVA: 0x000304FC File Offset: 0x0002E6FC
	// (remove) Token: 0x06000B00 RID: 2816 RVA: 0x00030530 File Offset: 0x0002E730
	public static event Action<ItemAmountDisplay> OnMouseEnter;

	// Token: 0x14000051 RID: 81
	// (add) Token: 0x06000B01 RID: 2817 RVA: 0x00030564 File Offset: 0x0002E764
	// (remove) Token: 0x06000B02 RID: 2818 RVA: 0x00030598 File Offset: 0x0002E798
	public static event Action<ItemAmountDisplay> OnMouseExit;

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06000B03 RID: 2819 RVA: 0x000305CB File Offset: 0x0002E7CB
	public int TypeID
	{
		get
		{
			return this.typeID;
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06000B04 RID: 2820 RVA: 0x000305D3 File Offset: 0x0002E7D3
	public ItemMetaData MetaData
	{
		get
		{
			return this.metaData;
		}
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x000305DB File Offset: 0x0002E7DB
	public ItemMetaData GetMetaData()
	{
		return this.metaData;
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x000305E3 File Offset: 0x0002E7E3
	private void Awake()
	{
		ItemUtilities.OnPlayerItemOperation += this.Refresh;
		LevelManager.OnLevelInitialized += this.Refresh;
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x00030607 File Offset: 0x0002E807
	private void OnDestroy()
	{
		ItemUtilities.OnPlayerItemOperation -= this.Refresh;
		LevelManager.OnLevelInitialized -= this.Refresh;
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0003062B File Offset: 0x0002E82B
	public void Setup(int itemTypeID, long amount)
	{
		this.typeID = itemTypeID;
		this.amount = amount;
		this.Refresh();
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x00030644 File Offset: 0x0002E844
	private void Refresh()
	{
		int itemCount = ItemUtilities.GetItemCount(this.typeID);
		this.metaData = ItemAssetsCollection.GetMetaData(this.typeID);
		this.icon.sprite = this.metaData.icon;
		this.amountText.text = StringExtensions.Format(this.amountFormat, new
		{
			amount = this.amount,
			possess = itemCount
		});
		bool flag = (long)itemCount >= this.amount;
		this.background.color = (flag ? this.enoughColor : this.normalColor);
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x000306D0 File Offset: 0x0002E8D0
	public void OnPointerEnter(PointerEventData eventData)
	{
		Action<ItemAmountDisplay> onMouseEnter = ItemAmountDisplay.OnMouseEnter;
		if (onMouseEnter == null)
		{
			return;
		}
		onMouseEnter(this);
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x000306E2 File Offset: 0x0002E8E2
	public void OnPointerExit(PointerEventData eventData)
	{
		Action<ItemAmountDisplay> onMouseExit = ItemAmountDisplay.OnMouseExit;
		if (onMouseExit == null)
		{
			return;
		}
		onMouseExit(this);
	}

	// Token: 0x040009AD RID: 2477
	[SerializeField]
	private Image background;

	// Token: 0x040009AE RID: 2478
	[SerializeField]
	private Image icon;

	// Token: 0x040009AF RID: 2479
	[SerializeField]
	private TextMeshProUGUI amountText;

	// Token: 0x040009B0 RID: 2480
	[SerializeField]
	private string amountFormat = "( {possess} / {amount} )";

	// Token: 0x040009B1 RID: 2481
	[SerializeField]
	private Color normalColor;

	// Token: 0x040009B2 RID: 2482
	[SerializeField]
	private Color enoughColor;

	// Token: 0x040009B3 RID: 2483
	private int typeID;

	// Token: 0x040009B4 RID: 2484
	private long amount;

	// Token: 0x040009B5 RID: 2485
	private ItemMetaData metaData;
}
