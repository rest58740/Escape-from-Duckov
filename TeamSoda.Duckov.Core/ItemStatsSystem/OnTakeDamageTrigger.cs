using System;
using UnityEngine;
using UnityEngine.Events;

namespace ItemStatsSystem
{
	// Token: 0x02000234 RID: 564
	[MenuPath("General/On Take Damage")]
	public class OnTakeDamageTrigger : EffectTrigger
	{
		// Token: 0x06001197 RID: 4503 RVA: 0x00044B45 File Offset: 0x00042D45
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00044B4D File Offset: 0x00042D4D
		protected override void OnDisable()
		{
			base.OnDisable();
			this.UnregisterEvents();
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00044B5B File Offset: 0x00042D5B
		protected override void OnMasterSetTargetItem(Effect effect, Item item)
		{
			this.RegisterEvents();
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00044B64 File Offset: 0x00042D64
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
			CharacterMainControl characterMainControl = item.GetCharacterMainControl();
			if (characterMainControl == null)
			{
				return;
			}
			this.target = characterMainControl.Health;
			this.target.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnTookDamage));
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00044BD5 File Offset: 0x00042DD5
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnTookDamage));
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00044C02 File Offset: 0x00042E02
		private void OnTookDamage(DamageInfo info)
		{
			if (info.damageValue < (float)this.threshold)
			{
				return;
			}
			base.Trigger(true);
		}

		// Token: 0x04000DCB RID: 3531
		[SerializeField]
		public int threshold;

		// Token: 0x04000DCC RID: 3532
		private Health target;
	}
}
