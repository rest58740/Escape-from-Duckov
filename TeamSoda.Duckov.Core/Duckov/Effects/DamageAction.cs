using System;
using Duckov.Buffs;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Effects
{
	// Token: 0x0200040B RID: 1035
	public class DamageAction : EffectAction
	{
		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x0600257B RID: 9595 RVA: 0x00082D74 File Offset: 0x00080F74
		private CharacterMainControl MainControl
		{
			get
			{
				Effect master = base.Master;
				if (master == null)
				{
					return null;
				}
				Item item = master.Item;
				if (item == null)
				{
					return null;
				}
				return item.GetCharacterMainControl();
			}
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x00082D94 File Offset: 0x00080F94
		protected override void OnTriggeredPositive()
		{
			if (this.MainControl == null)
			{
				return;
			}
			if (this.MainControl.Health == null)
			{
				return;
			}
			this.damageInfo.isFromBuffOrEffect = true;
			if (this.buff != null)
			{
				this.damageInfo.fromCharacter = this.buff.fromWho;
				this.damageInfo.fromWeaponItemID = this.buff.fromWeaponID;
			}
			this.damageInfo.damagePoint = this.MainControl.transform.position + Vector3.up * 0.8f;
			this.damageInfo.damageNormal = Vector3.up;
			if (this.percentDamage && this.MainControl.Health != null)
			{
				this.damageInfo.damageValue = this.percentDamageValue * this.MainControl.Health.MaxHealth * ((this.buff == null) ? 1f : ((float)this.buff.CurrentLayers));
			}
			else
			{
				this.damageInfo.damageValue = this.damageValue * ((this.buff == null) ? 1f : ((float)this.buff.CurrentLayers));
			}
			this.MainControl.Health.Hurt(this.damageInfo);
			if (this.fx)
			{
				Object.Instantiate<GameObject>(this.fx, this.damageInfo.damagePoint, Quaternion.identity);
			}
		}

		// Token: 0x04001982 RID: 6530
		[SerializeField]
		private Buff buff;

		// Token: 0x04001983 RID: 6531
		[SerializeField]
		private bool percentDamage;

		// Token: 0x04001984 RID: 6532
		[SerializeField]
		private float damageValue = 1f;

		// Token: 0x04001985 RID: 6533
		[SerializeField]
		private float percentDamageValue;

		// Token: 0x04001986 RID: 6534
		[SerializeField]
		private DamageInfo damageInfo = new DamageInfo(null);

		// Token: 0x04001987 RID: 6535
		[SerializeField]
		private GameObject fx;
	}
}
