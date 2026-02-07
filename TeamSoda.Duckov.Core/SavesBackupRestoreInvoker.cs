using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using Duckov.UI.SavesRestore;
using Saves;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200016F RID: 367
public class SavesBackupRestoreInvoker : MonoBehaviour
{
	// Token: 0x06000B50 RID: 2896 RVA: 0x00030EBC File Offset: 0x0002F0BC
	private void Awake()
	{
		this.mainButton.onClick.AddListener(new UnityAction(this.OnMainButtonClicked));
		for (int i = 0; i < this.buttons.Count; i++)
		{
			Button button = this.buttons[i];
			int slot = i + 1;
			button.onClick.AddListener(delegate()
			{
				this.OnButtonClicked(slot);
			});
		}
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x00030F33 File Offset: 0x0002F133
	private void OnMainButtonClicked()
	{
		this.menuFadeGroup.Toggle();
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x00030F40 File Offset: 0x0002F140
	private void OnButtonClicked(int index)
	{
		this.menuFadeGroup.Hide();
		SavesSystem.SetFile(index);
		this.restorePanel.Open(index);
	}

	// Token: 0x040009D1 RID: 2513
	[SerializeField]
	private Button mainButton;

	// Token: 0x040009D2 RID: 2514
	[SerializeField]
	private FadeGroup menuFadeGroup;

	// Token: 0x040009D3 RID: 2515
	[SerializeField]
	private List<Button> buttons;

	// Token: 0x040009D4 RID: 2516
	[SerializeField]
	private SavesBackupRestorePanel restorePanel;
}
