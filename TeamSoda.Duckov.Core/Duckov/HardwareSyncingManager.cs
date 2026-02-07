using System;
using Duckov.Buildings;
using Duckov.Economy;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000240 RID: 576
	public class HardwareSyncingManager : MonoBehaviour
	{
		// Token: 0x14000080 RID: 128
		// (add) Token: 0x0600122B RID: 4651 RVA: 0x00046BFC File Offset: 0x00044DFC
		// (remove) Token: 0x0600122C RID: 4652 RVA: 0x00046C30 File Offset: 0x00044E30
		public static event Action<string> OnSetEvent;

		// Token: 0x0600122D RID: 4653 RVA: 0x00046C64 File Offset: 0x00044E64
		public static void SetEvent(string eventName)
		{
			try
			{
				Action<string> onSetEvent = HardwareSyncingManager.OnSetEvent;
				if (onSetEvent != null)
				{
					onSetEvent(eventName);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00046C9C File Offset: 0x00044E9C
		private void Awake()
		{
			InteractableLootbox.OnStartLoot += this.OnStartLoot;
			Health.OnHurt += this.OnHurt;
			Health.OnDead += this.OnDead;
			MainMenu.OnMainMenuAwake = (Action)Delegate.Combine(MainMenu.OnMainMenuAwake, new Action(this.OnMainMenuAwake));
			MainMenu.OnMainMenuDestroy = (Action)Delegate.Combine(MainMenu.OnMainMenuDestroy, new Action(this.OnMainMenuDestroy));
			LevelManager.OnEvacuated += this.OnEvacuated;
			PauseMenu.onPauseMenuOn += this.OnPauseMenuOn;
			PauseMenu.onPauseMenuOff += this.OnPauseMenuOff;
			StockShop.OnItemPurchased += this.OnItemPurchased;
			StockShop.OnItemSoldByPlayer += this.OnItemSoldByPlayer;
			CA_UseItem.OnItemUsedByPlayer += this.OnItemUsedByPlayer;
			BuildingManager.OnBuildingBuilt += this.OnBuildingBuilt;
			EXPManager.onLevelChanged = (Action<int, int>)Delegate.Combine(EXPManager.onLevelChanged, new Action<int, int>(this.OnLevelChanged));
			ItemUtilities.OnItemSentToPlayerInventory += this.OnItemSentToPlayerInventory;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00046DC4 File Offset: 0x00044FC4
		private void OnDestroy()
		{
			InteractableLootbox.OnStartLoot -= this.OnStartLoot;
			Health.OnHurt -= this.OnHurt;
			Health.OnDead -= this.OnDead;
			MainMenu.OnMainMenuAwake = (Action)Delegate.Remove(MainMenu.OnMainMenuAwake, new Action(this.OnMainMenuAwake));
			MainMenu.OnMainMenuDestroy = (Action)Delegate.Remove(MainMenu.OnMainMenuDestroy, new Action(this.OnMainMenuDestroy));
			LevelManager.OnEvacuated -= this.OnEvacuated;
			PauseMenu.onPauseMenuOn -= this.OnPauseMenuOn;
			PauseMenu.onPauseMenuOff -= this.OnPauseMenuOff;
			CA_UseItem.OnItemUsedByPlayer -= this.OnItemUsedByPlayer;
			BuildingManager.OnBuildingBuilt -= this.OnBuildingBuilt;
			EXPManager.onLevelChanged = (Action<int, int>)Delegate.Remove(EXPManager.onLevelChanged, new Action<int, int>(this.OnLevelChanged));
			ItemUtilities.OnItemSentToPlayerInventory -= this.OnItemSentToPlayerInventory;
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00046ECA File Offset: 0x000450CA
		private void OnItemSentToPlayerInventory(Item item)
		{
			HardwareSyncingManager.SetEvent("Get_Item");
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00046ED6 File Offset: 0x000450D6
		private void OnLevelChanged(int arg1, int arg2)
		{
			HardwareSyncingManager.SetEvent("LevelUp");
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00046EE2 File Offset: 0x000450E2
		private void OnBuildingBuilt(int obj)
		{
			HardwareSyncingManager.SetEvent("Build_Furniture");
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x00046EEE File Offset: 0x000450EE
		private void OnItemUsedByPlayer(Item item)
		{
			HardwareSyncingManager.SetEvent("Use_Item");
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00046EFA File Offset: 0x000450FA
		private void OnItemSoldByPlayer(StockShop shop, Item item, int arg3)
		{
			HardwareSyncingManager.SetEvent("Sell");
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00046F06 File Offset: 0x00045106
		private void OnItemPurchased(StockShop shop, Item item)
		{
			HardwareSyncingManager.SetEvent("Buy");
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00046F12 File Offset: 0x00045112
		private void OnPauseMenuOn()
		{
			HardwareSyncingManager.SetEvent("PauseMenu_On");
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x00046F1E File Offset: 0x0004511E
		private void OnPauseMenuOff()
		{
			HardwareSyncingManager.SetEvent("PauseMenu_Off");
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x00046F2A File Offset: 0x0004512A
		private void OnEvacuated(EvacuationInfo info)
		{
			HardwareSyncingManager.SetEvent("Escaped");
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00046F36 File Offset: 0x00045136
		private void OnMainMenuAwake()
		{
			HardwareSyncingManager.SetEvent("MainMenu_On");
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00046F42 File Offset: 0x00045142
		private void OnMainMenuDestroy()
		{
			HardwareSyncingManager.SetEvent("MainMenu_Off");
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00046F50 File Offset: 0x00045150
		private void OnDead(Health health, DamageInfo info)
		{
			if (health == null)
			{
				return;
			}
			if (health.IsMainCharacterHealth)
			{
				HardwareSyncingManager.SetEvent("Die");
				return;
			}
			if (info.fromCharacter != null && info.fromCharacter.IsMainCharacter)
			{
				HardwareSyncingManager.SetEvent("Kill_Enemy");
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00046F9F File Offset: 0x0004519F
		private void OnHurt(Health health, DamageInfo info)
		{
			if (health == null)
			{
				return;
			}
			if (health.IsMainCharacterHealth)
			{
				HardwareSyncingManager.SetEvent("Take_Damage");
			}
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00046FBD File Offset: 0x000451BD
		private void OnStartLoot(InteractableLootbox lootbox)
		{
			HardwareSyncingManager.SetEvent("Interaction");
		}
	}
}
