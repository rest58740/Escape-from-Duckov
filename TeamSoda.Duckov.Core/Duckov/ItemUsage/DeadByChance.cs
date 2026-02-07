using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.ItemUsage
{
	// Token: 0x02000382 RID: 898
	[MenuPath("概率死亡")]
	public class DeadByChance : UsageBehavior
	{
		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001F58 RID: 8024 RVA: 0x0006EDF4 File Offset: 0x0006CFF4
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				UsageBehavior.DisplaySettingsData result = default(UsageBehavior.DisplaySettingsData);
				result.display = true;
				result.description = string.Format("{0}:  {1:0}%", LocalizationManager.ToPlainText(this.descriptionKey), this.chance * 100f);
				return result;
			}
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0006EE3F File Offset: 0x0006D03F
		public override bool CanBeUsed(Item item, object user)
		{
			return user as CharacterMainControl;
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x0006EE54 File Offset: 0x0006D054
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (!characterMainControl)
			{
				return;
			}
			if (Random.Range(0f, 1f) > this.chance)
			{
				return;
			}
			this.KillSelf(characterMainControl, item.TypeID).Forget();
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x0006EEA0 File Offset: 0x0006D0A0
		private UniTaskVoid KillSelf(CharacterMainControl character, int weaponID)
		{
			DeadByChance.<KillSelf>d__8 <KillSelf>d__;
			<KillSelf>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<KillSelf>d__.<>4__this = this;
			<KillSelf>d__.character = character;
			<KillSelf>d__.weaponID = weaponID;
			<KillSelf>d__.<>1__state = -1;
			<KillSelf>d__.<>t__builder.Start<DeadByChance.<KillSelf>d__8>(ref <KillSelf>d__);
			return <KillSelf>d__.<>t__builder.Task;
		}

		// Token: 0x04001562 RID: 5474
		public int damageValue = 9999;

		// Token: 0x04001563 RID: 5475
		public float chance;

		// Token: 0x04001564 RID: 5476
		[LocalizationKey("Default")]
		public string descriptionKey = "Usage_DeadByChance";

		// Token: 0x04001565 RID: 5477
		[LocalizationKey("Default")]
		public string popTextKey = "Usage_DeadByChance_PopText";
	}
}
