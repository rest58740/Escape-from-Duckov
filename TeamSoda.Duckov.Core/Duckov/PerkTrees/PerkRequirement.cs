using System;
using Duckov.Economy;
using ItemStatsSystem;

namespace Duckov.PerkTrees
{
	// Token: 0x02000258 RID: 600
	[Serializable]
	public class PerkRequirement
	{
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060012FC RID: 4860 RVA: 0x00048A44 File Offset: 0x00046C44
		public TimeSpan RequireTime
		{
			get
			{
				return TimeSpan.FromTicks(this.requireTime);
			}
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x00048A51 File Offset: 0x00046C51
		internal bool AreSatisfied()
		{
			return this.level <= EXPManager.Level && this.cost.Enough;
		}

		// Token: 0x04000E8B RID: 3723
		public int level;

		// Token: 0x04000E8C RID: 3724
		public Cost cost;

		// Token: 0x04000E8D RID: 3725
		[TimeSpan]
		public long requireTime;

		// Token: 0x02000555 RID: 1365
		[Serializable]
		public class RequireItemEntry
		{
			// Token: 0x04001F69 RID: 8041
			[ItemTypeID]
			public int id = 1;

			// Token: 0x04001F6A RID: 8042
			public int amount = 1;
		}
	}
}
