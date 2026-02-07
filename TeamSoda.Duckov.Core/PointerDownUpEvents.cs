using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x0200020F RID: 527
public class PointerDownUpEvents : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x06000FA2 RID: 4002 RVA: 0x0003E55B File Offset: 0x0003C75B
	public void OnPointerDown(PointerEventData eventData)
	{
		UnityEvent<PointerEventData> unityEvent = this.onPointerDown;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(eventData);
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x0003E56E File Offset: 0x0003C76E
	public void OnPointerUp(PointerEventData eventData)
	{
		UnityEvent<PointerEventData> unityEvent = this.onPointerUp;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(eventData);
	}

	// Token: 0x04000CD9 RID: 3289
	public UnityEvent<PointerEventData> onPointerDown;

	// Token: 0x04000CDA RID: 3290
	public UnityEvent<PointerEventData> onPointerUp;
}
