using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Bilibili.BDS;
using Duckov;
using Duckov.Buffs;
using Duckov.Buildings;
using Duckov.Economy;
using Duckov.MasterKeys;
using Duckov.PerkTrees;
using Duckov.Quests;
using Duckov.Rules;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using Saves;
using Steamworks;
using UnityEngine;

namespace EventReports
{
	// Token: 0x02000231 RID: 561
	public class BDSManager : MonoBehaviour
	{
		// Token: 0x0600115B RID: 4443 RVA: 0x00043CBC File Offset: 0x00041EBC
		private void Awake()
		{
			if (PlatformInfo.Platform == Platform.Steam)
			{
				if (SteamManager.Initialized && SteamUtils.IsSteamChinaLauncher())
				{
				}
			}
			else
			{
				string.Format("{0}", PlatformInfo.Platform);
			}
			Debug.Log("Player Info:\n" + BDSManager.PlayerInfo.GetCurrent().ToJson());
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00043D12 File Offset: 0x00041F12
		private void Start()
		{
			this.OnGameStarted();
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00043D1A File Offset: 0x00041F1A
		private void OnDestroy()
		{
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x00043D1C File Offset: 0x00041F1C
		private float TimeSinceLastHeartbeat
		{
			get
			{
				return Time.unscaledTime - this.lastTimeHeartbeat;
			}
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00043D2A File Offset: 0x00041F2A
		private void Update()
		{
			bool isPlaying = Application.isPlaying;
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00043D32 File Offset: 0x00041F32
		private void UpdateHeartbeat()
		{
			if (this.TimeSinceLastHeartbeat > 60f)
			{
				this.ReportCustomEvent(BDSManager.EventName.heartbeat, "");
				this.lastTimeHeartbeat = Time.unscaledTime;
			}
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x00043D5C File Offset: 0x00041F5C
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			SavesSystem.OnSaveDeleted += this.OnSaveDeleted;
			RaidUtilities.OnNewRaid += this.OnNewRaid;
			RaidUtilities.OnRaidEnd += this.OnRaidEnd;
			SceneLoader.onStartedLoadingScene += this.OnSceneLoadingStart;
			SceneLoader.onFinishedLoadingScene += this.OnSceneLoadingFinish;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			LevelManager.OnEvacuated += this.OnEvacuated;
			LevelManager.OnMainCharacterDead += this.OnMainCharacterDead;
			Quest.onQuestActivated += this.OnQuestActivated;
			Quest.onQuestCompleted += this.OnQuestCompleted;
			EconomyManager.OnCostPaid += this.OnCostPaid;
			EconomyManager.OnMoneyPaid += this.OnMoneyPaid;
			ItemUtilities.OnItemSentToPlayerInventory += this.OnItemSentToPlayerInventory;
			ItemUtilities.OnItemSentToPlayerStorage += this.OnItemSentToPlayerStorage;
			StockShop.OnItemPurchased += this.OnItemPurchased;
			CraftingManager.OnItemCrafted = (Action<CraftingFormula, Item>)Delegate.Combine(CraftingManager.OnItemCrafted, new Action<CraftingFormula, Item>(this.OnItemCrafted));
			CraftingManager.OnFormulaUnlocked = (Action<string>)Delegate.Combine(CraftingManager.OnFormulaUnlocked, new Action<string>(this.OnFormulaUnlocked));
			Health.OnDead += this.OnHealthDead;
			EXPManager.onLevelChanged = (Action<int, int>)Delegate.Combine(EXPManager.onLevelChanged, new Action<int, int>(this.OnLevelChanged));
			BuildingManager.OnBuildingBuiltComplex += this.OnBuildingBuilt;
			BuildingManager.OnBuildingDestroyedComplex += this.OnBuildingDestroyed;
			Perk.OnPerkUnlockConfirmed += this.OnPerkUnlockConfirmed;
			MasterKeysManager.OnMasterKeyUnlocked += this.OnMasterKeyUnlocked;
			CharacterMainControl.OnMainCharacterSlotContentChangedEvent = (Action<CharacterMainControl, Slot>)Delegate.Combine(CharacterMainControl.OnMainCharacterSlotContentChangedEvent, new Action<CharacterMainControl, Slot>(this.OnMainCharacterSlotContentChanged));
			StockShop.OnItemSoldByPlayer += this.OnItemSold;
			Reward.OnRewardClaimed += this.OnRewardClaimed;
			UsageUtilities.OnItemUsedStaticEvent += this.OnItemUsed;
			InteractableBase.OnInteractStartStaticEvent += this.OnInteractStart;
			LevelManager.OnNewGameReport += this.OnNewGameReport;
			Interact_CustomFace.OnCustomFaceStartEvent += this.OnCustomFaceStart;
			Interact_CustomFace.OnCustomFaceFinishedEvent += this.OnCustomFaceFinish;
			CheatMode.OnCheatModeStatusChanged += this.OnCheatModeStatusChanged;
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x00043FCC File Offset: 0x000421CC
		private void UnregisterEvents()
		{
			SavesSystem.OnSaveDeleted -= this.OnSaveDeleted;
			RaidUtilities.OnNewRaid -= this.OnNewRaid;
			RaidUtilities.OnRaidEnd -= this.OnRaidEnd;
			SceneLoader.onStartedLoadingScene -= this.OnSceneLoadingStart;
			SceneLoader.onFinishedLoadingScene -= this.OnSceneLoadingFinish;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
			LevelManager.OnEvacuated -= this.OnEvacuated;
			LevelManager.OnMainCharacterDead -= this.OnMainCharacterDead;
			Quest.onQuestActivated -= this.OnQuestActivated;
			Quest.onQuestCompleted -= this.OnQuestCompleted;
			EconomyManager.OnCostPaid -= this.OnCostPaid;
			EconomyManager.OnMoneyPaid -= this.OnMoneyPaid;
			ItemUtilities.OnItemSentToPlayerInventory -= this.OnItemSentToPlayerInventory;
			ItemUtilities.OnItemSentToPlayerStorage -= this.OnItemSentToPlayerStorage;
			StockShop.OnItemPurchased -= this.OnItemPurchased;
			CraftingManager.OnItemCrafted = (Action<CraftingFormula, Item>)Delegate.Remove(CraftingManager.OnItemCrafted, new Action<CraftingFormula, Item>(this.OnItemCrafted));
			CraftingManager.OnFormulaUnlocked = (Action<string>)Delegate.Remove(CraftingManager.OnFormulaUnlocked, new Action<string>(this.OnFormulaUnlocked));
			Health.OnDead -= this.OnHealthDead;
			EXPManager.onLevelChanged = (Action<int, int>)Delegate.Remove(EXPManager.onLevelChanged, new Action<int, int>(this.OnLevelChanged));
			BuildingManager.OnBuildingBuiltComplex -= this.OnBuildingBuilt;
			BuildingManager.OnBuildingDestroyedComplex -= this.OnBuildingDestroyed;
			Perk.OnPerkUnlockConfirmed -= this.OnPerkUnlockConfirmed;
			MasterKeysManager.OnMasterKeyUnlocked -= this.OnMasterKeyUnlocked;
			CharacterMainControl.OnMainCharacterSlotContentChangedEvent = (Action<CharacterMainControl, Slot>)Delegate.Remove(CharacterMainControl.OnMainCharacterSlotContentChangedEvent, new Action<CharacterMainControl, Slot>(this.OnMainCharacterSlotContentChanged));
			StockShop.OnItemSoldByPlayer -= this.OnItemSold;
			Reward.OnRewardClaimed -= this.OnRewardClaimed;
			UsageUtilities.OnItemUsedStaticEvent -= this.OnItemUsed;
			InteractableBase.OnInteractStartStaticEvent -= this.OnInteractStart;
			LevelManager.OnNewGameReport -= this.OnNewGameReport;
			Interact_CustomFace.OnCustomFaceStartEvent -= this.OnCustomFaceStart;
			Interact_CustomFace.OnCustomFaceFinishedEvent -= this.OnCustomFaceFinish;
			CheatMode.OnCheatModeStatusChanged -= this.OnCheatModeStatusChanged;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00044238 File Offset: 0x00042438
		private void OnCheatModeStatusChanged(bool value)
		{
			this.ReportCustomEvent<BDSManager.CheatModeStatusChangeContext>(BDSManager.EventName.cheat_mode_changed, new BDSManager.CheatModeStatusChangeContext
			{
				cheatModeActive = value
			});
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0004425E File Offset: 0x0004245E
		private void OnCustomFaceFinish()
		{
			this.ReportCustomEvent(BDSManager.EventName.face_customize_finish, "");
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0004426D File Offset: 0x0004246D
		private void OnCustomFaceStart()
		{
			this.ReportCustomEvent(BDSManager.EventName.face_customize_begin, "");
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x0004427C File Offset: 0x0004247C
		private void OnNewGameReport()
		{
			this.ReportCustomEvent(BDSManager.EventName.begin_new_game, "");
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x0004428C File Offset: 0x0004248C
		private void OnInteractStart(InteractableBase target)
		{
			if (target == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.InteractEventContext>(BDSManager.EventName.interact_start, new BDSManager.InteractEventContext
			{
				interactGameObjectName = target.name,
				typeName = target.GetType().Name
			});
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x000442D4 File Offset: 0x000424D4
		private void OnItemUsed(Item item)
		{
			this.ReportCustomEvent<BDSManager.ItemUseEventContext>(BDSManager.EventName.item_use, new BDSManager.ItemUseEventContext
			{
				itemTypeID = item.TypeID
			});
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00044300 File Offset: 0x00042500
		private void OnRewardClaimed(Reward reward)
		{
			int questID = (reward.Master != null) ? reward.Master.ID : -1;
			this.ReportCustomEvent<BDSManager.RewardClaimEventContext>(BDSManager.EventName.reward_claimed, new BDSManager.RewardClaimEventContext
			{
				questID = questID,
				rewardID = reward.ID
			});
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00044350 File Offset: 0x00042550
		private void OnItemSold(StockShop shop, Item item, int price)
		{
			if (item == null)
			{
				return;
			}
			string stockShopID = (shop != null) ? shop.MerchantID : null;
			this.ReportCustomEvent<BDSManager.ItemSoldEventContext>(BDSManager.EventName.item_sold, new BDSManager.ItemSoldEventContext
			{
				stockShopID = stockShopID,
				itemID = item.TypeID,
				price = price
			});
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x000443A4 File Offset: 0x000425A4
		private void OnMainCharacterSlotContentChanged(CharacterMainControl control, Slot slot)
		{
			if (control == null || slot == null)
			{
				return;
			}
			if (slot.Content == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.EquipEventContext>(BDSManager.EventName.role_equip, new BDSManager.EquipEventContext
			{
				slotKey = slot.Key,
				contentItemTypeID = slot.Content.TypeID
			});
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x00044400 File Offset: 0x00042600
		private void OnMasterKeyUnlocked(int id)
		{
			this.ReportCustomEvent<BDSManager.MasterKeyUnlockContext>(BDSManager.EventName.masterkey_unlocked, new BDSManager.MasterKeyUnlockContext
			{
				keyID = id
			});
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00044428 File Offset: 0x00042628
		private void OnPerkUnlockConfirmed(Perk perk)
		{
			if (perk == null)
			{
				return;
			}
			BDSManager.EventName eventName = BDSManager.EventName.perk_unlocked;
			BDSManager.PerkInfo customParameters = default(BDSManager.PerkInfo);
			PerkTree master = perk.Master;
			customParameters.perkTreeID = ((master != null) ? master.ID : null);
			customParameters.perkName = perk.name;
			this.ReportCustomEvent<BDSManager.PerkInfo>(eventName, customParameters);
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00044478 File Offset: 0x00042678
		private void OnBuildingBuilt(int guid, BuildingInfo info)
		{
			this.ReportCustomEvent<BDSManager.BuildingEventContext>(BDSManager.EventName.building_built, new BDSManager.BuildingEventContext
			{
				buildingID = info.id
			});
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x000444A4 File Offset: 0x000426A4
		private void OnBuildingDestroyed(int guid, BuildingInfo info)
		{
			this.ReportCustomEvent<BDSManager.BuildingEventContext>(BDSManager.EventName.building_destroyed, new BDSManager.BuildingEventContext
			{
				buildingID = info.id
			});
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x000444CF File Offset: 0x000426CF
		private void OnLevelChanged(int from, int to)
		{
			this.ReportCustomEvent<BDSManager.LevelChangedEventContext>(BDSManager.EventName.role_level_changed, new BDSManager.LevelChangedEventContext(from, to));
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x000444E0 File Offset: 0x000426E0
		private void OnHealthDead(Health health, DamageInfo info)
		{
			if (health == null)
			{
				return;
			}
			Teams team = health.team;
			bool flag = false;
			if (info.fromCharacter != null && info.fromCharacter.IsMainCharacter())
			{
				flag = true;
			}
			if (flag)
			{
				this.ReportCustomEvent<BDSManager.EnemyKillInfo>(BDSManager.EventName.enemy_kill, new BDSManager.EnemyKillInfo
				{
					enemyPresetName = BDSManager.<OnHealthDead>g__GetPresetName|36_0(health),
					damageInfo = info
				});
			}
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00044546 File Offset: 0x00042746
		private void OnFormulaUnlocked(string formulaID)
		{
			this.ReportCustomEvent(BDSManager.EventName.craft_formula_unlock, StrJson.Create(new string[]
			{
				"id",
				formulaID
			}));
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00044567 File Offset: 0x00042767
		private void OnItemCrafted(CraftingFormula formula, Item item)
		{
			this.ReportCustomEvent<CraftingFormula>(BDSManager.EventName.craft_craft, formula);
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00044574 File Offset: 0x00042774
		private void OnItemPurchased(StockShop shop, Item item)
		{
			if (shop == null || item == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.PurchaseInfo>(BDSManager.EventName.shop_purchased, new BDSManager.PurchaseInfo
			{
				shopID = shop.MerchantID,
				itemTypeID = item.TypeID,
				itemAmount = item.StackCount
			});
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x000445CC File Offset: 0x000427CC
		private void OnItemSentToPlayerStorage(Item item)
		{
			if (item == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.ItemInfo>(BDSManager.EventName.item_to_storage, new BDSManager.ItemInfo
			{
				itemId = item.TypeID,
				amount = item.StackCount
			});
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00044610 File Offset: 0x00042810
		private void OnItemSentToPlayerInventory(Item item)
		{
			if (item == null)
			{
				return;
			}
			this.ReportCustomEvent<BDSManager.ItemInfo>(BDSManager.EventName.item_to_inventory, new BDSManager.ItemInfo
			{
				itemId = item.TypeID,
				amount = item.StackCount
			});
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00044654 File Offset: 0x00042854
		private void OnMoneyPaid(long money)
		{
			this.ReportCustomEvent<Cost>(BDSManager.EventName.pay_money, new Cost
			{
				money = money,
				items = new Cost.ItemEntry[0]
			});
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00044687 File Offset: 0x00042887
		private void OnCostPaid(Cost cost)
		{
			this.ReportCustomEvent<Cost>(BDSManager.EventName.pay_cost, cost);
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00044692 File Offset: 0x00042892
		private void OnQuestActivated(Quest quest)
		{
			if (quest == null)
			{
				return;
			}
			this.ReportCustomEvent<Quest.QuestInfo>(BDSManager.EventName.quest_activate, quest.GetInfo());
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x000446AC File Offset: 0x000428AC
		private void OnQuestCompleted(Quest quest)
		{
			if (quest == null)
			{
				return;
			}
			this.ReportCustomEvent<Quest.QuestInfo>(BDSManager.EventName.quest_complete, quest.GetInfo());
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x000446C8 File Offset: 0x000428C8
		private void OnMainCharacterDead(DamageInfo info)
		{
			string fromCharacterPresetName = "None";
			string fromCharacterNameKey = "None";
			if (info.fromCharacter)
			{
				CharacterRandomPreset characterPreset = info.fromCharacter.characterPreset;
				if (characterPreset != null)
				{
					fromCharacterPresetName = characterPreset.name;
					fromCharacterNameKey = characterPreset.nameKey;
				}
			}
			this.ReportCustomEvent<BDSManager.CharacterDeathContext>(BDSManager.EventName.main_character_dead, new BDSManager.CharacterDeathContext
			{
				damageInfo = info,
				levelInfo = LevelManager.GetCurrentLevelInfo(),
				fromCharacterPresetName = fromCharacterPresetName,
				fromCharacterNameKey = fromCharacterNameKey
			});
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x00044748 File Offset: 0x00042948
		private void OnEvacuated(EvacuationInfo evacuationInfo)
		{
			LevelManager.LevelInfo currentLevelInfo = LevelManager.GetCurrentLevelInfo();
			RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
			BDSManager.PlayerStatus playerStatus = BDSManager.PlayerStatus.CreateFromCurrent();
			this.ReportCustomEvent<BDSManager.EvacuationEventData>(BDSManager.EventName.level_evacuated, new BDSManager.EvacuationEventData
			{
				evacuationInfo = evacuationInfo,
				mapID = currentLevelInfo.activeSubSceneID,
				raidInfo = currentRaid,
				playerStatus = playerStatus
			});
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x0004479D File Offset: 0x0004299D
		private void OnLevelInitialized()
		{
			this.ReportCustomEvent<LevelManager.LevelInfo>(BDSManager.EventName.level_initialized, LevelManager.GetCurrentLevelInfo());
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x000447AB File Offset: 0x000429AB
		private void OnSceneLoadingFinish(SceneLoadingContext context)
		{
			this.ReportCustomEvent<SceneLoadingContext>(BDSManager.EventName.scene_load_start, context);
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x000447B5 File Offset: 0x000429B5
		private void OnSceneLoadingStart(SceneLoadingContext context)
		{
			this.ReportCustomEvent<SceneLoadingContext>(BDSManager.EventName.scene_load_finish, context);
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x000447BF File Offset: 0x000429BF
		private void OnRaidEnd(RaidUtilities.RaidInfo info)
		{
			this.ReportCustomEvent<RaidUtilities.RaidInfo>(BDSManager.EventName.raid_end, info);
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x000447C9 File Offset: 0x000429C9
		private void OnNewRaid(RaidUtilities.RaidInfo info)
		{
			this.ReportCustomEvent<RaidUtilities.RaidInfo>(BDSManager.EventName.raid_new, info);
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x000447D3 File Offset: 0x000429D3
		private void OnSaveDeleted()
		{
			this.ReportCustomEvent(BDSManager.EventName.delete_save_data, StrJson.Create(new string[]
			{
				"slot",
				string.Format("{0}", SavesSystem.CurrentSlot)
			}));
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x00044808 File Offset: 0x00042A08
		private void OnGameStarted()
		{
			int @int = PlayerPrefs.GetInt("AppStartCount", 0);
			this.sessionInfo = new BDSManager.SessionInfo
			{
				startCount = @int,
				isFirstTimeStart = (@int <= 0),
				session_id = DateTime.Now.ToBinary().GetHashCode()
			};
			this.sessionStartTime = DateTime.Now;
			this.ReportCustomEvent<BDSManager.SessionInfo>(BDSManager.EventName.app_start, this.sessionInfo);
			PlayerPrefs.SetInt("AppStartCount", @int + 1);
			PlayerPrefs.Save();
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x0004488E File Offset: 0x00042A8E
		private void ReportCustomEvent(BDSManager.EventName eventName, StrJson customParameters)
		{
			this.ReportCustomEvent(eventName, customParameters.ToString());
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x000448A0 File Offset: 0x00042AA0
		private void ReportCustomEvent<T>(BDSManager.EventName eventName, T customParameters)
		{
			string customParameters2 = (customParameters != null) ? JsonUtility.ToJson(customParameters) : "";
			this.ReportCustomEvent(eventName, customParameters2);
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x000448D0 File Offset: 0x00042AD0
		private void ReportCustomEvent(BDSManager.EventName eventName, string customParameters = "")
		{
			string text = BDSManager.PlayerInfo.GetCurrent().ToJson();
			SDK.ReportCustomEvent(eventName.ToString(), text, "", customParameters);
			try
			{
				Action<string, string> onReportCustomEvent = BDSManager.OnReportCustomEvent;
				if (onReportCustomEvent != null)
				{
					onReportCustomEvent(eventName.ToString(), customParameters);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x00044944 File Offset: 0x00042B44
		[CompilerGenerated]
		internal static string <OnHealthDead>g__GetPresetName|36_0(Health health)
		{
			CharacterMainControl characterMainControl = health.TryGetCharacter();
			if (characterMainControl == null)
			{
				return "None";
			}
			CharacterRandomPreset characterPreset = characterMainControl.characterPreset;
			if (characterPreset == null)
			{
				return "None";
			}
			return characterPreset.Name;
		}

		// Token: 0x04000DC1 RID: 3521
		private float lastTimeHeartbeat;

		// Token: 0x04000DC2 RID: 3522
		private int sessionID;

		// Token: 0x04000DC3 RID: 3523
		private DateTime sessionStartTime;

		// Token: 0x04000DC4 RID: 3524
		private BDSManager.SessionInfo sessionInfo;

		// Token: 0x04000DC5 RID: 3525
		public static Action<string, string> OnReportCustomEvent;

		// Token: 0x02000530 RID: 1328
		private enum EventName
		{
			// Token: 0x04001ECC RID: 7884
			none,
			// Token: 0x04001ECD RID: 7885
			app_start,
			// Token: 0x04001ECE RID: 7886
			begin_new_game,
			// Token: 0x04001ECF RID: 7887
			delete_save_data,
			// Token: 0x04001ED0 RID: 7888
			raid_new,
			// Token: 0x04001ED1 RID: 7889
			raid_end,
			// Token: 0x04001ED2 RID: 7890
			scene_load_start,
			// Token: 0x04001ED3 RID: 7891
			scene_load_finish,
			// Token: 0x04001ED4 RID: 7892
			level_initialized,
			// Token: 0x04001ED5 RID: 7893
			level_evacuated,
			// Token: 0x04001ED6 RID: 7894
			main_character_dead,
			// Token: 0x04001ED7 RID: 7895
			quest_activate,
			// Token: 0x04001ED8 RID: 7896
			quest_complete,
			// Token: 0x04001ED9 RID: 7897
			pay_money,
			// Token: 0x04001EDA RID: 7898
			pay_cost,
			// Token: 0x04001EDB RID: 7899
			item_to_inventory,
			// Token: 0x04001EDC RID: 7900
			item_to_storage,
			// Token: 0x04001EDD RID: 7901
			shop_purchased,
			// Token: 0x04001EDE RID: 7902
			craft_craft,
			// Token: 0x04001EDF RID: 7903
			craft_formula_unlock,
			// Token: 0x04001EE0 RID: 7904
			enemy_kill,
			// Token: 0x04001EE1 RID: 7905
			role_level_changed,
			// Token: 0x04001EE2 RID: 7906
			building_built,
			// Token: 0x04001EE3 RID: 7907
			building_destroyed,
			// Token: 0x04001EE4 RID: 7908
			perk_unlocked,
			// Token: 0x04001EE5 RID: 7909
			masterkey_unlocked,
			// Token: 0x04001EE6 RID: 7910
			role_equip,
			// Token: 0x04001EE7 RID: 7911
			item_sold,
			// Token: 0x04001EE8 RID: 7912
			reward_claimed,
			// Token: 0x04001EE9 RID: 7913
			item_use,
			// Token: 0x04001EEA RID: 7914
			interact_start,
			// Token: 0x04001EEB RID: 7915
			face_customize_begin,
			// Token: 0x04001EEC RID: 7916
			face_customize_finish,
			// Token: 0x04001EED RID: 7917
			heartbeat,
			// Token: 0x04001EEE RID: 7918
			cheat_mode_changed,
			// Token: 0x04001EEF RID: 7919
			app_end
		}

		// Token: 0x02000531 RID: 1329
		private struct CheatModeStatusChangeContext
		{
			// Token: 0x04001EF0 RID: 7920
			public bool cheatModeActive;
		}

		// Token: 0x02000532 RID: 1330
		private struct InteractEventContext
		{
			// Token: 0x04001EF1 RID: 7921
			public string interactGameObjectName;

			// Token: 0x04001EF2 RID: 7922
			public string typeName;
		}

		// Token: 0x02000533 RID: 1331
		private struct ItemUseEventContext
		{
			// Token: 0x04001EF3 RID: 7923
			public int itemTypeID;
		}

		// Token: 0x02000534 RID: 1332
		private struct RewardClaimEventContext
		{
			// Token: 0x04001EF4 RID: 7924
			public int questID;

			// Token: 0x04001EF5 RID: 7925
			public int rewardID;
		}

		// Token: 0x02000535 RID: 1333
		private struct ItemSoldEventContext
		{
			// Token: 0x04001EF6 RID: 7926
			public string stockShopID;

			// Token: 0x04001EF7 RID: 7927
			public int itemID;

			// Token: 0x04001EF8 RID: 7928
			public int price;
		}

		// Token: 0x02000536 RID: 1334
		private struct EquipEventContext
		{
			// Token: 0x04001EF9 RID: 7929
			public string slotKey;

			// Token: 0x04001EFA RID: 7930
			public int contentItemTypeID;
		}

		// Token: 0x02000537 RID: 1335
		private struct MasterKeyUnlockContext
		{
			// Token: 0x04001EFB RID: 7931
			public int keyID;
		}

		// Token: 0x02000538 RID: 1336
		private struct PerkInfo
		{
			// Token: 0x04001EFC RID: 7932
			public string perkTreeID;

			// Token: 0x04001EFD RID: 7933
			public string perkName;
		}

		// Token: 0x02000539 RID: 1337
		private struct BuildingEventContext
		{
			// Token: 0x04001EFE RID: 7934
			public string buildingID;
		}

		// Token: 0x0200053A RID: 1338
		private struct LevelChangedEventContext
		{
			// Token: 0x060028B6 RID: 10422 RVA: 0x0009592A File Offset: 0x00093B2A
			public LevelChangedEventContext(int from, int to)
			{
				this.from = from;
				this.to = to;
			}

			// Token: 0x04001EFF RID: 7935
			public int from;

			// Token: 0x04001F00 RID: 7936
			public int to;
		}

		// Token: 0x0200053B RID: 1339
		private struct EnemyKillInfo
		{
			// Token: 0x04001F01 RID: 7937
			public string enemyPresetName;

			// Token: 0x04001F02 RID: 7938
			public DamageInfo damageInfo;
		}

		// Token: 0x0200053C RID: 1340
		[Serializable]
		public struct PurchaseInfo
		{
			// Token: 0x04001F03 RID: 7939
			public string shopID;

			// Token: 0x04001F04 RID: 7940
			public int itemTypeID;

			// Token: 0x04001F05 RID: 7941
			public int itemAmount;
		}

		// Token: 0x0200053D RID: 1341
		private struct ItemInfo
		{
			// Token: 0x04001F06 RID: 7942
			public int itemId;

			// Token: 0x04001F07 RID: 7943
			public int amount;
		}

		// Token: 0x0200053E RID: 1342
		public struct CharacterDeathContext
		{
			// Token: 0x04001F08 RID: 7944
			public DamageInfo damageInfo;

			// Token: 0x04001F09 RID: 7945
			public string fromCharacterPresetName;

			// Token: 0x04001F0A RID: 7946
			public string fromCharacterNameKey;

			// Token: 0x04001F0B RID: 7947
			public LevelManager.LevelInfo levelInfo;
		}

		// Token: 0x0200053F RID: 1343
		[Serializable]
		private struct PlayerStatus
		{
			// Token: 0x060028B7 RID: 10423 RVA: 0x0009593C File Offset: 0x00093B3C
			public static BDSManager.PlayerStatus CreateFromCurrent()
			{
				CharacterMainControl main = CharacterMainControl.Main;
				if (main == null)
				{
					return default(BDSManager.PlayerStatus);
				}
				Health health = main.Health;
				if (health == null)
				{
					return default(BDSManager.PlayerStatus);
				}
				CharacterBuffManager buffManager = main.GetBuffManager();
				if (buffManager == null)
				{
					return default(BDSManager.PlayerStatus);
				}
				if (main.CharacterItem == null)
				{
					return default(BDSManager.PlayerStatus);
				}
				string[] array = new string[buffManager.Buffs.Count];
				for (int i = 0; i < buffManager.Buffs.Count; i++)
				{
					Buff buff = buffManager.Buffs[i];
					if (!(buff == null))
					{
						array[i] = string.Format("{0} {1}", buff.ID, buff.DisplayNameKey);
					}
				}
				int totalRawValue = main.CharacterItem.GetTotalRawValue();
				return new BDSManager.PlayerStatus
				{
					valid = true,
					healthMax = health.MaxHealth,
					health = main.CurrentEnergy,
					water = main.CurrentWater,
					food = main.CurrentEnergy,
					waterMax = main.MaxWater,
					foodMax = main.MaxEnergy,
					totalItemValue = totalRawValue
				};
			}

			// Token: 0x04001F0C RID: 7948
			public bool valid;

			// Token: 0x04001F0D RID: 7949
			public float healthMax;

			// Token: 0x04001F0E RID: 7950
			public float health;

			// Token: 0x04001F0F RID: 7951
			public float waterMax;

			// Token: 0x04001F10 RID: 7952
			public float foodMax;

			// Token: 0x04001F11 RID: 7953
			public float water;

			// Token: 0x04001F12 RID: 7954
			public float food;

			// Token: 0x04001F13 RID: 7955
			public string[] activeEffects;

			// Token: 0x04001F14 RID: 7956
			public int totalItemValue;
		}

		// Token: 0x02000540 RID: 1344
		private struct EvacuationEventData
		{
			// Token: 0x04001F15 RID: 7957
			public EvacuationInfo evacuationInfo;

			// Token: 0x04001F16 RID: 7958
			public string mapID;

			// Token: 0x04001F17 RID: 7959
			public RaidUtilities.RaidInfo raidInfo;

			// Token: 0x04001F18 RID: 7960
			public BDSManager.PlayerStatus playerStatus;
		}

		// Token: 0x02000541 RID: 1345
		[Serializable]
		private struct SessionInfo
		{
			// Token: 0x04001F19 RID: 7961
			public int startCount;

			// Token: 0x04001F1A RID: 7962
			public bool isFirstTimeStart;

			// Token: 0x04001F1B RID: 7963
			public int session_id;

			// Token: 0x04001F1C RID: 7964
			public int session_duration_seconds;
		}

		// Token: 0x02000542 RID: 1346
		public struct PlayerInfo
		{
			// Token: 0x060028B8 RID: 10424 RVA: 0x00095A90 File Offset: 0x00093C90
			public PlayerInfo(int level, string steamAccountID, int saveSlot, string location, string language, string displayName, string difficulty, string platform, string version, string system)
			{
				this.role_name = displayName;
				this.profession_type = language;
				this.gender = version;
				this.level = string.Format("{0}", level);
				this.b_account_id = steamAccountID;
				this.b_role_id = string.Format("{0}|{1}", saveSlot, difficulty);
				this.b_tour_indicator = "0";
				this.b_zone_id = location;
				this.b_sdk_uid = platform + "|" + system;
			}

			// Token: 0x060028B9 RID: 10425 RVA: 0x00095B14 File Offset: 0x00093D14
			public static BDSManager.PlayerInfo GetCurrent()
			{
				string id = PlatformInfo.GetID();
				string displayName = PlatformInfo.GetDisplayName();
				return new BDSManager.PlayerInfo(EXPManager.Level, id, SavesSystem.CurrentSlot, RegionInfo.CurrentRegion.Name, Application.systemLanguage.ToString(), displayName, GameRulesManager.Current.displayNameKey, PlatformInfo.Platform.ToString(), GameMetaData.Instance.Version.ToString(), Environment.OSVersion.Platform.ToString())
				{
					gender = GameMetaData.Instance.Version.ToString()
				};
			}

			// Token: 0x060028BA RID: 10426 RVA: 0x00095BD0 File Offset: 0x00093DD0
			public static string GetCurrentJson()
			{
				return BDSManager.PlayerInfo.GetCurrent().ToJson();
			}

			// Token: 0x060028BB RID: 10427 RVA: 0x00095BEA File Offset: 0x00093DEA
			public string ToJson()
			{
				return JsonUtility.ToJson(this);
			}

			// Token: 0x04001F1D RID: 7965
			public string role_name;

			// Token: 0x04001F1E RID: 7966
			public string profession_type;

			// Token: 0x04001F1F RID: 7967
			public string gender;

			// Token: 0x04001F20 RID: 7968
			public string level;

			// Token: 0x04001F21 RID: 7969
			public string b_account_id;

			// Token: 0x04001F22 RID: 7970
			public string b_role_id;

			// Token: 0x04001F23 RID: 7971
			public string b_tour_indicator;

			// Token: 0x04001F24 RID: 7972
			public string b_zone_id;

			// Token: 0x04001F25 RID: 7973
			public string b_sdk_uid;
		}
	}
}
