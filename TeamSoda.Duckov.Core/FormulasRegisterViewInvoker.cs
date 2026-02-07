using System;
using System.Collections.Generic;
using Duckov.UI;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class FormulasRegisterViewInvoker : InteractableBase
{
	// Token: 0x06000D67 RID: 3431 RVA: 0x000384F1 File Offset: 0x000366F1
	protected override void Awake()
	{
		base.Awake();
		this.finishWhenTimeOut = true;
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x00038500 File Offset: 0x00036700
	protected override void OnInteractFinished()
	{
		FormulasRegisterView.Show(this.additionalTags);
	}

	// Token: 0x04000B9D RID: 2973
	[SerializeField]
	private List<Tag> additionalTags;
}
