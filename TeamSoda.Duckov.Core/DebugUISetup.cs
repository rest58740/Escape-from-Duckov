using System;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000200 RID: 512
public class DebugUISetup : MonoBehaviour
{
	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06000F37 RID: 3895 RVA: 0x0003D519 File Offset: 0x0003B719
	private CharacterMainControl Character
	{
		get
		{
			return LevelManager.Instance.MainCharacter;
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0003D525 File Offset: 0x0003B725
	private Item CharacterItem
	{
		get
		{
			return this.Character.CharacterItem;
		}
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x0003D532 File Offset: 0x0003B732
	public void Setup()
	{
		this.slotCollectionDisplay.Setup(this.CharacterItem, false);
		this.inventoryDisplay.Setup(this.CharacterItem.Inventory, null, null, false, null);
	}

	// Token: 0x04000CA0 RID: 3232
	[SerializeField]
	private ItemSlotCollectionDisplay slotCollectionDisplay;

	// Token: 0x04000CA1 RID: 3233
	[SerializeField]
	private InventoryDisplay inventoryDisplay;
}
