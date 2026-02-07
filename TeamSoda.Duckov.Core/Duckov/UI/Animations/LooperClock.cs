using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003F7 RID: 1015
	public class LooperClock : MonoBehaviour
	{
		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06002502 RID: 9474 RVA: 0x000818C1 File Offset: 0x0007FAC1
		public float t
		{
			get
			{
				if (this.duration > 0f)
				{
					return this.time / this.duration;
				}
				return 1f;
			}
		}

		// Token: 0x140000FC RID: 252
		// (add) Token: 0x06002503 RID: 9475 RVA: 0x000818E4 File Offset: 0x0007FAE4
		// (remove) Token: 0x06002504 RID: 9476 RVA: 0x0008191C File Offset: 0x0007FB1C
		public event Action<LooperClock, float> onTick;

		// Token: 0x06002505 RID: 9477 RVA: 0x00081951 File Offset: 0x0007FB51
		private void Update()
		{
			if (this.duration > 0f)
			{
				this.time += Time.unscaledDeltaTime;
				this.time %= this.duration;
				this.Tick();
			}
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x0008198B File Offset: 0x0007FB8B
		private void Tick()
		{
			Action<LooperClock, float> action = this.onTick;
			if (action == null)
			{
				return;
			}
			action(this, this.t);
		}

		// Token: 0x0400191E RID: 6430
		[SerializeField]
		private float duration = 1f;

		// Token: 0x0400191F RID: 6431
		private float time;
	}
}
