using System;
using UnityEngine;

// Token: 0x020000E1 RID: 225
public class InteractMarker : MonoBehaviour
{
	// Token: 0x06000746 RID: 1862 RVA: 0x00020F68 File Offset: 0x0001F168
	public void MarkAsUsed()
	{
		if (this.markedAsUsed)
		{
			return;
		}
		this.markedAsUsed = true;
		if (this.hideIfUsedObject)
		{
			this.hideIfUsedObject.SetActive(false);
		}
		if (this.showIfUsedObject)
		{
			this.showIfUsedObject.SetActive(true);
		}
	}

	// Token: 0x040006F5 RID: 1781
	private bool markedAsUsed;

	// Token: 0x040006F6 RID: 1782
	public GameObject showIfUsedObject;

	// Token: 0x040006F7 RID: 1783
	public GameObject hideIfUsedObject;
}
