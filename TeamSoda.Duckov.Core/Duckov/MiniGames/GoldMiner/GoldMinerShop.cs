using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A0 RID: 672
	public class GoldMinerShop : MiniGameBehaviour
	{
		// Token: 0x06001638 RID: 5688 RVA: 0x00052B40 File Offset: 0x00050D40
		private void Clear()
		{
			this.capacity = this.master.run.shopCapacity;
			for (int i = 0; i < this.stock.Count; i++)
			{
				ShopEntity shopEntity = this.stock[i];
				if (shopEntity != null && (shopEntity.sold || !shopEntity.locked))
				{
					this.stock[i] = null;
				}
			}
			for (int j = this.capacity; j < this.stock.Count; j++)
			{
				if (this.stock[j] == null)
				{
					this.stock.RemoveAt(j);
				}
			}
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x00052BDC File Offset: 0x00050DDC
		private void Refill()
		{
			this.capacity = this.master.run.shopCapacity;
			for (int i = 0; i < this.capacity; i++)
			{
				if (this.stock.Count <= i)
				{
					this.stock.Add(null);
				}
				ShopEntity shopEntity = this.stock[i];
				if (shopEntity == null || shopEntity.sold)
				{
					this.stock[i] = this.GenerateNewShopItem();
				}
			}
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x00052C54 File Offset: 0x00050E54
		private void RefreshStock()
		{
			this.Clear();
			this.CacheValidCandiateLists();
			this.Refill();
			Action action = this.onAfterOperation;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x00052C78 File Offset: 0x00050E78
		private void CacheValidCandiateLists()
		{
			for (int i = 0; i < 5; i++)
			{
				int quality = i + 1;
				List<GoldMinerArtifact> list = this.SearchValidCandidateArtifactIDs(quality).ToList<GoldMinerArtifact>();
				this.validCandidateLists[i] = list;
			}
			foreach (ShopEntity shopEntity in this.stock)
			{
				if (shopEntity != null && !(shopEntity.artifact == null) && !shopEntity.artifact.AllowMultiple)
				{
					foreach (List<GoldMinerArtifact> list2 in this.validCandidateLists)
					{
						if (list2 != null)
						{
							list2.Remove(shopEntity.artifact);
						}
					}
				}
			}
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x00052D44 File Offset: 0x00050F44
		private IEnumerable<GoldMinerArtifact> SearchValidCandidateArtifactIDs(int quality)
		{
			using (IEnumerator<GoldMinerArtifact> enumerator = this.master.ArtifactPrefabs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GoldMinerArtifact artifact = enumerator.Current;
					if (artifact.Quality == quality && (artifact.AllowMultiple || (this.master.run.GetArtifactCount(artifact.ID) <= 0 && !this.stock.Any((ShopEntity e) => e != null && !e.sold && e.ID == artifact.ID))))
					{
						yield return artifact;
					}
				}
			}
			IEnumerator<GoldMinerArtifact> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x00052D5B File Offset: 0x00050F5B
		private List<GoldMinerArtifact> GetValidCandidateArtifactIDs(int q)
		{
			return this.validCandidateLists[q - 1];
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x00052D68 File Offset: 0x00050F68
		private ShopEntity GenerateNewShopItem()
		{
			int num = this.qualityDistribute.GetRandom(0f);
			List<GoldMinerArtifact> list = null;
			for (int i = num; i >= 1; i--)
			{
				list = this.GetValidCandidateArtifactIDs(i);
				if (list.Count > 0)
				{
					num = i;
					break;
				}
			}
			GoldMinerArtifact random = list.GetRandom(this.master.run.shopRandom);
			if (random != null && !random.AllowMultiple)
			{
				List<GoldMinerArtifact> validCandidateArtifactIDs = this.GetValidCandidateArtifactIDs(num);
				if (validCandidateArtifactIDs != null)
				{
					validCandidateArtifactIDs.Remove(random);
				}
			}
			if (random == null)
			{
				Debug.Log(string.Format("{0} failed to generate", num));
			}
			return new ShopEntity
			{
				artifact = random
			};
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x00052E14 File Offset: 0x00051014
		public bool Buy(ShopEntity entity)
		{
			if (!this.stock.Contains(entity))
			{
				Debug.LogError("Buying entity that doesn't exist in shop stock");
				return false;
			}
			if (entity.sold)
			{
				return false;
			}
			bool flag;
			int price = this.CalculateDealPrice(entity, out flag);
			if (this.master.run.shopTicket > 0)
			{
				this.master.run.shopTicket--;
			}
			else if (!this.master.PayMoney(price))
			{
				return false;
			}
			this.master.run.AttachArtifactFromPrefab(entity.artifact);
			entity.sold = true;
			Action action = this.onAfterOperation;
			if (action != null)
			{
				action();
			}
			return true;
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x00052EBC File Offset: 0x000510BC
		public int CalculateDealPrice(ShopEntity entity, out bool useTicket)
		{
			useTicket = false;
			if (entity == null)
			{
				return 0;
			}
			if (this.master.run.shopTicket > 0)
			{
				useTicket = true;
				return 0;
			}
			GoldMinerArtifact artifact = entity.artifact;
			if (artifact == null)
			{
				return 0;
			}
			return Mathf.CeilToInt((float)artifact.BasePrice * entity.priceFactor * this.master.GlobalPriceFactor);
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001641 RID: 5697 RVA: 0x00052F1A File Offset: 0x0005111A
		// (set) Token: 0x06001642 RID: 5698 RVA: 0x00052F22 File Offset: 0x00051122
		public int refreshChance { get; private set; }

		// Token: 0x06001643 RID: 5699 RVA: 0x00052F2C File Offset: 0x0005112C
		public UniTask Execute()
		{
			GoldMinerShop.<Execute>d__22 <Execute>d__;
			<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Execute>d__.<>4__this = this;
			<Execute>d__.<>1__state = -1;
			<Execute>d__.<>t__builder.Start<GoldMinerShop.<Execute>d__22>(ref <Execute>d__);
			return <Execute>d__.<>t__builder.Task;
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00052F6F File Offset: 0x0005116F
		internal void Continue()
		{
			this.complete = true;
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00052F78 File Offset: 0x00051178
		internal bool TryRefresh()
		{
			if (this.refreshChance <= 0)
			{
				return false;
			}
			int refreshCost = this.GetRefreshCost();
			if (!this.master.PayMoney(refreshCost))
			{
				return false;
			}
			int refreshChance = this.refreshChance;
			this.refreshChance = refreshChance - 1;
			this.refreshPrice += Mathf.RoundToInt(this.master.run.shopRefreshPriceIncrement.Value);
			this.RefreshStock();
			return true;
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x00052FE5 File Offset: 0x000511E5
		internal int GetRefreshCost()
		{
			return this.refreshPrice;
		}

		// Token: 0x0400106D RID: 4205
		[SerializeField]
		private GoldMiner master;

		// Token: 0x0400106E RID: 4206
		[SerializeField]
		private GoldMinerShopUI ui;

		// Token: 0x0400106F RID: 4207
		[SerializeField]
		private RandomContainer<int> qualityDistribute;

		// Token: 0x04001070 RID: 4208
		public List<ShopEntity> stock = new List<ShopEntity>();

		// Token: 0x04001071 RID: 4209
		public Action onAfterOperation;

		// Token: 0x04001072 RID: 4210
		private int capacity;

		// Token: 0x04001073 RID: 4211
		private List<GoldMinerArtifact>[] validCandidateLists = new List<GoldMinerArtifact>[5];

		// Token: 0x04001074 RID: 4212
		private bool complete;

		// Token: 0x04001075 RID: 4213
		private int refreshPrice = 100;
	}
}
