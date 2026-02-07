using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.Utilities;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Duckov.BlackMarkets
{
	// Token: 0x0200031E RID: 798
	public class BlackMarket : MonoBehaviour
	{
		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001A24 RID: 6692 RVA: 0x0005F79C File Offset: 0x0005D99C
		// (set) Token: 0x06001A25 RID: 6693 RVA: 0x0005F7A3 File Offset: 0x0005D9A3
		public static BlackMarket Instance { get; private set; }

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001A26 RID: 6694 RVA: 0x0005F7AB File Offset: 0x0005D9AB
		// (set) Token: 0x06001A27 RID: 6695 RVA: 0x0005F7BE File Offset: 0x0005D9BE
		public int RefreshChance
		{
			get
			{
				return Mathf.Min(this.refreshChance, this.MaxRefreshChance);
			}
			set
			{
				this.refreshChance = value;
				Action<BlackMarket> action = BlackMarket.onRefreshChanceChanged;
				if (action == null)
				{
					return;
				}
				action(this);
			}
		}

		// Token: 0x140000AC RID: 172
		// (add) Token: 0x06001A28 RID: 6696 RVA: 0x0005F7D8 File Offset: 0x0005D9D8
		// (remove) Token: 0x06001A29 RID: 6697 RVA: 0x0005F80C File Offset: 0x0005DA0C
		public static event Action<BlackMarket> onRefreshChanceChanged;

		// Token: 0x140000AD RID: 173
		// (add) Token: 0x06001A2A RID: 6698 RVA: 0x0005F840 File Offset: 0x0005DA40
		// (remove) Token: 0x06001A2B RID: 6699 RVA: 0x0005F874 File Offset: 0x0005DA74
		public static event Action<BlackMarket.OnRequestMaxRefreshChanceEventContext> onRequestMaxRefreshChance;

		// Token: 0x140000AE RID: 174
		// (add) Token: 0x06001A2C RID: 6700 RVA: 0x0005F8A8 File Offset: 0x0005DAA8
		// (remove) Token: 0x06001A2D RID: 6701 RVA: 0x0005F8DC File Offset: 0x0005DADC
		public static event Action<BlackMarket.OnRequestRefreshTimeFactorEventContext> onRequestRefreshTime;

		// Token: 0x06001A2E RID: 6702 RVA: 0x0005F90F File Offset: 0x0005DB0F
		public static void NotifyMaxRefreshChanceChanged()
		{
			BlackMarket.dirty = true;
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001A2F RID: 6703 RVA: 0x0005F918 File Offset: 0x0005DB18
		public int MaxRefreshChance
		{
			get
			{
				if (BlackMarket.dirty)
				{
					BlackMarket.OnRequestMaxRefreshChanceEventContext onRequestMaxRefreshChanceEventContext = new BlackMarket.OnRequestMaxRefreshChanceEventContext();
					onRequestMaxRefreshChanceEventContext.Add(1);
					Action<BlackMarket.OnRequestMaxRefreshChanceEventContext> action = BlackMarket.onRequestMaxRefreshChance;
					if (action != null)
					{
						action(onRequestMaxRefreshChanceEventContext);
					}
					this.cachedMaxRefreshChance = onRequestMaxRefreshChanceEventContext.Value;
				}
				return this.cachedMaxRefreshChance;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001A30 RID: 6704 RVA: 0x0005F95C File Offset: 0x0005DB5C
		private TimeSpan TimeToRefresh
		{
			get
			{
				BlackMarket.OnRequestRefreshTimeFactorEventContext onRequestRefreshTimeFactorEventContext = new BlackMarket.OnRequestRefreshTimeFactorEventContext();
				Action<BlackMarket.OnRequestRefreshTimeFactorEventContext> action = BlackMarket.onRequestRefreshTime;
				if (action != null)
				{
					action(onRequestRefreshTimeFactorEventContext);
				}
				float num = Mathf.Max(onRequestRefreshTimeFactorEventContext.Value, 0.01f);
				return TimeSpan.FromTicks((long)((float)this.timeToRefresh * num));
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001A31 RID: 6705 RVA: 0x0005F9A0 File Offset: 0x0005DBA0
		// (set) Token: 0x06001A32 RID: 6706 RVA: 0x0005F9AD File Offset: 0x0005DBAD
		private DateTime LastRefreshedTime
		{
			get
			{
				return DateTime.FromBinary(this.lastRefreshedTimeRaw);
			}
			set
			{
				this.lastRefreshedTimeRaw = value.ToBinary();
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001A33 RID: 6707 RVA: 0x0005F9BC File Offset: 0x0005DBBC
		private TimeSpan TimeSinceLastRefreshedTime
		{
			get
			{
				if (DateTime.UtcNow < this.LastRefreshedTime)
				{
					this.LastRefreshedTime = DateTime.UtcNow;
				}
				return DateTime.UtcNow - this.LastRefreshedTime;
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001A34 RID: 6708 RVA: 0x0005F9EB File Offset: 0x0005DBEB
		public TimeSpan RemainingTimeBeforeRefresh
		{
			get
			{
				return this.TimeToRefresh - this.TimeSinceLastRefreshedTime;
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001A35 RID: 6709 RVA: 0x0005F9FE File Offset: 0x0005DBFE
		public ReadOnlyCollection<BlackMarket.DemandSupplyEntry> Demands
		{
			get
			{
				if (this._demands_readonly == null)
				{
					this._demands_readonly = new ReadOnlyCollection<BlackMarket.DemandSupplyEntry>(this.demands);
				}
				return this._demands_readonly;
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001A36 RID: 6710 RVA: 0x0005FA1F File Offset: 0x0005DC1F
		public ReadOnlyCollection<BlackMarket.DemandSupplyEntry> Supplies
		{
			get
			{
				if (this._supplies_readonly == null)
				{
					this._supplies_readonly = new ReadOnlyCollection<BlackMarket.DemandSupplyEntry>(this.supplies);
				}
				return this._supplies_readonly;
			}
		}

		// Token: 0x140000AF RID: 175
		// (add) Token: 0x06001A37 RID: 6711 RVA: 0x0005FA40 File Offset: 0x0005DC40
		// (remove) Token: 0x06001A38 RID: 6712 RVA: 0x0005FA78 File Offset: 0x0005DC78
		public event Action onAfterGenerateEntries;

		// Token: 0x06001A39 RID: 6713 RVA: 0x0005FAB0 File Offset: 0x0005DCB0
		private ItemFilter ContructRandomFilter()
		{
			Tag random = this.tags.GetRandom(0f);
			int random2 = this.qualities.GetRandom(0f);
			if (GameMetaData.Instance.IsDemo)
			{
				this.excludeTags.Add(GameplayDataSettings.Tags.LockInDemoTag);
			}
			ItemFilter result = default(ItemFilter);
			result.requireTags = new Tag[]
			{
				random
			};
			result.excludeTags = this.excludeTags.ToArray();
			result.minQuality = random2;
			result.maxQuality = random2;
			return result;
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x0005FB3C File Offset: 0x0005DD3C
		public UniTask<bool> Buy(BlackMarket.DemandSupplyEntry entry)
		{
			BlackMarket.<Buy>d__59 <Buy>d__;
			<Buy>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Buy>d__.<>4__this = this;
			<Buy>d__.entry = entry;
			<Buy>d__.<>1__state = -1;
			<Buy>d__.<>t__builder.Start<BlackMarket.<Buy>d__59>(ref <Buy>d__);
			return <Buy>d__.<>t__builder.Task;
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x0005FB88 File Offset: 0x0005DD88
		public UniTask<bool> Sell(BlackMarket.DemandSupplyEntry entry)
		{
			BlackMarket.<Sell>d__60 <Sell>d__;
			<Sell>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Sell>d__.<>4__this = this;
			<Sell>d__.entry = entry;
			<Sell>d__.<>1__state = -1;
			<Sell>d__.<>t__builder.Start<BlackMarket.<Sell>d__60>(ref <Sell>d__);
			return <Sell>d__.<>t__builder.Task;
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x0005FBD4 File Offset: 0x0005DDD4
		private void GenerateDemandsAndSupplies()
		{
			this.demands.Clear();
			this.supplies.Clear();
			int num = 0;
			for (int i = 0; i < this.demandsCount; i++)
			{
				num++;
				if (num > 100)
				{
					Debug.LogError("黑市构建需求失败。尝试次数超过100次。");
					break;
				}
				int[] array = ItemAssetsCollection.Search(this.ContructRandomFilter());
				if (array.Length == 0)
				{
					i--;
				}
				else
				{
					int random = array.GetRandom<int>();
					ItemAssetsCollection.GetMetaData(random);
					int random2 = this.demandAmountRand.GetRandom(0f);
					float random3 = this.demandFactorRand.GetRandom(0f);
					int random4 = this.demandBatchCountRand.GetRandom(0f);
					BlackMarket.DemandSupplyEntry item = new BlackMarket.DemandSupplyEntry
					{
						itemID = random,
						remaining = random2,
						priceFactor = random3,
						batchCount = random4
					};
					this.demands.Add(item);
				}
			}
			num = 0;
			for (int j = 0; j < this.suppliesCount; j++)
			{
				num++;
				if (num > 100)
				{
					Debug.LogError("黑市构建供应失败。尝试次数超过100次。");
					break;
				}
				int[] array2 = ItemAssetsCollection.Search(this.ContructRandomFilter());
				if (array2.Length == 0)
				{
					j--;
				}
				else
				{
					int candidate = array2.GetRandom<int>();
					if (this.demands.Any((BlackMarket.DemandSupplyEntry e) => e.ItemID == candidate))
					{
						j--;
					}
					else
					{
						ItemAssetsCollection.GetMetaData(candidate);
						int random5 = this.supplyAmountRand.GetRandom(0f);
						float random6 = this.supplyFactorRand.GetRandom(0f);
						int random7 = this.supplyBatchCountRand.GetRandom(0f);
						BlackMarket.DemandSupplyEntry item2 = new BlackMarket.DemandSupplyEntry
						{
							itemID = candidate,
							remaining = random5,
							priceFactor = random6,
							batchCount = random7
						};
						this.supplies.Add(item2);
					}
				}
			}
			Action action = this.onAfterGenerateEntries;
			if (action != null)
			{
				action();
			}
			if (!LevelManager.LevelInited)
			{
				return;
			}
			LevelManager.Instance.SaveMainCharacter();
			SavesSystem.CollectSaveData();
			SavesSystem.SaveFile(true);
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x0005FDE8 File Offset: 0x0005DFE8
		public void PayAndRegenerate()
		{
			if (this.RefreshChance <= 0)
			{
				return;
			}
			int num = this.RefreshChance;
			this.RefreshChance = num - 1;
			this.GenerateDemandsAndSupplies();
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x0005FE18 File Offset: 0x0005E018
		private void FixedUpdate()
		{
			if (this.RefreshChance >= this.MaxRefreshChance)
			{
				this.LastRefreshedTime = DateTime.UtcNow;
				return;
			}
			TimeSpan timeSpan = this.TimeSinceLastRefreshedTime;
			if (timeSpan > this.TimeToRefresh)
			{
				while (timeSpan > this.TimeToRefresh)
				{
					timeSpan -= this.TimeToRefresh;
					this.RefreshChance++;
					if (this.RefreshChance >= this.MaxRefreshChance)
					{
						break;
					}
				}
				if (timeSpan > TimeSpan.Zero)
				{
					this.LastRefreshedTime = DateTime.UtcNow - timeSpan;
				}
			}
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x0005FEAB File Offset: 0x0005E0AB
		private void Awake()
		{
			BlackMarket.Instance = this;
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x0005FEC4 File Offset: 0x0005E0C4
		private void Start()
		{
			this.Load();
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x0005FECC File Offset: 0x0005E0CC
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x0005FEE0 File Offset: 0x0005E0E0
		private void Save()
		{
			BlackMarket.SaveData value = new BlackMarket.SaveData(this);
			SavesSystem.Save<BlackMarket.SaveData>("BlackMarket_Data", value);
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x0005FF00 File Offset: 0x0005E100
		private void Load()
		{
			BlackMarket.SaveData saveData = SavesSystem.Load<BlackMarket.SaveData>("BlackMarket_Data");
			if (!saveData.valid)
			{
				this.GenerateDemandsAndSupplies();
				return;
			}
			this.demands.Clear();
			this.demands.AddRange(saveData.demands);
			this.supplies.Clear();
			this.supplies.AddRange(saveData.supplies);
			this.lastRefreshedTimeRaw = saveData.lastRefreshedTimeRaw;
			this.refreshChance = saveData.refreshChance;
		}

		// Token: 0x040012F6 RID: 4854
		[SerializeField]
		private int demandsCount = 3;

		// Token: 0x040012F7 RID: 4855
		[SerializeField]
		private int suppliesCount = 3;

		// Token: 0x040012F8 RID: 4856
		[SerializeField]
		private List<Tag> excludeTags;

		// Token: 0x040012F9 RID: 4857
		[SerializeField]
		private RandomContainer<Tag> tags;

		// Token: 0x040012FA RID: 4858
		[SerializeField]
		private RandomContainer<int> qualities;

		// Token: 0x040012FB RID: 4859
		[SerializeField]
		private RandomContainer<int> demandAmountRand;

		// Token: 0x040012FC RID: 4860
		[SerializeField]
		private RandomContainer<float> demandFactorRand;

		// Token: 0x040012FD RID: 4861
		[SerializeField]
		private RandomContainer<int> demandBatchCountRand;

		// Token: 0x040012FE RID: 4862
		[SerializeField]
		private RandomContainer<int> supplyAmountRand;

		// Token: 0x040012FF RID: 4863
		[SerializeField]
		private RandomContainer<float> supplyFactorRand;

		// Token: 0x04001300 RID: 4864
		[SerializeField]
		private RandomContainer<int> supplyBatchCountRand;

		// Token: 0x04001301 RID: 4865
		[SerializeField]
		[TimeSpan]
		private long timeToRefresh;

		// Token: 0x04001302 RID: 4866
		[SerializeField]
		private int refreshChance;

		// Token: 0x04001306 RID: 4870
		private static bool dirty = true;

		// Token: 0x04001307 RID: 4871
		private int cachedMaxRefreshChance = -1;

		// Token: 0x04001308 RID: 4872
		[DateTime]
		private long lastRefreshedTimeRaw;

		// Token: 0x04001309 RID: 4873
		private List<BlackMarket.DemandSupplyEntry> demands = new List<BlackMarket.DemandSupplyEntry>();

		// Token: 0x0400130A RID: 4874
		private List<BlackMarket.DemandSupplyEntry> supplies = new List<BlackMarket.DemandSupplyEntry>();

		// Token: 0x0400130B RID: 4875
		private ReadOnlyCollection<BlackMarket.DemandSupplyEntry> _demands_readonly;

		// Token: 0x0400130C RID: 4876
		private ReadOnlyCollection<BlackMarket.DemandSupplyEntry> _supplies_readonly;

		// Token: 0x0400130E RID: 4878
		private const string SaveKey = "BlackMarket_Data";

		// Token: 0x020005BD RID: 1469
		public class OnRequestMaxRefreshChanceEventContext
		{
			// Token: 0x170007A2 RID: 1954
			// (get) Token: 0x060029CD RID: 10701 RVA: 0x0009B942 File Offset: 0x00099B42
			public int Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x060029CE RID: 10702 RVA: 0x0009B94A File Offset: 0x00099B4A
			public void Add(int count = 1)
			{
				this.value += count;
			}

			// Token: 0x04002101 RID: 8449
			private int value;
		}

		// Token: 0x020005BE RID: 1470
		public class OnRequestRefreshTimeFactorEventContext
		{
			// Token: 0x170007A3 RID: 1955
			// (get) Token: 0x060029D0 RID: 10704 RVA: 0x0009B962 File Offset: 0x00099B62
			public float Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x060029D1 RID: 10705 RVA: 0x0009B96A File Offset: 0x00099B6A
			public void Add(float count = -0.1f)
			{
				this.value += count;
			}

			// Token: 0x04002102 RID: 8450
			private float value = 1f;
		}

		// Token: 0x020005BF RID: 1471
		[Serializable]
		public class DemandSupplyEntry
		{
			// Token: 0x170007A4 RID: 1956
			// (get) Token: 0x060029D3 RID: 10707 RVA: 0x0009B98D File Offset: 0x00099B8D
			public int ItemID
			{
				get
				{
					return this.itemID;
				}
			}

			// Token: 0x170007A5 RID: 1957
			// (get) Token: 0x060029D4 RID: 10708 RVA: 0x0009B995 File Offset: 0x00099B95
			internal ItemMetaData ItemMetaData
			{
				get
				{
					return ItemAssetsCollection.GetMetaData(this.itemID);
				}
			}

			// Token: 0x170007A6 RID: 1958
			// (get) Token: 0x060029D5 RID: 10709 RVA: 0x0009B9A2 File Offset: 0x00099BA2
			public int Remaining
			{
				get
				{
					return this.remaining;
				}
			}

			// Token: 0x170007A7 RID: 1959
			// (get) Token: 0x060029D6 RID: 10710 RVA: 0x0009B9AA File Offset: 0x00099BAA
			public int TotalPrice
			{
				get
				{
					return Mathf.FloorToInt((float)this.ItemMetaData.priceEach * this.priceFactor * (float)this.ItemMetaData.defaultStackCount * (float)this.batchCount);
				}
			}

			// Token: 0x170007A8 RID: 1960
			// (get) Token: 0x060029D7 RID: 10711 RVA: 0x0009B9D9 File Offset: 0x00099BD9
			public Cost BuyCost
			{
				get
				{
					return new Cost((long)this.TotalPrice);
				}
			}

			// Token: 0x170007A9 RID: 1961
			// (get) Token: 0x060029D8 RID: 10712 RVA: 0x0009B9E7 File Offset: 0x00099BE7
			public Cost SellCost
			{
				get
				{
					return new Cost(new ValueTuple<int, long>[]
					{
						new ValueTuple<int, long>(this.ItemMetaData.id, (long)(this.ItemMetaData.defaultStackCount * this.batchCount))
					});
				}
			}

			// Token: 0x170007AA RID: 1962
			// (get) Token: 0x060029D9 RID: 10713 RVA: 0x0009BA20 File Offset: 0x00099C20
			public string ItemDisplayName
			{
				get
				{
					return this.ItemMetaData.DisplayName;
				}
			}

			// Token: 0x14000104 RID: 260
			// (add) Token: 0x060029DA RID: 10714 RVA: 0x0009BA3C File Offset: 0x00099C3C
			// (remove) Token: 0x060029DB RID: 10715 RVA: 0x0009BA74 File Offset: 0x00099C74
			public event Action<BlackMarket.DemandSupplyEntry> onChanged;

			// Token: 0x060029DC RID: 10716 RVA: 0x0009BAA9 File Offset: 0x00099CA9
			internal void NotifyChange()
			{
				Action<BlackMarket.DemandSupplyEntry> action = this.onChanged;
				if (action == null)
				{
					return;
				}
				action(this);
			}

			// Token: 0x04002103 RID: 8451
			[SerializeField]
			[ItemTypeID]
			internal int itemID;

			// Token: 0x04002104 RID: 8452
			[SerializeField]
			internal int remaining;

			// Token: 0x04002105 RID: 8453
			[SerializeField]
			internal float priceFactor;

			// Token: 0x04002106 RID: 8454
			[SerializeField]
			internal int batchCount;
		}

		// Token: 0x020005C0 RID: 1472
		[Serializable]
		public struct SaveData
		{
			// Token: 0x060029DE RID: 10718 RVA: 0x0009BAC4 File Offset: 0x00099CC4
			public SaveData(BlackMarket blackMarket)
			{
				this.valid = true;
				this.lastRefreshedTimeRaw = blackMarket.lastRefreshedTimeRaw;
				this.demands = blackMarket.demands.ToArray();
				this.supplies = blackMarket.supplies.ToArray();
				this.refreshChance = blackMarket.refreshChance;
			}

			// Token: 0x04002108 RID: 8456
			public bool valid;

			// Token: 0x04002109 RID: 8457
			public long lastRefreshedTimeRaw;

			// Token: 0x0400210A RID: 8458
			public int refreshChance;

			// Token: 0x0400210B RID: 8459
			public BlackMarket.DemandSupplyEntry[] demands;

			// Token: 0x0400210C RID: 8460
			public BlackMarket.DemandSupplyEntry[] supplies;
		}
	}
}
