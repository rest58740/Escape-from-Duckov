using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000158 RID: 344
public class ToggleHUD : MonoBehaviour
{
	// Token: 0x06000ACE RID: 2766 RVA: 0x0002FD84 File Offset: 0x0002DF84
	private void Awake()
	{
		foreach (GameObject gameObject in this.toggleTargets)
		{
			if (gameObject != null && !gameObject.activeInHierarchy)
			{
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x04000987 RID: 2439
	public List<GameObject> toggleTargets;
}
