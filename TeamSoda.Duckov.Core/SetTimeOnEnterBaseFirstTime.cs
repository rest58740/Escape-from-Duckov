using System;
using Saves;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public class SetTimeOnEnterBaseFirstTime : MonoBehaviour
{
	// Token: 0x060005F6 RID: 1526 RVA: 0x0001AD7C File Offset: 0x00018F7C
	private void Start()
	{
		if (SavesSystem.Load<bool>("FirstTimeToBaseTimeSetted"))
		{
			return;
		}
		SavesSystem.Save<bool>("FirstTimeToBaseTimeSetted", true);
		TimeSpan time = new TimeSpan(this.setTimeTo, 0, 0);
		GameClock.Instance.StepTimeTil(time);
	}

	// Token: 0x04000586 RID: 1414
	public int setTimeTo;
}
