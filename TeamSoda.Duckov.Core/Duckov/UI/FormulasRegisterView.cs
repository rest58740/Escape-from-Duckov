using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A2 RID: 930
	public class FormulasRegisterView : View
	{
		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06002055 RID: 8277 RVA: 0x00071A53 File Offset: 0x0006FC53
		public static FormulasRegisterView Instance
		{
			get
			{
				return View.GetViewInstance<FormulasRegisterView>();
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06002056 RID: 8278 RVA: 0x00071A5A File Offset: 0x0006FC5A
		private string FormulaUnlockedNotificationFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.formulaUnlockedFormatKey);
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06002057 RID: 8279 RVA: 0x00071A67 File Offset: 0x0006FC67
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

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06002058 RID: 8280 RVA: 0x00071A84 File Offset: 0x0006FC84
		private Slot SubmitItemSlot
		{
			get
			{
				if (this.keySlotItem == null)
				{
					return null;
				}
				if (this.keySlotItem.Slots == null)
				{
					return null;
				}
				return this.keySlotItem.Slots[this.slotKey];
			}
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x00071AC4 File Offset: 0x0006FCC4
		protected override void Awake()
		{
			base.Awake();
			this.submitButton.onClick.AddListener(new UnityAction(this.OnSubmitButtonClicked));
			this.succeedIndicator.SkipHide();
			this.detailsFadeGroup.SkipHide();
			this.registerSlotDisplay.onSlotDisplayDoubleClicked += this.OnSlotDoubleClicked;
			this.inventoryDisplay.onDisplayDoubleClicked += this.OnInventoryItemDoubleClicked;
			this.playerStorageInventoryDisplay.onDisplayDoubleClicked += this.OnInventoryItemDoubleClicked;
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x00071B50 File Offset: 0x0006FD50
		private void OnInventoryItemDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
		{
			if (!entry.Editable)
			{
				return;
			}
			Item item = entry.Item;
			if (item == null)
			{
				return;
			}
			if (!this.SubmitItemSlot.CanPlug(item))
			{
				return;
			}
			item.Detach();
			Item item2;
			this.SubmitItemSlot.Plug(item, ref item2);
			if (item2 != null)
			{
				ItemUtilities.SendToPlayer(item2, false, true);
			}
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x00071BAC File Offset: 0x0006FDAC
		private void OnSlotDoubleClicked(SlotDisplay display)
		{
			Item item = display.GetItem();
			if (item == null)
			{
				return;
			}
			item.Detach();
			ItemUtilities.SendToPlayer(item, false, true);
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x00071BD8 File Offset: 0x0006FDD8
		private void OnSubmitButtonClicked()
		{
			if (this.SubmitItemSlot != null && this.SubmitItemSlot.Content != null)
			{
				Item content = this.SubmitItemSlot.Content;
				string formulaID = FormulasRegisterView.GetFormulaID(content);
				if (string.IsNullOrEmpty(formulaID))
				{
					return;
				}
				if (CraftingManager.IsFormulaUnlocked(formulaID))
				{
					return;
				}
				CraftingManager.UnlockFormula(formulaID);
				CraftingFormula formula = CraftingManager.GetFormula(formulaID);
				if (formula.IDValid)
				{
					ItemMetaData metaData = ItemAssetsCollection.GetMetaData(formula.result.id);
					string mainText = StringExtensions.Format(this.FormulaUnlockedNotificationFormat, new
					{
						itemDisplayName = metaData.DisplayName
					});
					Sprite icon = metaData.icon;
					StrongNotification.Push(new StrongNotificationContent(mainText, "", icon));
				}
				content.Detach();
				ItemTreeExtensions.DestroyTreeImmediate(content);
				this.IndicateSuccess();
			}
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x00071C94 File Offset: 0x0006FE94
		private void IndicateSuccess()
		{
			this.SuccessIndicationTask().Forget();
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x00071CA4 File Offset: 0x0006FEA4
		private UniTask SuccessIndicationTask()
		{
			FormulasRegisterView.<SuccessIndicationTask>d__28 <SuccessIndicationTask>d__;
			<SuccessIndicationTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SuccessIndicationTask>d__.<>4__this = this;
			<SuccessIndicationTask>d__.<>1__state = -1;
			<SuccessIndicationTask>d__.<>t__builder.Start<FormulasRegisterView.<SuccessIndicationTask>d__28>(ref <SuccessIndicationTask>d__);
			return <SuccessIndicationTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x00071CE7 File Offset: 0x0006FEE7
		private void HideSuccessIndication()
		{
			this.succeedIndicator.Hide();
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x00071CF4 File Offset: 0x0006FEF4
		private bool EntryFunc_ShouldHighligh(Item e)
		{
			return !(e == null) && this.SubmitItemSlot.CanPlug(e) && !CraftingManager.IsFormulaUnlocked(FormulasRegisterView.GetFormulaID(e));
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x00071D21 File Offset: 0x0006FF21
		private bool EntryFunc_CanOperate(Item e)
		{
			return e == null || this.SubmitItemSlot.CanPlug(e);
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x00071D3C File Offset: 0x0006FF3C
		protected override void OnOpen()
		{
			this.UnregisterEvents();
			base.OnOpen();
			Item characterItem = this.CharacterItem;
			if (characterItem == null)
			{
				Debug.LogError("物品栏开启失败，角色物体不存在");
				base.Close();
				return;
			}
			base.gameObject.SetActive(true);
			this.inventoryDisplay.ShowOperationButtons = false;
			this.inventoryDisplay.Setup(characterItem.Inventory, new Func<Item, bool>(this.EntryFunc_ShouldHighligh), new Func<Item, bool>(this.EntryFunc_CanOperate), true, null);
			if (PlayerStorage.Inventory != null)
			{
				this.playerStorageInventoryDisplay.ShowOperationButtons = false;
				this.playerStorageInventoryDisplay.gameObject.SetActive(true);
				this.playerStorageInventoryDisplay.Setup(PlayerStorage.Inventory, new Func<Item, bool>(this.EntryFunc_ShouldHighligh), new Func<Item, bool>(this.EntryFunc_CanOperate), true, null);
			}
			else
			{
				this.playerStorageInventoryDisplay.gameObject.SetActive(false);
			}
			this.registerSlotDisplay.Setup(this.SubmitItemSlot);
			this.RefreshRecordExistsIndicator();
			this.RegisterEvents();
			this.fadeGroup.Show();
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x00071E48 File Offset: 0x00070048
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.detailsFadeGroup.Hide();
			if (this.SubmitItemSlot != null && this.SubmitItemSlot.Content != null)
			{
				Item content = this.SubmitItemSlot.Content;
				content.Detach();
				ItemUtilities.SendToPlayerCharacterInventory(content, false);
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x00071EAA File Offset: 0x000700AA
		private void RegisterEvents()
		{
			this.SubmitItemSlot.onSlotContentChanged += this.OnSlotContentChanged;
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x00071ED4 File Offset: 0x000700D4
		private void UnregisterEvents()
		{
			this.SubmitItemSlot.onSlotContentChanged -= this.OnSlotContentChanged;
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x00071EFE File Offset: 0x000700FE
		private void OnSlotContentChanged(Slot slot)
		{
			this.RefreshRecordExistsIndicator();
			this.HideSuccessIndication();
			if (((slot != null) ? slot.Content : null) != null)
			{
				AudioManager.PlayPutItemSFX(slot.Content, false);
			}
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x00071F2C File Offset: 0x0007012C
		private void RefreshRecordExistsIndicator()
		{
			Item content = this.SubmitItemSlot.Content;
			if (content == null)
			{
				this.recordExistsIndicator.SetActive(false);
				return;
			}
			bool active = CraftingManager.IsFormulaUnlocked(FormulasRegisterView.GetFormulaID(content));
			this.recordExistsIndicator.SetActive(active);
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x00071F73 File Offset: 0x00070173
		private bool IsFormulaItem(Item item)
		{
			return !(item == null) && item.GetComponent<ItemSetting_Formula>() != null;
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x00071F8C File Offset: 0x0007018C
		public static string GetFormulaID(Item item)
		{
			if (item == null)
			{
				return null;
			}
			ItemSetting_Formula component = item.GetComponent<ItemSetting_Formula>();
			if (component == null)
			{
				return null;
			}
			return component.formulaID;
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x00071FBC File Offset: 0x000701BC
		private void OnItemSelectionChanged()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.detailsDisplay.Setup(ItemUIUtilities.SelectedItem);
				this.detailsFadeGroup.Show();
				return;
			}
			this.detailsFadeGroup.Hide();
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x00071FF2 File Offset: 0x000701F2
		public static void Show(ICollection<Tag> requireTags = null)
		{
			if (FormulasRegisterView.Instance == null)
			{
				return;
			}
			FormulasRegisterView.SetupTags(requireTags);
			FormulasRegisterView.Instance.Open(null);
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x00072014 File Offset: 0x00070214
		private static void SetupTags(ICollection<Tag> requireTags = null)
		{
			if (FormulasRegisterView.Instance == null)
			{
				return;
			}
			Slot submitItemSlot = FormulasRegisterView.Instance.SubmitItemSlot;
			if (submitItemSlot == null)
			{
				return;
			}
			submitItemSlot.requireTags.Clear();
			submitItemSlot.requireTags.Add(FormulasRegisterView.Instance.formulaTag);
			if (requireTags != null)
			{
				submitItemSlot.requireTags.AddRange(requireTags);
			}
		}

		// Token: 0x04001617 RID: 5655
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001618 RID: 5656
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x04001619 RID: 5657
		[SerializeField]
		private InventoryDisplay playerStorageInventoryDisplay;

		// Token: 0x0400161A RID: 5658
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x0400161B RID: 5659
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x0400161C RID: 5660
		[SerializeField]
		private Button submitButton;

		// Token: 0x0400161D RID: 5661
		[SerializeField]
		private Tag formulaTag;

		// Token: 0x0400161E RID: 5662
		[SerializeField]
		private Item keySlotItem;

		// Token: 0x0400161F RID: 5663
		[SerializeField]
		private string slotKey = "SubmitItem";

		// Token: 0x04001620 RID: 5664
		[SerializeField]
		private SlotDisplay registerSlotDisplay;

		// Token: 0x04001621 RID: 5665
		[SerializeField]
		private GameObject recordExistsIndicator;

		// Token: 0x04001622 RID: 5666
		[SerializeField]
		private FadeGroup succeedIndicator;

		// Token: 0x04001623 RID: 5667
		[SerializeField]
		private float successIndicationTime = 1.5f;

		// Token: 0x04001624 RID: 5668
		private string sfx_Register = "UI/register";

		// Token: 0x04001625 RID: 5669
		[LocalizationKey("Default")]
		[SerializeField]
		private string formulaUnlockedFormatKey = "UI_Formulas_RegisterSucceedFormat";
	}
}
