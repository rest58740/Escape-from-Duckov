using System;
using UnityEngine;

namespace FOW
{
	// Token: 0x0200000A RID: 10
	public class HiderDisableRenderers : HiderBehavior
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00004478 File Offset: 0x00002678
		protected override void OnHide()
		{
			Renderer[] objectsToHide = this.ObjectsToHide;
			for (int i = 0; i < objectsToHide.Length; i++)
			{
				objectsToHide[i].enabled = false;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000044A4 File Offset: 0x000026A4
		protected override void OnReveal()
		{
			Renderer[] objectsToHide = this.ObjectsToHide;
			for (int i = 0; i < objectsToHide.Length; i++)
			{
				objectsToHide[i].enabled = true;
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000044CF File Offset: 0x000026CF
		public void ModifyHiddenRenderers(Renderer[] newObjectsToHide)
		{
			this.OnReveal();
			this.ObjectsToHide = newObjectsToHide;
			if (!base.enabled)
			{
				return;
			}
			if (!this.IsEnabled)
			{
				this.OnHide();
				return;
			}
			this.OnReveal();
		}

		// Token: 0x0400007E RID: 126
		[SerializeField]
		private Renderer[] ObjectsToHide;
	}
}
