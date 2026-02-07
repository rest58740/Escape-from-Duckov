using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Economy
{
	// Token: 0x0200033D RID: 829
	public class StockShop : MonoBehaviour, IMerchant, ISaveDataProvider
	{
		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001BE8 RID: 7144 RVA: 0x00065634 File Offset: 0x00063834
		public string MerchantID
		{
			get
			{
				return this.merchantID;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001BE9 RID: 7145 RVA: 0x0006563C File Offset: 0x0006383C
		public string OpinionKey
		{
			get
			{
				return "Opinion_" + this.merchantID;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001BEA RID: 7146 RVA: 0x0006564E File Offset: 0x0006384E
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.DisplayNameKey);
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001BEB RID: 7147 RVA: 0x0006565B File Offset: 0x0006385B
		// (set) Token: 0x06001BEC RID: 7148 RVA: 0x00065672 File Offset: 0x00063872
		private int Opinion
		{
			get
			{
				return Mathf.Clamp(CommonVariables.GetInt(this.OpinionKey, 0), -100, 100);
			}
			set
			{
				CommonVariables.SetInt(this.OpinionKey, value);
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001BED RID: 7149 RVA: 0x00065680 File Offset: 0x00063880
		public string PurchaseNotificationTextFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.purchaseNotificationTextFormatKey);
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001BEE RID: 7150 RVA: 0x0006568D File Offset: 0x0006388D
		public bool AccountAvaliable
		{
			get
			{
				return this.accountAvaliable;
			}
		}

		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x06001BEF RID: 7151 RVA: 0x00065698 File Offset: 0x00063898
		// (remove) Token: 0x06001BF0 RID: 7152 RVA: 0x000656CC File Offset: 0x000638CC
		public static event Action<StockShop> OnAfterItemSold;

		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x06001BF1 RID: 7153 RVA: 0x00065700 File Offset: 0x00063900
		// (remove) Token: 0x06001BF2 RID: 7154 RVA: 0x00065734 File Offset: 0x00063934
		public static event Action<StockShop, Item> OnItemPurchased;

		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x06001BF3 RID: 7155 RVA: 0x00065768 File Offset: 0x00063968
		// (remove) Token: 0x06001BF4 RID: 7156 RVA: 0x0006579C File Offset: 0x0006399C
		public static event Action<StockShop, Item, int> OnItemSoldByPlayer;

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001BF5 RID: 7157 RVA: 0x000657D0 File Offset: 0x000639D0
		public TimeSpan TimeSinceLastRefresh
		{
			get
			{
				DateTime dateTime = DateTime.FromBinary(this.lastTimeRefreshedStock);
				if (dateTime > DateTime.UtcNow)
				{
					dateTime = DateTime.UtcNow;
					this.lastTimeRefreshedStock = DateTime.UtcNow.ToBinary();
					GameManager.TimeTravelDetected();
				}
				return DateTime.UtcNow - dateTime;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001BF6 RID: 7158 RVA: 0x00065820 File Offset: 0x00063A20
		public TimeSpan NextRefreshETA
		{
			get
			{
				TimeSpan timeSinceLastRefresh = this.TimeSinceLastRefresh;
				TimeSpan timeSpan = TimeSpan.FromTicks(this.refreshAfterTimeSpan) - timeSinceLastRefresh;
				if (timeSpan < TimeSpan.Zero)
				{
					timeSpan = TimeSpan.Zero;
				}
				return timeSpan;
			}
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x0006585C File Offset: 0x00063A5C
		private UniTask<Item> GetItemInstance(int typeID)
		{
			StockShop.<GetItemInstance>d__40 <GetItemInstance>d__;
			<GetItemInstance>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<GetItemInstance>d__.<>4__this = this;
			<GetItemInstance>d__.typeID = typeID;
			<GetItemInstance>d__.<>1__state = -1;
			<GetItemInstance>d__.<>t__builder.Start<StockShop.<GetItemInstance>d__40>(ref <GetItemInstance>d__);
			return <GetItemInstance>d__.<>t__builder.Task;
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000658A8 File Offset: 0x00063AA8
		public Item GetItemInstanceDirect(int typeID)
		{
			Item result;
			if (this.itemInstances.TryGetValue(typeID, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x000658C8 File Offset: 0x00063AC8
		private void Awake()
		{
			this.InitializeEntries();
			SavesSystem.OnCollectSaveData += this.Save;
			SavesSystem.OnSetFile += this.Load;
			this.Load();
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x000658F8 File Offset: 0x00063AF8
		private void InitializeEntries()
		{
			StockShopDatabase.MerchantProfile merchantProfile = StockShopDatabase.Instance.GetMerchantProfile(this.merchantID);
			if (merchantProfile == null)
			{
				Debug.Log("未配置商人 " + this.merchantID);
				return;
			}
			foreach (StockShopDatabase.ItemEntry cur in merchantProfile.entries)
			{
				this.entries.Add(new StockShop.Entry(cur));
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001BFB RID: 7163 RVA: 0x00065980 File Offset: 0x00063B80
		private string SaveKey
		{
			get
			{
				return "StockShop_" + this.merchantID;
			}
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x00065994 File Offset: 0x00063B94
		private void Load()
		{
			if (!SavesSystem.KeyExisits(this.SaveKey))
			{
				return;
			}
			StockShop.SaveData dataRaw = SavesSystem.Load<StockShop.SaveData>(this.SaveKey);
			this.SetupSaveData(dataRaw);
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x000659C4 File Offset: 0x00063BC4
		private void Save()
		{
			StockShop.SaveData saveData = this.GenerateSaveData() as StockShop.SaveData;
			if (saveData == null)
			{
				Debug.LogError("没法正确生成StockShop的SaveData");
				return;
			}
			SavesSystem.Save<StockShop.SaveData>(this.SaveKey, saveData);
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000659F7 File Offset: 0x00063BF7
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
			SavesSystem.OnSetFile -= this.Load;
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x00065A1C File Offset: 0x00063C1C
		private void Start()
		{
			this.CacheItemInstances().Forget();
			if (this.refreshStockOnStart)
			{
				this.DoRefreshStock();
				this.lastTimeRefreshedStock = DateTime.UtcNow.ToBinary();
			}
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x00065A58 File Offset: 0x00063C58
		private UniTask CacheItemInstances()
		{
			StockShop.<CacheItemInstances>d__50 <CacheItemInstances>d__;
			<CacheItemInstances>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CacheItemInstances>d__.<>4__this = this;
			<CacheItemInstances>d__.<>1__state = -1;
			<CacheItemInstances>d__.<>t__builder.Start<StockShop.<CacheItemInstances>d__50>(ref <CacheItemInstances>d__);
			return <CacheItemInstances>d__.<>t__builder.Task;
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x00065A9C File Offset: 0x00063C9C
		internal void RefreshIfNeeded()
		{
			TimeSpan t = TimeSpan.FromTicks(this.refreshAfterTimeSpan);
			DateTime dateTime = DateTime.FromBinary(this.lastTimeRefreshedStock);
			if (dateTime > DateTime.UtcNow)
			{
				dateTime = DateTime.UtcNow;
				this.lastTimeRefreshedStock = dateTime.ToBinary();
			}
			DateTime t2 = DateTime.UtcNow - TimeSpan.FromDays(2.0);
			if (dateTime < t2)
			{
				this.lastTimeRefreshedStock = t2.ToBinary();
			}
			if (DateTime.UtcNow - dateTime > t)
			{
				this.DoRefreshStock();
				this.lastTimeRefreshedStock = DateTime.UtcNow.ToBinary();
			}
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x00065B3C File Offset: 0x00063D3C
		private void DoRefreshStock()
		{
			bool advancedDebuffMode = LevelManager.Rule.AdvancedDebuffMode;
			foreach (StockShop.Entry entry in this.entries)
			{
				if (entry.Possibility > 0f && entry.Possibility < 1f && Random.Range(0f, 1f) > entry.Possibility)
				{
					entry.Show = false;
					entry.CurrentStock = 0;
				}
				else
				{
					ItemMetaData metaData = ItemAssetsCollection.GetMetaData(entry.ItemTypeID);
					if (!advancedDebuffMode && metaData.tags.Contains(GameplayDataSettings.Tags.AdvancedDebuffMode))
					{
						entry.Show = false;
						entry.CurrentStock = 0;
					}
					else
					{
						entry.Show = true;
						entry.CurrentStock = entry.MaxStock;
					}
				}
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001C03 RID: 7171 RVA: 0x00065C24 File Offset: 0x00063E24
		public bool Busy
		{
			get
			{
				return this.buying || this.selling;
			}
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x00065C3C File Offset: 0x00063E3C
		public UniTask<bool> Buy(int itemTypeID, int amount = 1)
		{
			StockShop.<Buy>d__57 <Buy>d__;
			<Buy>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Buy>d__.<>4__this = this;
			<Buy>d__.itemTypeID = itemTypeID;
			<Buy>d__.amount = amount;
			<Buy>d__.<>1__state = -1;
			<Buy>d__.<>t__builder.Start<StockShop.<Buy>d__57>(ref <Buy>d__);
			return <Buy>d__.<>t__builder.Task;
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x00065C90 File Offset: 0x00063E90
		private UniTask<bool> BuyTask(int itemTypeID, int amount = 1)
		{
			StockShop.<BuyTask>d__58 <BuyTask>d__;
			<BuyTask>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<BuyTask>d__.<>4__this = this;
			<BuyTask>d__.itemTypeID = itemTypeID;
			<BuyTask>d__.amount = amount;
			<BuyTask>d__.<>1__state = -1;
			<BuyTask>d__.<>t__builder.Start<StockShop.<BuyTask>d__58>(ref <BuyTask>d__);
			return <BuyTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x00065CE4 File Offset: 0x00063EE4
		internal UniTask Sell(Item target)
		{
			StockShop.<Sell>d__59 <Sell>d__;
			<Sell>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Sell>d__.<>4__this = this;
			<Sell>d__.target = target;
			<Sell>d__.<>1__state = -1;
			<Sell>d__.<>t__builder.Start<StockShop.<Sell>d__59>(ref <Sell>d__);
			return <Sell>d__.<>t__builder.Task;
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x00065D2F File Offset: 0x00063F2F
		public void ShowUI()
		{
			if (!StockShopView.Instance)
			{
				return;
			}
			this.RefreshIfNeeded();
			StockShopView.Instance.SetupAndShow(this);
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x00065D50 File Offset: 0x00063F50
		public int ConvertPrice(Item item, bool selling = false)
		{
			int num = item.GetTotalRawValue();
			if (!selling)
			{
				StockShop.Entry entry = this.entries.Find((StockShop.Entry e) => e != null && e.ItemTypeID == item.TypeID);
				if (entry != null)
				{
					num = Mathf.FloorToInt((float)num * entry.PriceFactor);
				}
			}
			if (selling)
			{
				float factor = this.sellFactor;
				StockShop.OverrideSellingPriceEntry overrideSellingPriceEntry = this.overrideSellingPrice.Find((StockShop.OverrideSellingPriceEntry e) => e.typeID == item.TypeID);
				if (overrideSellingPriceEntry != null)
				{
					factor = overrideSellingPriceEntry.factor;
				}
				return Mathf.FloorToInt((float)num * factor);
			}
			return num;
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x00065DE0 File Offset: 0x00063FE0
		public object GenerateSaveData()
		{
			StockShop.SaveData saveData = new StockShop.SaveData();
			saveData.lastTimeRefreshedStock = this.lastTimeRefreshedStock;
			foreach (StockShop.Entry entry in this.entries)
			{
				saveData.stockCounts.Add(new StockShop.SaveData.StockCountEntry
				{
					itemTypeID = entry.ItemTypeID,
					stock = entry.CurrentStock
				});
			}
			return saveData;
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x00065E68 File Offset: 0x00064068
		public void SetupSaveData(object dataRaw)
		{
			StockShop.SaveData saveData = dataRaw as StockShop.SaveData;
			if (saveData == null)
			{
				return;
			}
			this.lastTimeRefreshedStock = saveData.lastTimeRefreshedStock;
			bool advancedDebuffMode = LevelManager.Rule.AdvancedDebuffMode;
			using (List<StockShop.Entry>.Enumerator enumerator = this.entries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					StockShop.Entry cur = enumerator.Current;
					StockShop.SaveData.StockCountEntry stockCountEntry = saveData.stockCounts.Find((StockShop.SaveData.StockCountEntry e) => e != null && e.itemTypeID == cur.ItemTypeID);
					if (stockCountEntry != null)
					{
						bool flag = true;
						ItemMetaData metaData = ItemAssetsCollection.GetMetaData(cur.ItemTypeID);
						if (!advancedDebuffMode && metaData.tags.Contains(GameplayDataSettings.Tags.AdvancedDebuffMode))
						{
							flag = false;
						}
						cur.Show = (flag && (stockCountEntry.stock > 0 || cur.Possibility >= 1f));
						cur.CurrentStock = stockCountEntry.stock;
					}
				}
			}
		}

		// Token: 0x040013D6 RID: 5078
		[SerializeField]
		private string merchantID = "Albert";

		// Token: 0x040013D7 RID: 5079
		[LocalizationKey("Default")]
		public string DisplayNameKey;

		// Token: 0x040013D8 RID: 5080
		[TimeSpan]
		[SerializeField]
		private long refreshAfterTimeSpan;

		// Token: 0x040013D9 RID: 5081
		[SerializeField]
		private string purchaseNotificationTextFormatKey = "UI_StockShop_PurchasedNotification";

		// Token: 0x040013DA RID: 5082
		[SerializeField]
		private bool accountAvaliable;

		// Token: 0x040013DB RID: 5083
		[SerializeField]
		private bool returnCash;

		// Token: 0x040013DC RID: 5084
		[SerializeField]
		private bool refreshStockOnStart;

		// Token: 0x040013DD RID: 5085
		public float sellFactor = 0.5f;

		// Token: 0x040013DE RID: 5086
		public List<StockShop.Entry> entries = new List<StockShop.Entry>();

		// Token: 0x040013DF RID: 5087
		public List<StockShop.OverrideSellingPriceEntry> overrideSellingPrice = new List<StockShop.OverrideSellingPriceEntry>();

		// Token: 0x040013E3 RID: 5091
		[DateTime]
		[SerializeField]
		private long lastTimeRefreshedStock;

		// Token: 0x040013E4 RID: 5092
		private Dictionary<int, Item> itemInstances = new Dictionary<int, Item>();

		// Token: 0x040013E5 RID: 5093
		private bool buying;

		// Token: 0x040013E6 RID: 5094
		private bool selling;

		// Token: 0x020005EA RID: 1514
		public class Entry
		{
			// Token: 0x06002A3F RID: 10815 RVA: 0x0009D2F2 File Offset: 0x0009B4F2
			public Entry(StockShopDatabase.ItemEntry cur)
			{
				this.entry = cur;
			}

			// Token: 0x170007BA RID: 1978
			// (get) Token: 0x06002A40 RID: 10816 RVA: 0x0009D308 File Offset: 0x0009B508
			public int MaxStock
			{
				get
				{
					if (this.entry.maxStock < 1)
					{
						this.entry.maxStock = 1;
					}
					return this.entry.maxStock;
				}
			}

			// Token: 0x170007BB RID: 1979
			// (get) Token: 0x06002A41 RID: 10817 RVA: 0x0009D32F File Offset: 0x0009B52F
			public int ItemTypeID
			{
				get
				{
					return this.entry.typeID;
				}
			}

			// Token: 0x170007BC RID: 1980
			// (get) Token: 0x06002A42 RID: 10818 RVA: 0x0009D33C File Offset: 0x0009B53C
			public bool ForceUnlock
			{
				get
				{
					return (!GameMetaData.Instance.IsDemo || !this.entry.lockInDemo) && this.entry.forceUnlock;
				}
			}

			// Token: 0x170007BD RID: 1981
			// (get) Token: 0x06002A43 RID: 10819 RVA: 0x0009D364 File Offset: 0x0009B564
			public float PriceFactor
			{
				get
				{
					return this.entry.priceFactor;
				}
			}

			// Token: 0x170007BE RID: 1982
			// (get) Token: 0x06002A44 RID: 10820 RVA: 0x0009D371 File Offset: 0x0009B571
			public float Possibility
			{
				get
				{
					return this.entry.possibility;
				}
			}

			// Token: 0x170007BF RID: 1983
			// (get) Token: 0x06002A45 RID: 10821 RVA: 0x0009D37E File Offset: 0x0009B57E
			// (set) Token: 0x06002A46 RID: 10822 RVA: 0x0009D386 File Offset: 0x0009B586
			public bool Show
			{
				get
				{
					return this.show;
				}
				set
				{
					this.show = value;
				}
			}

			// Token: 0x170007C0 RID: 1984
			// (get) Token: 0x06002A47 RID: 10823 RVA: 0x0009D38F File Offset: 0x0009B58F
			// (set) Token: 0x06002A48 RID: 10824 RVA: 0x0009D397 File Offset: 0x0009B597
			public int CurrentStock
			{
				get
				{
					return this.currentStock;
				}
				set
				{
					this.currentStock = value;
					Action<StockShop.Entry> action = this.onStockChanged;
					if (action == null)
					{
						return;
					}
					action(this);
				}
			}

			// Token: 0x14000105 RID: 261
			// (add) Token: 0x06002A49 RID: 10825 RVA: 0x0009D3B4 File Offset: 0x0009B5B4
			// (remove) Token: 0x06002A4A RID: 10826 RVA: 0x0009D3EC File Offset: 0x0009B5EC
			public event Action<StockShop.Entry> onStockChanged;

			// Token: 0x04002189 RID: 8585
			private StockShopDatabase.ItemEntry entry;

			// Token: 0x0400218A RID: 8586
			[SerializeField]
			private bool show = true;

			// Token: 0x0400218B RID: 8587
			[SerializeField]
			private int currentStock;
		}

		// Token: 0x020005EB RID: 1515
		[Serializable]
		public class OverrideSellingPriceEntry
		{
			// Token: 0x0400218D RID: 8589
			[ItemTypeID]
			public int typeID;

			// Token: 0x0400218E RID: 8590
			public float factor = 0.5f;
		}

		// Token: 0x020005EC RID: 1516
		[Serializable]
		private class SaveData
		{
			// Token: 0x0400218F RID: 8591
			[DateTime]
			public long lastTimeRefreshedStock;

			// Token: 0x04002190 RID: 8592
			public List<StockShop.SaveData.StockCountEntry> stockCounts = new List<StockShop.SaveData.StockCountEntry>();

			// Token: 0x020006A0 RID: 1696
			public class StockCountEntry
			{
				// Token: 0x04002444 RID: 9284
				public int itemTypeID;

				// Token: 0x04002445 RID: 9285
				public int stock;
			}
		}
	}
}
