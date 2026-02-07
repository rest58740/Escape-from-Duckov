using System;
using System.Collections.Generic;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000371 RID: 881
	public class SubmitItems : Task
	{
		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001EE5 RID: 7909 RVA: 0x0006E1B6 File Offset: 0x0006C3B6
		public int ItemTypeID
		{
			get
			{
				return this.itemTypeID;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001EE6 RID: 7910 RVA: 0x0006E1C0 File Offset: 0x0006C3C0
		private ItemMetaData CachedMeta
		{
			get
			{
				if (this._cachedMeta == null || this._cachedMeta.Value.id != this.itemTypeID)
				{
					this._cachedMeta = new ItemMetaData?(ItemAssetsCollection.GetMetaData(this.itemTypeID));
				}
				return this._cachedMeta.Value;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x0006E213 File Offset: 0x0006C413
		private string descriptionFormatKey
		{
			get
			{
				return "Task_SubmitItems";
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001EE8 RID: 7912 RVA: 0x0006E21A File Offset: 0x0006C41A
		private string DescriptionFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionFormatKey);
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001EE9 RID: 7913 RVA: 0x0006E227 File Offset: 0x0006C427
		private string havingAmountFormatKey
		{
			get
			{
				return "Task_SubmitItems_HavingAmount";
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001EEA RID: 7914 RVA: 0x0006E22E File Offset: 0x0006C42E
		private string HavingAmountFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.havingAmountFormatKey);
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001EEB RID: 7915 RVA: 0x0006E23C File Offset: 0x0006C43C
		public override string Description
		{
			get
			{
				string text = StringExtensions.Format(this.DescriptionFormat, new
				{
					ItemDisplayName = this.CachedMeta.DisplayName,
					submittedAmount = this.submittedAmount,
					requiredAmount = this.requiredAmount
				});
				if (!base.IsFinished())
				{
					text = text + " " + StringExtensions.Format(this.HavingAmountFormat, new
					{
						amount = ItemUtilities.GetItemCount(this.itemTypeID)
					});
				}
				return text;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001EEC RID: 7916 RVA: 0x0006E2A4 File Offset: 0x0006C4A4
		public override Sprite Icon
		{
			get
			{
				return this.CachedMeta.icon;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001EED RID: 7917 RVA: 0x0006E2B1 File Offset: 0x0006C4B1
		public override bool Interactable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001EEE RID: 7918 RVA: 0x0006E2B4 File Offset: 0x0006C4B4
		public override bool PossibleValidInteraction
		{
			get
			{
				return this.CheckItemEnough();
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001EEF RID: 7919 RVA: 0x0006E2BC File Offset: 0x0006C4BC
		public override string InteractText
		{
			get
			{
				return LocalizationManager.ToPlainText("Task_SubmitItems_Interact");
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001EF0 RID: 7920 RVA: 0x0006E2C8 File Offset: 0x0006C4C8
		public override bool NeedInspection
		{
			get
			{
				return !base.IsFinished() && this.CheckItemEnough();
			}
		}

		// Token: 0x140000DC RID: 220
		// (add) Token: 0x06001EF1 RID: 7921 RVA: 0x0006E2DC File Offset: 0x0006C4DC
		// (remove) Token: 0x06001EF2 RID: 7922 RVA: 0x0006E310 File Offset: 0x0006C510
		public static event Action<SubmitItems> onItemEnough;

		// Token: 0x06001EF3 RID: 7923 RVA: 0x0006E343 File Offset: 0x0006C543
		protected override void OnInit()
		{
			base.OnInit();
			PlayerStorage.OnPlayerStorageChange += this.OnPlayerStorageChanged;
			CharacterMainControl.OnMainCharacterInventoryChangedEvent = (Action<CharacterMainControl, Inventory, int>)Delegate.Combine(CharacterMainControl.OnMainCharacterInventoryChangedEvent, new Action<CharacterMainControl, Inventory, int>(this.OnMainCharacterInventoryChanged));
			this.CheckItemEnough();
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0006E383 File Offset: 0x0006C583
		private void OnDestroy()
		{
			PlayerStorage.OnPlayerStorageChange -= this.OnPlayerStorageChanged;
			CharacterMainControl.OnMainCharacterInventoryChangedEvent = (Action<CharacterMainControl, Inventory, int>)Delegate.Remove(CharacterMainControl.OnMainCharacterInventoryChangedEvent, new Action<CharacterMainControl, Inventory, int>(this.OnMainCharacterInventoryChanged));
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x0006E3B8 File Offset: 0x0006C5B8
		private void OnPlayerStorageChanged(PlayerStorage storage, Inventory inventory, int index)
		{
			if (base.Master.Complete)
			{
				return;
			}
			Item itemAt = inventory.GetItemAt(index);
			if (itemAt == null)
			{
				return;
			}
			if (itemAt.TypeID == this.itemTypeID)
			{
				this.CheckItemEnough();
			}
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0006E3FC File Offset: 0x0006C5FC
		private void OnMainCharacterInventoryChanged(CharacterMainControl control, Inventory inventory, int index)
		{
			if (base.Master.Complete)
			{
				return;
			}
			Item itemAt = inventory.GetItemAt(index);
			if (itemAt == null)
			{
				return;
			}
			if (itemAt.TypeID == this.itemTypeID)
			{
				this.CheckItemEnough();
			}
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x0006E43E File Offset: 0x0006C63E
		private bool CheckItemEnough()
		{
			if (ItemUtilities.GetItemCount(this.itemTypeID) >= this.requiredAmount)
			{
				Action<SubmitItems> action = SubmitItems.onItemEnough;
				if (action != null)
				{
					action(this);
				}
				this.SetMapElementVisable(false);
				return true;
			}
			this.SetMapElementVisable(true);
			return false;
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0006E475 File Offset: 0x0006C675
		private void SetMapElementVisable(bool visable)
		{
			if (!this.mapElement)
			{
				return;
			}
			if (visable)
			{
				this.mapElement.name = base.Master.DisplayName;
			}
			this.mapElement.SetVisibility(visable);
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x0006E4AC File Offset: 0x0006C6AC
		public void Submit(Item item)
		{
			if (item.TypeID != this.itemTypeID)
			{
				Debug.LogError("提交的物品类型与需求不一致。");
				return;
			}
			int num = this.requiredAmount - this.submittedAmount;
			if (num <= 0)
			{
				Debug.LogError("目标已达成，不需要继续提交物品");
				return;
			}
			int num2 = this.submittedAmount;
			if (num < item.StackCount)
			{
				item.StackCount -= num;
				this.submittedAmount += num;
			}
			else
			{
				foreach (Item item2 in ItemTreeExtensions.GetAllChildren(item, false, true))
				{
					item2.Detach();
					if (!ItemUtilities.SendToPlayerCharacter(item2, false))
					{
						item2.Drop(CharacterMainControl.Main, true);
					}
				}
				item.Detach();
				ItemTreeExtensions.DestroyTree(item);
				this.submittedAmount += item.StackCount;
			}
			Debug.Log("submission done");
			if (num2 != this.submittedAmount)
			{
				base.Master.NotifyTaskFinished(this);
			}
			base.ReportStatusChanged();
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x0006E5C0 File Offset: 0x0006C7C0
		protected override bool CheckFinished()
		{
			return this.submittedAmount >= this.requiredAmount;
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x0006E5D3 File Offset: 0x0006C7D3
		public override object GenerateSaveData()
		{
			return this.submittedAmount;
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x0006E5E0 File Offset: 0x0006C7E0
		public override void SetupSaveData(object data)
		{
			this.submittedAmount = (int)data;
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x0006E5F0 File Offset: 0x0006C7F0
		public override void Interact()
		{
			if (base.Master == null)
			{
				return;
			}
			List<Item> list = ItemUtilities.FindAllBelongsToPlayer((Item e) => e != null && e.TypeID == this.itemTypeID);
			for (int i = 0; i < list.Count; i++)
			{
				Item item = list[i];
				this.Submit(item);
				if (base.IsFinished())
				{
					break;
				}
			}
		}

		// Token: 0x04001543 RID: 5443
		[ItemTypeID]
		[SerializeField]
		private int itemTypeID;

		// Token: 0x04001544 RID: 5444
		[Range(1f, 100f)]
		[SerializeField]
		private int requiredAmount = 1;

		// Token: 0x04001545 RID: 5445
		[SerializeField]
		private int submittedAmount;

		// Token: 0x04001546 RID: 5446
		private ItemMetaData? _cachedMeta;

		// Token: 0x04001547 RID: 5447
		[SerializeField]
		private MapElementForTask mapElement;
	}
}
