using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using ItemStatsSystem.Data;
using Saves;
using UnityEngine;

namespace Duckov.Bitcoins
{
	// Token: 0x02000324 RID: 804
	public class BitcoinMiner : MonoBehaviour
	{
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001A97 RID: 6807 RVA: 0x00060B8B File Offset: 0x0005ED8B
		// (set) Token: 0x06001A98 RID: 6808 RVA: 0x00060B92 File Offset: 0x0005ED92
		public static BitcoinMiner Instance { get; private set; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001A99 RID: 6809 RVA: 0x00060B9A File Offset: 0x0005ED9A
		private double Progress
		{
			get
			{
				return this.work;
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001A9A RID: 6810 RVA: 0x00060BA2 File Offset: 0x0005EDA2
		private static double K_1_12
		{
			get
			{
				if (BitcoinMiner._cached_k == null)
				{
					BitcoinMiner._cached_k = new double?((BitcoinMiner.wps_12 - BitcoinMiner.wps_1) / 11.0);
				}
				return BitcoinMiner._cached_k.Value;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001A9B RID: 6811 RVA: 0x00060BDC File Offset: 0x0005EDDC
		public double WorkPerSecond
		{
			get
			{
				if (this.IsInventoryFull)
				{
					return 0.0;
				}
				if (this.cachedPerformance < 1f)
				{
					return (double)this.cachedPerformance * BitcoinMiner.wps_1;
				}
				return BitcoinMiner.wps_1 + (double)(this.cachedPerformance - 1f) * BitcoinMiner.K_1_12;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001A9C RID: 6812 RVA: 0x00060C2F File Offset: 0x0005EE2F
		public double HoursPerCoin
		{
			get
			{
				return this.workPerCoin / 3600.0 / this.WorkPerSecond;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001A9D RID: 6813 RVA: 0x00060C48 File Offset: 0x0005EE48
		public bool IsInventoryFull
		{
			get
			{
				return !(this.item == null) && this.item.Inventory.GetFirstEmptyPosition(0) < 0;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001A9E RID: 6814 RVA: 0x00060C6E File Offset: 0x0005EE6E
		public TimeSpan TimePerCoin
		{
			get
			{
				if (this.WorkPerSecond > 0.0)
				{
					return TimeSpan.FromSeconds(this.workPerCoin / this.WorkPerSecond);
				}
				return TimeSpan.MaxValue;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001A9F RID: 6815 RVA: 0x00060C99 File Offset: 0x0005EE99
		public TimeSpan RemainingTime
		{
			get
			{
				if (this.WorkPerSecond > 0.0)
				{
					return TimeSpan.FromSeconds((this.workPerCoin - this.work) / this.WorkPerSecond);
				}
				return TimeSpan.MaxValue;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001AA0 RID: 6816 RVA: 0x00060CCC File Offset: 0x0005EECC
		// (set) Token: 0x06001AA1 RID: 6817 RVA: 0x00060D11 File Offset: 0x0005EF11
		private DateTime LastUpdateDateTime
		{
			get
			{
				DateTime dateTime = DateTime.FromBinary(this.lastUpdateDateTimeRaw);
				if (dateTime > DateTime.UtcNow)
				{
					this.lastUpdateDateTimeRaw = DateTime.UtcNow.ToBinary();
					dateTime = DateTime.UtcNow;
					GameManager.TimeTravelDetected();
				}
				return dateTime;
			}
			set
			{
				this.lastUpdateDateTimeRaw = value.ToBinary();
			}
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x00060D20 File Offset: 0x0005EF20
		private void Awake()
		{
			if (BitcoinMiner.Instance != null)
			{
				Debug.LogError("存在多个BitcoinMiner");
				return;
			}
			BitcoinMiner.Instance = this;
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x00060D51 File Offset: 0x0005EF51
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x00060D64 File Offset: 0x0005EF64
		private void Start()
		{
			this.Load();
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001AA5 RID: 6821 RVA: 0x00060D6C File Offset: 0x0005EF6C
		// (set) Token: 0x06001AA6 RID: 6822 RVA: 0x00060D74 File Offset: 0x0005EF74
		public bool Loading { get; private set; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001AA7 RID: 6823 RVA: 0x00060D7D File Offset: 0x0005EF7D
		// (set) Token: 0x06001AA8 RID: 6824 RVA: 0x00060D85 File Offset: 0x0005EF85
		public bool Initialized { get; private set; }

		// Token: 0x06001AA9 RID: 6825 RVA: 0x00060D90 File Offset: 0x0005EF90
		private UniTask Setup(BitcoinMiner.SaveData data)
		{
			BitcoinMiner.<Setup>d__43 <Setup>d__;
			<Setup>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Setup>d__.<>4__this = this;
			<Setup>d__.data = data;
			<Setup>d__.<>1__state = -1;
			<Setup>d__.<>t__builder.Start<BitcoinMiner.<Setup>d__43>(ref <Setup>d__);
			return <Setup>d__.<>t__builder.Task;
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x00060DDC File Offset: 0x0005EFDC
		private UniTask Initialize()
		{
			BitcoinMiner.<Initialize>d__44 <Initialize>d__;
			<Initialize>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Initialize>d__.<>4__this = this;
			<Initialize>d__.<>1__state = -1;
			<Initialize>d__.<>t__builder.Start<BitcoinMiner.<Initialize>d__44>(ref <Initialize>d__);
			return <Initialize>d__.<>t__builder.Task;
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x00060E20 File Offset: 0x0005F020
		private void Load()
		{
			if (SavesSystem.KeyExisits("BitcoinMiner_Data"))
			{
				BitcoinMiner.SaveData data = SavesSystem.Load<BitcoinMiner.SaveData>("BitcoinMiner_Data");
				this.Setup(data).Forget();
				return;
			}
			this.Initialize().Forget();
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x00060E5C File Offset: 0x0005F05C
		private void Save()
		{
			if (this.Loading)
			{
				return;
			}
			if (!this.Initialized)
			{
				return;
			}
			BitcoinMiner.SaveData value = new BitcoinMiner.SaveData
			{
				itemData = ItemTreeData.FromItem(this.item),
				work = this.work,
				lastUpdateDateTimeRaw = this.lastUpdateDateTimeRaw,
				cachedPerformance = this.cachedPerformance
			};
			SavesSystem.Save<BitcoinMiner.SaveData>("BitcoinMiner_Data", value);
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x00060ECC File Offset: 0x0005F0CC
		private void UpdateWork()
		{
			if (this.Loading)
			{
				return;
			}
			if (!this.Initialized)
			{
				return;
			}
			double totalSeconds = (DateTime.UtcNow - this.LastUpdateDateTime).TotalSeconds;
			double num = this.WorkPerSecond * totalSeconds;
			bool isInventoryFull = this.IsInventoryFull;
			if (this.work < 0.0)
			{
				this.work = 0.0;
			}
			this.work += num;
			if (this.work >= this.workPerCoin && !this.CreatingCoin)
			{
				if (!isInventoryFull)
				{
					this.CreateCoin().Forget();
				}
				else
				{
					this.work = this.workPerCoin;
				}
			}
			this.cachedPerformance = this.item.GetStatValue("Performance".GetHashCode());
			this.LastUpdateDateTime = DateTime.UtcNow;
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001AAE RID: 6830 RVA: 0x00060F9A File Offset: 0x0005F19A
		// (set) Token: 0x06001AAF RID: 6831 RVA: 0x00060FA2 File Offset: 0x0005F1A2
		public bool CreatingCoin { get; private set; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001AB0 RID: 6832 RVA: 0x00060FAB File Offset: 0x0005F1AB
		public Item Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001AB1 RID: 6833 RVA: 0x00060FB3 File Offset: 0x0005F1B3
		public float NormalizedProgress
		{
			get
			{
				return (float)(this.work / this.workPerCoin);
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001AB2 RID: 6834 RVA: 0x00060FC3 File Offset: 0x0005F1C3
		public double Performance
		{
			get
			{
				if (this.Item == null)
				{
					return 0.0;
				}
				return (double)this.Item.GetStatValue("Performance".GetHashCode());
			}
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x00060FF4 File Offset: 0x0005F1F4
		private UniTask CreateCoin()
		{
			BitcoinMiner.<CreateCoin>d__60 <CreateCoin>d__;
			<CreateCoin>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CreateCoin>d__.<>4__this = this;
			<CreateCoin>d__.<>1__state = -1;
			<CreateCoin>d__.<>t__builder.Start<BitcoinMiner.<CreateCoin>d__60>(ref <CreateCoin>d__);
			return <CreateCoin>d__.<>t__builder.Task;
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x00061037 File Offset: 0x0005F237
		private void FixedUpdate()
		{
			this.UpdateWork();
		}

		// Token: 0x04001339 RID: 4921
		[SerializeField]
		[ItemTypeID]
		private int minerItemID = 397;

		// Token: 0x0400133A RID: 4922
		[SerializeField]
		[ItemTypeID]
		private int coinItemID = 388;

		// Token: 0x0400133B RID: 4923
		[SerializeField]
		private double workPerCoin = 1.0;

		// Token: 0x0400133C RID: 4924
		private Item item;

		// Token: 0x0400133D RID: 4925
		private double work;

		// Token: 0x0400133E RID: 4926
		private static readonly double wps_1 = 2.3148148148148147E-05;

		// Token: 0x0400133F RID: 4927
		private static readonly double wps_12 = 5.555555555555556E-05;

		// Token: 0x04001340 RID: 4928
		private static double? _cached_k;

		// Token: 0x04001341 RID: 4929
		[DateTime]
		private long lastUpdateDateTimeRaw;

		// Token: 0x04001342 RID: 4930
		private float cachedPerformance;

		// Token: 0x04001345 RID: 4933
		public const string SaveKey = "BitcoinMiner_Data";

		// Token: 0x04001346 RID: 4934
		private const string PerformaceStatKey = "Performance";

		// Token: 0x020005C5 RID: 1477
		[Serializable]
		private struct SaveData
		{
			// Token: 0x0400211C RID: 8476
			public ItemTreeData itemData;

			// Token: 0x0400211D RID: 8477
			public double work;

			// Token: 0x0400211E RID: 8478
			public float cachedPerformance;

			// Token: 0x0400211F RID: 8479
			public long lastUpdateDateTimeRaw;
		}
	}
}
