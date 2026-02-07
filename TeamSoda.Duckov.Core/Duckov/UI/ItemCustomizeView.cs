using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003D0 RID: 976
	public class ItemCustomizeView : View, ISingleSelectionMenu<SlotDisplay>
	{
		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x0600233C RID: 9020 RVA: 0x0007B27B File Offset: 0x0007947B
		public static ItemCustomizeView Instance
		{
			get
			{
				return View.GetViewInstance<ItemCustomizeView>();
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x0600233D RID: 9021 RVA: 0x0007B284 File Offset: 0x00079484
		private PrefabPool<ItemDisplay> ItemDisplayPool
		{
			get
			{
				if (this._itemDisplayPool == null)
				{
					this.itemDisplayTemplate.gameObject.SetActive(false);
					this._itemDisplayPool = new PrefabPool<ItemDisplay>(this.itemDisplayTemplate, this.itemDisplayTemplate.transform.parent, null, null, null, true, 10, 10000, null);
				}
				return this._itemDisplayPool;
			}
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x0007B2DD File Offset: 0x000794DD
		private void OnGetInventoryDisplay(InventoryDisplay display)
		{
			display.onDisplayDoubleClicked += this.OnInventoryDoubleClicked;
			display.ShowOperationButtons = false;
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x0007B2F8 File Offset: 0x000794F8
		private void OnReleaseInventoryDisplay(InventoryDisplay display)
		{
			display.onDisplayDoubleClicked -= this.OnInventoryDoubleClicked;
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x0007B30C File Offset: 0x0007950C
		private void OnInventoryDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
		{
			if (entry.Item != null)
			{
				this.target.TryPlug(entry.Item, false, entry.Master.Target, 0);
				data.Use();
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002341 RID: 9025 RVA: 0x0007B341 File Offset: 0x00079541
		public Item Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x0007B349 File Offset: 0x00079549
		public void Setup(Item target, List<Inventory> avaliableInventories)
		{
			this.target = target;
			this.customizingTargetDisplay.Setup(target);
			this.avaliableInventories.Clear();
			this.avaliableInventories.AddRange(avaliableInventories);
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x0007B375 File Offset: 0x00079575
		public void DebugSetup(Item target, Inventory inventory1, Inventory inventory2)
		{
			this.Setup(target, new List<Inventory>
			{
				inventory1,
				inventory2
			});
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x0007B391 File Offset: 0x00079591
		protected override void OnOpen()
		{
			base.OnOpen();
			ItemUIUtilities.Select(null);
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
			this.fadeGroup.Show();
			this.SetSelection(null);
			this.RefreshDetails();
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x0007B3C9 File Offset: 0x000795C9
		protected override void OnClose()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
			base.OnClose();
			this.fadeGroup.Hide();
			this.selectedItemDisplayFadeGroup.Hide();
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x0007B3F8 File Offset: 0x000795F8
		private void OnItemSelectionChanged()
		{
			this.RefreshDetails();
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x0007B400 File Offset: 0x00079600
		private void RefreshDetails()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.selectedItemDisplayFadeGroup.Show();
				this.selectedItemDisplay.Setup(ItemUIUtilities.SelectedItem);
				Item item = this.selectedItemDisplay.Target;
				bool flag = this.selectedSlotDisplay.Target.Content != item;
				this.equipButton.gameObject.SetActive(flag);
				this.unequipButton.gameObject.SetActive(!flag);
				return;
			}
			this.selectedItemDisplayFadeGroup.Hide();
			this.equipButton.gameObject.SetActive(false);
			this.unequipButton.gameObject.SetActive(false);
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x0007B4AC File Offset: 0x000796AC
		protected override void Awake()
		{
			base.Awake();
			this.equipButton.onClick.AddListener(new UnityAction(this.OnEquipButtonClicked));
			this.unequipButton.onClick.AddListener(new UnityAction(this.OnUnequipButtonClicked));
			this.customizingTargetDisplay.SlotCollectionDisplay.onElementClicked += this.OnSlotElementClicked;
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x0007B514 File Offset: 0x00079714
		private void OnUnequipButtonClicked()
		{
			if (this.selectedSlotDisplay == null)
			{
				return;
			}
			if (this.selectedItemDisplay == null)
			{
				return;
			}
			Slot slot = this.selectedSlotDisplay.Target;
			if (slot.Content != null)
			{
				Item item = slot.Unplug();
				this.HandleUnpluggledItem(item);
			}
			this.RefreshAvaliableItems();
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x0007B570 File Offset: 0x00079770
		private void OnEquipButtonClicked()
		{
			if (this.selectedSlotDisplay == null)
			{
				return;
			}
			if (this.selectedItemDisplay == null)
			{
				return;
			}
			Slot slot = this.selectedSlotDisplay.Target;
			Item item = this.selectedItemDisplay.Target;
			if (slot == null)
			{
				return;
			}
			if (item == null)
			{
				return;
			}
			if (slot.Content != null)
			{
				Item item2 = slot.Unplug();
				this.HandleUnpluggledItem(item2);
			}
			item.Detach();
			Item item3;
			if (!slot.Plug(item, ref item3))
			{
				Debug.LogError("装备失败！");
				this.HandleUnpluggledItem(item);
			}
			this.RefreshAvaliableItems();
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x0007B605 File Offset: 0x00079805
		private void HandleUnpluggledItem(Item item)
		{
			if (PlayerStorage.Inventory)
			{
				ItemUtilities.SendToPlayerStorage(item, false);
				return;
			}
			if (!ItemUtilities.SendToPlayerCharacterInventory(item, false))
			{
				ItemUtilities.SendToPlayerStorage(item, false);
			}
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x0007B62B File Offset: 0x0007982B
		private void OnSlotElementClicked(ItemSlotCollectionDisplay collection, SlotDisplay slot)
		{
			this.SetSelection(slot);
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x0007B635 File Offset: 0x00079835
		public SlotDisplay GetSelection()
		{
			return this.selectedSlotDisplay;
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x0007B63D File Offset: 0x0007983D
		public bool SetSelection(SlotDisplay selection)
		{
			this.selectedSlotDisplay = selection;
			this.RefreshSelectionIndicator();
			this.OnSlotSelectionChanged();
			return true;
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x0007B654 File Offset: 0x00079854
		private void RefreshSelectionIndicator()
		{
			this.slotSelectionIndicator.gameObject.SetActive(this.selectedSlotDisplay);
			if (this.selectedSlotDisplay != null)
			{
				this.slotSelectionIndicator.position = this.selectedSlotDisplay.transform.position;
			}
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x0007B6A5 File Offset: 0x000798A5
		private void OnSlotSelectionChanged()
		{
			ItemUIUtilities.Select(null);
			this.RefreshAvaliableItems();
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x0007B6B4 File Offset: 0x000798B4
		private void RefreshAvaliableItems()
		{
			this.avaliableItems.Clear();
			if (!(this.selectedSlotDisplay == null))
			{
				Slot slot = this.selectedSlotDisplay.Target;
				if (!(this.selectedSlotDisplay == null))
				{
					foreach (Inventory inventory in this.avaliableInventories)
					{
						foreach (Item item in inventory)
						{
							if (!(item == null) && slot.CanPlug(item))
							{
								this.avaliableItems.Add(item);
							}
						}
					}
				}
			}
			this.RefreshItemListGraphics();
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x0007B788 File Offset: 0x00079988
		private void RefreshItemListGraphics()
		{
			Debug.Log("Refreshing Item List Graphics");
			bool flag = this.selectedSlotDisplay != null;
			bool flag2 = this.avaliableItems.Count > 0;
			this.selectSlotPlaceHolder.SetActive(!flag);
			this.noAvaliableItemPlaceHolder.SetActive(flag && !flag2);
			this.avaliableItemsContainer.SetActive(flag2);
			this.ItemDisplayPool.ReleaseAll();
			if (flag2)
			{
				foreach (Item item in this.avaliableItems)
				{
					if (!(item == null))
					{
						ItemDisplay itemDisplay = this.ItemDisplayPool.Get(null);
						itemDisplay.ShowOperationButtons = false;
						itemDisplay.Setup(item);
					}
				}
			}
		}

		// Token: 0x040017E7 RID: 6119
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040017E8 RID: 6120
		[SerializeField]
		private Button equipButton;

		// Token: 0x040017E9 RID: 6121
		[SerializeField]
		private Button unequipButton;

		// Token: 0x040017EA RID: 6122
		[SerializeField]
		private ItemDetailsDisplay customizingTargetDisplay;

		// Token: 0x040017EB RID: 6123
		[SerializeField]
		private ItemDetailsDisplay selectedItemDisplay;

		// Token: 0x040017EC RID: 6124
		[SerializeField]
		private FadeGroup selectedItemDisplayFadeGroup;

		// Token: 0x040017ED RID: 6125
		[SerializeField]
		private RectTransform slotSelectionIndicator;

		// Token: 0x040017EE RID: 6126
		[SerializeField]
		private GameObject selectSlotPlaceHolder;

		// Token: 0x040017EF RID: 6127
		[SerializeField]
		private GameObject avaliableItemsContainer;

		// Token: 0x040017F0 RID: 6128
		[SerializeField]
		private GameObject noAvaliableItemPlaceHolder;

		// Token: 0x040017F1 RID: 6129
		[SerializeField]
		private ItemDisplay itemDisplayTemplate;

		// Token: 0x040017F2 RID: 6130
		private PrefabPool<ItemDisplay> _itemDisplayPool;

		// Token: 0x040017F3 RID: 6131
		private Item target;

		// Token: 0x040017F4 RID: 6132
		private SlotDisplay selectedSlotDisplay;

		// Token: 0x040017F5 RID: 6133
		private List<Inventory> avaliableInventories = new List<Inventory>();

		// Token: 0x040017F6 RID: 6134
		private List<Item> avaliableItems = new List<Item>();
	}
}
