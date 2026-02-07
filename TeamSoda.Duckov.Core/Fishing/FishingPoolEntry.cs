using System;
using ItemStatsSystem;
using UnityEngine;

namespace Fishing
{
	// Token: 0x0200021E RID: 542
	[Serializable]
	internal struct FishingPoolEntry
	{
		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x00040B9D File Offset: 0x0003ED9D
		public int ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x00040BA5 File Offset: 0x0003EDA5
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x04000D35 RID: 3381
		[SerializeField]
		[ItemTypeID]
		private int id;

		// Token: 0x04000D36 RID: 3382
		[SerializeField]
		private float weight;
	}
}
