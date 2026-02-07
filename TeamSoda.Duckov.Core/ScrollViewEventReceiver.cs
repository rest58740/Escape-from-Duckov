using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000211 RID: 529
public class ScrollViewEventReceiver : MonoBehaviour, IScrollHandler, IEventSystemHandler
{
	// Token: 0x06000FAB RID: 4011 RVA: 0x0003E660 File Offset: 0x0003C860
	private void Awake()
	{
		if (this.scrollRect == null)
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
		}
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x0003E67C File Offset: 0x0003C87C
	public void OnScroll(PointerEventData eventData)
	{
	}

	// Token: 0x04000CDD RID: 3293
	[SerializeField]
	private ScrollRect scrollRect;
}
