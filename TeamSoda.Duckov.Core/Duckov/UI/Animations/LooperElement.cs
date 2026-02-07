using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003F8 RID: 1016
	public abstract class LooperElement : MonoBehaviour
	{
		// Token: 0x06002508 RID: 9480 RVA: 0x000819B7 File Offset: 0x0007FBB7
		protected virtual void OnEnable()
		{
			this.clock.onTick += this.OnTick;
			this.OnTick(this.clock, this.clock.t);
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x000819E8 File Offset: 0x0007FBE8
		protected virtual void OnDisable()
		{
			if (this.clock != null)
			{
				this.clock.onTick -= this.OnTick;
			}
		}

		// Token: 0x0600250A RID: 9482
		protected abstract void OnTick(LooperClock clock, float t);

		// Token: 0x04001921 RID: 6433
		[SerializeField]
		private LooperClock clock;
	}
}
