using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003DC RID: 988
	public class CloseViewOnPointerClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06002404 RID: 9220 RVA: 0x0007EA04 File Offset: 0x0007CC04
		private void OnValidate()
		{
			if (this.view == null)
			{
				this.view = base.GetComponent<View>();
			}
			if (this.graphic == null)
			{
				this.graphic = base.GetComponent<Graphic>();
			}
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x0007EA3C File Offset: 0x0007CC3C
		private void Awake()
		{
			if (this.view == null)
			{
				this.view = base.GetComponent<View>();
			}
			if (this.graphic == null)
			{
				this.graphic = base.GetComponent<Graphic>();
			}
			ManagedUIElement.onOpen += this.OnViewOpen;
			ManagedUIElement.onClose += this.OnViewClose;
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x0007EA9F File Offset: 0x0007CC9F
		private void OnDestroy()
		{
			ManagedUIElement.onOpen -= this.OnViewOpen;
			ManagedUIElement.onClose -= this.OnViewClose;
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x0007EAC3 File Offset: 0x0007CCC3
		private void OnViewClose(ManagedUIElement element)
		{
			if (element != this.view)
			{
				return;
			}
			if (this.graphic == null)
			{
				return;
			}
			this.graphic.enabled = false;
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x0007EAEF File Offset: 0x0007CCEF
		private void OnViewOpen(ManagedUIElement element)
		{
			if (element != this.view)
			{
				return;
			}
			if (this.graphic == null)
			{
				return;
			}
			this.graphic.enabled = true;
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x0007EB1C File Offset: 0x0007CD1C
		public void OnPointerClick(PointerEventData eventData)
		{
		}

		// Token: 0x04001882 RID: 6274
		private const bool FunctionEnabled = false;

		// Token: 0x04001883 RID: 6275
		[SerializeField]
		private View view;

		// Token: 0x04001884 RID: 6276
		[SerializeField]
		private Graphic graphic;
	}
}
