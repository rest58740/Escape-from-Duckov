using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002E0 RID: 736
	public class FPS_Enemy_HomingFly : MiniGameBehaviour
	{
		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x0600177C RID: 6012 RVA: 0x0005685A File Offset: 0x00054A5A
		private bool CanSeeTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x0600177D RID: 6013 RVA: 0x0005685D File Offset: 0x00054A5D
		private bool Dead
		{
			get
			{
				return this.health.Dead;
			}
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x0005686A File Offset: 0x00054A6A
		private void Awake()
		{
			if (this.rigidbody == null)
			{
				this.rigidbody = base.GetComponent<Rigidbody>();
			}
			this.health.onDead += this.OnDead;
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x0005689D File Offset: 0x00054A9D
		private void OnDead(FPSHealth health)
		{
			this.rigidbody.useGravity = true;
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x000568AB File Offset: 0x00054AAB
		protected override void OnUpdate(float deltaTime)
		{
			if (this.Dead)
			{
				this.UpdateDead(deltaTime);
				return;
			}
			if (this.CanSeeTarget)
			{
				this.UpdateHoming(deltaTime);
				return;
			}
			this.UpdateIdle(deltaTime);
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x000568D4 File Offset: 0x00054AD4
		private void UpdateIdle(float deltaTime)
		{
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x000568D6 File Offset: 0x00054AD6
		private void UpdateDead(float deltaTime)
		{
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x000568D8 File Offset: 0x00054AD8
		private void UpdateHoming(float deltaTime)
		{
		}

		// Token: 0x0400112B RID: 4395
		[SerializeField]
		private Rigidbody rigidbody;

		// Token: 0x0400112C RID: 4396
		[SerializeField]
		private FPSHealth health;
	}
}
