using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000350 RID: 848
	public class RewardItem : Reward
	{
		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001D4B RID: 7499 RVA: 0x0006A7EB File Offset: 0x000689EB
		public override bool Claimed
		{
			get
			{
				return this.claimed;
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001D4C RID: 7500 RVA: 0x0006A7F3 File Offset: 0x000689F3
		public override bool Claiming
		{
			get
			{
				return this.claiming;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001D4D RID: 7501 RVA: 0x0006A7FB File Offset: 0x000689FB
		private ItemMetaData CachedMeta
		{
			get
			{
				if (this._cachedMeta == null)
				{
					this._cachedMeta = new ItemMetaData?(ItemAssetsCollection.GetMetaData(this.itemTypeID));
				}
				return this._cachedMeta.Value;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001D4E RID: 7502 RVA: 0x0006A82B File Offset: 0x00068A2B
		public override Sprite Icon
		{
			get
			{
				return this.CachedMeta.icon;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001D4F RID: 7503 RVA: 0x0006A838 File Offset: 0x00068A38
		public override string Description
		{
			get
			{
				return string.Format("{0} x{1}", this.CachedMeta.DisplayName, this.amount);
			}
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x0006A868 File Offset: 0x00068A68
		public override object GenerateSaveData()
		{
			return this.claimed;
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x0006A875 File Offset: 0x00068A75
		public override void SetupSaveData(object data)
		{
			this.claimed = (bool)data;
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x0006A883 File Offset: 0x00068A83
		public override void OnClaim()
		{
			if (this.claimed)
			{
				return;
			}
			if (this.claiming)
			{
				return;
			}
			this.claiming = true;
			this.GenerateAndGiveItems().Forget();
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x0006A8AC File Offset: 0x00068AAC
		private UniTask GenerateAndGiveItems()
		{
			RewardItem.<GenerateAndGiveItems>d__18 <GenerateAndGiveItems>d__;
			<GenerateAndGiveItems>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<GenerateAndGiveItems>d__.<>4__this = this;
			<GenerateAndGiveItems>d__.<>1__state = -1;
			<GenerateAndGiveItems>d__.<>t__builder.Start<RewardItem.<GenerateAndGiveItems>d__18>(ref <GenerateAndGiveItems>d__);
			return <GenerateAndGiveItems>d__.<>t__builder.Task;
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x0006A8EF File Offset: 0x00068AEF
		private void SendItemToPlayerStorage(Item item)
		{
			PlayerStorage.Push(item, true);
		}

		// Token: 0x04001479 RID: 5241
		[ItemTypeID]
		public int itemTypeID;

		// Token: 0x0400147A RID: 5242
		public int amount = 1;

		// Token: 0x0400147B RID: 5243
		private bool claimed;

		// Token: 0x0400147C RID: 5244
		private bool claiming;

		// Token: 0x0400147D RID: 5245
		private ItemMetaData? _cachedMeta;
	}
}
