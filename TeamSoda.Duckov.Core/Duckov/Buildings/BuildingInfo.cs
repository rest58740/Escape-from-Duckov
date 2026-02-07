using System;
using Duckov.Economy;
using Duckov.Quests;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x0200032B RID: 811
	[Serializable]
	public struct BuildingInfo
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001AEF RID: 6895 RVA: 0x00061CDF File Offset: 0x0005FEDF
		public bool Valid
		{
			get
			{
				return !string.IsNullOrEmpty(this.id);
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x00061CEF File Offset: 0x0005FEEF
		public Building Prefab
		{
			get
			{
				return BuildingDataCollection.GetPrefab(this.prefabName);
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x00061CFC File Offset: 0x0005FEFC
		public Vector2Int Dimensions
		{
			get
			{
				if (!this.Prefab)
				{
					return default(Vector2Int);
				}
				return this.Prefab.Dimensions;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x00061D2B File Offset: 0x0005FF2B
		[LocalizationKey("Default")]
		public string DisplayNameKey
		{
			get
			{
				return "Building_" + this.id;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x00061D3D File Offset: 0x0005FF3D
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.DisplayNameKey);
			}
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x00061D4A File Offset: 0x0005FF4A
		public static string GetDisplayName(string id)
		{
			return LocalizationManager.ToPlainText("Building_" + id);
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x00061D5C File Offset: 0x0005FF5C
		internal bool RequirementsSatisfied()
		{
			string[] array = this.requireBuildings;
			for (int i = 0; i < array.Length; i++)
			{
				if (!BuildingManager.Any(array[i], false))
				{
					return false;
				}
			}
			return QuestManager.AreQuestFinished(this.requireQuests);
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x00061D9B File Offset: 0x0005FF9B
		[LocalizationKey("Default")]
		public string DescriptionKey
		{
			get
			{
				return "Building_" + this.id + "_Desc";
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x00061DB2 File Offset: 0x0005FFB2
		public string Description
		{
			get
			{
				return LocalizationManager.ToPlainText(this.DescriptionKey);
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x00061DBF File Offset: 0x0005FFBF
		public int CurrentAmount
		{
			get
			{
				if (BuildingManager.Instance == null)
				{
					return 0;
				}
				return BuildingManager.GetBuildingAmount(this.id);
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x00061DDB File Offset: 0x0005FFDB
		public bool ReachedAmountLimit
		{
			get
			{
				return this.maxAmount > 0 && this.CurrentAmount >= this.maxAmount;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001AFA RID: 6906 RVA: 0x00061DF9 File Offset: 0x0005FFF9
		public int TokenAmount
		{
			get
			{
				if (BuildingManager.Instance == null)
				{
					return 0;
				}
				return BuildingManager.Instance.GetTokenAmount(this.id);
			}
		}

		// Token: 0x0400135D RID: 4957
		public string id;

		// Token: 0x0400135E RID: 4958
		public string prefabName;

		// Token: 0x0400135F RID: 4959
		public int maxAmount;

		// Token: 0x04001360 RID: 4960
		public Cost cost;

		// Token: 0x04001361 RID: 4961
		public string[] requireBuildings;

		// Token: 0x04001362 RID: 4962
		public string[] alternativeFor;

		// Token: 0x04001363 RID: 4963
		public int[] requireQuests;

		// Token: 0x04001364 RID: 4964
		public Sprite iconReference;
	}
}
