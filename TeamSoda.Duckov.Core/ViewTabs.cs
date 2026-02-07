using System;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

// Token: 0x0200020C RID: 524
public class ViewTabs : MonoBehaviour
{
	// Token: 0x06000F94 RID: 3988 RVA: 0x0003E3F4 File Offset: 0x0003C5F4
	public void Show()
	{
		this.fadeGroup.Show();
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x0003E401 File Offset: 0x0003C601
	public void Hide()
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x0003E40E File Offset: 0x0003C60E
	private void Update()
	{
		if (this.fadeGroup.IsShown && View.ActiveView == null)
		{
			this.Hide();
		}
	}

	// Token: 0x04000CD5 RID: 3285
	[SerializeField]
	private FadeGroup fadeGroup;
}
