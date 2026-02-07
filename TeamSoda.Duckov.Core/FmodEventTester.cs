using System;
using Duckov;
using UnityEngine;

// Token: 0x020001A6 RID: 422
public class FmodEventTester : MonoBehaviour
{
	// Token: 0x06000CB3 RID: 3251 RVA: 0x00036113 File Offset: 0x00034313
	public void PlayEvent()
	{
		AudioManager.Post(this.e, base.gameObject);
	}

	// Token: 0x04000B0F RID: 2831
	[SerializeField]
	private string e;
}
