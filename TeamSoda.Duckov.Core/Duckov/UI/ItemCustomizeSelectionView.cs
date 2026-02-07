using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003CF RID: 975
	public class ItemCustomizeSelectionView : View
	{
		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x0600232D RID: 9005 RVA: 0x0007AE9B File Offset: 0x0007909B
		public static ItemCustomizeSelectionView Instance
		{
			get
			{
				return View.GetViewInstance<ItemCustomizeSelectionView>();
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x0600232E RID: 9006 RVA: 0x0007AEA2 File Offset: 0x000790A2
		private Item CharacterItem
		{
			get
			{
				LevelManager instance = LevelManager.Instance;
				if (instance == null)
				{
					return null;
				}
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter == null)
				{
					return null;
				}
				return mainCharacter.CharacterItem;
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x0600232F RID: 9007 RVA: 0x0007AEC0 File Offset: 0x000790C0
		private bool CanCustomize
		{
			get
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				return !(selectedItem == null) && !(selectedItem.Slots == null) && selectedItem.Slots.Count >= 1;
			}
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x0007AEFF File Offset: 0x000790FF
		protected override void Awake()
		{
			base.Awake();
			this.beginCustomizeButton.onClick.AddListener(new UnityAction(this.OnBeginCustomizeButtonClicked));
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x0007AF24 File Offset: 0x00079124
		private void OnBeginCustomizeButtonClicked()
		{
			Item selectedItem = ItemUIUtilities.SelectedItem;
			ItemCustomizeView instance = ItemCustomizeView.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Setup(ItemUIUtilities.SelectedItem, this.GetAvaliableInventories());
			instance.Open(null);
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x0007AF60 File Offset: 0x00079160
		private List<Inventory> GetAvaliableInventories()
		{
			this.avaliableInventories.Clear();
			LevelManager instance = LevelManager.Instance;
			Inventory inventory;
			if (instance == null)
			{
				inventory = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter == null)
				{
					inventory = null;
				}
				else
				{
					Item characterItem = mainCharacter.CharacterItem;
					inventory = ((characterItem != null) ? characterItem.Inventory : null);
				}
			}
			Inventory inventory2 = inventory;
			if (inventory2 != null)
			{
				this.avaliableInventories.Add(inventory2);
			}
			Inventory inventory3 = PlayerStorage.Inventory;
			if (inventory3 != null)
			{
				this.avaliableInventories.Add(inventory3);
			}
			return this.avaliableInventories;
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0007AFD8 File Offset: 0x000791D8
		protected override void OnOpen()
		{
			this.UnregisterEvents();
			base.OnOpen();
			Item characterItem = this.CharacterItem;
			if (characterItem == null)
			{
				Debug.LogError("物品栏开启失败，角色物体不存在");
				return;
			}
			base.gameObject.SetActive(true);
			this.slotDisplay.Setup(characterItem, false);
			this.inventoryDisplay.Setup(characterItem.Inventory, null, null, false, null);
			this.RegisterEvents();
			this.fadeGroup.Show();
			this.customizeButtonFadeGroup.SkipHide();
			this.placeHolderFadeGroup.SkipHide();
			ItemUIUtilities.Select(null);
			this.RefreshSelectedItemInfo();
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0007B06D File Offset: 0x0007926D
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.itemDetailsFadeGroup.Hide();
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x0007B091 File Offset: 0x00079291
		private void RegisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x0007B0A4 File Offset: 0x000792A4
		private void OnItemSelectionChanged()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.detailsDisplay.Setup(ItemUIUtilities.SelectedItem);
				this.itemDetailsFadeGroup.Show();
			}
			else
			{
				this.itemDetailsFadeGroup.Hide();
			}
			if (this.CanCustomize)
			{
				this.placeHolderFadeGroup.Hide();
				this.customizeButtonFadeGroup.Show();
			}
			else
			{
				this.customizeButtonFadeGroup.Hide();
				this.placeHolderFadeGroup.Show();
			}
			this.RefreshSelectedItemInfo();
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0007B122 File Offset: 0x00079322
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x0007B135 File Offset: 0x00079335
		public static void Show()
		{
			if (ItemCustomizeSelectionView.Instance == null)
			{
				return;
			}
			ItemCustomizeSelectionView.Instance.Open(null);
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x0007B150 File Offset: 0x00079350
		public static void Hide()
		{
			if (ItemCustomizeSelectionView.Instance == null)
			{
				return;
			}
			ItemCustomizeSelectionView.Instance.Close();
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x0007B16C File Offset: 0x0007936C
		private void RefreshSelectedItemInfo()
		{
			Item selectedItem = ItemUIUtilities.SelectedItem;
			if (selectedItem == null)
			{
				this.selectedItemName.text = this.noItemSelectedNameText;
				this.selectedItemIcon.sprite = this.noItemSelectedIconSprite;
				this.selectedItemShadow.enabled = false;
				this.customizableIndicator.SetActive(false);
				this.uncustomizableIndicator.SetActive(false);
				this.selectedItemIcon.color = Color.clear;
				return;
			}
			this.selectedItemShadow.enabled = true;
			this.selectedItemIcon.color = Color.white;
			this.selectedItemName.text = selectedItem.DisplayName;
			this.selectedItemIcon.sprite = selectedItem.Icon;
			GameplayDataSettings.UIStyle.GetDisplayQualityLook(selectedItem.DisplayQuality).Apply(this.selectedItemShadow);
			this.customizableIndicator.SetActive(this.CanCustomize);
			this.uncustomizableIndicator.SetActive(!this.CanCustomize);
		}

		// Token: 0x040017D7 RID: 6103
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040017D8 RID: 6104
		[SerializeField]
		private ItemSlotCollectionDisplay slotDisplay;

		// Token: 0x040017D9 RID: 6105
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x040017DA RID: 6106
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x040017DB RID: 6107
		[SerializeField]
		private FadeGroup itemDetailsFadeGroup;

		// Token: 0x040017DC RID: 6108
		[SerializeField]
		private FadeGroup customizeButtonFadeGroup;

		// Token: 0x040017DD RID: 6109
		[SerializeField]
		private FadeGroup placeHolderFadeGroup;

		// Token: 0x040017DE RID: 6110
		[SerializeField]
		private Button beginCustomizeButton;

		// Token: 0x040017DF RID: 6111
		[SerializeField]
		private TextMeshProUGUI selectedItemName;

		// Token: 0x040017E0 RID: 6112
		[SerializeField]
		private Image selectedItemIcon;

		// Token: 0x040017E1 RID: 6113
		[SerializeField]
		private TrueShadow selectedItemShadow;

		// Token: 0x040017E2 RID: 6114
		[SerializeField]
		private GameObject customizableIndicator;

		// Token: 0x040017E3 RID: 6115
		[SerializeField]
		private GameObject uncustomizableIndicator;

		// Token: 0x040017E4 RID: 6116
		[SerializeField]
		private string noItemSelectedNameText = "-";

		// Token: 0x040017E5 RID: 6117
		[SerializeField]
		private Sprite noItemSelectedIconSprite;

		// Token: 0x040017E6 RID: 6118
		private List<Inventory> avaliableInventories = new List<Inventory>();
	}
}
