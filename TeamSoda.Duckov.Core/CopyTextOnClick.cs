using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200016B RID: 363
public class CopyTextOnClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06000B39 RID: 2873 RVA: 0x00030CA4 File Offset: 0x0002EEA4
	[SerializeField]
	private string content
	{
		get
		{
			return Path.Combine(Application.persistentDataPath, "Saves");
		}
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x00030CB5 File Offset: 0x0002EEB5
	public void OnPointerClick(PointerEventData eventData)
	{
		GUIUtility.systemCopyBuffer = this.content;
	}
}
