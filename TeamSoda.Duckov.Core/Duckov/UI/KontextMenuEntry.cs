using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003E5 RID: 997
	public class KontextMenuEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06002463 RID: 9315 RVA: 0x0007FE0B File Offset: 0x0007E00B
		public void NotifyPooled()
		{
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x0007FE0D File Offset: 0x0007E00D
		public void NotifyReleased()
		{
			this.target = null;
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x0007FE16 File Offset: 0x0007E016
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.menu != null)
			{
				this.menu.InstanceHide();
			}
			if (this.target != null)
			{
				Action action = this.target.action;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x0007FE50 File Offset: 0x0007E050
		public void Setup(KontextMenu menu, int index, KontextMenuDataEntry data)
		{
			this.menu = menu;
			this.target = data;
			if (this.icon)
			{
				if (data.icon)
				{
					this.icon.sprite = data.icon;
					this.icon.gameObject.SetActive(true);
				}
				else
				{
					this.icon.gameObject.SetActive(false);
				}
			}
			if (this.text)
			{
				if (!string.IsNullOrEmpty(this.target.text))
				{
					this.text.text = this.target.text;
					this.text.gameObject.SetActive(true);
				}
				else
				{
					this.text.gameObject.SetActive(false);
				}
			}
			foreach (FadeElement fadeElement in this.fadeInElements)
			{
				fadeElement.SkipHide();
				fadeElement.Show(this.delayByIndex * (float)index).Forget();
			}
		}

		// Token: 0x040018BF RID: 6335
		[SerializeField]
		private Image icon;

		// Token: 0x040018C0 RID: 6336
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040018C1 RID: 6337
		[SerializeField]
		private float delayByIndex = 0.1f;

		// Token: 0x040018C2 RID: 6338
		[SerializeField]
		private List<FadeElement> fadeInElements;

		// Token: 0x040018C3 RID: 6339
		private KontextMenu menu;

		// Token: 0x040018C4 RID: 6340
		private KontextMenuDataEntry target;
	}
}
