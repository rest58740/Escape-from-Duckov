using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000418 RID: 1048
	public class LootBoxLoader : MonoBehaviour
	{
		// Token: 0x06002618 RID: 9752 RVA: 0x00084332 File Offset: 0x00082532
		public void CalculateChances()
		{
			this.randomPool.RefreshPercent();
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002619 RID: 9753 RVA: 0x0008433F File Offset: 0x0008253F
		public List<int> FixedItems
		{
			get
			{
				return this.fixedItems;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x0600261A RID: 9754 RVA: 0x00084347 File Offset: 0x00082547
		[SerializeField]
		private Inventory Inventory
		{
			get
			{
				if (this._lootBox == null)
				{
					this._lootBox = base.GetComponent<InteractableLootbox>();
					if (this._lootBox == null)
					{
						return null;
					}
				}
				return this._lootBox.Inventory;
			}
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x0008437E File Offset: 0x0008257E
		public static int[] Search(ItemFilter filter)
		{
			return ItemAssetsCollection.Search(filter);
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x00084386 File Offset: 0x00082586
		private void Awake()
		{
			if (this._lootBox == null)
			{
				this._lootBox = base.GetComponent<InteractableLootbox>();
			}
			this.RandomActive();
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x000843A8 File Offset: 0x000825A8
		private int GetKey()
		{
			Vector3 vector = base.transform.position * 10f;
			int num = Mathf.RoundToInt(vector.x);
			int num2 = Mathf.RoundToInt(vector.y);
			int num3 = Mathf.RoundToInt(vector.z);
			Vector3Int vector3Int;
			vector3Int..ctor(num, num2, num3);
			return string.Format("LootBoxLoader_{0}", vector3Int).GetHashCode();
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x0008440C File Offset: 0x0008260C
		private void RandomActive()
		{
			bool flag = false;
			int key = this.GetKey();
			object obj;
			if (MultiSceneCore.Instance.inLevelData.TryGetValue(key, out obj))
			{
				if (obj is bool)
				{
					bool flag2 = (bool)obj;
					flag = flag2;
				}
			}
			else
			{
				flag = (Random.Range(0f, 1f) < this.activeChance);
				MultiSceneCore.Instance.inLevelData.Add(key, flag);
			}
			base.gameObject.SetActive(flag);
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x00084483 File Offset: 0x00082683
		public void StartSetup()
		{
			this.Setup().Forget();
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x00084490 File Offset: 0x00082690
		public UniTask Setup()
		{
			LootBoxLoader.<Setup>d__27 <Setup>d__;
			<Setup>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Setup>d__.<>4__this = this;
			<Setup>d__.<>1__state = -1;
			<Setup>d__.<>t__builder.Start<LootBoxLoader.<Setup>d__27>(ref <Setup>d__);
			return <Setup>d__.<>t__builder.Task;
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x000844D4 File Offset: 0x000826D4
		private UniTask CreateCash()
		{
			LootBoxLoader.<CreateCash>d__28 <CreateCash>d__;
			<CreateCash>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CreateCash>d__.<>4__this = this;
			<CreateCash>d__.<>1__state = -1;
			<CreateCash>d__.<>t__builder.Start<LootBoxLoader.<CreateCash>d__28>(ref <CreateCash>d__);
			return <CreateCash>d__.<>t__builder.Task;
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x00084517 File Offset: 0x00082717
		private void OnValidate()
		{
			this.tags.RefreshPercent();
			this.qualities.RefreshPercent();
		}

		// Token: 0x040019F6 RID: 6646
		public bool autoSetup = true;

		// Token: 0x040019F7 RID: 6647
		public bool dropOnSpawnItem;

		// Token: 0x040019F8 RID: 6648
		[SerializeField]
		[Range(0f, 1f)]
		private float activeChance = 1f;

		// Token: 0x040019F9 RID: 6649
		[SerializeField]
		private int inventorySize = 8;

		// Token: 0x040019FA RID: 6650
		public bool ignoreLevelConfig;

		// Token: 0x040019FB RID: 6651
		[SerializeField]
		private Vector2Int randomCount = new Vector2Int(1, 1);

		// Token: 0x040019FC RID: 6652
		public bool randomFromPool;

		// Token: 0x040019FD RID: 6653
		[SerializeField]
		private RandomContainer<Tag> tags;

		// Token: 0x040019FE RID: 6654
		[SerializeField]
		private List<Tag> excludeTags;

		// Token: 0x040019FF RID: 6655
		[SerializeField]
		private RandomContainer<int> qualities;

		// Token: 0x04001A00 RID: 6656
		[SerializeField]
		private RandomContainer<LootBoxLoader.Entry> randomPool;

		// Token: 0x04001A01 RID: 6657
		[Range(0f, 1f)]
		public float GenrateCashChance;

		// Token: 0x04001A02 RID: 6658
		public int maxRandomCash;

		// Token: 0x04001A03 RID: 6659
		[ItemTypeID]
		[SerializeField]
		private List<int> fixedItems;

		// Token: 0x04001A04 RID: 6660
		[Range(0f, 1f)]
		[SerializeField]
		private float fixedItemSpawnChance = 1f;

		// Token: 0x04001A05 RID: 6661
		[SerializeField]
		private InteractableLootbox _lootBox;

		// Token: 0x0200068E RID: 1678
		[Serializable]
		private struct Entry
		{
			// Token: 0x04002410 RID: 9232
			[ItemTypeID]
			[SerializeField]
			public int itemTypeID;
		}
	}
}
