using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002E2 RID: 738
	public class FPSDamageReceiver : MonoBehaviour
	{
		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001786 RID: 6022 RVA: 0x000568EA File Offset: 0x00054AEA
		public ParticleSystem DamageFX
		{
			get
			{
				if (GameManager.BloodFxOn)
				{
					return this.damageEffectPrefab;
				}
				return this.damageEffectPrefab_Censored;
			}
		}

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06001787 RID: 6023 RVA: 0x00056900 File Offset: 0x00054B00
		// (remove) Token: 0x06001788 RID: 6024 RVA: 0x00056938 File Offset: 0x00054B38
		public event Action<FPSDamageReceiver, FPSDamageInfo> onReceiveDamage;

		// Token: 0x06001789 RID: 6025 RVA: 0x00056970 File Offset: 0x00054B70
		internal void CastDamage(FPSDamageInfo damage)
		{
			if (this.DamageFX == null)
			{
				return;
			}
			FXPool.Play(this.DamageFX, damage.point, Quaternion.FromToRotation(Vector3.forward, damage.normal));
			Action<FPSDamageReceiver, FPSDamageInfo> action = this.onReceiveDamage;
			if (action == null)
			{
				return;
			}
			action(this, damage);
		}

		// Token: 0x0400112F RID: 4399
		[SerializeField]
		private ParticleSystem damageEffectPrefab;

		// Token: 0x04001130 RID: 4400
		[SerializeField]
		private ParticleSystem damageEffectPrefab_Censored;
	}
}
