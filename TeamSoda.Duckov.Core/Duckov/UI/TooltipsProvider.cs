using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x0200039B RID: 923
	public class TooltipsProvider : MonoBehaviour, ITooltipsProvider, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x0600201E RID: 8222 RVA: 0x000711A7 File Offset: 0x0006F3A7
		public string GetTooltipsText()
		{
			return this.text;
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x000711AF File Offset: 0x0006F3AF
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				return;
			}
			Tooltips.NotifyEnterTooltipsProvider(this);
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x000711C5 File Offset: 0x0006F3C5
		public void OnPointerExit(PointerEventData eventData)
		{
			Tooltips.NotifyExitTooltipsProvider(this);
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x000711CD File Offset: 0x0006F3CD
		private void OnDisable()
		{
			Tooltips.NotifyExitTooltipsProvider(this);
		}

		// Token: 0x040015FB RID: 5627
		public string text;
	}
}
