using System;
using Duckov.Buffs;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x02000196 RID: 406
public class StormWeather : MonoBehaviour
{
	// Token: 0x06000C41 RID: 3137 RVA: 0x0003460C File Offset: 0x0003280C
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		SubSceneEntry subSceneInfo = MultiSceneCore.Instance.GetSubSceneInfo();
		if (this.onlyOutDoor && subSceneInfo.IsInDoor)
		{
			return;
		}
		if (!this.target)
		{
			this.target = CharacterMainControl.Main;
			if (!this.target)
			{
				return;
			}
		}
		this.addBuffTimer -= Time.deltaTime;
		if (this.addBuffTimer <= 0f)
		{
			this.addBuffTimer = this.addBuffTimeSpace;
			if (this.target.StormProtection > this.stormProtectionThreshold)
			{
				return;
			}
			this.target.AddBuff(this.buff, null, 0);
		}
	}

	// Token: 0x04000A90 RID: 2704
	public Buff buff;

	// Token: 0x04000A91 RID: 2705
	public float addBuffTimeSpace = 1f;

	// Token: 0x04000A92 RID: 2706
	private float addBuffTimer;

	// Token: 0x04000A93 RID: 2707
	private CharacterMainControl target;

	// Token: 0x04000A94 RID: 2708
	private bool onlyOutDoor = true;

	// Token: 0x04000A95 RID: 2709
	public float stormProtectionThreshold = 0.9f;
}
