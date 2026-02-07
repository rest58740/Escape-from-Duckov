using System;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class DamageToSelf : MonoBehaviour
{
	// Token: 0x06000A61 RID: 2657 RVA: 0x0002CEBA File Offset: 0x0002B0BA
	private void Start()
	{
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x0002CEBC File Offset: 0x0002B0BC
	private void Update()
	{
		if (Input.GetKeyDown(107))
		{
			this.dmg.fromCharacter = CharacterMainControl.Main;
			CharacterMainControl.Main.Health.Hurt(this.dmg);
		}
		if (Input.GetKeyDown(108))
		{
			float value = CharacterMainControl.Main.CharacterItem.GetStat("InventoryCapacity").Value;
			Debug.Log(string.Format("InventorySize:{0}", value));
		}
	}

	// Token: 0x0400092B RID: 2347
	public DamageInfo dmg;
}
