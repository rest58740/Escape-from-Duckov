using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x02000164 RID: 356
public class DragHandler : MonoBehaviour, IDragHandler, IEventSystemHandler
{
	// Token: 0x06000B18 RID: 2840 RVA: 0x00030816 File Offset: 0x0002EA16
	public void OnDrag(PointerEventData eventData)
	{
		UnityEvent<PointerEventData> unityEvent = this.onDrag;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(eventData);
	}

	// Token: 0x040009BA RID: 2490
	public UnityEvent<PointerEventData> onDrag;
}
