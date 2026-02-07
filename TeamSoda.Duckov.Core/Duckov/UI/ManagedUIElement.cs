using System;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003C3 RID: 963
	public abstract class ManagedUIElement : MonoBehaviour
	{
		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x0600228F RID: 8847 RVA: 0x00079444 File Offset: 0x00077644
		// (set) Token: 0x06002290 RID: 8848 RVA: 0x0007944C File Offset: 0x0007764C
		public bool open { get; private set; }

		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x06002291 RID: 8849 RVA: 0x00079458 File Offset: 0x00077658
		// (remove) Token: 0x06002292 RID: 8850 RVA: 0x0007948C File Offset: 0x0007768C
		public static event Action<ManagedUIElement> onOpen;

		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x06002293 RID: 8851 RVA: 0x000794C0 File Offset: 0x000776C0
		// (remove) Token: 0x06002294 RID: 8852 RVA: 0x000794F4 File Offset: 0x000776F4
		public static event Action<ManagedUIElement> onClose;

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06002295 RID: 8853 RVA: 0x00079527 File Offset: 0x00077727
		protected virtual bool ShowOpenCloseButtons
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x0007952A File Offset: 0x0007772A
		protected virtual void Awake()
		{
			this.RegisterEvents();
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x00079532 File Offset: 0x00077732
		protected virtual void OnDestroy()
		{
			this.UnregisterEvents();
			if (this.open)
			{
				this.Close();
			}
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x00079548 File Offset: 0x00077748
		public void Open(ManagedUIElement parent = null)
		{
			this.open = true;
			this.parent = parent;
			Action<ManagedUIElement> action = ManagedUIElement.onOpen;
			if (action != null)
			{
				action(this);
			}
			this.OnOpen();
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x0007956F File Offset: 0x0007776F
		public void Close()
		{
			this.open = false;
			this.parent = null;
			Action<ManagedUIElement> action = ManagedUIElement.onClose;
			if (action != null)
			{
				action(this);
			}
			this.OnClose();
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x00079596 File Offset: 0x00077796
		private void RegisterEvents()
		{
			ManagedUIElement.onOpen += this.OnManagedUIBehaviorOpen;
			ManagedUIElement.onClose += this.OnManagedUIBehaviorClose;
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x000795BA File Offset: 0x000777BA
		private void UnregisterEvents()
		{
			ManagedUIElement.onOpen -= this.OnManagedUIBehaviorOpen;
			ManagedUIElement.onClose -= this.OnManagedUIBehaviorClose;
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x000795DE File Offset: 0x000777DE
		private void OnManagedUIBehaviorClose(ManagedUIElement obj)
		{
			if (obj != null && obj == this.parent)
			{
				this.Close();
			}
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x000795FD File Offset: 0x000777FD
		private void OnManagedUIBehaviorOpen(ManagedUIElement obj)
		{
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x000795FF File Offset: 0x000777FF
		protected virtual void OnOpen()
		{
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x00079601 File Offset: 0x00077801
		protected virtual void OnClose()
		{
		}

		// Token: 0x04001772 RID: 6002
		[SerializeField]
		private ManagedUIElement parent;
	}
}
