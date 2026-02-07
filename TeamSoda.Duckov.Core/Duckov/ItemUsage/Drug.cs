using System;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.ItemUsage
{
	// Token: 0x02000383 RID: 899
	[MenuPath("医疗/药")]
	public class Drug : UsageBehavior
	{
		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001F5D RID: 8029 RVA: 0x0006EF1C File Offset: 0x0006D11C
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				UsageBehavior.DisplaySettingsData result = default(UsageBehavior.DisplaySettingsData);
				result.display = true;
				result.description = string.Format("{0} : {1}", LocalizationManager.ToPlainText(this.healValueDescriptionKey), this.healValue);
				if (this.useDurability)
				{
					result.description += string.Format(" ({0} : {1})", LocalizationManager.ToPlainText(this.durabilityUsageDescriptionKey), this.durabilityUsage);
				}
				return result;
			}
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x0006EF98 File Offset: 0x0006D198
		public override bool CanBeUsed(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			return characterMainControl && this.CheckCanHeal(characterMainControl);
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0006EFC4 File Offset: 0x0006D1C4
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (!characterMainControl)
			{
				return;
			}
			float num = (float)this.healValue;
			if (this.useDurability && item.UseDurability)
			{
				float num2 = this.durabilityUsage;
				if (this.canUsePart)
				{
					num = characterMainControl.Health.MaxHealth - characterMainControl.Health.CurrentHealth;
					if (num > (float)this.healValue)
					{
						num = (float)this.healValue;
					}
					num2 = num / (float)this.healValue * this.durabilityUsage;
					if (num2 > item.Durability)
					{
						num2 = item.Durability;
						num = (float)this.healValue * item.Durability / this.durabilityUsage;
					}
					Debug.Log(string.Format("治疗：{0}耐久消耗：{1}", num, num2));
					item.Durability -= num2;
				}
			}
			this.Heal(characterMainControl, item, num);
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x0006F0A4 File Offset: 0x0006D2A4
		private bool CheckCanHeal(CharacterMainControl character)
		{
			return this.healValue <= 0 || character.Health.CurrentHealth < character.Health.MaxHealth;
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x0006F0CC File Offset: 0x0006D2CC
		private void Heal(CharacterMainControl character, Item selfItem, float _healValue)
		{
			if (_healValue > 0f)
			{
				character.AddHealth((float)Mathf.CeilToInt(_healValue));
				return;
			}
			if (_healValue < 0f)
			{
				DamageInfo damageInfo = new DamageInfo(null);
				damageInfo.damageValue = -_healValue;
				damageInfo.damagePoint = character.transform.position;
				damageInfo.damageNormal = Vector3.up;
				character.Health.Hurt(damageInfo);
			}
		}

		// Token: 0x04001566 RID: 5478
		public int healValue;

		// Token: 0x04001567 RID: 5479
		[LocalizationKey("Default")]
		public string healValueDescriptionKey = "Usage_HealValue";

		// Token: 0x04001568 RID: 5480
		[LocalizationKey("Default")]
		public string durabilityUsageDescriptionKey = "Usage_Durability";

		// Token: 0x04001569 RID: 5481
		public bool useDurability;

		// Token: 0x0400156A RID: 5482
		public float durabilityUsage;

		// Token: 0x0400156B RID: 5483
		public bool canUsePart;
	}
}
