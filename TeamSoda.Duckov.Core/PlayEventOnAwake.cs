using System;
using Duckov;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class PlayEventOnAwake : MonoBehaviour
{
	// Token: 0x060001C5 RID: 453 RVA: 0x00008A3A File Offset: 0x00006C3A
	private void Awake()
	{
		AudioManager.Post(this.sfx, base.gameObject);
	}

	// Token: 0x0400016F RID: 367
	[SerializeField]
	private string sfx;
}
