using System;
using UnityEngine;

namespace EPOOutline
{
	// Token: 0x02000010 RID: 16
	[ExecuteAlways]
	public class OnPreRenderEventTransferer : MonoBehaviour
	{
		// Token: 0x06000041 RID: 65 RVA: 0x000032D6 File Offset: 0x000014D6
		private void Awake()
		{
			this.attachedCamera = base.GetComponent<Camera>();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000032E4 File Offset: 0x000014E4
		private void OnPreRender()
		{
			if (this.OnPreRenderEvent != null)
			{
				this.OnPreRenderEvent(this.attachedCamera);
			}
		}

		// Token: 0x0400003A RID: 58
		private Camera attachedCamera;

		// Token: 0x0400003B RID: 59
		public Action<Camera> OnPreRenderEvent;
	}
}
