using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x02000401 RID: 1025
	public class ToggleComponent : MonoBehaviour
	{
		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x0600252E RID: 9518 RVA: 0x00082067 File Offset: 0x00080267
		private bool Status
		{
			get
			{
				return this.master && this.master.Status;
			}
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x00082083 File Offset: 0x00080283
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<ToggleAnimation>();
			}
			this.master.onSetToggle += this.OnSetToggle;
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x000820B7 File Offset: 0x000802B7
		private void OnDestroy()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.onSetToggle -= this.OnSetToggle;
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x000820E0 File Offset: 0x000802E0
		protected virtual void OnSetToggle(ToggleAnimation master, bool value)
		{
		}

		// Token: 0x04001941 RID: 6465
		[SerializeField]
		private ToggleAnimation master;
	}
}
