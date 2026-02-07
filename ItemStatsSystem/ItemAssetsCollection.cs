using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000005 RID: 5
	[CreateAssetMenu(menuName = "Items/Item Asset Collection")]
	public class ItemAssetsCollection : ScriptableObject, ISelfValidator
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002380 File Offset: 0x00000580
		public static ItemAssetsCollection Instance
		{
			get
			{
				if (ItemAssetsCollection.instanceCache)
				{
					return ItemAssetsCollection.instanceCache;
				}
				ItemAssetsCollection.instanceCache = Resources.Load<ItemAssetsCollection>("ItemAssetsCollection");
				return ItemAssetsCollection.instanceCache;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000023A8 File Offset: 0x000005A8
		public int NextTypeID
		{
			get
			{
				int num = this.entries.Max((ItemAssetsCollection.Entry e) => e.typeID);
				if (this.nextTypeID <= num)
				{
					this.nextTypeID = num + 1;
				}
				return this.nextTypeID;
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000023F8 File Offset: 0x000005F8
		public static bool TryGetDynamicEntry(int typeID, out ItemAssetsCollection.DynamicEntry entry)
		{
			return ItemAssetsCollection.dynamicDic.TryGetValue(typeID, out entry) && entry != null;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002414 File Offset: 0x00000614
		public static bool AddDynamicEntry(Item prefab)
		{
			if (prefab == null)
			{
				return false;
			}
			if (ItemAssetsCollection.Instance == null)
			{
				return false;
			}
			int typeID = prefab.TypeID;
			if (ItemAssetsCollection.Instance.entries.Any((ItemAssetsCollection.Entry e) => e != null && e.typeID == typeID))
			{
				Debug.LogWarning(string.Format("Warning from Dynamic Item:{0}\n Dynamic Item Type ID collides with the main game. This will override the main game's item. Please make sure this is intentional, or avoid it.", typeID));
			}
			ItemAssetsCollection.DynamicEntry dynamicEntry;
			if (ItemAssetsCollection.TryGetDynamicEntry(typeID, out dynamicEntry))
			{
				string arg = "?";
				if (dynamicEntry.prefab != null)
				{
					arg = dynamicEntry.prefab.name;
				}
				Debug.LogWarning(string.Format("Warning from Dynamic Item:{0}\nOld GameObject: {1}\nNew GameObject:{2}\nDynamic Item Overwrite detected! May cause some of the mod work incorrectly. Please avoid colliding item type ids.", typeID, arg, prefab.name));
			}
			ItemAssetsCollection.DynamicEntry value = new ItemAssetsCollection.DynamicEntry
			{
				typeID = typeID,
				prefab = prefab
			};
			ItemAssetsCollection.dynamicDic[typeID] = value;
			return true;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002500 File Offset: 0x00000700
		public static bool RemoveDynamicEntry(Item prefab)
		{
			if (prefab == null)
			{
				return false;
			}
			if (ItemAssetsCollection.Instance == null)
			{
				return false;
			}
			int typeID = prefab.TypeID;
			ItemAssetsCollection.DynamicEntry dynamicEntry;
			return ItemAssetsCollection.TryGetDynamicEntry(typeID, out dynamicEntry) && !(dynamicEntry.prefab != prefab) && ItemAssetsCollection.dynamicDic.Remove(typeID);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002558 File Offset: 0x00000758
		private ItemAssetsCollection.Entry GetEntry(int typeID)
		{
			if (this.dic == null)
			{
				this.dic = new Dictionary<int, ItemAssetsCollection.Entry>();
				foreach (ItemAssetsCollection.Entry entry in this.entries)
				{
					this.dic[entry.typeID] = entry;
				}
			}
			ItemAssetsCollection.Entry result;
			if (this.dic.TryGetValue(typeID, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000025DC File Offset: 0x000007DC
		public UniTask<Item> InstantiateAsync_Local(int typeID)
		{
			ItemAssetsCollection.<InstantiateAsync_Local>d__16 <InstantiateAsync_Local>d__;
			<InstantiateAsync_Local>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<InstantiateAsync_Local>d__.<>4__this = this;
			<InstantiateAsync_Local>d__.typeID = typeID;
			<InstantiateAsync_Local>d__.<>1__state = -1;
			<InstantiateAsync_Local>d__.<>t__builder.Start<ItemAssetsCollection.<InstantiateAsync_Local>d__16>(ref <InstantiateAsync_Local>d__);
			return <InstantiateAsync_Local>d__.<>t__builder.Task;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002628 File Offset: 0x00000828
		public static UniTask<Item> InstantiateAsync(int typeID)
		{
			ItemAssetsCollection.<InstantiateAsync>d__17 <InstantiateAsync>d__;
			<InstantiateAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<InstantiateAsync>d__.typeID = typeID;
			<InstantiateAsync>d__.<>1__state = -1;
			<InstantiateAsync>d__.<>t__builder.Start<ItemAssetsCollection.<InstantiateAsync>d__17>(ref <InstantiateAsync>d__);
			return <InstantiateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000266C File Offset: 0x0000086C
		public static Item InstantiateSync(int typeID)
		{
			if (ItemAssetsCollection.Instance == null)
			{
				Debug.LogError("Instance of ItemAssetsCollection not found");
			}
			else
			{
				ItemAssetsCollection.DynamicEntry dynamicEntry;
				if (ItemAssetsCollection.TryGetDynamicEntry(typeID, out dynamicEntry))
				{
					return UnityEngine.Object.Instantiate<Item>(dynamicEntry.prefab);
				}
				ItemAssetsCollection.Entry entry = ItemAssetsCollection.Instance.GetEntry(typeID);
				if (!(entry.prefab == null))
				{
					return UnityEngine.Object.Instantiate<Item>(entry.prefab);
				}
				Debug.LogWarning(string.Format("在 ItemAssetCollection 中未配置 Item ID:{0} 的 Asset。", typeID));
			}
			return ItemAssetsCollection.InstantiateFallbackItem(typeID);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000026EB File Offset: 0x000008EB
		public static Item InstantiateFallbackItem(int typeID)
		{
			Item item = new GameObject(string.Format("FallbackItem_{0}", typeID)).AddComponent<Item>();
			item.TypeID = typeID;
			return item;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002710 File Offset: 0x00000910
		public static ItemMetaData GetMetaData(int typeID)
		{
			ItemAssetsCollection.DynamicEntry dynamicEntry;
			if (ItemAssetsCollection.TryGetDynamicEntry(typeID, out dynamicEntry))
			{
				return dynamicEntry.MetaData;
			}
			ItemAssetsCollection.Entry entry = ItemAssetsCollection.Instance.GetEntry(typeID);
			if (entry == null)
			{
				return default(ItemMetaData);
			}
			return entry.metaData;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002750 File Offset: 0x00000950
		public static Item GetPrefab(int typeID)
		{
			ItemAssetsCollection.DynamicEntry dynamicEntry;
			if (ItemAssetsCollection.TryGetDynamicEntry(typeID, out dynamicEntry))
			{
				return dynamicEntry.prefab;
			}
			ItemAssetsCollection.Entry entry = ItemAssetsCollection.Instance.GetEntry(typeID);
			if (entry == null)
			{
				return null;
			}
			return entry.prefab;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002785 File Offset: 0x00000985
		public void Validate(SelfValidationResult result)
		{
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002787 File Offset: 0x00000987
		public void Collect()
		{
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002789 File Offset: 0x00000989
		private void SetFolderTag(Item item)
		{
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000278C File Offset: 0x0000098C
		public void RefreshMeta()
		{
			foreach (ItemAssetsCollection.Entry entry in this.entries)
			{
				entry.RefreshMetaData();
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000027DC File Offset: 0x000009DC
		public static int[] GetAllTypeIds(ItemFilter filter)
		{
			if (ItemAssetsCollection.Instance == null)
			{
				return null;
			}
			bool matchCaliber = !string.IsNullOrEmpty(filter.caliber);
			IEnumerable<int> collection = from e in ItemAssetsCollection.Instance.entries.FindAll(delegate(ItemAssetsCollection.Entry entry)
			{
				ItemMetaData metaData = entry.metaData;
				return base.<GetAllTypeIds>g__EvaluateFilter|0(metaData, filter);
			})
			select e.typeID;
			IEnumerable<int> range = from e in ItemAssetsCollection.dynamicDic.Where(delegate(KeyValuePair<int, ItemAssetsCollection.DynamicEntry> e)
			{
				ItemAssetsCollection.DynamicEntry value = e.Value;
				return !(value.prefab == null) && base.<GetAllTypeIds>g__EvaluateFilter|0(value.MetaData, filter);
			})
			select e.Key;
			HashSet<int> hashSet = new HashSet<int>(collection);
			hashSet.AddRange(range);
			return hashSet.ToArray<int>();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000028AC File Offset: 0x00000AAC
		public static int[] Search(ItemFilter filter)
		{
			ItemAssetsCollection.<>c__DisplayClass28_0 CS$<>8__locals1;
			CS$<>8__locals1.filter = filter;
			int[] result;
			if (ItemAssetsCollection.cachedSearchResults.TryGetValue(CS$<>8__locals1.filter.GetHashCode(), out result))
			{
				return result;
			}
			CS$<>8__locals1.result = ItemAssetsCollection.GetAllTypeIds(CS$<>8__locals1.filter);
			while (CS$<>8__locals1.result.Length < 1)
			{
				ItemAssetsCollection.<Search>g__DownGradeSearch|28_0(ref CS$<>8__locals1);
				if (CS$<>8__locals1.filter.maxQuality < 0 || CS$<>8__locals1.filter.minQuality < 0)
				{
					break;
				}
			}
			ItemAssetsCollection.cachedSearchResults[CS$<>8__locals1.filter.GetHashCode()] = CS$<>8__locals1.result;
			return CS$<>8__locals1.result;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002950 File Offset: 0x00000B50
		public static int TryGetIDByName(string name, bool ignoreCase = false)
		{
			if (ItemAssetsCollection.Instance == null)
			{
				return -1;
			}
			ItemAssetsCollection.Entry entry = ItemAssetsCollection.Instance.entries.Find(delegate(ItemAssetsCollection.Entry e)
			{
				string name2 = e.metaData.Name;
				return string.Compare(name, name2, ignoreCase) == 0;
			});
			if (entry == null)
			{
				return -1;
			}
			return entry.typeID;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000029C8 File Offset: 0x00000BC8
		[CompilerGenerated]
		internal static void <Search>g__DownGradeSearch|28_0(ref ItemAssetsCollection.<>c__DisplayClass28_0 A_0)
		{
			int num = Mathf.Min(A_0.filter.maxQuality, A_0.filter.minQuality) - 1;
			A_0.filter.maxQuality = num;
			A_0.filter.minQuality = num;
			if (num < 0)
			{
				return;
			}
			A_0.result = ItemAssetsCollection.GetAllTypeIds(A_0.filter);
		}

		// Token: 0x04000007 RID: 7
		private static ItemAssetsCollection instanceCache;

		// Token: 0x04000008 RID: 8
		private bool editNextTypeID;

		// Token: 0x04000009 RID: 9
		[SerializeField]
		private int nextTypeID;

		// Token: 0x0400000A RID: 10
		public List<ItemAssetsCollection.Entry> entries;

		// Token: 0x0400000B RID: 11
		private Dictionary<int, ItemAssetsCollection.Entry> dic;

		// Token: 0x0400000C RID: 12
		private static Dictionary<int, ItemAssetsCollection.DynamicEntry> dynamicDic = new Dictionary<int, ItemAssetsCollection.DynamicEntry>();

		// Token: 0x0400000D RID: 13
		private static Dictionary<int, int[]> cachedSearchResults = new Dictionary<int, int[]>();

		// Token: 0x02000030 RID: 48
		[Serializable]
		public class Entry : ISelfValidator
		{
			// Token: 0x06000246 RID: 582 RVA: 0x00008F19 File Offset: 0x00007119
			public void RefreshMetaData()
			{
			}

			// Token: 0x06000247 RID: 583 RVA: 0x00008F1B File Offset: 0x0000711B
			public void Validate(SelfValidationResult result)
			{
			}

			// Token: 0x040000DC RID: 220
			public int typeID;

			// Token: 0x040000DD RID: 221
			public Item prefab;

			// Token: 0x040000DE RID: 222
			public ItemMetaData metaData;
		}

		// Token: 0x02000031 RID: 49
		public class DynamicEntry
		{
			// Token: 0x1700009F RID: 159
			// (get) Token: 0x06000249 RID: 585 RVA: 0x00008F28 File Offset: 0x00007128
			public ItemMetaData MetaData
			{
				get
				{
					if (this.prefab == null)
					{
						return default(ItemMetaData);
					}
					if (this._metaData == null)
					{
						this._metaData = new ItemMetaData?(new ItemMetaData(this.prefab));
					}
					return this._metaData.Value;
				}
			}

			// Token: 0x040000DF RID: 223
			public int typeID;

			// Token: 0x040000E0 RID: 224
			public Item prefab;

			// Token: 0x040000E1 RID: 225
			private ItemMetaData? _metaData;
		}
	}
}
