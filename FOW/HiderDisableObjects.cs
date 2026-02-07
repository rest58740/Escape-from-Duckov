using System;
using UnityEngine;

namespace FOW
{
	// Token: 0x02000009 RID: 9
	public class HiderDisableObjects : HiderBehavior
	{
		// Token: 0x0600004A RID: 74 RVA: 0x000043EC File Offset: 0x000025EC
		protected override void OnHide()
		{
			GameObject[] objectsToHide = this.ObjectsToHide;
			for (int i = 0; i < objectsToHide.Length; i++)
			{
				objectsToHide[i].SetActive(false);
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004418 File Offset: 0x00002618
		protected override void OnReveal()
		{
			GameObject[] objectsToHide = this.ObjectsToHide;
			for (int i = 0; i < objectsToHide.Length; i++)
			{
				objectsToHide[i].SetActive(true);
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004443 File Offset: 0x00002643
		public void ModifyHiddenObjects(GameObject[] newObjectsToHide)
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

		// Token: 0x0400007D RID: 125
		[SerializeField]
		private GameObject[] ObjectsToHide;
	}
}
