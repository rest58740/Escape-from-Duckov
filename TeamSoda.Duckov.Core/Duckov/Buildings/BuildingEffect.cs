using System;
using System.Collections.Generic;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x0200032C RID: 812
	public class BuildingEffect : MonoBehaviour
	{
		// Token: 0x06001AFB RID: 6907 RVA: 0x00061E1A File Offset: 0x0006001A
		private void Awake()
		{
			BuildingManager.OnBuildingListChanged += this.OnBuildingStatusChanged;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x00061E3E File Offset: 0x0006003E
		private void OnDestroy()
		{
			this.DisableEffects();
			BuildingManager.OnBuildingListChanged -= this.OnBuildingStatusChanged;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x00061E68 File Offset: 0x00060068
		private void OnLevelInitialized()
		{
			this.Refresh();
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x00061E70 File Offset: 0x00060070
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x00061E78 File Offset: 0x00060078
		private void OnBuildingStatusChanged()
		{
			this.Refresh();
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x00061E80 File Offset: 0x00060080
		private void Refresh()
		{
			this.DisableEffects();
			if (this.IsBuildingConstructed())
			{
				this.EnableEffects();
			}
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x00061E96 File Offset: 0x00060096
		private bool IsBuildingConstructed()
		{
			return BuildingManager.Any(this.buildingID, false);
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x00061EA4 File Offset: 0x000600A4
		private void DisableEffects()
		{
			foreach (Modifier modifier in this.modifiers)
			{
				if (modifier != null)
				{
					modifier.RemoveFromTarget();
				}
			}
			this.modifiers.Clear();
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x00061F04 File Offset: 0x00060104
		private void EnableEffects()
		{
			this.DisableEffects();
			if (CharacterMainControl.Main == null)
			{
				return;
			}
			foreach (BuildingEffect.ModifierDescription description in this.modifierDescriptions)
			{
				this.Apply(description);
			}
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x00061F6C File Offset: 0x0006016C
		private void Apply(BuildingEffect.ModifierDescription description)
		{
			CharacterMainControl main = CharacterMainControl.Main;
			Stat stat;
			if (main == null)
			{
				stat = null;
			}
			else
			{
				Item characterItem = main.CharacterItem;
				stat = ((characterItem != null) ? characterItem.GetStat(description.stat) : null);
			}
			Stat stat2 = stat;
			if (stat2 == null)
			{
				return;
			}
			Modifier modifier = new Modifier(description.type, description.value, this);
			stat2.AddModifier(modifier);
			this.modifiers.Add(modifier);
		}

		// Token: 0x04001365 RID: 4965
		[SerializeField]
		private string buildingID;

		// Token: 0x04001366 RID: 4966
		[SerializeField]
		private List<BuildingEffect.ModifierDescription> modifierDescriptions = new List<BuildingEffect.ModifierDescription>();

		// Token: 0x04001367 RID: 4967
		private List<Modifier> modifiers = new List<Modifier>();

		// Token: 0x020005CC RID: 1484
		[Serializable]
		public struct ModifierDescription
		{
			// Token: 0x04002130 RID: 8496
			public string stat;

			// Token: 0x04002131 RID: 8497
			public ModifierType type;

			// Token: 0x04002132 RID: 8498
			public float value;
		}
	}
}
