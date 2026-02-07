using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002F1 RID: 753
	public class MasterKeysRegisterView : View
	{
		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x0600185C RID: 6236 RVA: 0x00059D44 File Offset: 0x00057F44
		public static MasterKeysRegisterView Instance
		{
			get
			{
				return View.GetViewInstance<MasterKeysRegisterView>();
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x0600185D RID: 6237 RVA: 0x00059D4B File Offset: 0x00057F4B
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

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x0600185E RID: 6238 RVA: 0x00059D68 File Offset: 0x00057F68
		private Slot KeySlot
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
				return this.keySlotItem.Slots[this.keySlotKey];
			}
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x00059DA8 File Offset: 0x00057FA8
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

		// Token: 0x06001860 RID: 6240 RVA: 0x00059E34 File Offset: 0x00058034
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
			if (!this.KeySlot.CanPlug(item))
			{
				return;
			}
			item.Detach();
			Item item2;
			this.KeySlot.Plug(item, ref item2);
			if (item2 != null)
			{
				ItemUtilities.SendToPlayer(item2, false, true);
			}
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x00059E90 File Offset: 0x00058090
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

		// Token: 0x06001862 RID: 6242 RVA: 0x00059EBC File Offset: 0x000580BC
		private void OnSubmitButtonClicked()
		{
			if (this.KeySlot != null && this.KeySlot.Content != null && MasterKeysManager.SubmitAndActivate(this.KeySlot.Content))
			{
				this.IndicateSuccess();
			}
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x00059EF1 File Offset: 0x000580F1
		private void IndicateSuccess()
		{
			this.SuccessIndicationTask().Forget();
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x00059F00 File Offset: 0x00058100
		private UniTask SuccessIndicationTask()
		{
			MasterKeysRegisterView.<SuccessIndicationTask>d__24 <SuccessIndicationTask>d__;
			<SuccessIndicationTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SuccessIndicationTask>d__.<>4__this = this;
			<SuccessIndicationTask>d__.<>1__state = -1;
			<SuccessIndicationTask>d__.<>t__builder.Start<MasterKeysRegisterView.<SuccessIndicationTask>d__24>(ref <SuccessIndicationTask>d__);
			return <SuccessIndicationTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x00059F43 File Offset: 0x00058143
		private void HideSuccessIndication()
		{
			this.succeedIndicator.Hide();
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x00059F50 File Offset: 0x00058150
		private bool EntryFunc_ShouldHighligh(Item e)
		{
			return !(e == null) && this.KeySlot.CanPlug(e) && !MasterKeysManager.IsActive(e.TypeID);
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00059F7D File Offset: 0x0005817D
		private bool EntryFunc_CanOperate(Item e)
		{
			return e == null || this.KeySlot.CanPlug(e);
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00059F98 File Offset: 0x00058198
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
			this.inventoryDisplay.Setup(characterItem.Inventory, new Func<Item, bool>(this.EntryFunc_ShouldHighligh), new Func<Item, bool>(this.EntryFunc_CanOperate), false, null);
			if (PlayerStorage.Inventory != null)
			{
				this.playerStorageInventoryDisplay.ShowOperationButtons = false;
				this.playerStorageInventoryDisplay.gameObject.SetActive(true);
				this.playerStorageInventoryDisplay.Setup(PlayerStorage.Inventory, new Func<Item, bool>(this.EntryFunc_ShouldHighligh), new Func<Item, bool>(this.EntryFunc_CanOperate), false, null);
			}
			else
			{
				this.playerStorageInventoryDisplay.gameObject.SetActive(false);
			}
			this.registerSlotDisplay.Setup(this.KeySlot);
			this.RefreshRecordExistsIndicator();
			this.RegisterEvents();
			this.fadeGroup.Show();
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x0005A0A4 File Offset: 0x000582A4
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.detailsFadeGroup.Hide();
			if (this.KeySlot != null && this.KeySlot.Content != null)
			{
				Item content = this.KeySlot.Content;
				content.Detach();
				ItemUtilities.SendToPlayerCharacterInventory(content, false);
			}
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x0005A106 File Offset: 0x00058306
		private void RegisterEvents()
		{
			this.KeySlot.onSlotContentChanged += this.OnSlotContentChanged;
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x0005A130 File Offset: 0x00058330
		private void UnregisterEvents()
		{
			this.KeySlot.onSlotContentChanged -= this.OnSlotContentChanged;
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x0005A15A File Offset: 0x0005835A
		private void OnSlotContentChanged(Slot slot)
		{
			this.RefreshRecordExistsIndicator();
			this.HideSuccessIndication();
			if (((slot != null) ? slot.Content : null) != null)
			{
				AudioManager.PlayPutItemSFX(slot.Content, false);
			}
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x0005A188 File Offset: 0x00058388
		private void RefreshRecordExistsIndicator()
		{
			Item content = this.KeySlot.Content;
			if (content == null)
			{
				this.recordExistsIndicator.SetActive(false);
				return;
			}
			bool active = MasterKeysManager.IsActive(content.TypeID);
			this.recordExistsIndicator.SetActive(active);
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x0005A1CF File Offset: 0x000583CF
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

		// Token: 0x0600186F RID: 6255 RVA: 0x0005A205 File Offset: 0x00058405
		public static void Show()
		{
			if (MasterKeysRegisterView.Instance == null)
			{
				return;
			}
			MasterKeysRegisterView.Instance.Open(null);
		}

		// Token: 0x040011CD RID: 4557
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040011CE RID: 4558
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x040011CF RID: 4559
		[SerializeField]
		private InventoryDisplay playerStorageInventoryDisplay;

		// Token: 0x040011D0 RID: 4560
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x040011D1 RID: 4561
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x040011D2 RID: 4562
		[SerializeField]
		private Button submitButton;

		// Token: 0x040011D3 RID: 4563
		[SerializeField]
		private Item keySlotItem;

		// Token: 0x040011D4 RID: 4564
		[SerializeField]
		private string keySlotKey = "Key";

		// Token: 0x040011D5 RID: 4565
		[SerializeField]
		private SlotDisplay registerSlotDisplay;

		// Token: 0x040011D6 RID: 4566
		[SerializeField]
		private GameObject recordExistsIndicator;

		// Token: 0x040011D7 RID: 4567
		[SerializeField]
		private FadeGroup succeedIndicator;

		// Token: 0x040011D8 RID: 4568
		[SerializeField]
		private float successIndicationTime = 1.5f;

		// Token: 0x040011D9 RID: 4569
		private string sfx_Register = "UI/register";
	}
}
