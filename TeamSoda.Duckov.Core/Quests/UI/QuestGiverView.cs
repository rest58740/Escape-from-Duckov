using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x0200035D RID: 861
	public class QuestGiverView : View, ISingleSelectionMenu<QuestEntry>, IQuestSortable
	{
		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001DD1 RID: 7633 RVA: 0x0006BA13 File Offset: 0x00069C13
		public static QuestGiverView Instance
		{
			get
			{
				return View.GetViewInstance<QuestGiverView>();
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001DD2 RID: 7634 RVA: 0x0006BA1A File Offset: 0x00069C1A
		public string BtnText_CompleteQuest
		{
			get
			{
				return LocalizationManager.ToPlainText(this.btnText_CompleteQuest);
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001DD3 RID: 7635 RVA: 0x0006BA27 File Offset: 0x00069C27
		public string BtnText_AcceptQuest
		{
			get
			{
				return LocalizationManager.ToPlainText(this.btnText_AcceptQuest);
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001DD4 RID: 7636 RVA: 0x0006BA34 File Offset: 0x00069C34
		// (set) Token: 0x06001DD5 RID: 7637 RVA: 0x0006BA3C File Offset: 0x00069C3C
		public Quest.SortingMode SortingMode
		{
			get
			{
				return this._sortingMode;
			}
			set
			{
				this._sortingMode = value;
				this.RefreshList();
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x0006BA4B File Offset: 0x00069C4B
		// (set) Token: 0x06001DD7 RID: 7639 RVA: 0x0006BA53 File Offset: 0x00069C53
		public bool SortRevert
		{
			get
			{
				return this._sortRevert;
			}
			set
			{
				this._sortRevert = value;
				this.RefreshList();
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x0006BA64 File Offset: 0x00069C64
		private PrefabPool<QuestEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<QuestEntry>(this.entryPrefab, this.questEntriesParent, delegate(QuestEntry e)
					{
						this.activeEntries.Add(e);
					}, delegate(QuestEntry e)
					{
						this.activeEntries.Remove(e);
					}, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x0006BAB8 File Offset: 0x00069CB8
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.RefreshList();
			this.RefreshDetails();
			QuestManager.onQuestListsChanged += this.OnQuestListChanged;
			Quest.onQuestStatusChanged += this.OnQuestStatusChanged;
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x0006BB04 File Offset: 0x00069D04
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
			QuestManager.onQuestListsChanged -= this.OnQuestListChanged;
			Quest.onQuestStatusChanged -= this.OnQuestStatusChanged;
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x0006BB39 File Offset: 0x00069D39
		private void OnDisable()
		{
			if (this.details != null)
			{
				this.details.Setup(null);
			}
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x0006BB55 File Offset: 0x00069D55
		private void OnQuestStatusChanged(Quest quest)
		{
			QuestEntry questEntry = this.selectedQuestEntry;
			if (quest == ((questEntry != null) ? questEntry.Target : null))
			{
				this.RefreshDetails();
			}
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x0006BB77 File Offset: 0x00069D77
		protected override void Awake()
		{
			base.Awake();
			this.tabs.onSelectionChanged += this.OnTabChanged;
			this.btn_Interact.onClick.AddListener(new UnityAction(this.OnInteractButtonClicked));
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x0006BBB4 File Offset: 0x00069DB4
		private void OnInteractButtonClicked()
		{
			if (this.btnAcceptQuest)
			{
				Quest quest = this.details.Target;
				if (quest != null && QuestManager.IsQuestAvaliable(quest.ID))
				{
					QuestManager.Instance.ActivateQuest(quest.ID, new QuestGiverID?(this.target.ID));
					AudioManager.Post(this.sfx_AcceptQuest);
					return;
				}
			}
			else if (this.btnCompleteQuest)
			{
				Quest quest2 = this.details.Target;
				if (quest2 == null)
				{
					return;
				}
				if (quest2.TryComplete())
				{
					this.ShowCompleteUI(quest2);
					AudioManager.Post(this.sfx_CompleteQuest);
				}
			}
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x0006BC51 File Offset: 0x00069E51
		private void ShowCompleteUI(Quest quest)
		{
			this.completeUITask = this.questCompletePanel.Show(quest);
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x0006BC65 File Offset: 0x00069E65
		private void OnTabChanged(QuestGiverTabs tabs)
		{
			this.RefreshList();
			this.RefreshDetails();
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x0006BC73 File Offset: 0x00069E73
		protected override void OnDestroy()
		{
			base.OnDestroy();
			QuestManager.onQuestListsChanged -= this.OnQuestListChanged;
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x0006BC8C File Offset: 0x00069E8C
		private void OnQuestListChanged(QuestManager manager)
		{
			this.RefreshList();
			this.SetSelection(null);
			this.RefreshDetails();
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x0006BCA2 File Offset: 0x00069EA2
		public void Setup(QuestGiver target)
		{
			this.target = target;
			this.RefreshList();
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x0006BCB4 File Offset: 0x00069EB4
		private void RefreshList()
		{
			QuestGiverView.<>c__DisplayClass50_0 CS$<>8__locals1 = new QuestGiverView.<>c__DisplayClass50_0();
			CS$<>8__locals1.<>4__this = this;
			QuestGiverView.<>c__DisplayClass50_0 CS$<>8__locals2 = CS$<>8__locals1;
			QuestEntry questEntry = this.selectedQuestEntry;
			CS$<>8__locals2.keepQuest = ((questEntry != null) ? questEntry.Target : null);
			this.selectedQuestEntry = null;
			this.EntryPool.ReleaseAll();
			List<Quest> questsToShow = this.GetQuestsToShow();
			questsToShow.Sort((Quest a, Quest b) => Quest.Compare(a, b, CS$<>8__locals1.<>4__this.SortingMode, CS$<>8__locals1.<>4__this.SortRevert));
			bool flag = questsToShow.Count > 0;
			this.entryPlaceHolder.SetActive(!flag);
			this.RefreshRedDots();
			if (!flag)
			{
				return;
			}
			foreach (Quest quest in questsToShow)
			{
				QuestEntry questEntry2 = this.EntryPool.Get(this.questEntriesParent);
				questEntry2.transform.SetAsLastSibling();
				questEntry2.SetMenu(this);
				questEntry2.Setup(quest);
			}
			QuestEntry questEntry3 = this.activeEntries.Find((QuestEntry e) => e.Target == CS$<>8__locals1.keepQuest);
			if (questEntry3 != null)
			{
				this.SetSelection(questEntry3);
				return;
			}
			this.SetSelection(null);
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x0006BDCC File Offset: 0x00069FCC
		private void RefreshRedDots()
		{
			this.uninspectedAvaliableRedDot.SetActive(this.AnyUninspectedAvaliableQuest());
			this.activeRedDot.SetActive(this.AnyUninspectedActiveQuest());
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x0006BDF0 File Offset: 0x00069FF0
		private bool AnyUninspectedActiveQuest()
		{
			return !(this.target == null) && QuestManager.AnyActiveQuestNeedsInspection(this.target.ID);
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x0006BE14 File Offset: 0x0006A014
		private bool AnyUninspectedAvaliableQuest()
		{
			if (this.target == null)
			{
				return false;
			}
			return this.target.GetAvaliableQuests().Any((Quest e) => e != null && e.NeedInspection);
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x0006BE60 File Offset: 0x0006A060
		private List<Quest> GetQuestsToShow()
		{
			List<Quest> list = new List<Quest>();
			if (this.target == null)
			{
				return list;
			}
			QuestStatus status = this.tabs.GetStatus();
			switch (status)
			{
			case QuestStatus.None:
				return list;
			case (QuestStatus)1:
			case (QuestStatus)3:
				break;
			case QuestStatus.Avaliable:
				list.AddRange(this.target.GetAvaliableQuests());
				break;
			case QuestStatus.Active:
				list.AddRange(QuestManager.GetActiveQuestsFromGiver(this.target.ID));
				break;
			default:
				if (status == QuestStatus.Finished)
				{
					list.AddRange(QuestManager.GetHistoryQuestsFromGiver(this.target.ID));
				}
				break;
			}
			return list;
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x0006BEF4 File Offset: 0x0006A0F4
		private void RefreshDetails()
		{
			QuestEntry questEntry = this.selectedQuestEntry;
			Quest quest = (questEntry != null) ? questEntry.Target : null;
			this.details.Setup(quest);
			this.RefreshInteractButton();
			bool interactable = quest && (QuestManager.IsQuestActive(quest) || quest.Complete);
			this.details.Interactable = interactable;
			this.details.Refresh();
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x0006BF5C File Offset: 0x0006A15C
		private void RefreshInteractButton()
		{
			this.btnAcceptQuest = false;
			this.btnCompleteQuest = false;
			QuestEntry questEntry = this.selectedQuestEntry;
			Quest quest = (questEntry != null) ? questEntry.Target : null;
			if (quest == null)
			{
				this.btn_Interact.gameObject.SetActive(false);
				return;
			}
			QuestStatus status = this.tabs.GetStatus();
			bool active = false;
			switch (status)
			{
			case QuestStatus.None:
			case (QuestStatus)1:
			case (QuestStatus)3:
				break;
			case QuestStatus.Avaliable:
				active = true;
				this.btn_Interact.interactable = true;
				this.btnImage.color = this.interactableBtnImageColor;
				this.btnText.text = this.BtnText_AcceptQuest;
				this.btnAcceptQuest = true;
				break;
			case QuestStatus.Active:
			{
				active = true;
				bool flag = quest.AreTasksFinished();
				this.btn_Interact.interactable = flag;
				this.btnImage.color = (flag ? this.interactableBtnImageColor : this.uninteractableBtnImageColor);
				this.btnText.text = this.BtnText_CompleteQuest;
				this.btnCompleteQuest = true;
				break;
			}
			default:
				if (status != QuestStatus.Finished)
				{
				}
				break;
			}
			this.btn_Interact.gameObject.SetActive(active);
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x0006C06C File Offset: 0x0006A26C
		public QuestEntry GetSelection()
		{
			return this.selectedQuestEntry;
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x0006C074 File Offset: 0x0006A274
		public bool SetSelection(QuestEntry selection)
		{
			this.selectedQuestEntry = selection;
			if (selection != null)
			{
				QuestManager.SetEverInspected(selection.Target.ID);
			}
			this.RefreshDetails();
			this.RefreshEntries();
			this.RefreshRedDots();
			return true;
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x0006C0AC File Offset: 0x0006A2AC
		private void RefreshEntries()
		{
			foreach (QuestEntry questEntry in this.activeEntries)
			{
				questEntry.NotifyRefresh();
			}
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x0006C0FC File Offset: 0x0006A2FC
		internal override void TryQuit()
		{
			if (this.questCompletePanel.isActiveAndEnabled)
			{
				this.questCompletePanel.Skip();
				return;
			}
			base.Close();
		}

		// Token: 0x040014B5 RID: 5301
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040014B6 RID: 5302
		[SerializeField]
		private RectTransform questEntriesParent;

		// Token: 0x040014B7 RID: 5303
		[SerializeField]
		private QuestCompletePanel questCompletePanel;

		// Token: 0x040014B8 RID: 5304
		[SerializeField]
		private QuestGiverTabs tabs;

		// Token: 0x040014B9 RID: 5305
		[SerializeField]
		private QuestEntry entryPrefab;

		// Token: 0x040014BA RID: 5306
		[SerializeField]
		private GameObject entryPlaceHolder;

		// Token: 0x040014BB RID: 5307
		[SerializeField]
		private QuestViewDetails details;

		// Token: 0x040014BC RID: 5308
		[SerializeField]
		private Button btn_Interact;

		// Token: 0x040014BD RID: 5309
		[SerializeField]
		private TextMeshProUGUI btnText;

		// Token: 0x040014BE RID: 5310
		[SerializeField]
		private Image btnImage;

		// Token: 0x040014BF RID: 5311
		[SerializeField]
		private string btnText_AcceptQuest = "接受任务";

		// Token: 0x040014C0 RID: 5312
		[SerializeField]
		private string btnText_CompleteQuest = "完成任务";

		// Token: 0x040014C1 RID: 5313
		[SerializeField]
		private Color interactableBtnImageColor = Color.green;

		// Token: 0x040014C2 RID: 5314
		[SerializeField]
		private Color uninteractableBtnImageColor = Color.gray;

		// Token: 0x040014C3 RID: 5315
		[SerializeField]
		private GameObject uninspectedAvaliableRedDot;

		// Token: 0x040014C4 RID: 5316
		[SerializeField]
		private GameObject activeRedDot;

		// Token: 0x040014C5 RID: 5317
		private string sfx_AcceptQuest = "UI/mission_accept";

		// Token: 0x040014C6 RID: 5318
		private string sfx_CompleteQuest = "UI/mission_large";

		// Token: 0x040014C7 RID: 5319
		private Quest.SortingMode _sortingMode;

		// Token: 0x040014C8 RID: 5320
		private bool _sortRevert;

		// Token: 0x040014C9 RID: 5321
		private PrefabPool<QuestEntry> _entryPool;

		// Token: 0x040014CA RID: 5322
		private List<QuestEntry> activeEntries = new List<QuestEntry>();

		// Token: 0x040014CB RID: 5323
		private QuestGiver target;

		// Token: 0x040014CC RID: 5324
		private QuestEntry selectedQuestEntry;

		// Token: 0x040014CD RID: 5325
		private UniTask completeUITask;

		// Token: 0x040014CE RID: 5326
		private bool btnAcceptQuest;

		// Token: 0x040014CF RID: 5327
		private bool btnCompleteQuest;
	}
}
