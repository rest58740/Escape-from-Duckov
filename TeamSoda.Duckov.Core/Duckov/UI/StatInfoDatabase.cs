using System;
using System.Collections.Generic;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003B3 RID: 947
	[CreateAssetMenu(menuName = "Duckov/Stat Info Database")]
	public class StatInfoDatabase : ScriptableObject
	{
		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600216D RID: 8557 RVA: 0x00075224 File Offset: 0x00073424
		public static StatInfoDatabase Instance
		{
			get
			{
				return GameplayDataSettings.StatInfo;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x0007522B File Offset: 0x0007342B
		private static Dictionary<string, StatInfoDatabase.Entry> Dic
		{
			get
			{
				return StatInfoDatabase.Instance._dic;
			}
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x00075238 File Offset: 0x00073438
		public static StatInfoDatabase.Entry Get(string statName)
		{
			if (!(StatInfoDatabase.Instance == null))
			{
				if (StatInfoDatabase.Dic == null)
				{
					StatInfoDatabase.RebuildDic();
				}
				StatInfoDatabase.Entry result;
				if (StatInfoDatabase.Dic.TryGetValue(statName, out result))
				{
					return result;
				}
			}
			return new StatInfoDatabase.Entry
			{
				statName = statName,
				polarity = 0,
				displayFormat = "0.##"
			};
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x00075294 File Offset: 0x00073494
		public static Polarity GetPolarity(string statName)
		{
			return StatInfoDatabase.Get(statName).polarity;
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x000752A4 File Offset: 0x000734A4
		[ContextMenu("Rebuild Dic")]
		private static void RebuildDic()
		{
			if (StatInfoDatabase.Instance == null)
			{
				return;
			}
			StatInfoDatabase.Instance._dic = new Dictionary<string, StatInfoDatabase.Entry>();
			foreach (StatInfoDatabase.Entry entry in StatInfoDatabase.Instance.entries)
			{
				if (StatInfoDatabase.Instance._dic.ContainsKey(entry.statName))
				{
					Debug.LogError("Stat Info 中有重复的 key: " + entry.statName);
				}
				else
				{
					StatInfoDatabase.Instance._dic[entry.statName] = entry;
				}
			}
		}

		// Token: 0x040016CA RID: 5834
		[SerializeField]
		private StatInfoDatabase.Entry[] entries = new StatInfoDatabase.Entry[0];

		// Token: 0x040016CB RID: 5835
		private Dictionary<string, StatInfoDatabase.Entry> _dic;

		// Token: 0x02000641 RID: 1601
		[Serializable]
		public struct Entry
		{
			// Token: 0x170007C5 RID: 1989
			// (get) Token: 0x06002B08 RID: 11016 RVA: 0x000A176A File Offset: 0x0009F96A
			public string DisplayFormat
			{
				get
				{
					if (string.IsNullOrEmpty(this.displayFormat))
					{
						return "0.##";
					}
					return this.displayFormat;
				}
			}

			// Token: 0x040022A4 RID: 8868
			public string statName;

			// Token: 0x040022A5 RID: 8869
			public Polarity polarity;

			// Token: 0x040022A6 RID: 8870
			public string displayFormat;
		}
	}
}
