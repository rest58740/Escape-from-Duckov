using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029C RID: 668
	public class Bomb : MiniGameBehaviour
	{
		// Token: 0x060015DC RID: 5596 RVA: 0x000516C4 File Offset: 0x0004F8C4
		protected override void OnUpdate(float deltaTime)
		{
			base.transform.position += base.transform.up * this.moveSpeed * deltaTime;
			this.hoveringTargets.RemoveAll((GoldMinerEntity e) => e == null);
			if (this.hoveringTargets.Count > 0)
			{
				this.Explode(this.hoveringTargets[0]);
			}
			this.lifeTime += deltaTime;
			if (this.lifeTime > this.maxLifeTime)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x00051775 File Offset: 0x0004F975
		private void Explode(GoldMinerEntity goldMinerTarget)
		{
			goldMinerTarget.Explode(base.transform.position);
			FXPool.Play(this.explodeFX, base.transform.position, base.transform.rotation);
			Object.Destroy(base.gameObject);
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x000517B8 File Offset: 0x0004F9B8
		private void OnCollisionEnter2D(Collision2D collision)
		{
			GoldMinerEntity component = collision.gameObject.GetComponent<GoldMinerEntity>();
			if (component != null)
			{
				this.hoveringTargets.Add(component);
			}
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x000517E8 File Offset: 0x0004F9E8
		private void OnCollisionExit2D(Collision2D collision)
		{
			GoldMinerEntity component = collision.gameObject.GetComponent<GoldMinerEntity>();
			if (component != null)
			{
				this.hoveringTargets.Remove(component);
			}
		}

		// Token: 0x0400100A RID: 4106
		[SerializeField]
		private float moveSpeed;

		// Token: 0x0400100B RID: 4107
		[SerializeField]
		private float maxLifeTime = 10f;

		// Token: 0x0400100C RID: 4108
		[SerializeField]
		private ParticleSystem explodeFX;

		// Token: 0x0400100D RID: 4109
		private float lifeTime;

		// Token: 0x0400100E RID: 4110
		private List<GoldMinerEntity> hoveringTargets = new List<GoldMinerEntity>();
	}
}
