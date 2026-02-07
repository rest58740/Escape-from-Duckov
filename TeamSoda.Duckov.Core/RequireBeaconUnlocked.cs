using System;
using Duckov.Quests;
using Duckov.Scenes;
using Duckvo.Beacons;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class RequireBeaconUnlocked : Condition
{
	// Token: 0x060009CB RID: 2507 RVA: 0x0002B4A8 File Offset: 0x000296A8
	public override bool Evaluate()
	{
		return BeaconManager.GetBeaconUnlocked(this.beaconID, this.beaconIndex);
	}

	// Token: 0x040008BC RID: 2236
	[SerializeField]
	[SceneID]
	private string beaconID;

	// Token: 0x040008BD RID: 2237
	[SerializeField]
	private int beaconIndex;
}
