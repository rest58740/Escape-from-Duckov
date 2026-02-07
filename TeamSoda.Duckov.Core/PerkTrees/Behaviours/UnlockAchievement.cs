using System;
using Duckov.Achievements;
using UnityEngine;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x02000267 RID: 615
	public class UnlockAchievement : PerkBehaviour
	{
		// Token: 0x06001365 RID: 4965 RVA: 0x000498C2 File Offset: 0x00047AC2
		protected override void OnUnlocked()
		{
			if (AchievementManager.Instance == null)
			{
				return;
			}
			AchievementManager.Instance.Unlock(this.achievementKey.Trim());
		}

		// Token: 0x04000EA8 RID: 3752
		[SerializeField]
		private string achievementKey;
	}
}
