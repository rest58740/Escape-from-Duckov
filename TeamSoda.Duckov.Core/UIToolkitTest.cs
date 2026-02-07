using System;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x02000212 RID: 530
public class UIToolkitTest : MonoBehaviour
{
	// Token: 0x06000FAE RID: 4014 RVA: 0x0003E688 File Offset: 0x0003C888
	private void Awake()
	{
		VisualElement visualElement = UQueryExtensions.Q(this.doc.rootVisualElement, "Button", null);
		CallbackEventHandler callbackEventHandler = UQueryExtensions.Q(this.doc.rootVisualElement, "Button2", null);
		visualElement.RegisterCallback<ClickEvent>(new EventCallback<ClickEvent>(this.OnButtonClicked), 0);
		callbackEventHandler.RegisterCallback<ClickEvent>(new EventCallback<ClickEvent>(this.OnButton2Clicked), 0);
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x0003E6E7 File Offset: 0x0003C8E7
	private void OnButton2Clicked(ClickEvent evt)
	{
		Debug.Log("Button 2 Clicked");
	}

	// Token: 0x06000FB0 RID: 4016 RVA: 0x0003E6F3 File Offset: 0x0003C8F3
	private void OnButtonClicked(ClickEvent evt)
	{
		Debug.Log("Button Clicked");
	}

	// Token: 0x04000CDE RID: 3294
	[SerializeField]
	private UIDocument doc;
}
