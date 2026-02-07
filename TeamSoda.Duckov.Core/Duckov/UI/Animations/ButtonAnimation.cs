using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI.Animations
{
	// Token: 0x020003FC RID: 1020
	public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x06002513 RID: 9491 RVA: 0x00081C3C File Offset: 0x0007FE3C
		private void Awake()
		{
			this.SetAll(false);
			if (this.hoveringIndicator)
			{
				this.hoveringIndicator.SetActive(false);
			}
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x00081C5E File Offset: 0x0007FE5E
		private void OnEnable()
		{
			this.SetAll(false);
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x00081C67 File Offset: 0x0007FE67
		private void OnDisable()
		{
			if (this.hoveringIndicator)
			{
				this.hoveringIndicator.SetActive(false);
			}
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x00081C84 File Offset: 0x0007FE84
		private void SetAll(bool value)
		{
			foreach (ToggleAnimation toggleAnimation in this.toggles)
			{
				if (!(toggleAnimation == null))
				{
					toggleAnimation.SetToggle(value);
				}
			}
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x00081CE0 File Offset: 0x0007FEE0
		public void OnPointerDown(PointerEventData eventData)
		{
			this.SetAll(true);
			if (!this.mute)
			{
				AudioManager.Post("UI/click");
			}
			HardwareSyncingManager.SetEvent("Interact_UI");
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x00081D06 File Offset: 0x0007FF06
		public void OnPointerUp(PointerEventData eventData)
		{
			this.SetAll(false);
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x00081D0F File Offset: 0x0007FF0F
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.hoveringIndicator)
			{
				this.hoveringIndicator.SetActive(true);
			}
			if (!this.mute)
			{
				AudioManager.Post("UI/hover");
			}
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x00081D3D File Offset: 0x0007FF3D
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.hoveringIndicator)
			{
				this.hoveringIndicator.SetActive(false);
			}
		}

		// Token: 0x0400192C RID: 6444
		[SerializeField]
		private GameObject hoveringIndicator;

		// Token: 0x0400192D RID: 6445
		[SerializeField]
		private List<ToggleAnimation> toggles = new List<ToggleAnimation>();

		// Token: 0x0400192E RID: 6446
		[SerializeField]
		private bool mute;
	}
}
