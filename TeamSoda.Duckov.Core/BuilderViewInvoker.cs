using System;
using Duckov.Buildings;
using Duckov.Buildings.UI;
using UnityEngine;

// Token: 0x020001A8 RID: 424
public class BuilderViewInvoker : InteractableBase
{
	// Token: 0x06000CB7 RID: 3255 RVA: 0x00036154 File Offset: 0x00034354
	protected override void OnInteractFinished()
	{
		if (this.buildingArea == null)
		{
			return;
		}
		BuilderView.Show(this.buildingArea);
	}

	// Token: 0x04000B11 RID: 2833
	[SerializeField]
	private BuildingArea buildingArea;
}
