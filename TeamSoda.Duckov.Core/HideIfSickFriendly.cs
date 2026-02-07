using System;
using System.Collections.Generic;
using Duckov.Options;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class HideIfSickFriendly : MonoBehaviour
{
	// Token: 0x060001E0 RID: 480 RVA: 0x000094BD File Offset: 0x000076BD
	private void Start()
	{
		this.Sync();
		OptionsManager.OnOptionsChanged += this.OnOptionsChanged;
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x000094D6 File Offset: 0x000076D6
	private void OnDestroy()
	{
		OptionsManager.OnOptionsChanged -= this.OnOptionsChanged;
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x000094E9 File Offset: 0x000076E9
	private void OnOptionsChanged(string option)
	{
		this.Sync();
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x000094F4 File Offset: 0x000076F4
	private void Sync()
	{
		bool disableCameraOffset = DisableCameraOffset.disableCameraOffset;
		if (this.sickFriendly != disableCameraOffset)
		{
			this.sickFriendly = disableCameraOffset;
		}
		foreach (GameObject gameObject in this.hideList)
		{
			if (gameObject)
			{
				gameObject.SetActive(!this.sickFriendly);
			}
		}
	}

	// Token: 0x0400019B RID: 411
	public List<GameObject> hideList = new List<GameObject>();

	// Token: 0x0400019C RID: 412
	private bool sickFriendly;
}
