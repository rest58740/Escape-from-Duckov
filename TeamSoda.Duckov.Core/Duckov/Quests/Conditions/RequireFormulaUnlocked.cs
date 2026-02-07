using System;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x0200037B RID: 891
	public class RequireFormulaUnlocked : Condition
	{
		// Token: 0x06001F43 RID: 8003 RVA: 0x0006EB4D File Offset: 0x0006CD4D
		public override bool Evaluate()
		{
			return CraftingManager.IsFormulaUnlocked(this.formulaID);
		}

		// Token: 0x04001556 RID: 5462
		[ItemTypeID]
		[SerializeField]
		private int itemID;

		// Token: 0x04001557 RID: 5463
		[SerializeField]
		private string formulaID;

		// Token: 0x04001558 RID: 5464
		public Item setFromItem;
	}
}
