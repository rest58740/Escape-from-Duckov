using System;
using Duckov;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class PlayHurtEventProxy : MonoBehaviour
{
	// Token: 0x060001C7 RID: 455 RVA: 0x00008A56 File Offset: 0x00006C56
	public void Play(bool crit)
	{
		if (crit)
		{
			AudioManager.Post(this.critSfx, base.gameObject);
			return;
		}
		AudioManager.Post(this.nonCritSfx, base.gameObject);
	}

	// Token: 0x04000170 RID: 368
	[SerializeField]
	private string critSfx;

	// Token: 0x04000171 RID: 369
	[SerializeField]
	private string nonCritSfx;
}
