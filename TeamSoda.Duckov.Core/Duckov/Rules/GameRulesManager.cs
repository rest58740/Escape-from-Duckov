using System;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Rules
{
	// Token: 0x0200040C RID: 1036
	public class GameRulesManager : MonoBehaviour
	{
		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x0600257E RID: 9598 RVA: 0x00082F3C File Offset: 0x0008113C
		public static GameRulesManager Instance
		{
			get
			{
				return GameManager.DifficultyManager;
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x0600257F RID: 9599 RVA: 0x00082F43 File Offset: 0x00081143
		public static Ruleset Current
		{
			get
			{
				return GameRulesManager.Instance.mCurrent;
			}
		}

		// Token: 0x140000FF RID: 255
		// (add) Token: 0x06002580 RID: 9600 RVA: 0x00082F50 File Offset: 0x00081150
		// (remove) Token: 0x06002581 RID: 9601 RVA: 0x00082F84 File Offset: 0x00081184
		public static event Action OnRuleChanged;

		// Token: 0x06002582 RID: 9602 RVA: 0x00082FB7 File Offset: 0x000811B7
		public static void NotifyRuleChanged()
		{
			Action onRuleChanged = GameRulesManager.OnRuleChanged;
			if (onRuleChanged == null)
			{
				return;
			}
			onRuleChanged();
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06002583 RID: 9603 RVA: 0x00082FC8 File Offset: 0x000811C8
		private Ruleset mCurrent
		{
			get
			{
				if (GameRulesManager.SelectedRuleIndex == RuleIndex.Custom)
				{
					return this.CustomRuleSet;
				}
				foreach (GameRulesManager.RuleIndexFileEntry ruleIndexFileEntry in this.entries)
				{
					if (ruleIndexFileEntry.index == GameRulesManager.SelectedRuleIndex)
					{
						return ruleIndexFileEntry.file.Data;
					}
				}
				return this.entries[0].file.Data;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06002584 RID: 9604 RVA: 0x00083030 File Offset: 0x00081230
		// (set) Token: 0x06002585 RID: 9605 RVA: 0x0008304A File Offset: 0x0008124A
		public static RuleIndex SelectedRuleIndex
		{
			get
			{
				if (SavesSystem.KeyExisits("GameRulesManager_RuleIndex"))
				{
					return SavesSystem.Load<RuleIndex>("GameRulesManager_RuleIndex");
				}
				return RuleIndex.Standard;
			}
			internal set
			{
				SavesSystem.Save<RuleIndex>("GameRulesManager_RuleIndex", value);
				GameRulesManager.NotifyRuleChanged();
			}
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x0008305C File Offset: 0x0008125C
		public static RuleIndex GetRuleIndexOfSaveSlot(int slot)
		{
			return SavesSystem.Load<RuleIndex>("GameRulesManager_RuleIndex", slot);
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06002587 RID: 9607 RVA: 0x00083069 File Offset: 0x00081269
		private Ruleset CustomRuleSet
		{
			get
			{
				if (this.customRuleSet == null)
				{
					this.ReloadCustomRuleSet();
				}
				return this.customRuleSet;
			}
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x0008307F File Offset: 0x0008127F
		private void Awake()
		{
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			SavesSystem.OnSetFile += this.OnSetFile;
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000830A3 File Offset: 0x000812A3
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
			SavesSystem.OnSetFile -= this.OnSetFile;
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x000830C7 File Offset: 0x000812C7
		private void OnSetFile()
		{
			this.ReloadCustomRuleSet();
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x000830D0 File Offset: 0x000812D0
		private void ReloadCustomRuleSet()
		{
			if (SavesSystem.KeyExisits("Rule_Custom"))
			{
				this.customRuleSet = SavesSystem.Load<Ruleset>("Rule_Custom");
			}
			if (this.customRuleSet == null)
			{
				this.customRuleSet = new Ruleset();
				this.customRuleSet.displayNameKey = "Rule_Custom";
			}
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x0008311C File Offset: 0x0008131C
		private void OnCollectSaveData()
		{
			if (GameRulesManager.SelectedRuleIndex == RuleIndex.Custom && this.customRuleSet != null)
			{
				SavesSystem.Save<Ruleset>("Rule_Custom", this.customRuleSet);
			}
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x00083140 File Offset: 0x00081340
		internal static string GetRuleIndexDisplayNameOfSlot(int slotIndex)
		{
			RuleIndex ruleIndexOfSaveSlot = GameRulesManager.GetRuleIndexOfSaveSlot(slotIndex);
			return LocalizationManager.ToPlainText(string.Format("Rule_{0}", ruleIndexOfSaveSlot));
		}

		// Token: 0x04001989 RID: 6537
		private const string SelectedRuleIndexSaveKey = "GameRulesManager_RuleIndex";

		// Token: 0x0400198A RID: 6538
		private Ruleset customRuleSet;

		// Token: 0x0400198B RID: 6539
		private const string CustomRuleSetKey = "Rule_Custom";

		// Token: 0x0400198C RID: 6540
		[SerializeField]
		private GameRulesManager.RuleIndexFileEntry[] entries;

		// Token: 0x02000677 RID: 1655
		[Serializable]
		private struct RuleIndexFileEntry
		{
			// Token: 0x04002393 RID: 9107
			public RuleIndex index;

			// Token: 0x04002394 RID: 9108
			public RulesetFile file;
		}
	}
}
