using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using Duckov.UI.Animations;
using Duckov.Utilities;
using Saves;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Rules.UI
{
	// Token: 0x02000410 RID: 1040
	public class DifficultySelection : MonoBehaviour
	{
		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x0008326C File Offset: 0x0008146C
		private PrefabPool<DifficultySelection_Entry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<DifficultySelection_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x000832A5 File Offset: 0x000814A5
		private void Awake()
		{
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x000832C3 File Offset: 0x000814C3
		private void OnConfirmButtonClicked()
		{
			this.confirmed = true;
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x000832CC File Offset: 0x000814CC
		public UniTask Execute()
		{
			DifficultySelection.<Execute>d__15 <Execute>d__;
			<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Execute>d__.<>4__this = this;
			<Execute>d__.<>1__state = -1;
			<Execute>d__.<>t__builder.Start<DifficultySelection.<Execute>d__15>(ref <Execute>d__);
			return <Execute>d__.<>t__builder.Task;
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x00083310 File Offset: 0x00081510
		private bool CheckUnlocked(DifficultySelection.SettingEntry setting)
		{
			bool flag = !MultiSceneCore.GetVisited("Base");
			RuleIndex ruleIndex = setting.ruleIndex;
			if (ruleIndex <= RuleIndex.Custom)
			{
				if (ruleIndex != RuleIndex.Standard)
				{
					if (ruleIndex != RuleIndex.Custom)
					{
						return false;
					}
					return flag || GameRulesManager.SelectedRuleIndex == RuleIndex.Custom;
				}
			}
			else if (ruleIndex - RuleIndex.Easy > 2 && ruleIndex - RuleIndex.Hard > 1)
			{
				if (ruleIndex != RuleIndex.Rage)
				{
					return false;
				}
				return this.GetRageUnlocked(flag);
			}
			return flag || (GameRulesManager.SelectedRuleIndex != RuleIndex.Custom && GameRulesManager.SelectedRuleIndex != RuleIndex.Rage);
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x00083387 File Offset: 0x00081587
		public static void UnlockRage()
		{
			SavesSystem.SaveGlobal<bool>("Difficulty/RageUnlocked", true);
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x00083394 File Offset: 0x00081594
		public bool GetRageUnlocked(bool isFirstSelect)
		{
			return SavesSystem.LoadGlobal<bool>("Difficulty/RageUnlocked", false) && (isFirstSelect || (GameRulesManager.SelectedRuleIndex != RuleIndex.Custom && GameRulesManager.SelectedRuleIndex == RuleIndex.Rage));
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x000833C0 File Offset: 0x000815C0
		private bool CheckShouldDisplay(DifficultySelection.SettingEntry setting)
		{
			return true;
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x060025A8 RID: 9640 RVA: 0x000833C3 File Offset: 0x000815C3
		// (set) Token: 0x060025A9 RID: 9641 RVA: 0x000833CF File Offset: 0x000815CF
		public static bool CustomDifficultyMarker
		{
			get
			{
				return SavesSystem.Load<bool>("CustomDifficultyMarker");
			}
			set
			{
				SavesSystem.Save<bool>("CustomDifficultyMarker", value);
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x060025AA RID: 9642 RVA: 0x000833DC File Offset: 0x000815DC
		public RuleIndex SelectedRuleIndex
		{
			get
			{
				if (this.SelectedEntry == null)
				{
					return RuleIndex.Standard;
				}
				return this.SelectedEntry.Setting.ruleIndex;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x060025AB RID: 9643 RVA: 0x000833FE File Offset: 0x000815FE
		// (set) Token: 0x060025AC RID: 9644 RVA: 0x00083406 File Offset: 0x00081606
		public DifficultySelection_Entry SelectedEntry { get; private set; }

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x060025AD RID: 9645 RVA: 0x0008340F File Offset: 0x0008160F
		// (set) Token: 0x060025AE RID: 9646 RVA: 0x00083417 File Offset: 0x00081617
		public DifficultySelection_Entry HoveringEntry { get; private set; }

		// Token: 0x060025AF RID: 9647 RVA: 0x00083420 File Offset: 0x00081620
		private UniTask<RuleIndex> WaitForConfirmation()
		{
			DifficultySelection.<WaitForConfirmation>d__34 <WaitForConfirmation>d__;
			<WaitForConfirmation>d__.<>t__builder = AsyncUniTaskMethodBuilder<RuleIndex>.Create();
			<WaitForConfirmation>d__.<>4__this = this;
			<WaitForConfirmation>d__.<>1__state = -1;
			<WaitForConfirmation>d__.<>t__builder.Start<DifficultySelection.<WaitForConfirmation>d__34>(ref <WaitForConfirmation>d__);
			return <WaitForConfirmation>d__.<>t__builder.Task;
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x00083464 File Offset: 0x00081664
		internal void NotifySelected(DifficultySelection_Entry entry)
		{
			this.SelectedEntry = entry;
			GameRulesManager.SelectedRuleIndex = this.SelectedRuleIndex;
			foreach (DifficultySelection_Entry difficultySelection_Entry in this.EntryPool.ActiveEntries)
			{
				if (!(difficultySelection_Entry == null))
				{
					difficultySelection_Entry.Refresh();
				}
			}
			this.RefreshDescription();
			if (this.SelectedRuleIndex == RuleIndex.Custom)
			{
				this.ShowCustomRuleSetupPanel();
			}
			bool flag = this.SelectedRuleIndex == RuleIndex.Custom;
			this.achievementDisabledIndicator.SetActive(flag || DifficultySelection.CustomDifficultyMarker);
			this.selectedCustomDifficultyBefore.SetActive(DifficultySelection.CustomDifficultyMarker);
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x00083518 File Offset: 0x00081718
		private void ShowCustomRuleSetupPanel()
		{
			FadeGroup fadeGroup = this.customPanel;
			if (fadeGroup == null)
			{
				return;
			}
			fadeGroup.Show();
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x0008352A File Offset: 0x0008172A
		internal void NotifyEntryPointerEnter(DifficultySelection_Entry entry)
		{
			this.HoveringEntry = entry;
			this.RefreshDescription();
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x00083539 File Offset: 0x00081739
		internal void NotifyEntryPointerExit(DifficultySelection_Entry entry)
		{
			if (this.HoveringEntry == entry)
			{
				this.HoveringEntry = null;
				this.RefreshDescription();
			}
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x00083558 File Offset: 0x00081758
		private void RefreshDescription()
		{
			string text;
			if (this.SelectedEntry != null)
			{
				text = this.SelectedEntry.Setting.Description;
			}
			else
			{
				text = LocalizationManager.ToPlainText(this.description_PlaceHolderKey);
			}
			this.textDescription.text = text;
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x000835A1 File Offset: 0x000817A1
		internal void SkipHide()
		{
			if (this.fadeGroup != null)
			{
				this.fadeGroup.SkipHide();
			}
		}

		// Token: 0x040019A2 RID: 6562
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040019A3 RID: 6563
		[SerializeField]
		private TextMeshProUGUI textDescription;

		// Token: 0x040019A4 RID: 6564
		[SerializeField]
		[LocalizationKey("Default")]
		private string description_PlaceHolderKey = "DifficultySelection_Desc_PlaceHolder";

		// Token: 0x040019A5 RID: 6565
		[SerializeField]
		private Button confirmButton;

		// Token: 0x040019A6 RID: 6566
		[SerializeField]
		private FadeGroup customPanel;

		// Token: 0x040019A7 RID: 6567
		[SerializeField]
		private DifficultySelection_Entry entryTemplate;

		// Token: 0x040019A8 RID: 6568
		[SerializeField]
		private GameObject achievementDisabledIndicator;

		// Token: 0x040019A9 RID: 6569
		[SerializeField]
		private GameObject selectedCustomDifficultyBefore;

		// Token: 0x040019AA RID: 6570
		private PrefabPool<DifficultySelection_Entry> _entryPool;

		// Token: 0x040019AB RID: 6571
		[SerializeField]
		private DifficultySelection.SettingEntry[] displaySettings;

		// Token: 0x040019AE RID: 6574
		private bool confirmed;

		// Token: 0x02000678 RID: 1656
		[Serializable]
		public struct SettingEntry
		{
			// Token: 0x170007C6 RID: 1990
			// (get) Token: 0x06002B6C RID: 11116 RVA: 0x000A567E File Offset: 0x000A387E
			// (set) Token: 0x06002B6D RID: 11117 RVA: 0x000A5695 File Offset: 0x000A3895
			[LocalizationKey("Default")]
			private string TitleKey
			{
				get
				{
					return string.Format("Rule_{0}", this.ruleIndex);
				}
				set
				{
				}
			}

			// Token: 0x170007C7 RID: 1991
			// (get) Token: 0x06002B6E RID: 11118 RVA: 0x000A5697 File Offset: 0x000A3897
			public string Title
			{
				get
				{
					return LocalizationManager.ToPlainText(this.TitleKey);
				}
			}

			// Token: 0x170007C8 RID: 1992
			// (get) Token: 0x06002B6F RID: 11119 RVA: 0x000A56A4 File Offset: 0x000A38A4
			// (set) Token: 0x06002B70 RID: 11120 RVA: 0x000A56BB File Offset: 0x000A38BB
			[LocalizationKey("Default")]
			private string DescriptionKey
			{
				get
				{
					return string.Format("Rule_{0}_Desc", this.ruleIndex);
				}
				set
				{
				}
			}

			// Token: 0x170007C9 RID: 1993
			// (get) Token: 0x06002B71 RID: 11121 RVA: 0x000A56BD File Offset: 0x000A38BD
			public string Description
			{
				get
				{
					return LocalizationManager.ToPlainText(this.DescriptionKey);
				}
			}

			// Token: 0x04002395 RID: 9109
			public RuleIndex ruleIndex;

			// Token: 0x04002396 RID: 9110
			public Sprite icon;

			// Token: 0x04002397 RID: 9111
			public bool recommended;
		}
	}
}
