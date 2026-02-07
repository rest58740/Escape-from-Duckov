using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Economy;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200020B RID: 523
public class ItemRepair_RepairAllPanel : MonoBehaviour
{
	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x06000F87 RID: 3975 RVA: 0x0003E190 File Offset: 0x0003C390
	private PrefabPool<ItemDisplay> Pool
	{
		get
		{
			if (this._pool == null)
			{
				this._pool = new PrefabPool<ItemDisplay>(this.itemDisplayTemplate, null, null, null, null, true, 10, 10000, delegate(ItemDisplay e)
				{
					e.onPointerClick += this.OnPointerClickEntry;
				});
			}
			return this._pool;
		}
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x0003E1D4 File Offset: 0x0003C3D4
	private void OnPointerClickEntry(ItemDisplay display, PointerEventData data)
	{
		data.Use();
	}

	// Token: 0x06000F89 RID: 3977 RVA: 0x0003E1DC File Offset: 0x0003C3DC
	private void Awake()
	{
		this.itemDisplayTemplate.gameObject.SetActive(false);
		this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x0003E20C File Offset: 0x0003C40C
	private void OnButtonClicked()
	{
		if (this.master == null)
		{
			return;
		}
		List<Item> allEquippedItems = this.master.GetAllEquippedItems();
		this.master.RepairItems(allEquippedItems);
		this.needsRefresh = true;
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x0003E247 File Offset: 0x0003C447
	private void OnEnable()
	{
		ItemUtilities.OnPlayerItemOperation += this.OnPlayerItemOperation;
		ItemRepairView.OnRepaireOptionDone += this.OnRepairOptionDone;
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x0003E26B File Offset: 0x0003C46B
	private void OnDisable()
	{
		ItemUtilities.OnPlayerItemOperation -= this.OnPlayerItemOperation;
		ItemRepairView.OnRepaireOptionDone -= this.OnRepairOptionDone;
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x0003E28F File Offset: 0x0003C48F
	public void Setup(ItemRepairView master)
	{
		this.master = master;
		this.Refresh();
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x0003E29E File Offset: 0x0003C49E
	private void OnPlayerItemOperation()
	{
		this.needsRefresh = true;
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x0003E2A7 File Offset: 0x0003C4A7
	private void OnRepairOptionDone()
	{
		this.needsRefresh = true;
	}

	// Token: 0x06000F90 RID: 3984 RVA: 0x0003E2B0 File Offset: 0x0003C4B0
	private void Refresh()
	{
		this.needsRefresh = false;
		this.Pool.ReleaseAll();
		List<Item> list = (from e in this.master.GetAllEquippedItems()
		where e.Durability < e.MaxDurabilityWithLoss
		select e).ToList<Item>();
		int num = 0;
		if (list != null && list.Count > 0)
		{
			foreach (Item target in list)
			{
				this.Pool.Get(null).Setup(target);
			}
			num = this.master.CalculateRepairPrice(list);
			this.placeholder.SetActive(false);
			Cost cost = new Cost((long)num);
			bool enough = cost.Enough;
			this.button.interactable = enough;
		}
		else
		{
			this.placeholder.SetActive(true);
			this.button.interactable = false;
		}
		this.priceDisplay.text = num.ToString();
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x0003E3C8 File Offset: 0x0003C5C8
	private void Update()
	{
		if (this.needsRefresh)
		{
			this.Refresh();
		}
	}

	// Token: 0x04000CCE RID: 3278
	[SerializeField]
	private ItemRepairView master;

	// Token: 0x04000CCF RID: 3279
	[SerializeField]
	private TextMeshProUGUI priceDisplay;

	// Token: 0x04000CD0 RID: 3280
	[SerializeField]
	private ItemDisplay itemDisplayTemplate;

	// Token: 0x04000CD1 RID: 3281
	[SerializeField]
	private Button button;

	// Token: 0x04000CD2 RID: 3282
	[SerializeField]
	private GameObject placeholder;

	// Token: 0x04000CD3 RID: 3283
	private PrefabPool<ItemDisplay> _pool;

	// Token: 0x04000CD4 RID: 3284
	private bool needsRefresh;
}
