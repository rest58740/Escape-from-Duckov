using System;
using UnityEngine;

namespace FOW
{
	// Token: 0x02000008 RID: 8
	[RequireComponent(typeof(FogOfWarHider))]
	public abstract class HiderBehavior : MonoBehaviour
	{
		// Token: 0x06000045 RID: 69 RVA: 0x000043AB File Offset: 0x000025AB
		protected virtual void Awake()
		{
			this.OnHide();
			base.GetComponent<FogOfWarHider>().OnActiveChanged += this.OnStatusChanged;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000043CA File Offset: 0x000025CA
		private void OnStatusChanged(bool isEnabled)
		{
			this.IsEnabled = isEnabled;
			if (isEnabled)
			{
				this.OnReveal();
				return;
			}
			this.OnHide();
		}

		// Token: 0x06000047 RID: 71
		protected abstract void OnReveal();

		// Token: 0x06000048 RID: 72
		protected abstract void OnHide();

		// Token: 0x0400007C RID: 124
		protected bool IsEnabled;
	}
}
