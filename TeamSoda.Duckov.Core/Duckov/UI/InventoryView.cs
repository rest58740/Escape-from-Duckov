using System;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003CE RID: 974
	public class InventoryView : View
	{
		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002321 RID: 8993 RVA: 0x0007ACD1 File Offset: 0x00078ED1
		private static InventoryView Instance
		{
			get
			{
				return View.GetViewInstance<InventoryView>();
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002322 RID: 8994 RVA: 0x0007ACD8 File Offset: 0x00078ED8
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

		// Token: 0x06002323 RID: 8995 RVA: 0x0007ACF5 File Offset: 0x00078EF5
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x0007AD00 File Offset: 0x00078F00
		private void Update()
		{
			bool editable = true;
			this.inventoryDisplay.Editable = editable;
			this.slotDisplay.Editable = editable;
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x0007AD28 File Offset: 0x00078F28
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
			this.slotDisplay.Setup(characterItem, false);
			this.inventoryDisplay.Setup(characterItem.Inventory, null, null, false, null);
			this.RegisterEvents();
			this.fadeGroup.Show();
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x0007ADA4 File Offset: 0x00078FA4
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.itemDetailsFadeGroup.Hide();
			if (SplitDialogue.Instance && SplitDialogue.Instance.isActiveAndEnabled)
			{
				SplitDialogue.Instance.Cancel();
			}
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x0007ADF5 File Offset: 0x00078FF5
		private void RegisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0007AE08 File Offset: 0x00079008
		private void OnItemSelectionChanged()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.detailsDisplay.Setup(ItemUIUtilities.SelectedItem);
				this.itemDetailsFadeGroup.Show();
				return;
			}
			this.itemDetailsFadeGroup.Hide();
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x0007AE3E File Offset: 0x0007903E
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x0007AE51 File Offset: 0x00079051
		public static void Show()
		{
			if (!LevelManager.LevelInited)
			{
				return;
			}
			LootView instance = LootView.Instance;
			if (instance != null)
			{
				instance.Show();
			}
			if (LootView.Instance == null)
			{
				Debug.Log("LOOTVIEW INSTANCE IS NULL");
			}
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x0007AE82 File Offset: 0x00079082
		public static void Hide()
		{
			LootView instance = LootView.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Close();
		}

		// Token: 0x040017D2 RID: 6098
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040017D3 RID: 6099
		[SerializeField]
		private ItemSlotCollectionDisplay slotDisplay;

		// Token: 0x040017D4 RID: 6100
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x040017D5 RID: 6101
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x040017D6 RID: 6102
		[SerializeField]
		private FadeGroup itemDetailsFadeGroup;
	}
}
