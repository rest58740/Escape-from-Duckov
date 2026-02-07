using System;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000205 RID: 517
public class MultiInteractionMenuButton : MonoBehaviour
{
	// Token: 0x06000F59 RID: 3929 RVA: 0x0003DAF2 File Offset: 0x0003BCF2
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x0003DB10 File Offset: 0x0003BD10
	private void OnButtonClicked()
	{
		if (this.target == null)
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		main.Interact(this.target);
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x0003DB36 File Offset: 0x0003BD36
	internal void Setup(InteractableBase target)
	{
		base.gameObject.SetActive(true);
		this.target = target;
		this.text.text = target.InteractName;
		this.fadeGroup.SkipHide();
	}

	// Token: 0x06000F5C RID: 3932 RVA: 0x0003DB67 File Offset: 0x0003BD67
	internal void Show()
	{
		this.fadeGroup.Show();
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x0003DB74 File Offset: 0x0003BD74
	internal void Hide()
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x04000CBA RID: 3258
	[SerializeField]
	private Button button;

	// Token: 0x04000CBB RID: 3259
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x04000CBC RID: 3260
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000CBD RID: 3261
	private InteractableBase target;
}
