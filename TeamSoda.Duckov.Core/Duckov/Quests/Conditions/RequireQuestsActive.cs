using System;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x0200037F RID: 895
	public class RequireQuestsActive : Condition
	{
		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001F4E RID: 8014 RVA: 0x0006EC8C File Offset: 0x0006CE8C
		public int[] RequiredQuestIDs
		{
			get
			{
				return this.requiredQuestIDs;
			}
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x0006EC94 File Offset: 0x0006CE94
		public override bool Evaluate()
		{
			return QuestManager.AreQuestsActive(this.requiredQuestIDs);
		}

		// Token: 0x0400155D RID: 5469
		[SerializeField]
		private int[] requiredQuestIDs;
	}
}
