using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200020E RID: 526
public class ForceUnmaskable : MonoBehaviour
{
	// Token: 0x06000FA0 RID: 4000 RVA: 0x0003E528 File Offset: 0x0003C728
	private void OnEnable()
	{
		MaskableGraphic[] components = base.GetComponents<MaskableGraphic>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].maskable = false;
		}
	}
}
