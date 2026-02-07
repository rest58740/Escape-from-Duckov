using System;
using UnityEngine;

namespace Duckov.Quests.Conditions
{
	// Token: 0x02000380 RID: 896
	public class RequireQuestsFinished : Condition
	{
		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x0006ECA9 File Offset: 0x0006CEA9
		public int[] RequiredQuestIDs
		{
			get
			{
				return this.requiredQuestIDs;
			}
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x0006ECB1 File Offset: 0x0006CEB1
		public override bool Evaluate()
		{
			return QuestManager.AreQuestFinished(this.requiredQuestIDs);
		}

		// Token: 0x0400155E RID: 5470
		[SerializeField]
		private int[] requiredQuestIDs;
	}
}
