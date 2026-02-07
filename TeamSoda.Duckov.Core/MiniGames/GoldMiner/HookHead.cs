using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A3 RID: 675
	public class HookHead : MonoBehaviour
	{
		// Token: 0x06001674 RID: 5748 RVA: 0x00053BC6 File Offset: 0x00051DC6
		private void OnCollisionEnter2D(Collision2D collision)
		{
			Action<HookHead, Collision2D> action = this.onCollisionEnter;
			if (action == null)
			{
				return;
			}
			action(this, collision);
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00053BDA File Offset: 0x00051DDA
		private void OnCollisionExit2D(Collision2D collision)
		{
			Action<HookHead, Collision2D> action = this.onCollisionExit;
			if (action == null)
			{
				return;
			}
			action(this, collision);
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00053BEE File Offset: 0x00051DEE
		private void OnCollisionStay2D(Collision2D collision)
		{
			Action<HookHead, Collision2D> action = this.onCollisionStay;
			if (action == null)
			{
				return;
			}
			action(this, collision);
		}

		// Token: 0x0400109A RID: 4250
		public Action<HookHead, Collision2D> onCollisionEnter;

		// Token: 0x0400109B RID: 4251
		public Action<HookHead, Collision2D> onCollisionExit;

		// Token: 0x0400109C RID: 4252
		public Action<HookHead, Collision2D> onCollisionStay;
	}
}
