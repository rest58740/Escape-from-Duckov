using System;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class MiningMachineCardDisplay : MonoBehaviour
{
	// Token: 0x06000BFA RID: 3066 RVA: 0x00033218 File Offset: 0x00031418
	public void SetVisualActive(bool active, MiningMachineCardDisplay.CardTypes cardType)
	{
		this.activeVisual.SetActive(active);
		this.deactiveVisual.SetActive(!active);
		if (cardType == MiningMachineCardDisplay.CardTypes.normal)
		{
			this.normalGPU.SetActive(true);
			this.potatoGPU.SetActive(false);
			return;
		}
		if (cardType != MiningMachineCardDisplay.CardTypes.potato)
		{
			throw new ArgumentOutOfRangeException("cardType", cardType, null);
		}
		this.normalGPU.SetActive(false);
		this.potatoGPU.SetActive(true);
	}

	// Token: 0x04000A45 RID: 2629
	public GameObject activeVisual;

	// Token: 0x04000A46 RID: 2630
	public GameObject deactiveVisual;

	// Token: 0x04000A47 RID: 2631
	public GameObject normalGPU;

	// Token: 0x04000A48 RID: 2632
	public GameObject potatoGPU;

	// Token: 0x020004D9 RID: 1241
	public enum CardTypes
	{
		// Token: 0x04001D6B RID: 7531
		normal,
		// Token: 0x04001D6C RID: 7532
		potato
	}
}
