using System;
using UnityEngine;

// Token: 0x020000A3 RID: 163
public class FillWaterAndFood : MonoBehaviour
{
	// Token: 0x0600057F RID: 1407 RVA: 0x00018B88 File Offset: 0x00016D88
	public void Fill()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		if (!main)
		{
			return;
		}
		main.AddWater(this.water);
		main.AddEnergy(this.food);
	}

	// Token: 0x040004FF RID: 1279
	public float water;

	// Token: 0x04000500 RID: 1280
	public float food;
}
