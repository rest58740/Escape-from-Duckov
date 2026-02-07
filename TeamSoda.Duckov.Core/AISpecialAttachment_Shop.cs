using System;
using UnityEngine;

// Token: 0x0200008D RID: 141
public class AISpecialAttachment_Shop : AISpecialAttachmentBase
{
	// Token: 0x06000505 RID: 1285 RVA: 0x00016BC2 File Offset: 0x00014DC2
	protected override void OnInited()
	{
		base.OnInited();
		this.aiCharacterController.hideIfFoundEnemy = this.shop;
	}

	// Token: 0x0400043B RID: 1083
	public GameObject shop;
}
