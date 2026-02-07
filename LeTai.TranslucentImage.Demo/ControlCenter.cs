using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LeTai.Asset.TranslucentImage.Demo
{
	// Token: 0x02000006 RID: 6
	public class ControlCenter : MonoBehaviour
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002359 File Offset: 0x00000559
		private void Start()
		{
			this.rt = base.GetComponent<RectTransform>();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002368 File Offset: 0x00000568
		private void Update()
		{
			if (Mathf.Approximately(this.handle.rect.height, 0f))
			{
				return;
			}
			this.rt.anchoredPosition = new Vector2(this.rt.anchoredPosition.x, Mathf.Clamp(this.rt.anchoredPosition.y, -this.rt.rect.height / 2f + this.handle.rect.height, this.rt.rect.height / 2f - 1f));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002418 File Offset: 0x00000618
		public void Drag(BaseEventData baseEventData)
		{
			PointerEventData pointerEventData = (PointerEventData)baseEventData;
			this.rt.position = new Vector2(this.rt.position.x, this.rt.position.y + pointerEventData.delta.y);
		}

		// Token: 0x04000011 RID: 17
		public RectTransform handle;

		// Token: 0x04000012 RID: 18
		private RectTransform rt;
	}
}
