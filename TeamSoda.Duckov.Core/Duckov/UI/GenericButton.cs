using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x020003AA RID: 938
	public class GenericButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x060020C4 RID: 8388 RVA: 0x00072F48 File Offset: 0x00071148
		public void OnPointerClick(PointerEventData eventData)
		{
			UnityEvent unityEvent = this.onPointerClick;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x00072F5C File Offset: 0x0007115C
		public void OnPointerDown(PointerEventData eventData)
		{
			foreach (ToggleAnimation toggleAnimation in this.toggleAnimations)
			{
				toggleAnimation.SetToggle(true);
			}
			UnityEvent unityEvent = this.onPointerDown;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x00072FC0 File Offset: 0x000711C0
		public void OnPointerUp(PointerEventData eventData)
		{
			foreach (ToggleAnimation toggleAnimation in this.toggleAnimations)
			{
				toggleAnimation.SetToggle(false);
			}
			UnityEvent unityEvent = this.onPointerUp;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x04001664 RID: 5732
		public List<ToggleAnimation> toggleAnimations = new List<ToggleAnimation>();

		// Token: 0x04001665 RID: 5733
		public UnityEvent onPointerClick;

		// Token: 0x04001666 RID: 5734
		public UnityEvent onPointerDown;

		// Token: 0x04001667 RID: 5735
		public UnityEvent onPointerUp;
	}
}
