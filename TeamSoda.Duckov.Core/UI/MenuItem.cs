using System;
using UnityEngine;

namespace UI
{
	// Token: 0x0200021A RID: 538
	public class MenuItem : MonoBehaviour
	{
		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x00040887 File Offset: 0x0003EA87
		// (set) Token: 0x06001045 RID: 4165 RVA: 0x000408BA File Offset: 0x0003EABA
		public Menu Master
		{
			get
			{
				if (this._master == null)
				{
					Transform parent = base.transform.parent;
					this._master = ((parent != null) ? parent.GetComponent<Menu>() : null);
				}
				return this._master;
			}
			set
			{
				this._master = value;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x000408C3 File Offset: 0x0003EAC3
		// (set) Token: 0x06001047 RID: 4167 RVA: 0x000408DA File Offset: 0x0003EADA
		public bool Selectable
		{
			get
			{
				return base.gameObject.activeSelf && this.selectable;
			}
			set
			{
				this.selectable = value;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x000408E3 File Offset: 0x0003EAE3
		public bool IsSelected
		{
			get
			{
				return this.cacheSelected;
			}
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x000408EB File Offset: 0x0003EAEB
		private void OnTransformParentChanged()
		{
			if (this.Master == null)
			{
				return;
			}
			this.Master.Register(this);
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x00040908 File Offset: 0x0003EB08
		private void OnEnable()
		{
			if (this.Master == null)
			{
				return;
			}
			this.Master.Register(this);
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x00040925 File Offset: 0x0003EB25
		private void OnDisable()
		{
			if (this.Master == null)
			{
				return;
			}
			this.Master.Unegister(this);
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x00040942 File Offset: 0x0003EB42
		public void Select()
		{
			if (this.Master == null)
			{
				Debug.LogError("Menu Item " + base.name + " 没有Master。");
				return;
			}
			this.Master.Select(this);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00040979 File Offset: 0x0003EB79
		internal void NotifySelected()
		{
			this.cacheSelected = true;
			Action<MenuItem> action = this.onSelected;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00040993 File Offset: 0x0003EB93
		internal void NotifyDeselected()
		{
			this.cacheSelected = false;
			Action<MenuItem> action = this.onDeselected;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x000409AD File Offset: 0x0003EBAD
		internal void NotifyConfirmed()
		{
			Action<MenuItem> action = this.onConfirmed;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x000409C0 File Offset: 0x0003EBC0
		internal void NotifyCanceled()
		{
			Action<MenuItem> action = this.onCanceled;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x000409D3 File Offset: 0x0003EBD3
		internal void NotifyMasterFocusStatusChanged()
		{
			Action<MenuItem, bool> action = this.onFocusStatusChanged;
			if (action == null)
			{
				return;
			}
			action(this, this.Master.Focused);
		}

		// Token: 0x04000D27 RID: 3367
		private Menu _master;

		// Token: 0x04000D28 RID: 3368
		[SerializeField]
		private bool selectable = true;

		// Token: 0x04000D29 RID: 3369
		private bool cacheSelected;

		// Token: 0x04000D2A RID: 3370
		public Action<MenuItem> onSelected;

		// Token: 0x04000D2B RID: 3371
		public Action<MenuItem> onDeselected;

		// Token: 0x04000D2C RID: 3372
		public Action<MenuItem> onConfirmed;

		// Token: 0x04000D2D RID: 3373
		public Action<MenuItem> onCanceled;

		// Token: 0x04000D2E RID: 3374
		public Action<MenuItem, bool> onFocusStatusChanged;
	}
}
