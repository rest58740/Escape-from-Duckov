using System;
using Duckov.UI.Animations;
using Saves;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class RestoreFailureDetectedIndicator : MonoBehaviour
{
	// Token: 0x060009F5 RID: 2549 RVA: 0x0002BA2B File Offset: 0x00029C2B
	private void OnEnable()
	{
		SavesSystem.OnRestoreFailureDetected += this.Refresh;
		SavesSystem.OnSetFile += this.Refresh;
		this.Refresh();
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x0002BA55 File Offset: 0x00029C55
	private void OnDisable()
	{
		SavesSystem.OnRestoreFailureDetected -= this.Refresh;
		SavesSystem.OnSetFile -= this.Refresh;
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x0002BA79 File Offset: 0x00029C79
	private void Refresh()
	{
		if (SavesSystem.RestoreFailureMarker)
		{
			this.fadeGroup.Show();
			return;
		}
		this.fadeGroup.Hide();
	}

	// Token: 0x040008D6 RID: 2262
	[SerializeField]
	private FadeGroup fadeGroup;
}
