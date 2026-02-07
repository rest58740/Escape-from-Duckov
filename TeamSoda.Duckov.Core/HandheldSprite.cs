using System;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class HandheldSprite : MonoBehaviour
{
	// Token: 0x06000BF0 RID: 3056 RVA: 0x0003308B File Offset: 0x0003128B
	private void Start()
	{
		if (this.agent.Item)
		{
			this.spriteRenderer.sprite = this.agent.Item.Icon;
		}
	}

	// Token: 0x04000A3C RID: 2620
	public DuckovItemAgent agent;

	// Token: 0x04000A3D RID: 2621
	public SpriteRenderer spriteRenderer;
}
