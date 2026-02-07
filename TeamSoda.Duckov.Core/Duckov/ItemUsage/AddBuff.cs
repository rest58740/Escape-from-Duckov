using System;
using Duckov.Buffs;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.ItemUsage
{
	// Token: 0x02000381 RID: 897
	public class AddBuff : UsageBehavior
	{
		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001F54 RID: 8020 RVA: 0x0006ECC8 File Offset: 0x0006CEC8
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				UsageBehavior.DisplaySettingsData result = default(UsageBehavior.DisplaySettingsData);
				result.display = true;
				result.description = "";
				result.description = (this.buffPrefab.DisplayName ?? "");
				if (this.buffPrefab.LimitedLifeTime)
				{
					result.description += string.Format(" : {0}s ", this.buffPrefab.TotalLifeTime);
				}
				if (this.chance < 1f)
				{
					result.description += string.Format(" ({0} : {1}%)", LocalizationManager.ToPlainText(this.chanceKey), Mathf.RoundToInt(this.chance * 100f));
				}
				return result;
			}
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x0006ED8A File Offset: 0x0006CF8A
		public override bool CanBeUsed(Item item, object user)
		{
			return true;
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0006ED90 File Offset: 0x0006CF90
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (characterMainControl == null)
			{
				return;
			}
			if (Random.Range(0f, 1f) > this.chance)
			{
				return;
			}
			characterMainControl.AddBuff(this.buffPrefab, characterMainControl, 0);
		}

		// Token: 0x0400155F RID: 5471
		public Buff buffPrefab;

		// Token: 0x04001560 RID: 5472
		[Range(0.01f, 1f)]
		public float chance = 1f;

		// Token: 0x04001561 RID: 5473
		[LocalizationKey("Default")]
		private string chanceKey = "UI_AddBuffChance";
	}
}
