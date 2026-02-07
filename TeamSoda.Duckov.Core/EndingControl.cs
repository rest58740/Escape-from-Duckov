using System;
using Duckov.Achievements;
using Duckov.Rules.UI;
using Saves;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class EndingControl : MonoBehaviour
{
	// Token: 0x0600057D RID: 1405 RVA: 0x00018B00 File Offset: 0x00016D00
	public void SetEndingIndex()
	{
		Ending.endingIndex = this.endingIndex;
		AchievementManager instance = AchievementManager.Instance;
		bool flag = SavesSystem.Load<bool>(this.MissleLuncherClosedKey);
		DifficultySelection.UnlockRage();
		if (instance)
		{
			if (this.endingIndex == 0)
			{
				if (!flag)
				{
					instance.Unlock("Ending_0");
					return;
				}
				instance.Unlock("Ending_3");
				return;
			}
			else
			{
				if (!flag)
				{
					instance.Unlock("Ending_1");
					return;
				}
				instance.Unlock("Ending_2");
			}
		}
	}

	// Token: 0x040004FD RID: 1277
	public int endingIndex;

	// Token: 0x040004FE RID: 1278
	public string MissleLuncherClosedKey = "MissleLuncherClosed";
}
