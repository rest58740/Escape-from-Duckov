using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003D3 RID: 979
	public class LootView : View
	{
		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002380 RID: 9088 RVA: 0x0007C666 File Offset: 0x0007A866
		public static LootView Instance
		{
			get
			{
				return View.GetViewInstance<LootView>();
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002381 RID: 9089 RVA: 0x0007C66D File Offset: 0x0007A86D
		private CharacterMainControl Character
		{
			get
			{
				return LevelManager.Instance.MainCharacter;
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002382 RID: 9090 RVA: 0x0007C679 File Offset: 0x0007A879
		private Item CharacterItem
		{
			get
			{
				if (this.Character == null)
				{
					return null;
				}
				return this.Character.CharacterItem;
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002383 RID: 9091 RVA: 0x0007C696 File Offset: 0x0007A896
		public Inventory TargetInventory
		{
			get
			{
				if (this.targetLootBox != null)
				{
					return this.targetLootBox.Inventory;
				}
				if (this.targetInventory)
				{
					return this.targetInventory;
				}
				return null;
			}
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x0007C6C7 File Offset: 0x0007A8C7
		public static bool HasInventoryEverBeenLooted(Inventory inventory)
		{
			return !(LootView.Instance == null) && LootView.Instance.lootedInventories != null && !(inventory == null) && LootView.Instance.lootedInventories.Contains(inventory);
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x0007C704 File Offset: 0x0007A904
		protected override void Awake()
		{
			base.Awake();
			InteractableLootbox.OnStartLoot += this.OnStartLoot;
			this.pickAllButton.onClick.AddListener(new UnityAction(this.OnPickAllButtonClicked));
			CharacterMainControl.OnMainCharacterStartUseItem += this.OnMainCharacterStartUseItem;
			LevelManager.OnMainCharacterDead += this.OnMainCharacterDead;
			this.storeAllButton.onClick.AddListener(new UnityAction(this.OnStoreAllButtonClicked));
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x0007C784 File Offset: 0x0007A984
		private void OnStoreAllButtonClicked()
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			if (this.TargetInventory != PlayerStorage.Inventory)
			{
				return;
			}
			if (this.CharacterItem == null)
			{
				return;
			}
			Inventory inventory = this.CharacterItem.Inventory;
			if (inventory == null)
			{
				return;
			}
			int lastItemPosition = inventory.GetLastItemPosition();
			for (int i = 0; i <= lastItemPosition; i++)
			{
				if (!inventory.lockedIndexes.Contains(i))
				{
					Item itemAt = inventory.GetItemAt(i);
					if (!(itemAt == null))
					{
						if (!this.TargetInventory.AddAndMerge(itemAt, 0))
						{
							break;
						}
						if (i == 0)
						{
							AudioManager.PlayPutItemSFX(itemAt, false);
						}
					}
				}
			}
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x0007C823 File Offset: 0x0007AA23
		protected override void OnDestroy()
		{
			this.UnregisterEvents();
			InteractableLootbox.OnStartLoot -= this.OnStartLoot;
			LevelManager.OnMainCharacterDead -= this.OnMainCharacterDead;
			base.OnDestroy();
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x0007C853 File Offset: 0x0007AA53
		private void OnMainCharacterStartUseItem(Item _item)
		{
			if (base.open)
			{
				base.Close();
			}
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x0007C863 File Offset: 0x0007AA63
		private void OnMainCharacterDead(DamageInfo dmgInfo)
		{
			if (base.open)
			{
				base.Close();
			}
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x0007C873 File Offset: 0x0007AA73
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x0007C87B File Offset: 0x0007AA7B
		private void OnDisable()
		{
			this.UnregisterEvents();
			InteractableLootbox interactableLootbox = this.targetLootBox;
			if (interactableLootbox != null)
			{
				interactableLootbox.StopInteract();
			}
			this.targetLootBox = null;
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x0007C89B File Offset: 0x0007AA9B
		public void Show()
		{
			base.Open(null);
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x0007C8A4 File Offset: 0x0007AAA4
		private void OnStartLoot(InteractableLootbox lootbox)
		{
			this.targetLootBox = lootbox;
			if (this.targetLootBox == null || this.targetLootBox.Inventory == null)
			{
				Debug.LogError("Target loot box could not be found");
				return;
			}
			base.Open(null);
			if (this.TargetInventory != null)
			{
				this.lootedInventories.Add(this.TargetInventory);
			}
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x0007C90B File Offset: 0x0007AB0B
		private void OnStopLoot(InteractableLootbox lootbox)
		{
			if (lootbox == this.targetLootBox)
			{
				this.targetLootBox = null;
				base.Close();
			}
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x0007C928 File Offset: 0x0007AB28
		public static void LootItem(Item item)
		{
			if (item == null)
			{
				return;
			}
			if (LootView.Instance == null)
			{
				return;
			}
			LootView.Instance.targetInventory = item.Inventory;
			LootView.Instance.Open(null);
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x0007C960 File Offset: 0x0007AB60
		protected override void OnOpen()
		{
			base.OnOpen();
			this.UnregisterEvents();
			base.gameObject.SetActive(true);
			this.characterSlotCollectionDisplay.Setup(this.CharacterItem, true);
			if (PetProxy.PetInventory && LevelConfig.SavePet)
			{
				this.petInventoryDisplay.gameObject.SetActive(true);
				this.petInventoryDisplay.Setup(PetProxy.PetInventory, null, null, false, null);
			}
			else
			{
				this.petInventoryDisplay.gameObject.SetActive(false);
			}
			this.characterInventoryDisplay.Setup(this.CharacterItem.Inventory, null, null, true, null);
			if (this.targetLootBox != null)
			{
				this.lootTargetInventoryDisplay.ShowSortButton = this.targetLootBox.ShowSortButton;
				this.lootTargetInventoryDisplay.Setup(this.TargetInventory, null, null, true, null);
				this.lootTargetDisplayName.text = this.TargetInventory.DisplayName;
				if (this.TargetInventory.GetComponent<InventoryFilterProvider>())
				{
					this.lootTargetFilterDisplay.gameObject.SetActive(true);
					this.lootTargetFilterDisplay.Setup(this.lootTargetInventoryDisplay);
					this.lootTargetFilterDisplay.Select(0);
				}
				else
				{
					this.lootTargetFilterDisplay.gameObject.SetActive(false);
				}
				this.lootTargetFadeGroup.Show();
			}
			else if (this.targetInventory != null)
			{
				this.lootTargetInventoryDisplay.ShowSortButton = false;
				this.lootTargetInventoryDisplay.Setup(this.TargetInventory, null, null, true, null);
				this.lootTargetFadeGroup.Show();
				this.lootTargetFilterDisplay.gameObject.SetActive(false);
			}
			else
			{
				this.lootTargetFadeGroup.SkipHide();
			}
			bool active = this.TargetInventory != null && this.TargetInventory == PlayerStorage.Inventory;
			this.storeAllButton.gameObject.SetActive(active);
			this.fadeGroup.Show();
			this.RefreshDetails();
			this.RefreshPickAllButton();
			this.RegisterEvents();
			this.RefreshCapacityText();
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x0007CB5C File Offset: 0x0007AD5C
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
			this.detailsFadeGroup.Hide();
			InteractableLootbox interactableLootbox = this.targetLootBox;
			if (interactableLootbox != null)
			{
				interactableLootbox.StopInteract();
			}
			this.targetLootBox = null;
			this.targetInventory = null;
			if (SplitDialogue.Instance && SplitDialogue.Instance.isActiveAndEnabled)
			{
				SplitDialogue.Instance.Cancel();
			}
			this.UnregisterEvents();
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x0007CBCC File Offset: 0x0007ADCC
		private void OnTargetInventoryContentChanged(Inventory inventory, int arg2)
		{
			this.RefreshPickAllButton();
			this.RefreshCapacityText();
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x0007CBDC File Offset: 0x0007ADDC
		private void RefreshCapacityText()
		{
			if (this.targetLootBox != null)
			{
				this.lootTargetCapacityText.text = StringExtensions.Format(this.lootTargetCapacityTextFormat, new
				{
					itemCount = this.TargetInventory.GetItemCount(),
					capacity = this.TargetInventory.Capacity
				});
			}
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x0007CC28 File Offset: 0x0007AE28
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			ItemUIUtilities.OnSelectionChanged += this.OnSelectionChanged;
			this.lootTargetInventoryDisplay.onDisplayDoubleClicked += this.OnLootTargetItemDoubleClicked;
			this.characterInventoryDisplay.onDisplayDoubleClicked += this.OnCharacterInventoryItemDoubleClicked;
			this.petInventoryDisplay.onDisplayDoubleClicked += this.OnCharacterInventoryItemDoubleClicked;
			this.characterSlotCollectionDisplay.onElementDoubleClicked += this.OnCharacterSlotItemDoubleClicked;
			if (this.TargetInventory)
			{
				this.TargetInventory.onContentChanged += this.OnTargetInventoryContentChanged;
			}
			UIInputManager.OnNextPage += this.OnNextPage;
			UIInputManager.OnPreviousPage += this.OnPreviousPage;
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x0007CCEE File Offset: 0x0007AEEE
		private void OnPreviousPage(UIInputEventData data)
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			if (!this.lootTargetInventoryDisplay.UsePages)
			{
				return;
			}
			this.lootTargetInventoryDisplay.PreviousPage();
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x0007CD18 File Offset: 0x0007AF18
		private void OnNextPage(UIInputEventData data)
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			if (!this.lootTargetInventoryDisplay.UsePages)
			{
				return;
			}
			this.lootTargetInventoryDisplay.NextPage();
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x0007CD44 File Offset: 0x0007AF44
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnSelectionChanged;
			if (this.lootTargetInventoryDisplay)
			{
				this.lootTargetInventoryDisplay.onDisplayDoubleClicked -= this.OnLootTargetItemDoubleClicked;
			}
			if (this.characterInventoryDisplay)
			{
				this.characterInventoryDisplay.onDisplayDoubleClicked -= this.OnCharacterInventoryItemDoubleClicked;
			}
			if (this.petInventoryDisplay)
			{
				this.petInventoryDisplay.onDisplayDoubleClicked -= this.OnCharacterInventoryItemDoubleClicked;
			}
			if (this.characterSlotCollectionDisplay)
			{
				this.characterSlotCollectionDisplay.onElementDoubleClicked -= this.OnCharacterSlotItemDoubleClicked;
			}
			if (this.TargetInventory)
			{
				this.TargetInventory.onContentChanged -= this.OnTargetInventoryContentChanged;
			}
			UIInputManager.OnNextPage -= this.OnNextPage;
			UIInputManager.OnPreviousPage -= this.OnPreviousPage;
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x0007CE38 File Offset: 0x0007B038
		private void OnCharacterSlotItemDoubleClicked(ItemSlotCollectionDisplay collectionDisplay, SlotDisplay slotDisplay)
		{
			if (slotDisplay == null)
			{
				return;
			}
			Slot target = slotDisplay.Target;
			if (target == null)
			{
				return;
			}
			Item content = target.Content;
			if (content == null)
			{
				return;
			}
			if (this.TargetInventory == null)
			{
				return;
			}
			if (content.Sticky && !this.TargetInventory.AcceptSticky)
			{
				return;
			}
			AudioManager.PlayPutItemSFX(content, false);
			content.Detach();
			if (this.TargetInventory.AddAndMerge(content, 0))
			{
				this.RefreshDetails();
				return;
			}
			Item item;
			if (!target.Plug(content, ref item))
			{
				Debug.LogError("Failed plugging back!");
			}
			if (item != null)
			{
				Debug.Log("Unplugged item should be null!");
			}
			this.RefreshDetails();
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x0007CEE4 File Offset: 0x0007B0E4
		private void OnCharacterInventoryItemDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
		{
			Item content = entry.Content;
			if (content == null)
			{
				return;
			}
			Inventory inInventory = content.InInventory;
			if (this.TargetInventory == null)
			{
				return;
			}
			if (content.Sticky && !this.TargetInventory.AcceptSticky)
			{
				return;
			}
			AudioManager.PlayPutItemSFX(content, false);
			content.Detach();
			if (this.TargetInventory.AddAndMerge(content, 0))
			{
				this.RefreshDetails();
				return;
			}
			if (!inInventory.AddAndMerge(content, 0))
			{
				Debug.LogError("Failed sending back item");
			}
			this.RefreshDetails();
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0007CF6B File Offset: 0x0007B16B
		private void OnSelectionChanged()
		{
			this.RefreshDetails();
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0007CF73 File Offset: 0x0007B173
		private void RefreshDetails()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.detailsFadeGroup.Show();
				this.detailsDisplay.Setup(ItemUIUtilities.SelectedItem);
				return;
			}
			this.detailsFadeGroup.Hide();
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x0007CFAC File Offset: 0x0007B1AC
		private void OnLootTargetItemDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
		{
			Item item = entry.Item;
			if (item == null)
			{
				return;
			}
			if (!item.IsInPlayerCharacter())
			{
				if (this.targetLootBox != null && this.targetLootBox.needInspect && !item.Inspected)
				{
					data.Use();
					return;
				}
				data.Use();
				bool flag = false;
				LevelManager instance = LevelManager.Instance;
				bool? flag2;
				if (instance == null)
				{
					flag2 = null;
				}
				else
				{
					CharacterMainControl mainCharacter = instance.MainCharacter;
					if (mainCharacter == null)
					{
						flag2 = null;
					}
					else
					{
						Item characterItem = mainCharacter.CharacterItem;
						flag2 = ((characterItem != null) ? new bool?(characterItem.TryPlug(item, true, null, 0)) : null);
					}
				}
				bool? flag3 = flag2;
				flag |= flag3.Value;
				if (flag3 == null || !flag3.Value)
				{
					flag |= ItemUtilities.SendToPlayerCharacterInventory(item, false);
				}
				if (flag)
				{
					AudioManager.PlayPutItemSFX(item, false);
					this.RefreshDetails();
				}
			}
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x0007D088 File Offset: 0x0007B288
		private void RefreshPickAllButton()
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			this.pickAllButton.gameObject.SetActive(false);
			bool interactable = this.TargetInventory.GetItemCount() > 0;
			this.pickAllButton.interactable = interactable;
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x0007D0D0 File Offset: 0x0007B2D0
		private void OnPickAllButtonClicked()
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			List<Item> list = new List<Item>();
			list.AddRange(this.TargetInventory);
			foreach (Item item in list)
			{
				if (!(item == null) && (!this.targetLootBox.needInspect || item.Inspected))
				{
					LevelManager instance = LevelManager.Instance;
					bool? flag;
					if (instance == null)
					{
						flag = null;
					}
					else
					{
						CharacterMainControl mainCharacter = instance.MainCharacter;
						if (mainCharacter == null)
						{
							flag = null;
						}
						else
						{
							Item characterItem = mainCharacter.CharacterItem;
							flag = ((characterItem != null) ? new bool?(characterItem.TryPlug(item, true, null, 0)) : null);
						}
					}
					bool? flag2 = flag;
					if (flag2 == null || !flag2.Value)
					{
						ItemUtilities.SendToPlayerCharacterInventory(item, false);
					}
				}
			}
			AudioManager.Post("UI/confirm");
		}

		// Token: 0x0400181D RID: 6173
		[SerializeField]
		private ItemSlotCollectionDisplay characterSlotCollectionDisplay;

		// Token: 0x0400181E RID: 6174
		[SerializeField]
		private InventoryDisplay characterInventoryDisplay;

		// Token: 0x0400181F RID: 6175
		[SerializeField]
		private InventoryDisplay petInventoryDisplay;

		// Token: 0x04001820 RID: 6176
		[SerializeField]
		private InventoryDisplay lootTargetInventoryDisplay;

		// Token: 0x04001821 RID: 6177
		[SerializeField]
		private InventoryFilterDisplay lootTargetFilterDisplay;

		// Token: 0x04001822 RID: 6178
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001823 RID: 6179
		[SerializeField]
		private Button pickAllButton;

		// Token: 0x04001824 RID: 6180
		[SerializeField]
		private TextMeshProUGUI lootTargetDisplayName;

		// Token: 0x04001825 RID: 6181
		[SerializeField]
		private TextMeshProUGUI lootTargetCapacityText;

		// Token: 0x04001826 RID: 6182
		[SerializeField]
		private string lootTargetCapacityTextFormat = "({itemCount}/{capacity})";

		// Token: 0x04001827 RID: 6183
		[SerializeField]
		private Button storeAllButton;

		// Token: 0x04001828 RID: 6184
		[SerializeField]
		private FadeGroup lootTargetFadeGroup;

		// Token: 0x04001829 RID: 6185
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x0400182A RID: 6186
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x0400182B RID: 6187
		[SerializeField]
		private InteractableLootbox targetLootBox;

		// Token: 0x0400182C RID: 6188
		private Inventory targetInventory;

		// Token: 0x0400182D RID: 6189
		private HashSet<Inventory> lootedInventories = new HashSet<Inventory>();
	}
}
