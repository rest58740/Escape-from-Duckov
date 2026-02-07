using System;
using UnityEngine;

namespace ParadoxNotion.Services
{
	// Token: 0x02000081 RID: 129
	public class EventRouterAnimatorMove : MonoBehaviour
	{
		// Token: 0x14000046 RID: 70
		// (add) Token: 0x0600055C RID: 1372 RVA: 0x0000F8E0 File Offset: 0x0000DAE0
		// (remove) Token: 0x0600055D RID: 1373 RVA: 0x0000F918 File Offset: 0x0000DB18
		public event EventRouter.EventDelegate onAnimatorMove;

		// Token: 0x0600055E RID: 1374 RVA: 0x0000F94D File Offset: 0x0000DB4D
		private void OnAnimatorMove()
		{
			if (this.onAnimatorMove != null)
			{
				this.onAnimatorMove(new EventData(base.gameObject, this));
			}
		}
	}
}
