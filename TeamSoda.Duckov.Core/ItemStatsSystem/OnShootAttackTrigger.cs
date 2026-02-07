using System;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000233 RID: 563
	[MenuPath("General/On Shoot&Attack")]
	public class OnShootAttackTrigger : EffectTrigger
	{
		// Token: 0x06001190 RID: 4496 RVA: 0x00044A17 File Offset: 0x00042C17
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00044A1F File Offset: 0x00042C1F
		protected override void OnDisable()
		{
			base.OnDisable();
			this.UnregisterEvents();
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x00044A2D File Offset: 0x00042C2D
		protected override void OnMasterSetTargetItem(Effect effect, Item item)
		{
			this.RegisterEvents();
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00044A38 File Offset: 0x00042C38
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			if (base.Master == null)
			{
				return;
			}
			Item item = base.Master.Item;
			if (item == null)
			{
				return;
			}
			this.target = item.GetCharacterMainControl();
			if (this.target == null)
			{
				return;
			}
			if (this.onShoot)
			{
				this.target.OnShootEvent += this.OnShootAttack;
			}
			if (this.onAttack)
			{
				this.target.OnAttackEvent += this.OnShootAttack;
			}
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00044ACC File Offset: 0x00042CCC
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			if (this.onShoot)
			{
				this.target.OnShootEvent -= this.OnShootAttack;
			}
			if (this.onAttack)
			{
				this.target.OnAttackEvent -= this.OnShootAttack;
			}
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x00044B26 File Offset: 0x00042D26
		private void OnShootAttack(DuckovItemAgent agent)
		{
			base.Trigger(true);
		}

		// Token: 0x04000DC8 RID: 3528
		[SerializeField]
		private bool onShoot = true;

		// Token: 0x04000DC9 RID: 3529
		[SerializeField]
		private bool onAttack = true;

		// Token: 0x04000DCA RID: 3530
		private CharacterMainControl target;
	}
}
