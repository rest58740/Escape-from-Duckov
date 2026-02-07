using System;
using System.Collections.Generic;
using System.Text;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x02000266 RID: 614
	public class ModifyCharacterStatsBase : PerkBehaviour
	{
		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001360 RID: 4960 RVA: 0x00049615 File Offset: 0x00047815
		private string DescriptionFormat
		{
			get
			{
				return LocalizationManager.ToPlainText("PerkBehaviour_ModifyCharacterStatsBase");
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001361 RID: 4961 RVA: 0x00049624 File Offset: 0x00047824
		public override string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ModifyCharacterStatsBase.Entry entry in this.entries)
				{
					if (entry != null && !string.IsNullOrEmpty(entry.key))
					{
						string statDisplayName = LocalizationManager.ToPlainText("Stat_" + entry.key.Trim());
						bool flag = entry.value > 0f;
						float value = entry.value;
						string str = entry.percentage ? string.Format("{0}%", value * 100f) : value.ToString();
						string value2 = (flag ? "+" : "") + str;
						string value3 = StringExtensions.Format(this.DescriptionFormat, new
						{
							statDisplayName = statDisplayName,
							value = value2
						});
						stringBuilder.AppendLine(value3);
					}
				}
				return stringBuilder.ToString().Trim();
			}
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0004972C File Offset: 0x0004792C
		protected override void OnUnlocked()
		{
			LevelManager instance = LevelManager.Instance;
			Item item;
			if (instance == null)
			{
				item = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				item = ((mainCharacter != null) ? mainCharacter.CharacterItem : null);
			}
			this.targetItem = item;
			if (this.targetItem == null)
			{
				return;
			}
			StatCollection stats = this.targetItem.Stats;
			if (stats == null)
			{
				return;
			}
			foreach (ModifyCharacterStatsBase.Entry entry in this.entries)
			{
				Stat stat = stats.GetStat(entry.key);
				if (stat == null)
				{
					break;
				}
				stat.BaseValue += entry.value;
				this.records.Add(new ModifyCharacterStatsBase.Record
				{
					stat = stat,
					value = entry.value
				});
			}
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00049810 File Offset: 0x00047A10
		protected override void OnLocked()
		{
			if (this.targetItem == null)
			{
				return;
			}
			if (this.targetItem.Stats == null)
			{
				return;
			}
			foreach (ModifyCharacterStatsBase.Record record in this.records)
			{
				if (record.stat == null)
				{
					break;
				}
				record.stat.BaseValue -= record.value;
			}
		}

		// Token: 0x04000EA5 RID: 3749
		[SerializeField]
		private List<ModifyCharacterStatsBase.Entry> entries = new List<ModifyCharacterStatsBase.Entry>();

		// Token: 0x04000EA6 RID: 3750
		private Item targetItem;

		// Token: 0x04000EA7 RID: 3751
		private List<ModifyCharacterStatsBase.Record> records = new List<ModifyCharacterStatsBase.Record>();

		// Token: 0x02000559 RID: 1369
		[Serializable]
		public class Entry
		{
			// Token: 0x1700078C RID: 1932
			// (get) Token: 0x060028FB RID: 10491 RVA: 0x00096BC7 File Offset: 0x00094DC7
			private StringList AvaliableKeys
			{
				get
				{
					return StringLists.StatKeys;
				}
			}

			// Token: 0x04001F6E RID: 8046
			public string key;

			// Token: 0x04001F6F RID: 8047
			public float value;

			// Token: 0x04001F70 RID: 8048
			public bool percentage;
		}

		// Token: 0x0200055A RID: 1370
		private struct Record
		{
			// Token: 0x04001F71 RID: 8049
			public Stat stat;

			// Token: 0x04001F72 RID: 8050
			public float value;
		}
	}
}
