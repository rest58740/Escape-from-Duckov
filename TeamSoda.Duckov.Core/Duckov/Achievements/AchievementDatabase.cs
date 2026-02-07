using System;
using System.Collections.Generic;
using System.IO;
using Duckov.Utilities;
using MiniExcelLibs;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Achievements
{
	// Token: 0x02000338 RID: 824
	[CreateAssetMenu]
	public class AchievementDatabase : ScriptableObject
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001B90 RID: 7056 RVA: 0x000642F2 File Offset: 0x000624F2
		public static AchievementDatabase Instance
		{
			get
			{
				return GameplayDataSettings.AchievementDatabase;
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001B91 RID: 7057 RVA: 0x000642F9 File Offset: 0x000624F9
		private Dictionary<string, AchievementDatabase.Achievement> dic
		{
			get
			{
				if (this._dic == null)
				{
					this.RebuildDictionary();
				}
				return this._dic;
			}
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x00064310 File Offset: 0x00062510
		private void RebuildDictionary()
		{
			if (this._dic == null)
			{
				this._dic = new Dictionary<string, AchievementDatabase.Achievement>();
			}
			this._dic.Clear();
			if (this.achievementChart == null)
			{
				Debug.LogError("Achievement Chart is not assinged", this);
				return;
			}
			using (MemoryStream memoryStream = new MemoryStream(this.achievementChart.bytes))
			{
				foreach (AchievementDatabase.Achievement achievement in MiniExcel.Query<AchievementDatabase.Achievement>(memoryStream, null, 2, "A1", null))
				{
					this._dic[achievement.id.Trim()] = achievement;
				}
			}
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x000643D4 File Offset: 0x000625D4
		public static bool TryGetAchievementData(string id, out AchievementDatabase.Achievement achievement)
		{
			achievement = null;
			return !(AchievementDatabase.Instance == null) && AchievementDatabase.Instance.dic.TryGetValue(id, out achievement);
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x000643F9 File Offset: 0x000625F9
		internal bool IsIDValid(string id)
		{
			return this.dic.ContainsKey(id);
		}

		// Token: 0x040013BD RID: 5053
		[SerializeField]
		private XlsxObject achievementChart;

		// Token: 0x040013BE RID: 5054
		private Dictionary<string, AchievementDatabase.Achievement> _dic;

		// Token: 0x020005E5 RID: 1509
		[Serializable]
		public class Achievement
		{
			// Token: 0x170007B3 RID: 1971
			// (get) Token: 0x06002A2F RID: 10799 RVA: 0x0009CF32 File Offset: 0x0009B132
			// (set) Token: 0x06002A30 RID: 10800 RVA: 0x0009CF3A File Offset: 0x0009B13A
			public string id { get; set; }

			// Token: 0x170007B4 RID: 1972
			// (get) Token: 0x06002A31 RID: 10801 RVA: 0x0009CF43 File Offset: 0x0009B143
			// (set) Token: 0x06002A32 RID: 10802 RVA: 0x0009CF4B File Offset: 0x0009B14B
			public string overrideDisplayNameKey { get; set; }

			// Token: 0x170007B5 RID: 1973
			// (get) Token: 0x06002A33 RID: 10803 RVA: 0x0009CF54 File Offset: 0x0009B154
			// (set) Token: 0x06002A34 RID: 10804 RVA: 0x0009CF5C File Offset: 0x0009B15C
			public string overrideDescriptionKey { get; set; }

			// Token: 0x170007B6 RID: 1974
			// (get) Token: 0x06002A35 RID: 10805 RVA: 0x0009CF65 File Offset: 0x0009B165
			// (set) Token: 0x06002A36 RID: 10806 RVA: 0x0009CF8B File Offset: 0x0009B18B
			[LocalizationKey("Default")]
			private string DisplayNameKey
			{
				get
				{
					if (!string.IsNullOrWhiteSpace(this.overrideDisplayNameKey))
					{
						return this.overrideDisplayNameKey;
					}
					return "Achievement_" + this.id;
				}
				set
				{
				}
			}

			// Token: 0x170007B7 RID: 1975
			// (get) Token: 0x06002A37 RID: 10807 RVA: 0x0009CF8D File Offset: 0x0009B18D
			// (set) Token: 0x06002A38 RID: 10808 RVA: 0x0009CFB8 File Offset: 0x0009B1B8
			[LocalizationKey("Default")]
			public string DescriptionKey
			{
				get
				{
					if (!string.IsNullOrWhiteSpace(this.overrideDescriptionKey))
					{
						return this.overrideDescriptionKey;
					}
					return "Achievement_" + this.id + "_Desc";
				}
				set
				{
				}
			}

			// Token: 0x170007B8 RID: 1976
			// (get) Token: 0x06002A39 RID: 10809 RVA: 0x0009CFBA File Offset: 0x0009B1BA
			public string DisplayName
			{
				get
				{
					return LocalizationManager.ToPlainText(this.DisplayNameKey);
				}
			}

			// Token: 0x170007B9 RID: 1977
			// (get) Token: 0x06002A3A RID: 10810 RVA: 0x0009CFC7 File Offset: 0x0009B1C7
			public string Description
			{
				get
				{
					return LocalizationManager.ToPlainText(this.DescriptionKey);
				}
			}
		}
	}
}
