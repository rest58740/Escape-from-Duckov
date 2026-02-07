using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A9 RID: 681
	public class PigBehaviour : MiniGameBehaviour
	{
		// Token: 0x0600168B RID: 5771 RVA: 0x00053EF0 File Offset: 0x000520F0
		private void Awake()
		{
			if (this.entity == null)
			{
				this.entity = base.GetComponent<GoldMinerEntity>();
			}
			GoldMinerEntity goldMinerEntity = this.entity;
			goldMinerEntity.OnAttached = (Action<GoldMinerEntity, Hook>)Delegate.Combine(goldMinerEntity.OnAttached, new Action<GoldMinerEntity, Hook>(this.OnAttached));
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x00053F40 File Offset: 0x00052140
		protected override void OnUpdate(float deltaTime)
		{
			Quaternion localRotation = Quaternion.AngleAxis((float)(this.movingRight ? 0 : 180), Vector3.up);
			base.transform.localRotation = localRotation;
			base.transform.localPosition += (this.movingRight ? Vector3.right : Vector3.left) * this.moveSpeed * this.entity.master.run.GameSpeedFactor * deltaTime;
			if (base.transform.localPosition.x > this.entity.master.Bounds.max.x)
			{
				this.movingRight = false;
				return;
			}
			if (base.transform.localPosition.x < this.entity.master.Bounds.min.x)
			{
				this.movingRight = true;
			}
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x00054037 File Offset: 0x00052237
		private void OnAttached(GoldMinerEntity entity, Hook hook)
		{
		}

		// Token: 0x040010AA RID: 4266
		[SerializeField]
		private GoldMinerEntity entity;

		// Token: 0x040010AB RID: 4267
		[SerializeField]
		private float moveSpeed = 50f;

		// Token: 0x040010AC RID: 4268
		private bool attached;

		// Token: 0x040010AD RID: 4269
		private bool movingRight;
	}
}
