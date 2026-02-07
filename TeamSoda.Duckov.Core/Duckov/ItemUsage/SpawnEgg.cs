using System;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.ItemUsage
{
	// Token: 0x02000386 RID: 902
	public class SpawnEgg : UsageBehavior
	{
		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001F6B RID: 8043 RVA: 0x0006F3E8 File Offset: 0x0006D5E8
		public override UsageBehavior.DisplaySettingsData DisplaySettings
		{
			get
			{
				UsageBehavior.DisplaySettingsData result = default(UsageBehavior.DisplaySettingsData);
				result.display = true;
				result.description = (LocalizationManager.ToPlainText(this.descriptionKey) ?? "");
				return result;
			}
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x0006F421 File Offset: 0x0006D621
		public override bool CanBeUsed(Item item, object user)
		{
			return true;
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x0006F424 File Offset: 0x0006D624
		protected override void OnUse(Item item, object user)
		{
			CharacterMainControl characterMainControl = user as CharacterMainControl;
			if (characterMainControl == null)
			{
				return;
			}
			Egg egg = Object.Instantiate<Egg>(this.eggPrefab, characterMainControl.transform.position, Quaternion.identity);
			Collider component = egg.GetComponent<Collider>();
			Collider component2 = characterMainControl.GetComponent<Collider>();
			if (component && component2)
			{
				Debug.Log("关掉角色和蛋的碰撞");
				Physics.IgnoreCollision(component, component2, true);
			}
			egg.Init(characterMainControl.transform.position, characterMainControl.CurrentAimDirection * 1f, characterMainControl, this.spawnCharacter, this.eggSpawnDelay);
		}

		// Token: 0x04001576 RID: 5494
		public Egg eggPrefab;

		// Token: 0x04001577 RID: 5495
		public CharacterRandomPreset spawnCharacter;

		// Token: 0x04001578 RID: 5496
		public float eggSpawnDelay = 2f;

		// Token: 0x04001579 RID: 5497
		[LocalizationKey("Default")]
		public string descriptionKey = "Usage_SpawnEgg";
	}
}
