using System;
using Duckov.Scenes;
using Duckvo.Beacons;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class TeleportBeacon : MonoBehaviour
{
	// Token: 0x0600061A RID: 1562 RVA: 0x0001B6B8 File Offset: 0x000198B8
	private void Start()
	{
		bool beaconUnlocked = BeaconManager.GetBeaconUnlocked(this.beaconScene, this.beaconIndex);
		this.activeByUnlocked.SetActive(beaconUnlocked);
		this.interactable.gameObject.SetActive(!beaconUnlocked);
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x0001B6F7 File Offset: 0x000198F7
	public void ActivateBeacon()
	{
		BeaconManager.UnlockBeacon(this.beaconScene, this.beaconIndex);
		this.activeByUnlocked.SetActive(true);
		this.interactable.gameObject.SetActive(false);
	}

	// Token: 0x040005B0 RID: 1456
	[SceneID]
	public string beaconScene;

	// Token: 0x040005B1 RID: 1457
	public int beaconIndex;

	// Token: 0x040005B2 RID: 1458
	public GameObject activeByUnlocked;

	// Token: 0x040005B3 RID: 1459
	public InteractableBase interactable;
}
