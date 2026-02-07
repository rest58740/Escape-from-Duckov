using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Economy
{
	// Token: 0x0200033B RID: 827
	public class EconomyManager : MonoBehaviour, ISaveDataProvider
	{
		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001BBB RID: 7099 RVA: 0x00064C48 File Offset: 0x00062E48
		public static string ItemUnlockNotificationTextMainFormat
		{
			get
			{
				EconomyManager instance = EconomyManager.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.itemUnlockNotificationTextMainFormat;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001BBC RID: 7100 RVA: 0x00064C5A File Offset: 0x00062E5A
		public static string ItemUnlockNotificationTextSubFormat
		{
			get
			{
				EconomyManager instance = EconomyManager.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.itemUnlockNotificationTextSubFormat;
			}
		}

		// Token: 0x140000BF RID: 191
		// (add) Token: 0x06001BBD RID: 7101 RVA: 0x00064C6C File Offset: 0x00062E6C
		// (remove) Token: 0x06001BBE RID: 7102 RVA: 0x00064CA0 File Offset: 0x00062EA0
		public static event Action OnEconomyManagerLoaded;

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001BBF RID: 7103 RVA: 0x00064CD3 File Offset: 0x00062ED3
		// (set) Token: 0x06001BC0 RID: 7104 RVA: 0x00064CDA File Offset: 0x00062EDA
		public static EconomyManager Instance { get; private set; }

		// Token: 0x06001BC1 RID: 7105 RVA: 0x00064CE2 File Offset: 0x00062EE2
		private void Awake()
		{
			if (EconomyManager.Instance == null)
			{
				EconomyManager.Instance = this;
			}
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			SavesSystem.OnSetFile += this.OnSetSaveFile;
			this.Load();
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x00064D1F File Offset: 0x00062F1F
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x00064D27 File Offset: 0x00062F27
		private void OnSetSaveFile()
		{
			this.Load();
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x00064D30 File Offset: 0x00062F30
		private void Load()
		{
			if (SavesSystem.KeyExisits("EconomyData"))
			{
				this.SetupSaveData(SavesSystem.Load<EconomyManager.SaveData>("EconomyData"));
			}
			try
			{
				Action onEconomyManagerLoaded = EconomyManager.OnEconomyManagerLoaded;
				if (onEconomyManagerLoaded != null)
				{
					onEconomyManagerLoaded();
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x00064D88 File Offset: 0x00062F88
		private void Save()
		{
			SavesSystem.Save<EconomyManager.SaveData>("EconomyData", (EconomyManager.SaveData)this.GenerateSaveData());
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x00064D9F File Offset: 0x00062F9F
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
			SavesSystem.OnSetFile -= this.OnSetSaveFile;
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x00064DC3 File Offset: 0x00062FC3
		// (set) Token: 0x06001BC8 RID: 7112 RVA: 0x00064DE0 File Offset: 0x00062FE0
		public static long Money
		{
			get
			{
				if (EconomyManager.Instance == null)
				{
					return 0L;
				}
				return EconomyManager.Instance.money;
			}
			private set
			{
				long arg = EconomyManager.Money;
				if (EconomyManager.Instance == null)
				{
					return;
				}
				EconomyManager.Instance.money = value;
				Action<long, long> onMoneyChanged = EconomyManager.OnMoneyChanged;
				if (onMoneyChanged == null)
				{
					return;
				}
				onMoneyChanged(arg, value);
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x00064E1D File Offset: 0x0006301D
		public static long Cash
		{
			get
			{
				return (long)ItemUtilities.GetItemCount(451);
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001BCA RID: 7114 RVA: 0x00064E2A File Offset: 0x0006302A
		public ReadOnlyCollection<int> UnlockedItemIds
		{
			get
			{
				return this.unlockedItemIds.AsReadOnly();
			}
		}

		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x06001BCB RID: 7115 RVA: 0x00064E38 File Offset: 0x00063038
		// (remove) Token: 0x06001BCC RID: 7116 RVA: 0x00064E6C File Offset: 0x0006306C
		public static event Action<long, long> OnMoneyChanged;

		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x06001BCD RID: 7117 RVA: 0x00064EA0 File Offset: 0x000630A0
		// (remove) Token: 0x06001BCE RID: 7118 RVA: 0x00064ED4 File Offset: 0x000630D4
		public static event Action<int> OnItemUnlockStateChanged;

		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x06001BCF RID: 7119 RVA: 0x00064F08 File Offset: 0x00063108
		// (remove) Token: 0x06001BD0 RID: 7120 RVA: 0x00064F3C File Offset: 0x0006313C
		public static event Action<long> OnMoneyPaid;

		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x06001BD1 RID: 7121 RVA: 0x00064F70 File Offset: 0x00063170
		// (remove) Token: 0x06001BD2 RID: 7122 RVA: 0x00064FA4 File Offset: 0x000631A4
		public static event Action<Cost> OnCostPaid;

		// Token: 0x06001BD3 RID: 7123 RVA: 0x00064FD8 File Offset: 0x000631D8
		private static bool Pay(long amount, bool accountAvaliable = true, bool cashAvaliale = true)
		{
			long num = accountAvaliable ? EconomyManager.Money : 0L;
			long num2 = cashAvaliale ? EconomyManager.Cash : 0L;
			if (num + num2 < amount)
			{
				return false;
			}
			long num3 = amount;
			if (accountAvaliable)
			{
				if (num > amount)
				{
					num3 = 0L;
					EconomyManager.Money -= amount;
				}
				else
				{
					num3 -= num;
					EconomyManager.Money = 0L;
				}
			}
			if (cashAvaliale && num3 > 0L)
			{
				ItemUtilities.ConsumeItems(451, num3);
			}
			if (amount > 0L)
			{
				Action<long> onMoneyPaid = EconomyManager.OnMoneyPaid;
				if (onMoneyPaid != null)
				{
					onMoneyPaid(amount);
				}
			}
			return true;
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x0006505A File Offset: 0x0006325A
		public static bool Pay(Cost cost, bool accountAvaliable = true, bool cashAvaliale = true)
		{
			if (!EconomyManager.IsEnough(cost, accountAvaliable, true))
			{
				return false;
			}
			if (!EconomyManager.Pay(cost.money, accountAvaliable, cashAvaliale))
			{
				return false;
			}
			if (!ItemUtilities.ConsumeItems(cost))
			{
				return false;
			}
			Action<Cost> onCostPaid = EconomyManager.OnCostPaid;
			if (onCostPaid != null)
			{
				onCostPaid(cost);
			}
			return true;
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x00065098 File Offset: 0x00063298
		public static bool IsEnough(Cost cost, bool accountAvaliable = true, bool cashAvaliale = true)
		{
			long num = accountAvaliable ? EconomyManager.Money : 0L;
			long num2 = cashAvaliale ? EconomyManager.Cash : 0L;
			if (num + num2 < cost.money)
			{
				return false;
			}
			if (cost.items != null)
			{
				foreach (Cost.ItemEntry itemEntry in cost.items)
				{
					if ((long)ItemUtilities.GetItemCount(itemEntry.id) < itemEntry.amount)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x00065106 File Offset: 0x00063306
		public static bool Add(long amount)
		{
			if (EconomyManager.Instance == null)
			{
				return false;
			}
			EconomyManager.Money += amount;
			return true;
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x00065124 File Offset: 0x00063324
		public static bool IsWaitingForUnlockConfirm(int itemTypeID)
		{
			return !GameplayDataSettings.Economy.UnlockedItemByDefault.Contains(itemTypeID) && !(EconomyManager.Instance == null) && EconomyManager.Instance.unlockesWaitingForConfirm.Contains(itemTypeID);
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x00065159 File Offset: 0x00063359
		public static bool IsUnlocked(int itemTypeID)
		{
			return GameplayDataSettings.Economy.UnlockedItemByDefault.Contains(itemTypeID) || (!(EconomyManager.Instance == null) && EconomyManager.Instance.UnlockedItemIds.Contains(itemTypeID));
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x00065190 File Offset: 0x00063390
		public static void Unlock(int itemTypeID, bool needConfirm = true, bool showUI = true)
		{
			if (EconomyManager.Instance == null)
			{
				return;
			}
			if (EconomyManager.Instance.unlockedItemIds.Contains(itemTypeID))
			{
				return;
			}
			if (EconomyManager.Instance.unlockesWaitingForConfirm.Contains(itemTypeID))
			{
				return;
			}
			if (needConfirm)
			{
				EconomyManager.Instance.unlockesWaitingForConfirm.Add(itemTypeID);
			}
			else
			{
				EconomyManager.Instance.unlockedItemIds.Add(itemTypeID);
			}
			Action<int> onItemUnlockStateChanged = EconomyManager.OnItemUnlockStateChanged;
			if (onItemUnlockStateChanged != null)
			{
				onItemUnlockStateChanged(itemTypeID);
			}
			ItemMetaData metaData = ItemAssetsCollection.GetMetaData(itemTypeID);
			Debug.Log(EconomyManager.ItemUnlockNotificationTextMainFormat);
			Debug.Log(metaData.DisplayName);
			if (showUI)
			{
				NotificationText.Push(StringExtensions.Format(LocalizationManager.ToPlainText("Notification_StockShoopItemUnlockFormat"), new
				{
					displayName = metaData.DisplayName
				}));
			}
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x00065248 File Offset: 0x00063448
		public static void ConfirmUnlock(int itemTypeID)
		{
			if (EconomyManager.Instance == null)
			{
				return;
			}
			EconomyManager.Instance.unlockesWaitingForConfirm.Remove(itemTypeID);
			EconomyManager.Instance.unlockedItemIds.Add(itemTypeID);
			Action<int> onItemUnlockStateChanged = EconomyManager.OnItemUnlockStateChanged;
			if (onItemUnlockStateChanged == null)
			{
				return;
			}
			onItemUnlockStateChanged(itemTypeID);
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x00065294 File Offset: 0x00063494
		public object GenerateSaveData()
		{
			return new EconomyManager.SaveData
			{
				money = EconomyManager.Money,
				unlockedItems = this.unlockedItemIds.ToArray(),
				unlockesWaitingForConfirm = this.unlockesWaitingForConfirm.ToArray()
			};
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x000652E0 File Offset: 0x000634E0
		public void SetupSaveData(object rawData)
		{
			if (rawData is EconomyManager.SaveData)
			{
				EconomyManager.SaveData saveData = (EconomyManager.SaveData)rawData;
				this.money = saveData.money;
				this.unlockedItemIds.Clear();
				if (saveData.unlockedItems != null)
				{
					this.unlockedItemIds.AddRange(saveData.unlockedItems);
				}
				this.unlockesWaitingForConfirm.Clear();
				if (saveData.unlockesWaitingForConfirm != null)
				{
					this.unlockesWaitingForConfirm.AddRange(saveData.unlockesWaitingForConfirm);
				}
				return;
			}
		}

		// Token: 0x040013C6 RID: 5062
		[SerializeField]
		private string itemUnlockNotificationTextMainFormat = "物品 {itemDisplayName} 已解锁";

		// Token: 0x040013C7 RID: 5063
		[SerializeField]
		private string itemUnlockNotificationTextSubFormat = "请在对应商店中查看";

		// Token: 0x040013CA RID: 5066
		private const string saveKey = "EconomyData";

		// Token: 0x040013CB RID: 5067
		private long money;

		// Token: 0x040013CC RID: 5068
		[SerializeField]
		private List<int> unlockedItemIds;

		// Token: 0x040013CD RID: 5069
		[SerializeField]
		private List<int> unlockesWaitingForConfirm;

		// Token: 0x040013D2 RID: 5074
		public const int CashItemID = 451;

		// Token: 0x020005E7 RID: 1511
		[Serializable]
		public struct SaveData
		{
			// Token: 0x04002176 RID: 8566
			public long money;

			// Token: 0x04002177 RID: 8567
			public int[] unlockedItems;

			// Token: 0x04002178 RID: 8568
			public int[] unlockesWaitingForConfirm;
		}
	}
}
