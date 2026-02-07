using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001CA RID: 458
public class MapMarkerPanelButton : MonoBehaviour
{
	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06000DDD RID: 3549 RVA: 0x00039CBB File Offset: 0x00037EBB
	public Image Image
	{
		get
		{
			return this.image;
		}
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x00039CC3 File Offset: 0x00037EC3
	public void Setup(UnityAction action, bool selected)
	{
		this.button.onClick.RemoveAllListeners();
		this.button.onClick.AddListener(action);
		this.selectionIndicator.gameObject.SetActive(selected);
	}

	// Token: 0x04000BD9 RID: 3033
	[SerializeField]
	private GameObject selectionIndicator;

	// Token: 0x04000BDA RID: 3034
	[SerializeField]
	private Image image;

	// Token: 0x04000BDB RID: 3035
	[SerializeField]
	private Button button;
}
