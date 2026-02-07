using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A8 RID: 680
	public class ExplodeOnAttach : MiniGameBehaviour
	{
		// Token: 0x06001687 RID: 5767 RVA: 0x00053DB4 File Offset: 0x00051FB4
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<GoldMinerEntity>();
			}
			if (this.goldMiner == null)
			{
				this.goldMiner = base.GetComponentInParent<GoldMiner>();
			}
			GoldMinerEntity goldMinerEntity = this.master;
			goldMinerEntity.OnAttached = (Action<GoldMinerEntity, Hook>)Delegate.Combine(goldMinerEntity.OnAttached, new Action<GoldMinerEntity, Hook>(this.OnAttached));
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x00053E1C File Offset: 0x0005201C
		private void OnAttached(GoldMinerEntity target, Hook hook)
		{
			if (this.goldMiner == null)
			{
				return;
			}
			if (this.goldMiner.run == null)
			{
				return;
			}
			if (this.goldMiner.run.defuse.Value > 0.1f)
			{
				return;
			}
			Collider2D[] array = Physics2D.OverlapCircleAll(base.transform.position, this.explodeRange);
			for (int i = 0; i < array.Length; i++)
			{
				GoldMinerEntity component = array[i].GetComponent<GoldMinerEntity>();
				if (!(component == null))
				{
					component.Explode(base.transform.position);
				}
			}
			this.master.Explode(base.transform.position);
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x00053EC6 File Offset: 0x000520C6
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(base.transform.position, this.explodeRange);
		}

		// Token: 0x040010A7 RID: 4263
		[SerializeField]
		private GoldMiner goldMiner;

		// Token: 0x040010A8 RID: 4264
		[SerializeField]
		private GoldMinerEntity master;

		// Token: 0x040010A9 RID: 4265
		[SerializeField]
		private float explodeRange;
	}
}
