using System;
using Duckov.Quests;

// Token: 0x0200011C RID: 284
public class Condition_RaidDead : Condition
{
	// Token: 0x060009C2 RID: 2498 RVA: 0x0002B35E File Offset: 0x0002955E
	public override bool Evaluate()
	{
		return RaidUtilities.CurrentRaid.dead;
	}
}
