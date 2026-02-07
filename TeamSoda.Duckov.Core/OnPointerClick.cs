using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x02000171 RID: 369
public class OnPointerClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000B5A RID: 2906 RVA: 0x0003106C File Offset: 0x0002F26C
	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		UnityEvent<PointerEventData> unityEvent = this.onPointerClick;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(eventData);
	}

	// Token: 0x040009D9 RID: 2521
	public UnityEvent<PointerEventData> onPointerClick;
}
