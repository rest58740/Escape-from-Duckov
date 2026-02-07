using System;
using Duckov;
using Duckov.Quests;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class SetEndingMissleParameter : MonoBehaviour
{
	// Token: 0x060004CB RID: 1227 RVA: 0x00016214 File Offset: 0x00014414
	private void Start()
	{
		bool flag = this.launcherClosedCondition.Evaluate();
		AudioManager.SetRTPC("Ending_Missile", (float)(flag ? 1 : 0), null);
	}

	// Token: 0x04000415 RID: 1045
	[SerializeField]
	private Condition launcherClosedCondition;
}
