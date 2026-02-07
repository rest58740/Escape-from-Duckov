using System;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200016D RID: 365
public class FadeGroupButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000B47 RID: 2887 RVA: 0x00030DFF File Offset: 0x0002EFFF
	private void OnEnable()
	{
		UIInputManager.OnCancel += this.OnCancel;
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x00030E12 File Offset: 0x0002F012
	private void OnDisable()
	{
		UIInputManager.OnCancel -= this.OnCancel;
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x00030E25 File Offset: 0x0002F025
	private void OnCancel(UIInputEventData data)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (data.Used)
		{
			return;
		}
		if (!this.triggerWhenCancel)
		{
			return;
		}
		this.Execute();
		data.Use();
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x00030E4E File Offset: 0x0002F04E
	public void OnPointerClick(PointerEventData eventData)
	{
		this.Execute();
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x00030E56 File Offset: 0x0002F056
	private void Execute()
	{
		if (this.closeOnClick)
		{
			this.closeOnClick.Hide();
		}
		if (this.openOnClick)
		{
			this.openOnClick.Show();
		}
	}

	// Token: 0x040009CC RID: 2508
	[SerializeField]
	private FadeGroup closeOnClick;

	// Token: 0x040009CD RID: 2509
	[SerializeField]
	private FadeGroup openOnClick;

	// Token: 0x040009CE RID: 2510
	[SerializeField]
	private bool triggerWhenCancel;
}
