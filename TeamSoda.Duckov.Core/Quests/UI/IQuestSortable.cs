using System;

namespace Duckov.Quests.UI
{
	// Token: 0x02000363 RID: 867
	public interface IQuestSortable
	{
		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001E2B RID: 7723
		// (set) Token: 0x06001E2C RID: 7724
		Quest.SortingMode SortingMode { get; set; }

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001E2D RID: 7725
		// (set) Token: 0x06001E2E RID: 7726
		bool SortRevert { get; set; }
	}
}
