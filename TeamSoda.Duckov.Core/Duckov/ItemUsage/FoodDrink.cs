using System;
using ItemStatsSystem;
using SodaCraft.Localizations;

namespace Duckov.ItemUsage
{
	// Token: 0x02000384 RID: 900
	[MenuPath("食物/食物")]
	public class FoodDrink : UsageBehavior
	{
		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001F63 RID: 8035 RVA: 0x0006F154 File Offset: 0x0006D354
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				UsageBehavior.DisplaySettingsData result = default(UsageBehavior.DisplaySettingsData);
				result.display = true;
				if (this.energyValue != 0f && this.waterValue != 0f)
				{
					result.description = string.Concat(new string[]
					{
						LocalizationManager.ToPlainText(this.energyKey),
						": ",
						this.energyValue.ToString(),
						"  ",
						LocalizationManager.ToPlainText(this.waterKey),
						": ",
						this.waterValue.ToString()
					});
				}
				else if (this.energyValue != 0f)
				{
					result.description = LocalizationManager.ToPlainText(this.energyKey) + ": " + this.energyValue.ToString();
				}
				else
				{
					result.description = LocalizationManager.ToPlainText(this.waterKey) + ": " + this.waterValue.ToString();
				}
				return result;
			}
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x0006F24D File Offset: 0x0006D44D
		public override bool CanBeUsed(Item item, object user)
		{
			return user as CharacterMainControl;
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x0006F260 File Offset: 0x0006D460
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (!characterMainControl)
			{
				return;
			}
			this.Eat(characterMainControl);
			if (this.UseDurability > 0f && item.UseDurability)
			{
				item.Durability -= this.UseDurability;
			}
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x0006F2AC File Offset: 0x0006D4AC
		private void Eat(CharacterMainControl character)
		{
			if (this.energyValue != 0f)
			{
				character.AddEnergy(this.energyValue);
			}
			if (this.waterValue != 0f)
			{
				character.AddWater(this.waterValue);
			}
		}

		// Token: 0x0400156C RID: 5484
		public float energyValue;

		// Token: 0x0400156D RID: 5485
		public float waterValue;

		// Token: 0x0400156E RID: 5486
		[LocalizationKey("Default")]
		public string energyKey = "Usage_Energy";

		// Token: 0x0400156F RID: 5487
		[LocalizationKey("Default")]
		public string waterKey = "Usage_Water";

		// Token: 0x04001570 RID: 5488
		public float UseDurability;
	}
}
