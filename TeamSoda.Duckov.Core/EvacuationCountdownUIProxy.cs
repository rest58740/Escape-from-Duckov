using System;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class EvacuationCountdownUIProxy : MonoBehaviour
{
	// Token: 0x0600099F RID: 2463 RVA: 0x0002B0CF File Offset: 0x000292CF
	public void Request(CountDownArea target)
	{
		EvacuationCountdownUI.Request(target);
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x0002B0D7 File Offset: 0x000292D7
	public void Release(CountDownArea target)
	{
		EvacuationCountdownUI.Release(target);
	}
}
