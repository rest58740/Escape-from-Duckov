using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Buildings;
using Duckov.Buildings.UI;
using Duckov.Economy;
using Duckov.Quests;
using Duckov.UI;
using Saves;
using UnityEngine;

// Token: 0x020001C6 RID: 454
public class ItemWishlist : MonoBehaviour
{
	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x00039712 File Offset: 0x00037912
	// (set) Token: 0x06000DB4 RID: 3508 RVA: 0x00039719 File Offset: 0x00037919
	public static ItemWishlist Instance { get; private set; }

	// Token: 0x1400006E RID: 110
	// (add) Token: 0x06000DB5 RID: 3509 RVA: 0x00039724 File Offset: 0x00037924
	// (remove) Token: 0x06000DB6 RID: 3510 RVA: 0x00039758 File Offset: 0x00037958
	public static event Action<int> OnWishlistChanged;

	// Token: 0x06000DB7 RID: 3511 RVA: 0x0003978C File Offset: 0x0003798C
	private void Awake()
	{
		ItemWishlist.Instance = this;
		QuestManager.onQuestListsChanged += this.OnQuestListChanged;
		BuildingManager.OnBuildingListChanged += this.OnBuildingListChanged;
		SavesSystem.OnCollectSaveData += this.Save;
		UIInputManager.OnWishlistHoveringItem += this.OnWishlistHoveringItem;
		this.Load();
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x000397E9 File Offset: 0x000379E9
	private void OnDestroy()
	{
		QuestManager.onQuestListsChanged -= this.OnQuestListChanged;
		SavesSystem.OnCollectSaveData -= this.Save;
		UIInputManager.OnWishlistHoveringItem -= this.OnWishlistHoveringItem;
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x00039820 File Offset: 0x00037A20
	private void OnWishlistHoveringItem(UIInputEventData data)
	{
		if (!ItemHoveringUI.Shown)
		{
			return;
		}
		int displayingItemID = ItemHoveringUI.DisplayingItemID;
		if (this.IsManuallyWishlisted(displayingItemID))
		{
			ItemWishlist.RemoveFromWishlist(displayingItemID);
		}
		else
		{
			ItemWishlist.AddToWishList(displayingItemID);
		}
		ItemHoveringUI.NotifyRefreshWishlistInfo();
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x00039858 File Offset: 0x00037A58
	private void Load()
	{
		this.manualWishList.Clear();
		List<int> list = SavesSystem.Load<List<int>>("ItemWishlist_Manual");
		if (list != null)
		{
			this.manualWishList.AddRange(list);
		}
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x0003988A File Offset: 0x00037A8A
	private void Save()
	{
		SavesSystem.Save<List<int>>("ItemWishlist_Manual", this.manualWishList);
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x0003989C File Offset: 0x00037A9C
	private void Start()
	{
		this.CacheQuestItems();
		this.CacheBuildingItems();
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x000398AA File Offset: 0x00037AAA
	private void OnQuestListChanged(QuestManager obj)
	{
		this.CacheQuestItems();
	}

	// Token: 0x06000DBE RID: 3518 RVA: 0x000398B2 File Offset: 0x00037AB2
	private void OnBuildingListChanged()
	{
		this.CacheBuildingItems();
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x000398BA File Offset: 0x00037ABA
	private void CacheQuestItems()
	{
		this._questRequiredItems = QuestManager.GetAllRequiredItems().ToHashSet<int>();
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x000398CC File Offset: 0x00037ACC
	private void CacheBuildingItems()
	{
		this._buildingRequiredItems.Clear();
		foreach (BuildingInfo buildingInfo in BuildingSelectionPanel.GetBuildingsToDisplay())
		{
			if (buildingInfo.RequirementsSatisfied() && buildingInfo.TokenAmount + buildingInfo.CurrentAmount < buildingInfo.maxAmount)
			{
				foreach (Cost.ItemEntry itemEntry in buildingInfo.cost.items)
				{
					this._buildingRequiredItems.Add(itemEntry.id);
				}
			}
		}
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x0003995B File Offset: 0x00037B5B
	private IEnumerable<int> IterateAll()
	{
		foreach (int num in this.manualWishList)
		{
			yield return num;
		}
		List<int>.Enumerator enumerator = default(List<int>.Enumerator);
		IEnumerable<int> allRequiredItems = QuestManager.GetAllRequiredItems();
		foreach (int num2 in allRequiredItems)
		{
			yield return num2;
		}
		IEnumerator<int> enumerator2 = null;
		yield break;
		yield break;
	}

	// Token: 0x06000DC2 RID: 3522 RVA: 0x0003996B File Offset: 0x00037B6B
	public bool IsQuestRequired(int itemTypeID)
	{
		return this._questRequiredItems.Contains(itemTypeID);
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x00039979 File Offset: 0x00037B79
	public bool IsManuallyWishlisted(int itemTypeID)
	{
		return this.manualWishList.Contains(itemTypeID);
	}

	// Token: 0x06000DC4 RID: 3524 RVA: 0x00039987 File Offset: 0x00037B87
	public bool IsBuildingRequired(int itemTypeID)
	{
		return this._buildingRequiredItems.Contains(itemTypeID);
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x00039998 File Offset: 0x00037B98
	public static void AddToWishList(int itemTypeID)
	{
		if (ItemWishlist.Instance == null)
		{
			return;
		}
		if (ItemWishlist.Instance.manualWishList.Contains(itemTypeID))
		{
			return;
		}
		ItemWishlist.Instance.manualWishList.Add(itemTypeID);
		Action<int> onWishlistChanged = ItemWishlist.OnWishlistChanged;
		if (onWishlistChanged == null)
		{
			return;
		}
		onWishlistChanged(itemTypeID);
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x000399E6 File Offset: 0x00037BE6
	public static bool RemoveFromWishlist(int itemTypeID)
	{
		if (ItemWishlist.Instance == null)
		{
			return false;
		}
		bool flag = ItemWishlist.Instance.manualWishList.Remove(itemTypeID);
		if (flag)
		{
			Action<int> onWishlistChanged = ItemWishlist.OnWishlistChanged;
			if (onWishlistChanged == null)
			{
				return flag;
			}
			onWishlistChanged(itemTypeID);
		}
		return flag;
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x00039A1C File Offset: 0x00037C1C
	public static ItemWishlist.WishlistInfo GetWishlistInfo(int itemTypeID)
	{
		if (ItemWishlist.Instance == null)
		{
			return default(ItemWishlist.WishlistInfo);
		}
		bool isManuallyWishlisted = ItemWishlist.Instance.IsManuallyWishlisted(itemTypeID);
		bool isQuestRequired = ItemWishlist.Instance.IsQuestRequired(itemTypeID);
		bool isBuildingRequired = ItemWishlist.Instance.IsBuildingRequired(itemTypeID);
		return new ItemWishlist.WishlistInfo
		{
			itemTypeID = itemTypeID,
			isManuallyWishlisted = isManuallyWishlisted,
			isQuestRequired = isQuestRequired,
			isBuildingRequired = isBuildingRequired
		};
	}

	// Token: 0x04000BCB RID: 3019
	private List<int> manualWishList = new List<int>();

	// Token: 0x04000BCC RID: 3020
	private HashSet<int> _questRequiredItems = new HashSet<int>();

	// Token: 0x04000BCD RID: 3021
	private HashSet<int> _buildingRequiredItems = new HashSet<int>();

	// Token: 0x04000BCF RID: 3023
	private const string SaveKey = "ItemWishlist_Manual";

	// Token: 0x020004F1 RID: 1265
	public struct WishlistInfo
	{
		// Token: 0x04001DC8 RID: 7624
		public int itemTypeID;

		// Token: 0x04001DC9 RID: 7625
		public bool isManuallyWishlisted;

		// Token: 0x04001DCA RID: 7626
		public bool isQuestRequired;

		// Token: 0x04001DCB RID: 7627
		public bool isBuildingRequired;
	}
}
