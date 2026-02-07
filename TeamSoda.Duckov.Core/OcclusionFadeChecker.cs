using System;
using UnityEngine;

// Token: 0x0200018C RID: 396
public class OcclusionFadeChecker : MonoBehaviour
{
	// Token: 0x06000C11 RID: 3089 RVA: 0x00033950 File Offset: 0x00031B50
	private void OnTriggerEnter(Collider other)
	{
		OcclusionFadeTrigger component = other.GetComponent<OcclusionFadeTrigger>();
		if (!component)
		{
			return;
		}
		component.Enter();
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x00033974 File Offset: 0x00031B74
	private void OnTriggerExit(Collider other)
	{
		OcclusionFadeTrigger component = other.GetComponent<OcclusionFadeTrigger>();
		if (!component)
		{
			return;
		}
		component.Leave();
	}
}
