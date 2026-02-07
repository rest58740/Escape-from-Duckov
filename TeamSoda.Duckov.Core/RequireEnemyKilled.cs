using System;
using Duckov.Quests;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class RequireEnemyKilled : Condition
{
	// Token: 0x060009CD RID: 2509 RVA: 0x0002B4C3 File Offset: 0x000296C3
	public override bool Evaluate()
	{
		return !(this.enemyPreset == null) && SavesCounter.GetKillCount(this.enemyPreset.nameKey) >= this.threshold;
	}

	// Token: 0x040008BE RID: 2238
	[SerializeField]
	private CharacterRandomPreset enemyPreset;

	// Token: 0x040008BF RID: 2239
	[SerializeField]
	private int threshold = 1;
}
