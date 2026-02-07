using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002E6 RID: 742
	public class FPSHealth : MiniGameBehaviour
	{
		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x0600179E RID: 6046 RVA: 0x00056F11 File Offset: 0x00055111
		public int HP
		{
			get
			{
				return this.hp;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x0600179F RID: 6047 RVA: 0x00056F19 File Offset: 0x00055119
		public bool Dead
		{
			get
			{
				return this.dead;
			}
		}

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x060017A0 RID: 6048 RVA: 0x00056F24 File Offset: 0x00055124
		// (remove) Token: 0x060017A1 RID: 6049 RVA: 0x00056F5C File Offset: 0x0005515C
		public event Action<FPSHealth> onDead;

		// Token: 0x060017A2 RID: 6050 RVA: 0x00056F94 File Offset: 0x00055194
		protected override void Start()
		{
			base.Start();
			this.hp = this.maxHp;
			this.materialPropertyBlock = new MaterialPropertyBlock();
			foreach (FPSDamageReceiver fpsdamageReceiver in this.damageReceivers)
			{
				fpsdamageReceiver.onReceiveDamage += this.OnReceiverReceiveDamage;
			}
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00057010 File Offset: 0x00055210
		protected override void OnUpdate(float deltaTime)
		{
			if (this.hurtValue > 0f)
			{
				this.hurtValue = Mathf.MoveTowards(this.hurtValue, 0f, deltaTime * this.hurtValueDropRate);
			}
			this.materialPropertyBlock.SetFloat("_HurtValue", this.hurtValue);
			this.meshRenderer.SetPropertyBlock(this.materialPropertyBlock, 0);
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00057070 File Offset: 0x00055270
		private void OnReceiverReceiveDamage(FPSDamageReceiver receiver, FPSDamageInfo info)
		{
			this.ReceiveDamage(info);
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x0005707C File Offset: 0x0005527C
		private void ReceiveDamage(FPSDamageInfo info)
		{
			if (this.dead)
			{
				return;
			}
			this.hurtValue = 1f;
			this.hp -= Mathf.FloorToInt(info.amount);
			if (this.hp <= 0)
			{
				this.hp = 0;
				this.Die();
			}
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x000570CB File Offset: 0x000552CB
		private void Die()
		{
			this.dead = true;
			Action<FPSHealth> action = this.onDead;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x0400114C RID: 4428
		[SerializeField]
		private int maxHp;

		// Token: 0x0400114D RID: 4429
		[SerializeField]
		private List<FPSDamageReceiver> damageReceivers;

		// Token: 0x0400114E RID: 4430
		[SerializeField]
		private MeshRenderer meshRenderer;

		// Token: 0x0400114F RID: 4431
		[SerializeField]
		private float hurtValueDropRate = 1f;

		// Token: 0x04001150 RID: 4432
		private int hp;

		// Token: 0x04001151 RID: 4433
		private bool dead;

		// Token: 0x04001152 RID: 4434
		private float hurtValue;

		// Token: 0x04001154 RID: 4436
		private MaterialPropertyBlock materialPropertyBlock;
	}
}
