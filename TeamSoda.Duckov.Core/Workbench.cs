using System;
using Duckov.UI;

// Token: 0x0200019F RID: 415
public class Workbench : InteractableBase
{
	// Token: 0x06000C6A RID: 3178 RVA: 0x00035327 File Offset: 0x00033527
	protected override void OnInteractFinished()
	{
		ItemCustomizeSelectionView.Show();
	}
}
