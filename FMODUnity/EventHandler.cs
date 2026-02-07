using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FMODUnity
{
	// Token: 0x02000100 RID: 256
	public abstract class EventHandler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x0600067D RID: 1661 RVA: 0x000073B6 File Offset: 0x000055B6
		protected virtual void Start()
		{
			this.HandleGameEvent(EmitterGameEvent.ObjectStart);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x000073BF File Offset: 0x000055BF
		protected virtual void OnDestroy()
		{
			this.HandleGameEvent(EmitterGameEvent.ObjectDestroy);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x000073C8 File Offset: 0x000055C8
		private void OnEnable()
		{
			this.HandleGameEvent(EmitterGameEvent.ObjectEnable);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x000073D2 File Offset: 0x000055D2
		private void OnDisable()
		{
			this.HandleGameEvent(EmitterGameEvent.ObjectDisable);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x000073DC File Offset: 0x000055DC
		private void OnTriggerEnter(Collider other)
		{
			if (string.IsNullOrEmpty(this.CollisionTag) || other.CompareTag(this.CollisionTag) || (other.attachedRigidbody && other.attachedRigidbody.CompareTag(this.CollisionTag)))
			{
				this.HandleGameEvent(EmitterGameEvent.TriggerEnter);
			}
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0000742C File Offset: 0x0000562C
		private void OnTriggerExit(Collider other)
		{
			if (string.IsNullOrEmpty(this.CollisionTag) || other.CompareTag(this.CollisionTag) || (other.attachedRigidbody && other.attachedRigidbody.CompareTag(this.CollisionTag)))
			{
				this.HandleGameEvent(EmitterGameEvent.TriggerExit);
			}
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0000747B File Offset: 0x0000567B
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (string.IsNullOrEmpty(this.CollisionTag) || other.CompareTag(this.CollisionTag))
			{
				this.HandleGameEvent(EmitterGameEvent.TriggerEnter2D);
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0000749F File Offset: 0x0000569F
		private void OnTriggerExit2D(Collider2D other)
		{
			if (string.IsNullOrEmpty(this.CollisionTag) || other.CompareTag(this.CollisionTag))
			{
				this.HandleGameEvent(EmitterGameEvent.TriggerExit2D);
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x000074C3 File Offset: 0x000056C3
		private void OnCollisionEnter()
		{
			this.HandleGameEvent(EmitterGameEvent.CollisionEnter);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x000074CC File Offset: 0x000056CC
		private void OnCollisionExit()
		{
			this.HandleGameEvent(EmitterGameEvent.CollisionExit);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x000074D5 File Offset: 0x000056D5
		private void OnCollisionEnter2D()
		{
			this.HandleGameEvent(EmitterGameEvent.CollisionEnter2D);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000074DF File Offset: 0x000056DF
		private void OnCollisionExit2D()
		{
			this.HandleGameEvent(EmitterGameEvent.CollisionExit2D);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x000074E9 File Offset: 0x000056E9
		private void OnMouseEnter()
		{
			this.HandleGameEvent(EmitterGameEvent.ObjectMouseEnter);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x000074F3 File Offset: 0x000056F3
		private void OnMouseExit()
		{
			this.HandleGameEvent(EmitterGameEvent.ObjectMouseExit);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x000074FD File Offset: 0x000056FD
		private void OnMouseDown()
		{
			this.HandleGameEvent(EmitterGameEvent.ObjectMouseDown);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00007507 File Offset: 0x00005707
		private void OnMouseUp()
		{
			this.HandleGameEvent(EmitterGameEvent.ObjectMouseUp);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00007511 File Offset: 0x00005711
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.HandleGameEvent(EmitterGameEvent.UIMouseEnter);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0000751B File Offset: 0x0000571B
		public void OnPointerExit(PointerEventData eventData)
		{
			this.HandleGameEvent(EmitterGameEvent.UIMouseExit);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00007525 File Offset: 0x00005725
		public void OnPointerDown(PointerEventData eventData)
		{
			this.HandleGameEvent(EmitterGameEvent.UIMouseDown);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0000752F File Offset: 0x0000572F
		public void OnPointerUp(PointerEventData eventData)
		{
			this.HandleGameEvent(EmitterGameEvent.UIMouseUp);
		}

		// Token: 0x06000691 RID: 1681
		protected abstract void HandleGameEvent(EmitterGameEvent gameEvent);

		// Token: 0x04000563 RID: 1379
		public string CollisionTag = "";
	}
}
