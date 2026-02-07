using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class HalfObsticleTrigger : MonoBehaviour
{
	// Token: 0x060003AC RID: 940 RVA: 0x0001045D File Offset: 0x0000E65D
	private void OnTriggerEnter(Collider other)
	{
		this.parent.OnTriggerEnter(other);
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0001046B File Offset: 0x0000E66B
	private void OnTriggerExit(Collider other)
	{
		this.parent.OnTriggerExit(other);
	}

	// Token: 0x040002CA RID: 714
	public HalfObsticle parent;
}
