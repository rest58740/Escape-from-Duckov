using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EPOOutline.Demo
{
	// Token: 0x02000009 RID: 9
	public class InteractableObject : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00002638 File Offset: 0x00000838
		private void Start()
		{
			if (!this.affectOutlinable)
			{
				return;
			}
			this.outlinable = base.GetComponent<Outlinable>();
			this.outlinable.enabled = false;
			this.outlinable.FrontParameters.FillPass.SetFloat("_PublicAngle", 35f);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002685 File Offset: 0x00000885
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!this.affectOutlinable)
			{
				return;
			}
			this.outlinable.enabled = true;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000269C File Offset: 0x0000089C
		public void OnPointerExit(PointerEventData eventData)
		{
			if (!this.affectOutlinable)
			{
				return;
			}
			this.outlinable.enabled = false;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000026B3 File Offset: 0x000008B3
		public void OnPointerClick(PointerEventData eventData)
		{
			AudioSource.PlayClipAtPoint(this.interactionSound, base.transform.position, 1f);
		}

		// Token: 0x04000023 RID: 35
		[SerializeField]
		private AudioClip interactionSound;

		// Token: 0x04000024 RID: 36
		[SerializeField]
		private bool affectOutlinable = true;

		// Token: 0x04000025 RID: 37
		private Outlinable outlinable;
	}
}
