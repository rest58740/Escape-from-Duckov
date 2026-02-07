using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002E5 RID: 741
	public class FPSGunControl : MiniGameBehaviour
	{
		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001796 RID: 6038 RVA: 0x00056E32 File Offset: 0x00055032
		public FPSGun Gun
		{
			get
			{
				return this.gun;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001797 RID: 6039 RVA: 0x00056E3A File Offset: 0x0005503A
		public float ScatterAngle
		{
			get
			{
				if (this.Gun)
				{
					return this.Gun.ScatterAngle;
				}
				return 0f;
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x00056E5A File Offset: 0x0005505A
		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.gun != null)
			{
				this.SetGun(this.gun);
			}
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x00056E7C File Offset: 0x0005507C
		protected override void OnUpdate(float deltaTime)
		{
			bool buttonDown = base.Game.GetButtonDown(MiniGame.Button.A);
			bool buttonUp = base.Game.GetButtonUp(MiniGame.Button.A);
			if (buttonDown)
			{
				this.gun.SetTrigger(true);
			}
			if (buttonUp)
			{
				this.gun.SetTrigger(false);
			}
			this.UpdateGunPhysicsStatus(deltaTime);
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x00056EC6 File Offset: 0x000550C6
		private void UpdateGunPhysicsStatus(float deltaTime)
		{
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x00056EC8 File Offset: 0x000550C8
		private void SetGun(FPSGun gunInstance)
		{
			if (gunInstance != this.gun)
			{
				Object.Destroy(this.gun);
			}
			this.gun = gunInstance;
			this.SetupGunData();
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x00056EF0 File Offset: 0x000550F0
		private void SetupGunData()
		{
			this.gun.Setup(this.mainCamera, this.gunParent);
		}

		// Token: 0x04001149 RID: 4425
		[SerializeField]
		private Camera mainCamera;

		// Token: 0x0400114A RID: 4426
		[SerializeField]
		private Transform gunParent;

		// Token: 0x0400114B RID: 4427
		[SerializeField]
		private FPSGun gun;
	}
}
