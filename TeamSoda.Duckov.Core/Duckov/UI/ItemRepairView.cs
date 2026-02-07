using System;
using System.Collections.Generic;
using Duckov.Economy;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003D2 RID: 978
	public class ItemRepairView : View
	{
		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002368 RID: 9064 RVA: 0x0007BD60 File Offset: 0x00079F60
		public static ItemRepairView Instance
		{
			get
			{
				return View.GetViewInstance<ItemRepairView>();
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002369 RID: 9065 RVA: 0x0007BD67 File Offset: 0x00079F67
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

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x0600236A RID: 9066 RVA: 0x0007BD84 File Offset: 0x00079F84
		private bool CanRepair
		{
			get
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				if (selectedItem == null)
				{
					return false;
				}
				if (!selectedItem.UseDurability)
				{
					return false;
				}
				if (selectedItem.MaxDurabilityWithLoss < 1f)
				{
					return false;
				}
				if (!selectedItem.Tags.Contains("Repairable"))
				{
					Debug.Log(selectedItem.DisplayName + " 不包含tag Repairable");
					return false;
				}
				return selectedItem.Durability < selectedItem.MaxDurabilityWithLoss;
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x0600236B RID: 9067 RVA: 0x0007BDF4 File Offset: 0x00079FF4
		private bool NoNeedToRepair
		{
			get
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				return !(selectedItem == null) && selectedItem.UseDurability && selectedItem.Durability >= selectedItem.MaxDurabilityWithLoss;
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x0600236C RID: 9068 RVA: 0x0007BE30 File Offset: 0x0007A030
		private bool Broken
		{
			get
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				return !(selectedItem == null) && selectedItem.UseDurability && selectedItem.MaxDurabilityWithLoss < 1f;
			}
		}

		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x0600236D RID: 9069 RVA: 0x0007BE68 File Offset: 0x0007A068
		// (remove) Token: 0x0600236E RID: 9070 RVA: 0x0007BE9C File Offset: 0x0007A09C
		public static event Action OnRepaireOptionDone;

		// Token: 0x0600236F RID: 9071 RVA: 0x0007BECF File Offset: 0x0007A0CF
		protected override void Awake()
		{
			base.Awake();
			this.repairButton.onClick.AddListener(new UnityAction(this.OnRepairButtonClicked));
			this.itemDetailsFadeGroup.SkipHide();
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x0007BF00 File Offset: 0x0007A100
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

		// Token: 0x06002371 RID: 9073 RVA: 0x0007BF78 File Offset: 0x0007A178
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
			this.repairButtonFadeGroup.SkipHide();
			this.placeHolderFadeGroup.SkipHide();
			ItemUIUtilities.Select(null);
			this.RefreshSelectedItemInfo();
			this.repairAllPanel.Setup(this);
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x0007C019 File Offset: 0x0007A219
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.itemDetailsFadeGroup.Hide();
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x0007C03D File Offset: 0x0007A23D
		private void RegisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x0007C050 File Offset: 0x0007A250
		private void OnItemSelectionChanged()
		{
			this.RefreshSelectedItemInfo();
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x0007C058 File Offset: 0x0007A258
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x0007C06B File Offset: 0x0007A26B
		public static void Show()
		{
			if (ItemRepairView.Instance == null)
			{
				return;
			}
			ItemRepairView.Instance.Open(null);
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x0007C086 File Offset: 0x0007A286
		public static void Hide()
		{
			if (ItemRepairView.Instance == null)
			{
				return;
			}
			ItemRepairView.Instance.Close();
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x0007C0A0 File Offset: 0x0007A2A0
		private void RefreshSelectedItemInfo()
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
			if (this.CanRepair)
			{
				this.placeHolderFadeGroup.Hide();
				this.repairButtonFadeGroup.Show();
			}
			else
			{
				this.repairButtonFadeGroup.Hide();
				this.placeHolderFadeGroup.Show();
			}
			Item selectedItem = ItemUIUtilities.SelectedItem;
			this.willLoseDurabilityText.text = "";
			if (selectedItem == null)
			{
				this.selectedItemName.text = this.noItemSelectedNameText;
				this.selectedItemIcon.sprite = this.noItemSelectedIconSprite;
				this.selectedItemShadow.enabled = false;
				this.noNeedToRepairIndicator.SetActive(false);
				this.brokenIndicator.SetActive(false);
				this.cannotRepairIndicator.SetActive(false);
				this.selectedItemIcon.color = Color.clear;
				this.barFill.fillAmount = 0f;
				this.lossBarFill.fillAmount = 0f;
				this.durabilityText.text = "-";
				return;
			}
			this.selectedItemShadow.enabled = true;
			this.selectedItemIcon.color = Color.white;
			this.selectedItemName.text = selectedItem.DisplayName;
			this.selectedItemIcon.sprite = selectedItem.Icon;
			GameplayDataSettings.UIStyle.GetDisplayQualityLook(selectedItem.DisplayQuality).Apply(this.selectedItemShadow);
			this.noNeedToRepairIndicator.SetActive(!this.Broken && this.NoNeedToRepair && selectedItem.Repairable);
			this.cannotRepairIndicator.SetActive(selectedItem.UseDurability && !selectedItem.Repairable && !this.Broken);
			this.brokenIndicator.SetActive(this.Broken);
			if (this.CanRepair)
			{
				float num2;
				float num3;
				float num4;
				int num = this.CalculateRepairPrice(selectedItem, out num2, out num3, out num4);
				this.repairPriceText.text = num.ToString();
				this.willLoseDurabilityText.text = LocalizationManager.ToPlainText("UI_MaxDurability") + " -" + num3.ToString("0.#");
				this.repairButton.interactable = (EconomyManager.Money >= (long)num);
			}
			if (selectedItem.UseDurability)
			{
				float durability = selectedItem.Durability;
				float maxDurability = selectedItem.MaxDurability;
				float maxDurabilityWithLoss = selectedItem.MaxDurabilityWithLoss;
				float num5 = durability / maxDurability;
				this.barFill.fillAmount = num5;
				this.lossBarFill.fillAmount = selectedItem.DurabilityLoss;
				this.durabilityText.text = string.Format("{0:0.#} / {1} ", durability, maxDurabilityWithLoss.ToString("0.#"));
				this.barFill.color = this.barFillColorOverT.Evaluate(num5);
				return;
			}
			this.barFill.fillAmount = 0f;
			this.lossBarFill.fillAmount = 0f;
			this.durabilityText.text = "-";
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x0007C3A4 File Offset: 0x0007A5A4
		private void OnRepairButtonClicked()
		{
			Item selectedItem = ItemUIUtilities.SelectedItem;
			if (selectedItem == null)
			{
				return;
			}
			if (!selectedItem.UseDurability)
			{
				return;
			}
			this.Repair(selectedItem, false);
			this.RefreshSelectedItemInfo();
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x0007C3D8 File Offset: 0x0007A5D8
		private void Repair(Item item, bool prepaied = false)
		{
			float num2;
			float num3;
			float num4;
			int num = this.CalculateRepairPrice(item, out num2, out num3, out num4);
			if (!prepaied && !EconomyManager.Pay(new Cost((long)num), true, true))
			{
				return;
			}
			item.DurabilityLoss += num4;
			item.Durability = item.MaxDurability * (1f - item.DurabilityLoss);
			Action onRepaireOptionDone = ItemRepairView.OnRepaireOptionDone;
			if (onRepaireOptionDone == null)
			{
				return;
			}
			onRepaireOptionDone();
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x0007C440 File Offset: 0x0007A640
		private int CalculateRepairPrice(Item item, out float repairAmount, out float lostAmount, out float lostPercentage)
		{
			repairAmount = 0f;
			lostAmount = 0f;
			lostPercentage = 0f;
			if (item == null)
			{
				return 0;
			}
			if (!item.UseDurability)
			{
				return 0;
			}
			float maxDurability = item.MaxDurability;
			float durabilityLoss = item.DurabilityLoss;
			float num = maxDurability * (1f - durabilityLoss);
			float durability = item.Durability;
			repairAmount = num - durability;
			float repairLossRatio = item.GetRepairLossRatio();
			lostAmount = repairAmount * repairLossRatio;
			repairAmount -= lostAmount;
			if (repairAmount <= 0f)
			{
				return 0;
			}
			lostPercentage = lostAmount / maxDurability;
			float num2 = repairAmount / maxDurability;
			return Mathf.CeilToInt((float)item.Value * num2 * 0.5f);
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x0007C4E0 File Offset: 0x0007A6E0
		public List<Item> GetAllEquippedItems()
		{
			CharacterMainControl main = CharacterMainControl.Main;
			if (main == null)
			{
				return null;
			}
			Item characterItem = main.CharacterItem;
			if (characterItem == null)
			{
				return null;
			}
			SlotCollection slots = characterItem.Slots;
			if (slots == null)
			{
				return null;
			}
			List<Item> list = new List<Item>();
			foreach (Slot slot in slots)
			{
				if (slot != null)
				{
					Item content = slot.Content;
					if (!(content == null))
					{
						list.Add(content);
					}
				}
			}
			return list;
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x0007C584 File Offset: 0x0007A784
		public int CalculateRepairPrice(List<Item> itemsToRepair)
		{
			int num = 0;
			foreach (Item item in itemsToRepair)
			{
				float num2;
				float num3;
				float num4;
				num += this.CalculateRepairPrice(item, out num2, out num3, out num4);
			}
			return num;
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x0007C5E0 File Offset: 0x0007A7E0
		public void RepairItems(List<Item> itemsToRepair)
		{
			if (!EconomyManager.Pay(new Cost((long)this.CalculateRepairPrice(itemsToRepair)), true, true))
			{
				return;
			}
			foreach (Item item in itemsToRepair)
			{
				this.Repair(item, true);
			}
		}

		// Token: 0x04001804 RID: 6148
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001805 RID: 6149
		[SerializeField]
		private ItemSlotCollectionDisplay slotDisplay;

		// Token: 0x04001806 RID: 6150
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x04001807 RID: 6151
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x04001808 RID: 6152
		[SerializeField]
		private FadeGroup itemDetailsFadeGroup;

		// Token: 0x04001809 RID: 6153
		[SerializeField]
		private ItemRepair_RepairAllPanel repairAllPanel;

		// Token: 0x0400180A RID: 6154
		[SerializeField]
		private FadeGroup repairButtonFadeGroup;

		// Token: 0x0400180B RID: 6155
		[SerializeField]
		private FadeGroup placeHolderFadeGroup;

		// Token: 0x0400180C RID: 6156
		[SerializeField]
		private Button repairButton;

		// Token: 0x0400180D RID: 6157
		[SerializeField]
		private TextMeshProUGUI repairPriceText;

		// Token: 0x0400180E RID: 6158
		[SerializeField]
		private TextMeshProUGUI selectedItemName;

		// Token: 0x0400180F RID: 6159
		[SerializeField]
		private Image selectedItemIcon;

		// Token: 0x04001810 RID: 6160
		[SerializeField]
		private TrueShadow selectedItemShadow;

		// Token: 0x04001811 RID: 6161
		[SerializeField]
		private string noItemSelectedNameText = "-";

		// Token: 0x04001812 RID: 6162
		[SerializeField]
		private Sprite noItemSelectedIconSprite;

		// Token: 0x04001813 RID: 6163
		[SerializeField]
		private GameObject noNeedToRepairIndicator;

		// Token: 0x04001814 RID: 6164
		[SerializeField]
		private GameObject brokenIndicator;

		// Token: 0x04001815 RID: 6165
		[SerializeField]
		private GameObject cannotRepairIndicator;

		// Token: 0x04001816 RID: 6166
		[SerializeField]
		private TextMeshProUGUI durabilityText;

		// Token: 0x04001817 RID: 6167
		[SerializeField]
		private TextMeshProUGUI willLoseDurabilityText;

		// Token: 0x04001818 RID: 6168
		[SerializeField]
		private Image barFill;

		// Token: 0x04001819 RID: 6169
		[SerializeField]
		private Image lossBarFill;

		// Token: 0x0400181A RID: 6170
		[SerializeField]
		private Gradient barFillColorOverT;

		// Token: 0x0400181B RID: 6171
		private List<Inventory> avaliableInventories = new List<Inventory>();
	}
}
