using System;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Crops.UI
{
	// Token: 0x02000303 RID: 771
	public class GardenViewCropSelector : MonoBehaviour
	{
		// Token: 0x06001945 RID: 6469 RVA: 0x0005CFFE File Offset: 0x0005B1FE
		private void Awake()
		{
			this.btnConfirm.onClick.AddListener(new UnityAction(this.OnConfirm));
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x0005D01C File Offset: 0x0005B21C
		private void OnConfirm()
		{
			Item selectedItem = ItemUIUtilities.SelectedItem;
			if (selectedItem != null)
			{
				this.master.SelectSeed(selectedItem.TypeID);
			}
			this.Hide();
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x0005D050 File Offset: 0x0005B250
		public void Show()
		{
			this.fadeGroup.Show();
			if (LevelManager.Instance == null)
			{
				return;
			}
			ItemUIUtilities.Select(null);
			this.playerInventoryDisplay.Setup(CharacterMainControl.Main.CharacterItem.Inventory, null, null, false, (Item e) => e != null && CropDatabase.IsSeed(e.TypeID));
			this.storageInventoryDisplay.Setup(PlayerStorage.Inventory, null, null, false, (Item e) => e != null && CropDatabase.IsSeed(e.TypeID));
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x0005D0EA File Offset: 0x0005B2EA
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnSelectionChanged;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x0005D0FD File Offset: 0x0005B2FD
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnSelectionChanged;
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x0005D110 File Offset: 0x0005B310
		private void OnSelectionChanged()
		{
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x0005D112 File Offset: 0x0005B312
		public void Hide()
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x04001274 RID: 4724
		[SerializeField]
		private GardenView master;

		// Token: 0x04001275 RID: 4725
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001276 RID: 4726
		[SerializeField]
		private Button btnConfirm;

		// Token: 0x04001277 RID: 4727
		[SerializeField]
		private InventoryDisplay playerInventoryDisplay;

		// Token: 0x04001278 RID: 4728
		[SerializeField]
		private InventoryDisplay storageInventoryDisplay;
	}
}
